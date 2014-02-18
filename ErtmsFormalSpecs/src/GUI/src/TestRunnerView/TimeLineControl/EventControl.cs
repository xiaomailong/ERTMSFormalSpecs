// ------------------------------------------------------------------------------
// -- Copyright ERTMS Solutions
// -- Licensed under the EUPL V.1.1
// -- http://joinup.ec.europa.eu/software/page/eupl/licence-eupl
// --
// -- This file is part of ERTMSFormalSpec software and documentation
// --
// --  ERTMSFormalSpec is free software: you can redistribute it and/or modify
// --  it under the terms of the EUPL General Public License, v.1.1
// --
// -- ERTMSFormalSpec is distributed in the hope that it will be useful,
// -- but WITHOUT ANY WARRANTY; without even the implied warranty of
// -- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// --
// ------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using DataDictionary.Tests;
using DataDictionary.Tests.Runner.Events;

namespace GUI.TestRunnerView.TimeLineControl
{
    public class EventControl : Button
    {
        /// <summary>
        /// The size of an event control button
        /// </summary>
        public static Size SIZE = new Size(40, 20);

        /// <summary>
        /// The event which is represented by this button
        /// </summary>
        public ModelEvent ModelEvent { get; set; }

        /// <summary>
        /// Last time a click was performed on this
        /// </summary>
        private DateTime lastClick;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modelEvent"></param>
        public EventControl(TimeLineControl controller, ModelEvent modelEvent)
        {
            Parent = controller;
            ModelEvent = modelEvent;
            Size = SIZE;
            Font = new System.Drawing.Font(FontFamily.GenericMonospace, 5);
            SetColor();
            SetText();

            Click += new EventHandler(EventControl_Click);
        }

        /// <summary>
        /// Provides the corresponding expectation, if any
        /// </summary>
        /// <returns></returns>
        private DataDictionary.Tests.Expectation getExpectation()
        {
            DataDictionary.Tests.Expectation retVal = null;

            ExpectationStateChange expectationChange = ModelEvent as ExpectationStateChange;
            if (expectationChange != null)
            {
                retVal = expectationChange.Expect.Expectation;

            }
            Expect expect = ModelEvent as Expect;
            if (expect != null)
            {
                retVal = expect.Expectation;
            }

            return retVal;
        }

        /// <summary>
        /// The enclosing time line control
        /// </summary>
        private TimeLineControl TimeLine
        {
            get { return Parent as TimeLineControl; }
        }

        /// <summary>
        /// Provides the enclosing window
        /// </summary>
        private Window EnclosingWindow
        {
            get { return TimeLine.Window; }
        }

        /// <summary>
        /// Double click does not work, implement it.
        /// </summary>
        /// <returns></returns>
        private bool isDoubleClick()
        {
            return (DateTime.Now - lastClick < new TimeSpan(0, 0, 0, 0, 500));
        }

        /// <summary>
        /// Click!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventControl_Click(object sender, EventArgs e)
        {
            if (ModelEvent is RuleFired)
            {
                if (GUIUtils.MDIWindow.DataDictionaryWindow != null)
                {
                    RuleFired ruleFired = ModelEvent as RuleFired;
                    GUIUtils.MDIWindow.DataDictionaryWindow.TreeView.Select(ruleFired.RuleCondition);
                }
            }
            else if (ModelEvent is VariableUpdate)
            {
                VariableUpdate variableUpdate = ModelEvent as VariableUpdate;

                if (isDoubleClick())
                {
                    if (variableUpdate != null)
                    {
                        DataDictionary.Interpreter.ExplanationPart explain = variableUpdate.Explanation;
                        ExplainBox explainTextBox = new ExplainBox();
                        explainTextBox.setExplanation(explain);
                        GUIUtils.MDIWindow.AddChildWindow(explainTextBox);
                    }
                }

                BaseTreeNode treeNode = null;

                if (GUIUtils.MDIWindow.DataDictionaryWindow != null)
                {
                    treeNode = GUIUtils.MDIWindow.DataDictionaryWindow.TreeView.Select(variableUpdate.Action);
                }
                if (treeNode == null)
                {
                    if (GUIUtils.MDIWindow.TestWindow != null)
                    {
                        GUIUtils.MDIWindow.TestWindow.TreeView.Select(variableUpdate.Action);
                    }
                }
            }
            else if (ModelEvent is Expect)
            {
                if (isDoubleClick())
                {
                    DataDictionary.Tests.Expectation expectation = getExpectation();

                    if (expectation != null)
                    {
                        DataDictionary.Interpreter.ExplanationPart explain = expectation.Expression.Explain();
                        ExplainBox explainTextBox = new ExplainBox();
                        explainTextBox.setExplanation(explain);
                        GUIUtils.MDIWindow.AddChildWindow(explainTextBox);
                    }
                }

                if (GUIUtils.MDIWindow.TestWindow != null)
                {
                    GUIUtils.MDIWindow.TestWindow.TreeView.Select((ModelEvent as Expect).Expectation);
                }
            }

            else if (ModelEvent is ModelInterpretationFailure)
            {
                if (GUIUtils.MDIWindow.DataDictionaryWindow != null)
                {
                    ModelInterpretationFailure failure = ModelEvent as ModelInterpretationFailure;
                    GUIUtils.MDIWindow.DataDictionaryWindow.TreeView.Select(failure.Instance as Utils.IModelElement);
                }
            }

            lastClick = DateTime.Now;
        }

        /// <summary>
        /// Sets the text of the button, according to modelized event
        /// </summary>
        private void SetText()
        {
            if (ModelEvent is RuleFired)
            {
                Text = "R";
            }
            else if (ModelEvent is VariableUpdate)
            {
                Text = "U";
            }
            else if (ModelEvent is Expect)
            {
                Text = "E";
            }
        }

        /// <summary>
        /// Provides the color associated to a model event
        /// </summary>
        /// <param name="modelEvent"></param>
        private void SetColor()
        {
            Color color = Color.Brown;

            if (ModelEvent is RuleFired)
            {
                color = Color.Blue;
            }
            else if (ModelEvent is VariableUpdate)
            {
                VariableUpdate update = ModelEvent as VariableUpdate;

                TestCase testCase = Utils.EnclosingFinder<TestCase>.find(update.Action);
                if (testCase != null)
                {
                    color = Color.LightGreen;
                }
                else
                {
                    color = Color.BlanchedAlmond;
                }
            }
            else if (ModelEvent is Expect)
            {
                Expect expect = ModelEvent as Expect;
                switch (expect.State)
                {
                    case Expect.EventState.Active:
                        color = Color.Silver;
                        break;
                    case Expect.EventState.Fullfilled:
                        color = Color.Gold;
                        break;
                    case Expect.EventState.TimeOut:
                        color = Color.Red;
                        break;
                }
            }
            else if (ModelEvent is ModelInterpretationFailure)
            {
                color = Color.Red;
            }

            BackColor = color;
        }

        /// <summary>
        /// Updates the state of the event control, according to the model event state
        /// </summary>
        public void UpdateState()
        {
            SetColor();
        }
    }
}
