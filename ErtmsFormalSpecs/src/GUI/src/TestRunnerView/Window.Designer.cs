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
namespace GUI.TestRunnerView
{
    partial class Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.testBrowserStatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.nextErrortoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.nextWarningToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.nextInfoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTimeTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.rewindButton = new System.Windows.Forms.ToolStripButton();
            this.RestartButton = new System.Windows.Forms.ToolStripButton();
            this.StepOnceButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.frameToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.subSequenceSelectorComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripCurrentStepTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.testBrowserTreeView = new GUI.TestRunnerView.TestTreeView();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid = new GUI.MyPropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.descriptionTabPage = new System.Windows.Forms.TabPage();
            this.explainTextBox = new GUI.ExplainTextBox();
            this.timeLineTabPage = new System.Windows.Forms.TabPage();
            this.evcTimeLineControl = new GUI.TestRunnerView.TimeLineControl.TimeLineControl();
            this.messagesTabPage = new System.Windows.Forms.TabPage();
            this.messageRichTextBox = new GUI.EditorTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.requirementsTextBox = new GUI.EditorTextBox();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.descriptionTabPage.SuspendLayout();
            this.timeLineTabPage.SuspendLayout();
            this.messagesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.toolStripSeparator1,
            this.testBrowserStatusLabel,
            this.toolStripSeparator2,
            this.nextErrortoolStripButton,
            this.nextWarningToolStripButton,
            this.nextInfoToolStripButton,
            this.toolStripSeparator4,
            this.toolStripLabel1,
            this.toolStripTimeTextBox,
            this.toolStripSeparator5,
            this.rewindButton,
            this.RestartButton,
            this.StepOnceButton,
            this.toolStripSeparator3,
            this.toolStripLabel5,
            this.frameToolStripComboBox,
            this.toolStripLabel2,
            this.subSequenceSelectorComboBox,
            this.toolStripLabel4,
            this.toolStripCurrentStepTextBox});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1492, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel3.Text = "Tests";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // testBrowserStatusLabel
            // 
            this.testBrowserStatusLabel.Name = "testBrowserStatusLabel";
            this.testBrowserStatusLabel.Size = new System.Drawing.Size(86, 22);
            this.testBrowserStatusLabel.Text = "toolStripLabel1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // nextErrortoolStripButton
            // 
            this.nextErrortoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextErrortoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("nextErrortoolStripButton.Image")));
            this.nextErrortoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextErrortoolStripButton.Name = "nextErrortoolStripButton";
            this.nextErrortoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.nextErrortoolStripButton.Text = "Next error";
            this.nextErrortoolStripButton.Click += new System.EventHandler(this.nextErrortoolStripButton_Click);
            // 
            // nextWarningToolStripButton
            // 
            this.nextWarningToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextWarningToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("nextWarningToolStripButton.Image")));
            this.nextWarningToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextWarningToolStripButton.Name = "nextWarningToolStripButton";
            this.nextWarningToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.nextWarningToolStripButton.Text = "Next warning";
            this.nextWarningToolStripButton.Click += new System.EventHandler(this.nextWarningToolStripButton_Click);
            // 
            // nextInfoToolStripButton
            // 
            this.nextInfoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextInfoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("nextInfoToolStripButton.Image")));
            this.nextInfoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextInfoToolStripButton.Name = "nextInfoToolStripButton";
            this.nextInfoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.nextInfoToolStripButton.Text = "Next info";
            this.nextInfoToolStripButton.Click += new System.EventHandler(this.nextInfoToolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel1.Text = "Time";
            // 
            // toolStripTimeTextBox
            // 
            this.toolStripTimeTextBox.Name = "toolStripTimeTextBox";
            this.toolStripTimeTextBox.ReadOnly = true;
            this.toolStripTimeTextBox.Size = new System.Drawing.Size(68, 25);
            this.toolStripTimeTextBox.Text = "0";
            this.toolStripTimeTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // rewindButton
            // 
            this.rewindButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rewindButton.Image = ((System.Drawing.Image)(resources.GetObject("rewindButton.Image")));
            this.rewindButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.rewindButton.Name = "rewindButton";
            this.rewindButton.Size = new System.Drawing.Size(23, 22);
            this.rewindButton.Text = "Step back";
            this.rewindButton.Click += new System.EventHandler(this.rewindButton_Click);
            // 
            // RestartButton
            // 
            this.RestartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RestartButton.Image = ((System.Drawing.Image)(resources.GetObject("RestartButton.Image")));
            this.RestartButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(23, 22);
            this.RestartButton.Text = "Restart";
            this.RestartButton.Click += new System.EventHandler(this.restart_Click);
            // 
            // StepOnceButton
            // 
            this.StepOnceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepOnceButton.Image = ((System.Drawing.Image)(resources.GetObject("StepOnceButton.Image")));
            this.StepOnceButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.StepOnceButton.Name = "StepOnceButton";
            this.StepOnceButton.Size = new System.Drawing.Size(23, 22);
            this.StepOnceButton.Text = "Step once";
            this.StepOnceButton.Click += new System.EventHandler(this.stepOnce_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(40, 22);
            this.toolStripLabel5.Text = "Frame";
            // 
            // frameToolStripComboBox
            // 
            this.frameToolStripComboBox.Name = "frameToolStripComboBox";
            this.frameToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.frameToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.frameSelectorComboBox_SelectionChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(80, 22);
            this.toolStripLabel2.Text = "Sub sequence";
            this.toolStripLabel2.Click += new System.EventHandler(this.toolStripLabel2_Click);
            // 
            // subSequenceSelectorComboBox
            // 
            this.subSequenceSelectorComboBox.Name = "subSequenceSelectorComboBox";
            this.subSequenceSelectorComboBox.Size = new System.Drawing.Size(201, 25);
            this.subSequenceSelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.testCaseSelectorComboBox_SelectionChanged);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(73, 22);
            this.toolStripLabel4.Text = "Current Step";
            this.toolStripLabel4.Click += new System.EventHandler(this.toolStripLabel4_Click);
            // 
            // toolStripCurrentStepTextBox
            // 
            this.toolStripCurrentStepTextBox.AutoToolTip = true;
            this.toolStripCurrentStepTextBox.Name = "toolStripCurrentStepTextBox";
            this.toolStripCurrentStepTextBox.ReadOnly = true;
            this.toolStripCurrentStepTextBox.Size = new System.Drawing.Size(501, 25);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.testBrowserTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer1.Size = new System.Drawing.Size(1492, 561);
            this.splitContainer1.SplitterDistance = 494;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 5;
            // 
            // testBrowserTreeView
            // 
            this.testBrowserTreeView.AllowDrop = true;
            this.testBrowserTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testBrowserTreeView.HideSelection = false;
            this.testBrowserTreeView.ImageIndex = 0;
            this.testBrowserTreeView.LabelEdit = true;
            this.testBrowserTreeView.Location = new System.Drawing.Point(0, 0);
            this.testBrowserTreeView.Name = "testBrowserTreeView";
            this.testBrowserTreeView.Root = null;
            this.testBrowserTreeView.Selected = null;
            this.testBrowserTreeView.SelectedImageIndex = 0;
            this.testBrowserTreeView.Size = new System.Drawing.Size(494, 561);
            this.testBrowserTreeView.TabIndex = 1;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.propertyGrid);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer5.Size = new System.Drawing.Size(995, 561);
            this.splitContainer5.SplitterDistance = 199;
            this.splitContainer5.SplitterWidth = 3;
            this.splitContainer5.TabIndex = 1;
            // 
            // propertyGrid
            // 
            this.propertyGrid.AllowDrop = true;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid.Size = new System.Drawing.Size(995, 199);
            this.propertyGrid.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.descriptionTabPage);
            this.tabControl1.Controls.Add(this.timeLineTabPage);
            this.tabControl1.Controls.Add(this.messagesTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(995, 359);
            this.tabControl1.TabIndex = 1;
            // 
            // descriptionTabPage
            // 
            this.descriptionTabPage.Controls.Add(this.splitContainer2);
            this.descriptionTabPage.Location = new System.Drawing.Point(4, 22);
            this.descriptionTabPage.Name = "descriptionTabPage";
            this.descriptionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.descriptionTabPage.Size = new System.Drawing.Size(987, 333);
            this.descriptionTabPage.TabIndex = 3;
            this.descriptionTabPage.Text = "Description";
            this.descriptionTabPage.UseVisualStyleBackColor = true;
            // 
            // explainTextBox
            // 
            this.explainTextBox.AutoComplete = true;
            this.explainTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.explainTextBox.Lines = new string[] {
        "",
        "",
        "",
        ""};
            this.explainTextBox.Location = new System.Drawing.Point(0, 0);
            this.explainTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.explainTextBox.Name = "explainTextBox";
            this.explainTextBox.ReadOnly = false;
            this.explainTextBox.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2060{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs24\\par\r\n\\fs17\\par\r\n\\par\r\n\\fs17\\par\r\n}\r\n";
            this.explainTextBox.Size = new System.Drawing.Size(446, 327);
            this.explainTextBox.TabIndex = 3;
            // 
            // timeLineTabPage
            // 
            this.timeLineTabPage.Controls.Add(this.evcTimeLineControl);
            this.timeLineTabPage.Location = new System.Drawing.Point(4, 22);
            this.timeLineTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.timeLineTabPage.Name = "timeLineTabPage";
            this.timeLineTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.timeLineTabPage.Size = new System.Drawing.Size(987, 333);
            this.timeLineTabPage.TabIndex = 0;
            this.timeLineTabPage.Text = "Time line";
            this.timeLineTabPage.UseVisualStyleBackColor = true;
            // 
            // evcTimeLineControl
            // 
            this.evcTimeLineControl.AutoScroll = true;
            this.evcTimeLineControl.AutoSize = true;
            this.evcTimeLineControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.evcTimeLineControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.evcTimeLineControl.Location = new System.Drawing.Point(2, 2);
            this.evcTimeLineControl.Name = "evcTimeLineControl";
            this.evcTimeLineControl.Size = new System.Drawing.Size(983, 329);
            this.evcTimeLineControl.TabIndex = 1;
            this.evcTimeLineControl.Text = "evcTimeLineControl1";
            // 
            // messagesTabPage
            // 
            this.messagesTabPage.Controls.Add(this.messageRichTextBox);
            this.messagesTabPage.Location = new System.Drawing.Point(4, 22);
            this.messagesTabPage.Name = "messagesTabPage";
            this.messagesTabPage.Size = new System.Drawing.Size(987, 333);
            this.messagesTabPage.TabIndex = 4;
            this.messagesTabPage.Text = "Messages";
            this.messagesTabPage.UseVisualStyleBackColor = true;
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.AutoComplete = true;
            this.messageRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageRichTextBox.Lines = new string[] {
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        ""};
            this.messageRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.ReadOnly = false;
            this.messageRichTextBox.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2060{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs24\\par\r\n\\par\r\n\\par\r\n\\fs17\\par\r\n\\par\r\n\\par" +
    "\r\n\\par\r\n\\par\r\n\\par\r\n\\par\r\n}\r\n";
            this.messageRichTextBox.Size = new System.Drawing.Size(987, 333);
            this.messageRichTextBox.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.requirementsTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.explainTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(981, 327);
            this.splitContainer2.SplitterDistance = 531;
            this.splitContainer2.TabIndex = 4;
            // 
            // requirementsTextBox
            // 
            this.requirementsTextBox.AutoComplete = true;
            this.requirementsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requirementsTextBox.Lines = new string[0];
            this.requirementsTextBox.Location = new System.Drawing.Point(0, 0);
            this.requirementsTextBox.Name = "requirementsTextBox";
            this.requirementsTextBox.ReadOnly = false;
            this.requirementsTextBox.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2060{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.requirementsTextBox.Size = new System.Drawing.Size(531, 327);
            this.requirementsTextBox.TabIndex = 0;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1492, 586);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip2);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Window";
            this.ShowInTaskbar = false;
            this.Text = "Test";
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.descriptionTabPage.ResumeLayout(false);
            this.timeLineTabPage.ResumeLayout(false);
            this.timeLineTabPage.PerformLayout();
            this.messagesTabPage.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel testBrowserStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private TimeLineControl.TimeLineControl evcTimeLineControl;
        private TestTreeView testBrowserTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private MyPropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStripTextBox toolStripTimeTextBox;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage timeLineTabPage;
        public GUI.ExplainTextBox explainTextBox;
        private System.Windows.Forms.ToolStripButton StepOnceButton;
        private System.Windows.Forms.ToolStripButton RestartButton;
        private System.Windows.Forms.ToolStripButton rewindButton;
        private System.Windows.Forms.ToolStripComboBox subSequenceSelectorComboBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox toolStripCurrentStepTextBox;
        private System.Windows.Forms.ToolStripButton nextInfoToolStripButton;
        private System.Windows.Forms.ToolStripButton nextWarningToolStripButton;
        private System.Windows.Forms.ToolStripButton nextErrortoolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox frameToolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.TabPage descriptionTabPage;
        private EditorTextBox messageRichTextBox;
        private System.Windows.Forms.TabPage messagesTabPage;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private EditorTextBox requirementsTextBox;

    }
}