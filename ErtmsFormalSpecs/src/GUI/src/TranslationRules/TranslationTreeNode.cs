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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DataDictionary.Tests;
using DataDictionary.Tests.Translations;
using System.Drawing.Design;


namespace GUI.TranslationRules
{
    public class TranslationTreeNode : ReferencesParagraphTreeNode<DataDictionary.Tests.Translations.Translation>
    {
        private class ItemEditor : ReferencesParagraphEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }

            /// <summary>
            /// The step name
            /// </summary>
            [Category("Description")]
            public bool Implemented
            {
                get { return Item.getImplemented(); }
                set { Item.setImplemented(value); }
            }
        }

        SourceTextsTreeNode sources;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public TranslationTreeNode(Translation item, bool buildSubNodes)
            : base(item, buildSubNodes)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            base.BuildSubNodes(buildSubNodes);

            sources = new SourceTextsTreeNode(Item, buildSubNodes);
            Nodes.Add(sources);

            foreach (SubStep subStep in Item.SubSteps)
            {
                Nodes.Add(new TestRunnerView.SubStepTreeNode(subStep, buildSubNodes));
            }
        }

        /// <summary>
        /// Creates the editor for this tree node
        /// </summary>
        /// <returns></returns>
        protected override Editor createEditor()
        {
            return new ItemEditor();
        }

        /// <summary>
        /// Creates a new source text
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SourceTextTreeNode createSourceText(SourceText sourceText)
        {
            return sources.createSourceText(sourceText);
        }

        public void AddSourceHandler(object sender, EventArgs args)
        {
            sources.AddHandler(sender, args);
        }

        /// <summary>
        /// Creates a new sub-step
        /// </summary>
        /// <param name="testCase"></param>
        /// <returns></returns>
        public TestRunnerView.SubStepTreeNode createSubStep(DataDictionary.Tests.SubStep subStep)
        {
            TestRunnerView.SubStepTreeNode retVal = new TestRunnerView.SubStepTreeNode(subStep, true);

            Item.appendSubSteps(subStep);
            Nodes.Add(retVal);

            return retVal;
        }

        /// <summary>
        /// Adds a step after this one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AddSubStepHandler(object sender, EventArgs args)
        {
            DataDictionary.Tests.SubStep subStep = (DataDictionary.Tests.SubStep)DataDictionary.Generated.acceptor.getFactory().createSubStep();
            subStep.Name = "Sub-step" + Nodes.Count;
            subStep.Enclosing = Item;
            createSubStep(subStep);
        }

        /// <summary>
        /// Finds all steps that are translated using a specific translation rule
        /// </summary>
        private class MarkUsageVisitor : DataDictionary.Generated.Visitor
        {
            /// <summary>
            /// The translation to be found
            /// </summary>
            public Translation Translation { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="translation"></param>
            public MarkUsageVisitor(Translation translation)
            {
                Translation = translation;
            }

            public override void  visit(DataDictionary.Generated.Step obj, bool visitSubNodes)
            {
                Step step = (Step) obj;

                if ( Translation == Translation.TranslationDictionary.findTranslation(step.getDescription()))
                {
                    step.AddInfo("Translation "+Translation.Name+" used");
                }

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        /// Marks all steps that use this translation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MarkUsageHandler(object sender, EventArgs args)
        {
            GUIUtils.MDIWindow.ClearMarks();
            MarkUsageVisitor finder = new MarkUsageVisitor(Item);
            foreach ( DataDictionary.Dictionary dictionary in DataDictionary.EFSSystem.INSTANCE.Dictionaries)
            {
                finder.visit(dictionary);
            }
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add source text", new EventHandler(AddSourceHandler)));
            retVal.Add(new MenuItem("Add sub-step", new EventHandler(AddSubStepHandler)));
            retVal.Add(new MenuItem("-"));
            retVal.Add(new MenuItem("Mark usages", new EventHandler(MarkUsageHandler)));
            retVal.Add(new MenuItem("-"));
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));

            return retVal;
        }

        /// <summary>
        /// Handles drop event
        /// </summary>
        /// <param name="SourceNode"></param>
        public override void AcceptDrop(BaseTreeNode SourceNode)
        {
            base.AcceptDrop(SourceNode);
            AcceptDropForTranslation(this, SourceNode);
        }

        /// <summary>
        /// Accepts the drop event
        /// </summary>
        /// <param name="translationTreeNode"></param>
        /// <param name="SourceNode"></param>
        public static void AcceptDropForTranslation(TranslationTreeNode translationTreeNode, BaseTreeNode SourceNode)
        {
            if (SourceNode is SourceTextTreeNode)
            {
                SourceTextTreeNode text = SourceNode as SourceTextTreeNode;

                DataDictionary.Tests.Translations.SourceText otherText = (DataDictionary.Tests.Translations.SourceText)DataDictionary.Generated.acceptor.getFactory().createSourceText();
                text.Item.copyTo(otherText);
                translationTreeNode.createSourceText(otherText);
                text.Delete();
            }
            else if (SourceNode is TestRunnerView.StepTreeNode)
            {
                TestRunnerView.StepTreeNode step = SourceNode as TestRunnerView.StepTreeNode;

                if (string.IsNullOrEmpty(step.Item.getDescription()))
                {
                    MessageBox.Show("Step has no description and cannot be automatically translated", "No description available", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DataDictionary.Tests.Translations.SourceText sourceText = (DataDictionary.Tests.Translations.SourceText)DataDictionary.Generated.acceptor.getFactory().createSourceText();
                    sourceText.Name = step.Item.getDescription();
                    translationTreeNode.createSourceText(sourceText);
                }
            }
        }

        /// <summary>
        /// Handles a selection change event
        /// </summary>
        /// <param name="displayStatistics">Indicates that statistics should be displayed in the MDI window</param>
        public override void SelectionChanged(bool displayStatistics)
        {
            base.SelectionChanged(displayStatistics);
            if (BaseTreeView != null && BaseTreeView.RefreshNodeContent)
            {
                IBaseForm baseForm = BaseForm;
                if (baseForm != null)
                {
                    if (baseForm.RequirementsTextBox != null)
                    {
                        baseForm.RequirementsTextBox.Text = Item.getSourceTextExplain();
                    }

                    Window window = baseForm as Window;
                    if (window != null)
                    {
                        window.SetSelection(Item);
                    }
                }
            }
        }
    }
}
