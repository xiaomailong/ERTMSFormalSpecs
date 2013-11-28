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
using System.Windows.Forms;

using Reports.Specs;
using Reports.Tests;
using DataDictionary.Specification;
using System.Collections;

namespace GUI.SpecificationView
{
    public partial class Window : BaseForm
    {
        public override MyPropertyGrid Properties
        {
            get { return propertyGrid; }
        }

        public RichTextBox ExpressionTextBox
        {
            get { return specBrowserTextView.TextBox; }
        }

        public override RichTextBox MessagesTextBox
        {
            get { return messagesRichTextBox.TextBox; }
        }

        public override BaseTreeView TreeView
        {
            get { return specBrowserTreeView; }
        }

        /// <summary>
        /// The rule set which is used to check the specifications
        /// </summary>
        private DataDictionary.Dictionary dictionary;
        public DataDictionary.Dictionary Dictionary
        {
            get { return dictionary; }
            private set
            {
                dictionary = value;
                specBrowserTreeView.Root = dictionary;
                Text = dictionary.Name + " specification view";
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="specification"></param>
        public Window(DataDictionary.Dictionary dictionary)
        {
            InitializeComponent();

            specBrowserTextView.AutoComplete = false;
            messagesRichTextBox.AutoComplete = false;

            specBrowserTextView.TextBox.TextChanged += new EventHandler(TextBox_TextChanged);
            FormClosed += new FormClosedEventHandler(Window_FormClosed);
            Visible = false;
            Dictionary = dictionary;
            Refresh();

            DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft;
        }

        void TextBox_TextChanged(object sender, EventArgs e)
        {
            Paragraph paragraph = Selected as Paragraph;
            if (paragraph != null)
            {
                if (paragraph.Text.CompareTo(specBrowserTextView.Text) != 0)
                {
                    paragraph.Text = specBrowserTextView.Text;
                }
            }
        }

        /// <summary>
        /// Handles the close event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            GUIUtils.MDIWindow.HandleSubWindowClosed(this);
        }

        public override void Refresh()
        {
            specBrowserTreeView.Refresh();
            int applicableCounter = Dictionary.ApplicableParagraphs.Count;
            int implementedCounter = SpecCoverageReport.CoveredRequirements(Dictionary, true).Count;
            int testedCounter = TestsCoverageReport.CoveredRequirements(Dictionary).Count;

            double percentageImplemented = (double)implementedCounter / (double)applicableCounter;
            double percentageTested = (double)testedCounter / (double)applicableCounter;
            specBrowserStatusLabel.Text = string.Format("{0} applicable paragraphs loaded. {1} ({2:P2}) implemented, {3} ({4:P2}) tested.", new object[] { applicableCounter, implementedCounter, percentageImplemented, testedCounter, percentageTested });
            base.Refresh();
        }

        /// <summary>
        /// Refreshes the model of the window
        /// </summary>
        public override void RefreshModel()
        {
            specBrowserTreeView.RefreshModel();
        }

        /// <summary>
        /// Selects the next node where error message is available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextErrortoolStripButton_Click(object sender, EventArgs e)
        {
            TreeView.SelectNext(Utils.ElementLog.LevelEnum.Error);
        }

        /// <summary>
        /// Selects the next node where warning message is available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextWarningToolStripButton_Click(object sender, EventArgs e)
        {
            TreeView.SelectNext(Utils.ElementLog.LevelEnum.Warning);
        }

        /// <summary>
        /// Selects the next node where info message is available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextInfoToolStripButton_Click(object sender, EventArgs e)
        {
            TreeView.SelectNext(Utils.ElementLog.LevelEnum.Info);
        }
    }
}
