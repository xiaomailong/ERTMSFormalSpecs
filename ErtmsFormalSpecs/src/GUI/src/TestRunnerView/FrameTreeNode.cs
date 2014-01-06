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
using Utils;

namespace GUI.TestRunnerView
{
    public class FrameTreeNode : ModelElementTreeNode<DataDictionary.Tests.Frame>
    {
        /// <summary>
        /// The value editor
        /// </summary>
        private class ItemEditor : NamedEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }

            /// <summary>
            /// The variable that identifies the time in the current system
            /// </summary>
            [Category("Description")]
            public string CycleDuration
            {
                get { return Item.getCycleDuration(); }
                set { Item.setCycleDuration(value); }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public FrameTreeNode(DataDictionary.Tests.Frame item)
            : base(item, null, true)
        {
        }

        protected override void BuildSubNodes()
        {
            base.BuildSubNodes();

            foreach (DataDictionary.Tests.SubSequence subSequence in Item.SubSequences)
            {
                Nodes.Add(new SubSequenceTreeNode(subSequence));
            }
            SortSubNodes();
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
        /// Creates a new sub sequence in the corresponding frame
        /// </summary>
        /// <param name="subSequenceName"></param>
        /// <returns></returns>
        public SubSequenceTreeNode createSubSequence(DataDictionary.Tests.SubSequence subSequence)
        {
            SubSequenceTreeNode retVal;

            subSequence.Enclosing = Item;
            Item.appendSubSequences(subSequence);

            retVal = new SubSequenceTreeNode(subSequence);
            Nodes.Add(retVal);
            SortSubNodes();

            return retVal;
        }

        /// <summary>
        /// Translates the corresponding sub sequence, according to translation rules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void TranslateHandler(object sender, EventArgs args)
        {
            ApplyRulesOperation applyRulesOperation = new ApplyRulesOperation(Item);
            ProgressDialog progress = new ProgressDialog("Applying translation rules", applyRulesOperation);
            progress.ShowDialog(GUIUtils.MDIWindow);
            GUIUtils.MDIWindow.RefreshModel();
        }

        #region Apply rules
        private class ApplyRulesOperation : ProgressHandler
        {
            /// <summary>
            /// The frams on which the rules should be applied
            /// </summary>
            private DataDictionary.Tests.Frame Frame { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="frame"></param>
            public ApplyRulesOperation(DataDictionary.Tests.Frame frame)
            {
                Frame = frame;
            }

            /// <summary>
            /// Perform the work as a background task
            /// </summary>
            public override void ExecuteWork()
            {
                Utils.FinderRepository.INSTANCE.ClearCache();
                Frame.Translate(Frame.Dictionary.TranslationDictionary);
            }
        }
        #endregion

        public void AddHandler(object sender, EventArgs args)
        {
            createSubSequence(DataDictionary.Tests.SubSequence.createDefault("Sequence" + (GetNodeCount(false) + 1)));
        }

        private void ClearAll()
        {
            TestTreeView treeView = TreeView as TestTreeView;
            if (treeView != null)
            {
                Window window = treeView.ParentForm as Window;
                window.Clear();
            }
        }

        #region ExecuteTests
        private class ExecuteTestsOperation : ProgressHandler
        {
            /// <summary>
            /// The number of failed tests 
            /// </summary>
            public int Failed { get; private set; }

            /// <summary>
            /// Execution time span
            /// </summary>
            public TimeSpan Span { get; private set; }

            /// <summary>
            /// The window in which the tests are executed
            /// </summary>
            private Window Window { get; set; }

            /// <summary>
            /// The frame to test
            /// </summary>
            private DataDictionary.Tests.Frame Frame { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="window"></param>
            /// <param name="frame"></param>
            public ExecuteTestsOperation(Window window, DataDictionary.Tests.Frame frame)
            {
                Window = window;
                Frame = frame;
            }

            /// <summary>
            /// Executes the work in the background task
            /// </summary>
            /// <param name="arg"></param>
            public override void ExecuteWork()
            {
                DateTime start = DateTime.Now;

                SynchronizerList.SuspendSynchronization();
                if (Window != null)
                {
                    Window.setFrame(Frame);
                    Failed = Frame.ExecuteAllTests();
                }
                SynchronizerList.ResumeSynchronization();

                Span = DateTime.Now.Subtract(start);
            }
        }

        /// <summary>
        /// Handles a run event on this test case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void RunHandler(object sender, EventArgs args)
        {
            ClearAll();
            ClearMessages();

            ExecuteTestsOperation executeTestsOperation = new ExecuteTestsOperation(BaseForm as Window, Item);
            ProgressDialog dialog = new ProgressDialog("Executing test sequences", executeTestsOperation);
            dialog.ShowDialog();

            string runtimeErrors = "";
            if (Utils.ModelElement.Errors.Values.Count > 0)
            {
                runtimeErrors += "Errors were raised while executing sub sequences(s).\n";
            }
            System.Windows.Forms.MessageBox.Show(Item.SubSequences.Count + " sub sequence(s) executed, " + executeTestsOperation.Failed + " sub sequence(s) failed.\n" + runtimeErrors + "Test duration : " + Math.Round(executeTestsOperation.Span.TotalSeconds) + " seconds", "Execution report");
        }

        #endregion

        /// <summary>
        /// Handles a run event on this frame and creates the associated report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ReportHandler(object sender, EventArgs args)
        {
            Report.TestReport aReport = new Report.TestReport(Item);
            aReport.Show();
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add sub-sequence", new EventHandler(AddHandler)));
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
            retVal.AddRange(base.GetMenuItems());
            retVal.Insert(5, new MenuItem("-"));
            retVal.Insert(6, new MenuItem("Apply translation rules", new EventHandler(TranslateHandler)));
            retVal.Insert(7, new MenuItem("-"));
            retVal.Insert(8, new MenuItem("Execute", new EventHandler(RunHandler)));
            retVal.Insert(9, new MenuItem("Create report", new EventHandler(ReportHandler)));

            return retVal;
        }

        /// <summary>
        /// Finds the subsequence tree node which corresponds to the name provided
        /// </summary>
        /// <param name="subSequenceName"></param>
        /// <returns></returns>
        public SubSequenceTreeNode findSubSequence(string subSequenceName)
        {
            return findSubNode(subSequenceName) as SubSequenceTreeNode;
        }

        /// <summary>
        /// Handles the drop event
        /// </summary>
        /// <param name="SourceNode"></param>
        public override void AcceptDrop(BaseTreeNode SourceNode)
        {
            base.AcceptDrop(SourceNode);
            if (SourceNode is SubSequenceTreeNode)
            {
                SubSequenceTreeNode subSequence = SourceNode as SubSequenceTreeNode;
                subSequence.Delete();

                createSubSequence(subSequence.Item);
            }
        }
    }
}
