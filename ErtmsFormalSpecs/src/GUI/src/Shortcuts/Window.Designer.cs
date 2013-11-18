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
namespace GUI.Shortcuts
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.historyDataGridView = new System.Windows.Forms.DataGridView();
            this.HistoryGroupBox = new System.Windows.Forms.GroupBox();
            this.ShortcutsGroupBox = new System.Windows.Forms.GroupBox();
            this.shortcutTreeView = new GUI.Shortcuts.ShortcutTreeView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historyDataGridView)).BeginInit();
            this.HistoryGroupBox.SuspendLayout();
            this.ShortcutsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ShortcutsGroupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 254);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.HistoryGroupBox);
            this.splitContainer1.Size = new System.Drawing.Size(202, 508);
            this.splitContainer1.SplitterDistance = 254;
            this.splitContainer1.TabIndex = 1;
            // 
            // historyDataGridView
            // 
            this.historyDataGridView.AllowUserToAddRows = false;
            this.historyDataGridView.AllowUserToDeleteRows = false;
            this.historyDataGridView.AllowUserToOrderColumns = true;
            this.historyDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.historyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.historyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyDataGridView.Location = new System.Drawing.Point(3, 16);
            this.historyDataGridView.Name = "historyDataGridView";
            this.historyDataGridView.ReadOnly = true;
            this.historyDataGridView.Size = new System.Drawing.Size(196, 231);
            this.historyDataGridView.TabIndex = 0;
            // 
            // HistoryGroupBox
            // 
            this.HistoryGroupBox.Controls.Add(this.historyDataGridView);
            this.HistoryGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HistoryGroupBox.Location = new System.Drawing.Point(0, 0);
            this.HistoryGroupBox.Name = "HistoryGroupBox";
            this.HistoryGroupBox.Size = new System.Drawing.Size(202, 250);
            this.HistoryGroupBox.TabIndex = 1;
            this.HistoryGroupBox.TabStop = false;
            this.HistoryGroupBox.Text = "History";
            // 
            // ShortcutsGroupBox
            // 
            this.ShortcutsGroupBox.Controls.Add(this.shortcutTreeView);
            this.ShortcutsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShortcutsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ShortcutsGroupBox.Name = "ShortcutsGroupBox";
            this.ShortcutsGroupBox.Size = new System.Drawing.Size(202, 254);
            this.ShortcutsGroupBox.TabIndex = 2;
            this.ShortcutsGroupBox.TabStop = false;
            this.ShortcutsGroupBox.Text = "Shortcuts";
            // 
            // shortcutTreeView
            // 
            this.shortcutTreeView.AllowDrop = true;
            this.shortcutTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shortcutTreeView.HideSelection = false;
            this.shortcutTreeView.ImageIndex = 0;
            this.shortcutTreeView.LabelEdit = true;
            this.shortcutTreeView.Location = new System.Drawing.Point(3, 16);
            this.shortcutTreeView.Name = "shortcutTreeView";
            this.shortcutTreeView.Root = null;
            this.shortcutTreeView.Selected = null;
            this.shortcutTreeView.SelectedImageIndex = 0;
            this.shortcutTreeView.Size = new System.Drawing.Size(196, 235);
            this.shortcutTreeView.TabIndex = 1;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 508);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "Window";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Shortcuts";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.historyDataGridView)).EndInit();
            this.HistoryGroupBox.ResumeLayout(false);
            this.ShortcutsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ShortcutTreeView shortcutTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox ShortcutsGroupBox;
        private System.Windows.Forms.GroupBox HistoryGroupBox;
        private System.Windows.Forms.DataGridView historyDataGridView;
    }
}