namespace EFSDriver
{
    partial class EFSDriver
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
            this.cycleButton = new System.Windows.Forms.Button();
            this.variableNameLabel = new System.Windows.Forms.Label();
            this.variableNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.variableValueTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cycleButton
            // 
            this.cycleButton.Location = new System.Drawing.Point(216, 287);
            this.cycleButton.Name = "cycleButton";
            this.cycleButton.Size = new System.Drawing.Size(106, 55);
            this.cycleButton.TabIndex = 0;
            this.cycleButton.Text = "Cycle";
            this.cycleButton.UseVisualStyleBackColor = true;
            this.cycleButton.Click += new System.EventHandler(this.cycleButton_Click);
            // 
            // variableNameLabel
            // 
            this.variableNameLabel.AutoSize = true;
            this.variableNameLabel.Location = new System.Drawing.Point(12, 16);
            this.variableNameLabel.Name = "variableNameLabel";
            this.variableNameLabel.Size = new System.Drawing.Size(76, 13);
            this.variableNameLabel.TabIndex = 1;
            this.variableNameLabel.Text = "Variable Name";
            // 
            // variableNameTextBox
            // 
            this.variableNameTextBox.Location = new System.Drawing.Point(134, 13);
            this.variableNameTextBox.Name = "variableNameTextBox";
            this.variableNameTextBox.Size = new System.Drawing.Size(339, 20);
            this.variableNameTextBox.TabIndex = 2;
            this.variableNameTextBox.TextChanged += new System.EventHandler(this.variableNameTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Variable Value";
            // 
            // variableValueTextBox
            // 
            this.variableValueTextBox.Location = new System.Drawing.Point(134, 39);
            this.variableValueTextBox.Name = "variableValueTextBox";
            this.variableValueTextBox.Size = new System.Drawing.Size(339, 20);
            this.variableValueTextBox.TabIndex = 4;
            this.variableValueTextBox.TextChanged += new System.EventHandler(this.variableValueTextBox_TextChanged);
            // 
            // EFSDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 354);
            this.Controls.Add(this.variableValueTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.variableNameTextBox);
            this.Controls.Add(this.variableNameLabel);
            this.Controls.Add(this.cycleButton);
            this.Name = "EFSDriver";
            this.Text = "EFSDriver";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cycleButton;
        private System.Windows.Forms.Label variableNameLabel;
        private System.Windows.Forms.TextBox variableNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox variableValueTextBox;
    }
}

