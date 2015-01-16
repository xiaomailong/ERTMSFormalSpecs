using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataDictionary;
using GUI.Properties;

namespace GUI.Options
{
    public partial class Options : Form
    {
        private class SettingsEditor
        {
            [Category("Display")]
            [DisplayName("Enclosing messages")]
            [Description("Indicates that the enclosing messages should be displayed when selecting a model element")]
            public bool DisplayEnclosingMessages
            {
                get { return Settings.Default.DisplayEnclosingMessages; }
                set { Settings.Default.DisplayEnclosingMessages = value; }
            }

            [Category("Display")]
            [DisplayName("Requirements as list")]
            [Description("When set to true, indicates that the requirements should only be displayed as a list of number, instead of the requirement number followed by the requirement text")]
            public bool DisplayRequirementsAsList
            {
                get { return Settings.Default.DisplayRequirementsAsList; }
                set { Settings.Default.DisplayRequirementsAsList = value; }
            }

            [Category("Files")]
            [DisplayName("Lock opened files")]
            [Description("When set to true, indicates that the files opened by EFS should be locked, which forbid other processes to access them")]
            public bool LockOpenedFiles
            {
                get { return Settings.Default.LockOpenedFiles; }
                set { Settings.Default.LockOpenedFiles = value; }
            }

            [Category("Display")]
            [DisplayName("Display all variables in structure editor")]
            [Description("When set to true, indicates that all the variables should be displayed in the structure editor, even those which are empty")]
            public bool DisplayAllVariablesInStructureEditor
            {
                get { return Settings.Default.DisplayAllVariablesInStructureEditor; }
                set { Settings.Default.DisplayAllVariablesInStructureEditor = value; }
            }

            [Category("Behaviour")]
            [DisplayName("Check parent relationship")]
            [Description("When animating the model, verify the correctness of the 'parent' relation for each model element")]
            public bool CheckParentRelationship
            {
                get { return Settings.Default.CheckParentRelationship; }
                set { Settings.Default.CheckParentRelationship= value; }
            }
        }

        public Options()
        {
            InitializeComponent();
            propertyGrid.SelectedObject = new SettingsEditor();
        }

        /// <summary>
        /// Sets the settings according to the application data
        /// </summary>
        /// <param name="syste"></param>
        public static void setSettings(EFSSystem system)
        {
            Settings settings = Settings.Default;

            system.DisplayEnclosingMessages = settings.DisplayEnclosingMessages;
            system.DisplayRequirementsAsList = settings.DisplayRequirementsAsList;
            system.CheckParentRelationship = settings.CheckParentRelationship;
            DataDictionary.Util.PleaseLockFiles = settings.LockOpenedFiles;

            settings.Save();
        }
    }
}
