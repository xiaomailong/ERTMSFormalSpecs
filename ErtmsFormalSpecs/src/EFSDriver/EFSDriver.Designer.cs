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
            this.SuspendLayout();
            // 
            // cycleButton
            // 
            this.cycleButton.Location = new System.Drawing.Point(196, 181);
            this.cycleButton.Name = "cycleButton";
            this.cycleButton.Size = new System.Drawing.Size(75, 23);
            this.cycleButton.TabIndex = 0;
            this.cycleButton.Text = "Cycle";
            this.cycleButton.UseVisualStyleBackColor = true;
            this.cycleButton.Click += new System.EventHandler(this.cycleButton_Click);
            // 
            // EFSDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 354);
            this.Controls.Add(this.cycleButton);
            this.Name = "EFSDriver";
            this.Text = "EFSDriver";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cycleButton;
    }
}

