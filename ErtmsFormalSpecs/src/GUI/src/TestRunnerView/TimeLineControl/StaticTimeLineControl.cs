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
namespace GUI.TestRunnerView.TimeLineControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DataDictionary.Tests;
    using DataDictionary.Tests.Runner.Events;
    using System.Windows.Forms;
    using DataDictionary;
    using Utils;
    using System.Collections;

    /// <summary>
    /// The static time line according to a test case
    /// </summary>
    public class StaticTimeLineControl : TimeLineControl
    {
        /// <summary>
        /// The test case
        /// </summary>
        private TestCase __testCase;

        /// <summary>
        /// The test case for which this time line is built
        /// </summary>
        public TestCase TestCase { get { return __testCase; } set { __testCase = value; CleanEventPositions(); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public StaticTimeLineControl()
            : base()
        {
            DoubleClick += new EventHandler(TimeLineControl_DoubleClick);
            MouseDown += new MouseEventHandler(StaticTimeLineControl_MouseDown);
            TestCase = null;
        }

        /// <summary>
        /// A base menu item element
        /// </summary>
        private class BaseToolStripButton : MenuItem
        {
            /// <summary>
            /// The selected event, if any
            /// </summary>
            protected ModelEvent Selected { get; private set; }

            /// <summary>
            /// The test case in which the action occurs
            /// </summary>
            protected TestCase TestCase { get; private set; }

            /// <summary>
            /// The time line control for which this menu item is built
            /// </summary>
            private StaticTimeLineControl TimeLineControl { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="timeLineControl"></param>
            /// <param name="modelEvent"></param>
            /// <param name="testCase"></param>
            /// <param name="caption"></param>
            public BaseToolStripButton(StaticTimeLineControl timeLineControl, ModelEvent modelEvent, TestCase testCase, string caption)
                : base(caption)
            {
                TimeLineControl = timeLineControl;
                Selected = modelEvent;
                TestCase = testCase;
            }

            /// <summary>
            /// Provides the step enclosing the selected event
            /// </summary>
            public Step Step
            {
                get
                {
                    Step retVal = null;

                    if (Selected != null)
                    {
                        retVal = EnclosingFinder<Step>.find(Selected.Instance as IEnclosed, true); ;
                    }

                    return retVal;
                }
            }

            /// <summary>
            /// Provides the substep enclosing the selected event
            /// </summary>
            public SubStep SubStep
            {
                get
                {
                    SubStep retVal = null;

                    if (Selected != null)
                    {
                        retVal = EnclosingFinder<SubStep>.find(Selected.Instance as IEnclosed, true); ;
                    }

                    return retVal;
                }
            }

            /// <summary>
            /// Refreshes the time line after click action has been performed
            /// </summary>
            /// <param name="e"></param>
            protected override void OnClick(EventArgs e)
            {
                TimeLineControl.HandledEvents = -1;
                TimeLineControl.Window.RefreshModel();
                TimeLineControl.Window.Refresh();
                base.OnClick(e);
            }
        }

        /// <summary>
        /// Deletes the current element
        /// </summary>
        private class DeleteMenuItem : BaseToolStripButton
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="timeLineControl"></param>
            /// <param name="modelEvent"></param>
            /// <param name="testCase"></param>
            public DeleteMenuItem(StaticTimeLineControl timeLineControl, ModelEvent modelEvent, TestCase testCase)
                : base(timeLineControl, modelEvent, testCase, "Delete")
            {
                Enabled = modelEvent != null;
            }

            /// <summary>
            /// Executes the action
            /// </summary>
            /// <param name="e"></param>
            protected override void OnClick(EventArgs e)
            {
                if (Selected != null && Selected.Instance != null)
                {
                    IModelElement model = Selected.Instance as IModelElement;
                    ArrayList collection = model.EnclosingCollection;
                    if (collection != null)
                    {
                        collection.Remove(model);
                    }
                }
                base.OnClick(e);
            }
        }

        /// <summary>
        /// Adds a step after the selected step
        /// </summary>
        private class AddStepMenuItem : BaseToolStripButton
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="timeLineControl"></param>
            /// <param name="modelEvent"></param>
            /// <param name="testCase"></param>
            public AddStepMenuItem(StaticTimeLineControl timeLineControl, ModelEvent modelEvent, TestCase testCase)
                : base(timeLineControl, modelEvent, testCase, "Add step")
            {
            }

            /// <summary>
            /// Executes the action
            /// </summary>
            /// <param name="e"></param>
            protected override void OnClick(EventArgs e)
            {
                Step newStep = (Step)DataDictionary.Generated.acceptor.getFactory().createStep();
                newStep.Enclosing = TestCase;

                SubStep subStep = (SubStep)DataDictionary.Generated.acceptor.getFactory().createSubStep();
                subStep.Name = "Substep 1";
                newStep.appendSubSteps(subStep);

                if (Step != null)
                {
                    newStep.Name = "NewStep";
                    int index = TestCase.Steps.IndexOf(Step);
                    TestCase.Steps.Insert(index + 1, newStep);
                }
                else
                {
                    newStep.Name = "Step " + (TestCase.Steps.Count + 1);
                    TestCase.Steps.Add(newStep);
                }
                base.OnClick(e);
            }
        }

        /// <summary>
        /// Adds a step after the selected step
        /// </summary>
        private class AddSubStepMenuItem : BaseToolStripButton
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="timeLineControl"></param>
            /// <param name="modelEvent"></param>
            /// <param name="testCase"></param>
            public AddSubStepMenuItem(StaticTimeLineControl timeLineControl, ModelEvent modelEvent, TestCase testCase)
                : base(timeLineControl, modelEvent, testCase, "Add substep")
            {
                Enabled = Step != null;
            }

            /// <summary>
            /// Executes the action
            /// </summary>
            /// <param name="e"></param>
            protected override void OnClick(EventArgs e)
            {
                SubStep newSubStep = (SubStep)DataDictionary.Generated.acceptor.getFactory().createSubStep();
                newSubStep.Enclosing = Step;

                if (SubStep != null)
                {
                    newSubStep.Name = "NewSubStep";
                    int index = Step.SubSteps.IndexOf(SubStep);
                    Step.SubSteps.Insert(index + 1, newSubStep);
                }
                else
                {
                    newSubStep.Name = "SubStep " + (Step.SubSteps.Count + 1);
                    Step.SubSteps.Add(newSubStep);
                }
                base.OnClick(e);
            }
        }

        /// <summary>
        /// Adds a step after the selected step
        /// </summary>
        private class AddChangeMenuItem : BaseToolStripButton
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="timeLineControl"></param>
            /// <param name="modelEvent"></param>
            /// <param name="testCase"></param>
            public AddChangeMenuItem(StaticTimeLineControl timeLineControl, ModelEvent modelEvent, TestCase testCase)
                : base(timeLineControl, modelEvent, testCase, "Add change")
            {
                Enabled = SubStep != null;
            }

            /// <summary>
            /// Executes the action
            /// </summary>
            /// <param name="e"></param>
            protected override void OnClick(EventArgs e)
            {
                if (SubStep != null)
                {
                    DataDictionary.Rules.Action action = (DataDictionary.Rules.Action)DataDictionary.Generated.acceptor.getFactory().createAction();
                    action.Name = "";
                    SubStep.appendActions(action);
                }
                base.OnClick(e);
            }
        }

        /// <summary>
        /// Adds a step after the selected step
        /// </summary>
        private class AddExpectationMenuItem : BaseToolStripButton
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="timeLineControl"></param>
            /// <param name="modelEvent"></param>
            /// <param name="testCase"></param>
            public AddExpectationMenuItem(StaticTimeLineControl timeLineControl, ModelEvent modelEvent, TestCase testCase)
                : base(timeLineControl, modelEvent, testCase, "Add expectation")
            {
                Enabled = SubStep != null;
            }

            /// <summary>
            /// Executes the action
            /// </summary>
            /// <param name="e"></param>
            protected override void OnClick(EventArgs e)
            {
                if (SubStep != null)
                {
                    DataDictionary.Tests.Expectation expectation = (DataDictionary.Tests.Expectation)DataDictionary.Generated.acceptor.getFactory().createExpectation();
                    expectation.Name = "";
                    SubStep.appendExpectations(expectation);
                }
                base.OnClick(e);
            }
        }

        /// <summary>
        /// Updates the contextual menu according to the position of the mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StaticTimeLineControl_MouseDown(object sender, MouseEventArgs e)
        {
            ContextMenu = new ContextMenu();

            ModelEvent evt = GetEventUnderMouse((MouseEventArgs)e);
            ContextMenu.MenuItems.Add(new AddStepMenuItem(this, evt, TestCase));
            ContextMenu.MenuItems.Add(new AddSubStepMenuItem(this, evt, TestCase));
            ContextMenu.MenuItems.Add(new AddChangeMenuItem(this, evt, TestCase));
            ContextMenu.MenuItems.Add(new AddExpectationMenuItem(this, evt, TestCase));
            ContextMenu.MenuItems.Add("-");
            ContextMenu.MenuItems.Add(new DeleteMenuItem(this, evt, TestCase));
        }

        /// <summary>
        /// Sets the string value into the right property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        private class IExpressionableTextChangeHandler : EditorForm.HandleTextChange
        {
            /// <summary>
            /// The time line control
            /// </summary>
            private StaticTimeLineControl TimeLine { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instance"></param>
            public IExpressionableTextChangeHandler(StaticTimeLineControl timeLine, IExpressionable instance)
                : base(instance as DataDictionary.ModelElement)
            {
                TimeLine = timeLine;
            }

            /// <summary>
            /// The way text is retrieved from the instance
            /// </summary>
            /// <returns></returns>
            public override string GetText()
            {
                string retVal = "";
                IExpressionable expressionable = Instance as IExpressionable;

                if (expressionable != null)
                {
                    retVal = expressionable.ExpressionText;
                }
                return retVal;
            }

            /// <summary>
            /// The way text is set back in the instance
            /// </summary>
            /// <returns></returns>
            public override void SetText(string text)
            {
                IExpressionable expressionable = Instance as IExpressionable;

                if (expressionable != null)
                {
                    expressionable.ExpressionText = text;
                }
                TimeLine.Refresh();
            }
        }

        /// <summary>
        /// Handles a double click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeLineControl_DoubleClick(object sender, EventArgs e)
        {
            ModelEvent evt = GetEventUnderMouse((MouseEventArgs)e);

            VariableUpdate variableUpdate = evt as VariableUpdate;
            if (variableUpdate != null)
            {
                EditorForm form = new EditorForm();
                IExpressionableTextChangeHandler handler = new IExpressionableTextChangeHandler(this, variableUpdate.Action);
                form.setChangeHandler(handler);
                GUIUtils.MDIWindow.AddChildWindow(form, WeifenLuo.WinFormsUI.Docking.DockAreas.Float);
            }

            Expect expect = evt as Expect;
            if (expect != null)
            {
                EditorForm form = new EditorForm();
                IExpressionableTextChangeHandler handler = new IExpressionableTextChangeHandler(this, expect.Expectation);
                form.setChangeHandler(handler);
                GUIUtils.MDIWindow.AddChildWindow(form, WeifenLuo.WinFormsUI.Docking.DockAreas.Float);
            }
        }
        /// <summary>
        /// Refreshes the view according to the test case
        /// </summary>
        public override void Refresh()
        {
            if (TestCase != null && TestCase.ActionCount != HandledEvents)
            {
                UpdatePositionHandler();
                UpdatePanelSize();
                HandledEvents = TestCase.ActionCount;
            }
            base.Refresh();
        }

        /// <summary>
        /// Update the information stored in the position handler according to the test case
        /// </summary>
        protected override void UpdatePositionHandler()
        {
            PositionHandler.CleanPositions();
            if (TestCase != null)
            {
                double currentTime = 0.0;
                foreach (Step step in TestCase.Steps)
                {
                    if (step.SubSteps.Count > 0)
                    {
                        foreach (SubStep subStep in step.SubSteps)
                        {
                            SubStepActivated subStepActivated = new SubStepActivated(subStep, null);
                            subStepActivated.Time = currentTime;
                            PositionHandler.RegisterEvent(subStepActivated);
                            foreach (DataDictionary.Rules.Action action in subStep.Actions)
                            {
                                VariableUpdate variableUpdate = new VariableUpdate(action, action, null);
                                PositionHandler.RegisterEvent(variableUpdate);
                            }
                            foreach (Expectation expectation in subStep.Expectations)
                            {
                                Expect expect = new Expect(expectation, null);
                                PositionHandler.RegisterEvent(expect);
                            }

                            currentTime += 1;
                        }
                    }
                    else
                    {
                        StepActivation stepActivated = new StepActivation(step);
                        stepActivated.Time = currentTime;
                        PositionHandler.RegisterEvent(stepActivated);
                        currentTime += 1;
                    }
                }
            }
        }

    }
}
