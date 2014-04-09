using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFSService;

namespace EFSDriver
{
    public partial class EFSDriver : Form
    {
        /// <summary>
        /// Access to EFS Service
        /// </summary>
        private EFSServiceClient EFS { get; set; }

        public EFSDriver()
        {
            InitializeComponent();
            EFS = new EFSServiceClient();
        }

        private void cycleButton_Click(object sender, EventArgs e)
        {
            EFS.Cycle(Priority.Verification);
            EFS.Cycle(Priority.UpdateInternal);
            EFS.Cycle(Priority.Process);
            EFS.Cycle(Priority.UpdateOutput);
            EFS.Cycle(Priority.CleanUp);

            UpdateVariableValue();
        }

        private void variableNameTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateVariableValue();
        }

        /// <summary>
        /// Updates the value of the variable referenced by the variable name text box
        /// </summary>
        private void UpdateVariableValue()
        {
            Value value = EFS.GetVariableValue(variableNameTextBox.Text);
            DisplayVariableValue(value);
        }

        /// <summary>
        /// Indicates that changes in the UI should be propagated in the EFS system
        /// </summary>
        private bool Propagate = true;

        /// <summary>
        /// Displays the value of the variable provided
        /// </summary>
        /// <param name="value"></param>
        private void DisplayVariableValue(Value value)
        {
            try
            {
                Propagate = false;
                if (value != null)
                {
                    variableValueTextBox.Text = value.DisplayValue();
                }
                else
                {
                    variableValueTextBox.Text = "<empty>";
                }
            }
            finally
            {
                Propagate = true;
            }
        }

        private void variableValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Propagate)
            {
                IntValue value = EFS.GetVariableValue(variableNameTextBox.Text) as IntValue;
                if (value != null)
                {
                    int result;
                    if (int.TryParse(variableValueTextBox.Text, out result))
                    {
                        value.Value = result;
                        EFS.SetVariableValue(variableNameTextBox.Text, value);
                    }
                }
            }
        }
    }
}
