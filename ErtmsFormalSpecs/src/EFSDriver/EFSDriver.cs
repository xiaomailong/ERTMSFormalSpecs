using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFSDriver.EFSService;

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
        }
    }
}
