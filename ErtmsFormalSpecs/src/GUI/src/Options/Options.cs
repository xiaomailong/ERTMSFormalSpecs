using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI.Options
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setup the option panel according to the configuration stored in the system
        /// </summary>
        /// <param name="system"></param>
        public void Setup(DataDictionary.EFSSystem system)
        {
            displayEnclosingMessagesCheckBox.Checked = system.DisplayEnclosingMessages;
            displayRequirementsAsListCheckBox.Checked = system.DisplayRequirementsAsList;
        }

        /// <summary>
        /// Updates the configuration stored in the system according to the option panel
        /// </summary>
        /// <param name="system"></param>
        public void UpdateSystem(DataDictionary.EFSSystem system)
        {
            system.DisplayEnclosingMessages = displayEnclosingMessagesCheckBox.Checked;
            system.DisplayRequirementsAsList = displayRequirementsAsListCheckBox.Checked;
        }
    }
}
