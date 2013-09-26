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
namespace GUI.DataDictionaryView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.nextErrortoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.nextWarningToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.nextInfoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.descriptionTabPage = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.usageTabPage = new System.Windows.Forms.TabPage();
            this.messagesTabPage = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataDictTree = new GUI.DataDictionaryView.DataDictionaryTreeView();
            this.dataDictPropertyGrid = new GUI.MyPropertyGrid();
            this.requirementsTextBox = new GUI.EditorTextBox();
            this.ruleExplainTextBox = new GUI.ExplainTextBox();
            this.stateDiagramPanel = new GUI.StateDiagram.StatePanel(this.components);
            this.usageTreeView = new GUI.DataDictionaryView.UsageTreeView.UsageTreeView();
            this.messagesRichTextBox = new GUI.EditorTextBox();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.descriptionTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.usageTabPage.SuspendLayout();
            this.messagesTabPage.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.nextErrortoolStripButton,
            this.nextWarningToolStripButton,
            this.nextInfoToolStripButton,
            this.toolStripSeparator1});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(926, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(87, 22);
            this.toolStripLabel1.Text = "Data dictionary";
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            this.splitContainer1.Panel1.Controls.Add(this.dataDictTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(926, 526);
            this.splitContainer1.SplitterDistance = 221;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataDictPropertyGrid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl);
            this.splitContainer2.Size = new System.Drawing.Size(702, 526);
            this.splitContainer2.SplitterDistance = 213;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 5;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.descriptionTabPage);
            this.tabControl.Controls.Add(this.usageTabPage);
            this.tabControl.Controls.Add(this.messagesTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(702, 310);
            this.tabControl.TabIndex = 1;
            // 
            // descriptionTabPage
            // 
            this.descriptionTabPage.Controls.Add(this.splitContainer3);
            this.descriptionTabPage.Location = new System.Drawing.Point(4, 22);
            this.descriptionTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.descriptionTabPage.Name = "descriptionTabPage";
            this.descriptionTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.descriptionTabPage.Size = new System.Drawing.Size(694, 284);
            this.descriptionTabPage.TabIndex = 4;
            this.descriptionTabPage.Text = "Description";
            this.descriptionTabPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(2, 2);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.requirementsTextBox);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.ruleExplainTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.stateDiagramPanel);
            this.splitContainer3.Size = new System.Drawing.Size(690, 280);
            this.splitContainer3.SplitterDistance = 341;
            this.splitContainer3.SplitterWidth = 3;
            this.splitContainer3.TabIndex = 0;
            // 
            // usageTabPage
            // 
            this.usageTabPage.Controls.Add(this.usageTreeView);
            this.usageTabPage.Location = new System.Drawing.Point(4, 22);
            this.usageTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.usageTabPage.Name = "usageTabPage";
            this.usageTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.usageTabPage.Size = new System.Drawing.Size(694, 284);
            this.usageTabPage.TabIndex = 3;
            this.usageTabPage.Text = "Usage";
            this.usageTabPage.UseVisualStyleBackColor = true;
            // 
            // messagesTabPage
            // 
            this.messagesTabPage.Controls.Add(this.messagesRichTextBox);
            this.messagesTabPage.Location = new System.Drawing.Point(4, 22);
            this.messagesTabPage.Name = "messagesTabPage";
            this.messagesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.messagesTabPage.Size = new System.Drawing.Size(694, 284);
            this.messagesTabPage.TabIndex = 5;
            this.messagesTabPage.Text = "Messages";
            this.messagesTabPage.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 551);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(926, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // dataDictTree
            // 
            this.dataDictTree.AllowDrop = true;
            this.dataDictTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataDictTree.HideSelection = false;
            this.dataDictTree.ImageIndex = 0;
            this.dataDictTree.LabelEdit = true;
            this.dataDictTree.Location = new System.Drawing.Point(0, 0);
            this.dataDictTree.Name = "dataDictTree";
            this.dataDictTree.Root = null;
            this.dataDictTree.Selected = null;
            this.dataDictTree.SelectedImageIndex = 0;
            this.dataDictTree.Size = new System.Drawing.Size(221, 526);
            this.dataDictTree.TabIndex = 3;
            // 
            // dataDictPropertyGrid
            // 
            this.dataDictPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataDictPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.dataDictPropertyGrid.Name = "dataDictPropertyGrid";
            this.dataDictPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.dataDictPropertyGrid.Size = new System.Drawing.Size(702, 213);
            this.dataDictPropertyGrid.TabIndex = 4;
            // 
            // requirementsTextBox
            // 
            this.requirementsTextBox.AllowDrop = true;
            this.requirementsTextBox.AutoComplete = true;
            this.requirementsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requirementsTextBox.Lines = new string[] {
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
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
            this.requirementsTextBox.Location = new System.Drawing.Point(0, 0);
            this.requirementsTextBox.Name = "requirementsTextBox";
            this.requirementsTextBox.ReadOnly = false;
            this.requirementsTextBox.Rtf = resources.GetString("requirementsTextBox.Rtf");
            this.requirementsTextBox.Size = new System.Drawing.Size(341, 280);
            this.requirementsTextBox.TabIndex = 22;
            // 
            // ruleExplainTextBox
            // 
            this.ruleExplainTextBox.AllowDrop = true;
            this.ruleExplainTextBox.AutoComplete = true;
            this.ruleExplainTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleExplainTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ruleExplainTextBox.Lines = new string[] {
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
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
            this.ruleExplainTextBox.Location = new System.Drawing.Point(0, 0);
            this.ruleExplainTextBox.Name = "ruleExplainTextBox";
            this.ruleExplainTextBox.ReadOnly = true;
            this.ruleExplainTextBox.Rtf = resources.GetString("ruleExplainTextBox.Rtf");
            this.ruleExplainTextBox.Size = new System.Drawing.Size(346, 280);
            this.ruleExplainTextBox.TabIndex = 24;
            // 
            // stateDiagramPanel
            // 
            this.stateDiagramPanel.AutoScroll = true;
            this.stateDiagramPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateDiagramPanel.Location = new System.Drawing.Point(0, 0);
            this.stateDiagramPanel.Name = "stateDiagramPanel";
            this.stateDiagramPanel.Size = new System.Drawing.Size(346, 280);
            this.stateDiagramPanel.StateMachine = null;
            this.stateDiagramPanel.StateMachineVariable = null;
            this.stateDiagramPanel.TabIndex = 0;
            this.stateDiagramPanel.Visible = false;
            // 
            // usageTreeView
            // 
            this.usageTreeView.AllowDrop = true;
            this.usageTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usageTreeView.HideSelection = false;
            this.usageTreeView.ImageIndex = 0;
            this.usageTreeView.LabelEdit = true;
            this.usageTreeView.Location = new System.Drawing.Point(2, 2);
            this.usageTreeView.Margin = new System.Windows.Forms.Padding(2);
            this.usageTreeView.Name = "usageTreeView";
            this.usageTreeView.Root = null;
            this.usageTreeView.Selected = null;
            this.usageTreeView.SelectedImageIndex = 0;
            this.usageTreeView.Size = new System.Drawing.Size(690, 280);
            this.usageTreeView.TabIndex = 0;
            // 
            // messagesRichTextBox
            // 
            this.messagesRichTextBox.AutoComplete = true;
            this.messagesRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesRichTextBox.Lines = new string[] {
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
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
            this.messagesRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.messagesRichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.messagesRichTextBox.Name = "messagesRichTextBox";
            this.messagesRichTextBox.ReadOnly = false;
            this.messagesRichTextBox.Rtf = resources.GetString("messagesRichTextBox.Rtf");
            this.messagesRichTextBox.Size = new System.Drawing.Size(688, 278);
            this.messagesRichTextBox.TabIndex = 1;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(926, 573);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Window";
            this.ShowInTaskbar = false;
            this.Text = "Data Dictionary";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.descriptionTabPage.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.usageTabPage.ResumeLayout(false);
            this.messagesTabPage.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private DataDictionaryTreeView dataDictTree;
        private MyPropertyGrid dataDictPropertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabPage usageTabPage;
        public UsageTreeView.UsageTreeView usageTreeView;
        private System.Windows.Forms.TabPage descriptionTabPage;
        private System.Windows.Forms.SplitContainer splitContainer3;
        public EditorTextBox requirementsTextBox;
        public ExplainTextBox ruleExplainTextBox;
        public StateDiagram.StatePanel stateDiagramPanel;
        private System.Windows.Forms.ToolStripButton nextErrortoolStripButton;
        private System.Windows.Forms.ToolStripButton nextWarningToolStripButton;
        private System.Windows.Forms.ToolStripButton nextInfoToolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private EditorTextBox messagesRichTextBox;
        private System.Windows.Forms.TabPage messagesTabPage;
    }
}