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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataDictionary;
using Utils;
using System.Threading;
using LibGit2Sharp;
using System.Diagnostics;

namespace GUI
{
    public partial class MainWindow : Form
    {
        /// <summary>
        /// The sub IBaseForms handled in this MDI
        /// </summary>
        private HashSet<IBaseForm> subWindows = new HashSet<IBaseForm>();

        public HashSet<IBaseForm> SubWindows
        {
            get
            {
                return subWindows;
            }
        }

        /// <summary>
        /// The Editors handled in this MDI
        /// </summary>
        private HashSet<EditorForm> editors = new HashSet<EditorForm>();

        public HashSet<EditorForm> Editors
        {
            get
            {
                return editors;
            }
        }

        /// <summary>
        /// Selects the model element in all opened sub windows
        /// </summary>
        /// <param name="model"></param>
        /// <param name="getFocus">Indicates whether the focus should be given to the enclosing form</param>
        public void Select(Utils.IModelElement model, bool getFocus = false)
        {
            if (model != null)
            {
                foreach (IBaseForm form in SubWindows)
                {
                    BaseTreeView treeView = form.TreeView;
                    if (treeView != null)
                    {
                        treeView.Select(model, getFocus);
                    }
                }
            }
        }

        public void HandleSubWindowClosed(Form form)
        {
            if (form is IBaseForm)
            {
                SubWindows.Remove((IBaseForm)form);
            }
            if (form is EditorForm)
            {
                Editors.Remove((EditorForm)form);
            }
        }

        /// <summary>
        /// Provides a data dictionary window
        /// </summary>
        public DataDictionaryView.Window DataDictionaryWindow
        {
            get
            {
                foreach (IBaseForm form in SubWindows)
                {
                    if (form is DataDictionaryView.Window)
                    {
                        return (DataDictionaryView.Window)form;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Provides a specification window
        /// </summary>
        public SpecificationView.Window SpecificationWindow
        {
            get
            {
                foreach (IBaseForm form in SubWindows)
                {
                    if (form is SpecificationView.Window)
                    {
                        return (SpecificationView.Window)form;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Provides a test runner window
        /// </summary>
        public TestRunnerView.Window TestWindow
        {
            get
            {
                foreach (IBaseForm form in SubWindows)
                {
                    if (form is TestRunnerView.Window)
                    {
                        return (TestRunnerView.Window)form;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// The translation window
        /// </summary>
        private TranslationRules.Window TranslationWindow
        {
            get
            {
                TranslationRules.Window retVal = null;
                DataDictionary.Dictionary dictionary = GetActiveDictionary();
                if (dictionary != null)
                {
                    retVal = new TranslationRules.Window(dictionary.TranslationDictionary);
                    AddChildWindow(retVal);
                }
                return retVal;
            }
        }

        /// <summary>
        /// The shortcuts window
        /// </summary>
        private Shortcuts.Window ShortcutsWindow
        {
            get
            {
                foreach (IBaseForm form in SubWindows)
                {
                    if (form is Shortcuts.Window)
                    {
                        return (Shortcuts.Window)form;
                    }
                }

                DataDictionary.Dictionary dictionary = GetActiveDictionary();
                if (dictionary != null)
                {
                    Shortcuts.Window newWindow = new Shortcuts.Window(dictionary.ShortcutsDictionary);
                    newWindow.Location = new System.Drawing.Point(Width - newWindow.Width - 20, 0);
                    AddChildWindow(newWindow);
                    return newWindow;
                }

                return null;
            }
        }

        /// <summary>
        /// The application version number
        /// </summary>
        private string versionNumber = "0.9.8.3";

        /// <summary>
        /// The thread used to synchronize node names with their model
        /// </summary>
        private class Synchronizer : GenericSynchronizationHandler<MainWindow>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instance"></param>
            public Synchronizer(MainWindow instance, int cycleTime)
                : base(instance, cycleTime)
            {
            }

            /// <summary>
            /// Synchronization
            /// </summary>
            /// <param name="instance"></param>
            public override void HandleSynchronization(MainWindow instance)
            {
                instance.Invoke((MethodInvoker)delegate
                {
                    instance.UpdateTitle();
                    foreach (EditorForm editor in instance.Editors)
                    {
                        if (!editor.EditorTextBoxHasFocus())
                        {
                            editor.RefreshText();
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Indicates that synchronization is required
        /// </summary>
        private Synchronizer WindowSynchronizer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            AllowRefresh = true;
            GUIUtils.MDIWindow = this;
            GUIUtils.Graphics = CreateGraphics();

            WindowSynchronizer = new Synchronizer(this, 300);

            Refresh();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindow()
        {
            try
            {
                GUIUtils.Graphics.Dispose();
                GUIUtils.Graphics = null;
            }
            catch (Exception)
            {
            }
            GUIUtils.MDIWindow = null;
        }

        /// <summary>
        /// Updates the title according to the windows state
        /// </summary>
        public void UpdateTitle()
        {
            String windowTitle = "ERTMS Formal Spec Workbench (version " + versionNumber + ")";

            if (EFSSystem != null && EFSSystem.ShouldSave)
            {
                windowTitle += " [modified]";
            }

            Text = windowTitle;
        }

        /// <summary>
        /// Adds a child window to this parent MDI
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public void AddChildWindow(Form window)
        {
            if (window != null)
            {
                window.MdiParent = this;
                window.Show();
                window.Activate();

                if (window is IBaseForm)
                {
                    SubWindows.Add((IBaseForm)window);
                }
                else if (window is EditorForm)
                {
                    Editors.Add((EditorForm)window);
                }

                ActivateMdiChild(window);
            }
        }

        /// <summary>
        /// Ensures that a window is closed
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        private void EnsureIsClosed(Form window)
        {
            if (window != null)
            {
                try
                {
                    window.Close();
                    window.MdiParent = null;

                    if (window is IBaseForm)
                    {
                        SubWindows.Remove((IBaseForm)window);
                    }
                    else if (window is EditorForm)
                    {
                        Editors.Remove((EditorForm)window);
                    }

                    RemoveOwnedForm(window);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Closes all child windows of this MDI window
        /// </summary>
        private void CloseChildWindows()
        {
            while (SubWindows.Count > 0)
            {
                Form window = (Form)SubWindows.First();
                EnsureIsClosed(window);
            }
        }

        /// <summary>
        /// Indicates that the refresh should be performed
        /// </summary>
        public bool AllowRefresh { get; set; }

        /// <summary>
        /// Refreshes the content of the window based on the associated model
        /// (changes may have occured)
        /// </summary>
        public void RefreshModel()
        {
            if (AllowRefresh)
            {
                foreach (IBaseForm form in SubWindows)
                {
                    form.RefreshModel();
                    form.Refresh();
                }
            }
        }

        /// <summary>
        /// Refreshes the display of the windows.
        /// No structural model change occurred.
        /// </summary>
        public override void Refresh()
        {
            try
            {
                DataDictionary.Generated.ControllersManager.DesactivateAllNotifications();
                foreach (IBaseForm form in SubWindows)
                {
                    form.Refresh();
                }
                UpdateTitle();
            }
            finally
            {
                DataDictionary.Generated.ControllersManager.ActivateAllNotifications();
            }

            base.Refresh();
        }

        #region OpenFile
        /// ------------------------------------------------------
        ///    OPEN OPERATIONS
        /// ------------------------------------------------------

        /// <summary>
        /// The efs system
        /// </summary>
        public DataDictionary.EFSSystem EFSSystem
        {
            get { return DataDictionary.EFSSystem.INSTANCE; }
        }

        private class OpenFileOperation : ProgressHandler
        {
            /// <summary>
            /// The name of the file to open
            /// </summary>
            private string FileName { get; set; }

            /// <summary>
            /// The system in which the dictionary should be loaded
            /// </summary>
            private DataDictionary.EFSSystem System { get; set; }

            /// <summary>
            /// The dictionary that has been opened
            /// </summary>
            public DataDictionary.Dictionary Dictionary { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="fileName"></param>
            public OpenFileOperation(string fileName, DataDictionary.EFSSystem system)
            {
                FileName = fileName;
                System = system;
            }

            /// <summary>
            /// Performs the job as a background task
            /// </summary>
            /// <param name="arg"></param>
            public override void ExecuteWork()
            {
                Dictionary = DataDictionary.Util.load(FileName, System, System != null);
            }
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open ERTMS Formal Spec file";
            openFileDialog.Filter = "EFS Files (*.efs)|*.efs|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    OpenFileOperation openFileOperation = new OpenFileOperation(openFileDialog.FileName, EFSSystem);
                    ProgressDialog dialog = new ProgressDialog("Opening file", openFileOperation);
                    dialog.ShowDialog();

                    // Open the windows
                    if (openFileOperation.Dictionary != null)
                    {
                        DataDictionary.Dictionary dictionary = openFileOperation.Dictionary;
                        DataDictionary.Generated.ControllersManager.DesactivateAllNotifications();

                        // Only open the specification window if specifications are available in the opened file
                        if (dictionary.Specifications != null && dictionary.Specifications.AllParagraphs.Count > 0)
                        {
                            AddChildWindow(new SpecificationView.Window(dictionary));
                        }

                        // Only open the model view window if model elements are available in the opened file
                        if (dictionary.NameSpaces.Count > 0)
                        {
                            AddChildWindow(new DataDictionaryView.Window(dictionary));
                        }

                        // Only shold the tests window if tests are defined in the opened file
                        if (dictionary.Tests.Count > 0)
                        {
                            IBaseForm testWindow = TestWindow;
                            if (testWindow == null)
                            {
                                AddChildWindow(new TestRunnerView.Window(EFSSystem));
                            }
                            else
                            {
                                testWindow.RefreshModel();
                            }
                        }

                        // Only open the shortcuts window if there are some shortcuts defined
                        if (dictionary.ShortcutsDictionary != null)
                        {
                            IBaseForm shortcutsWindow = ShortcutsWindow;
                            if (shortcutsWindow != null)
                            {
                                shortcutsWindow.RefreshModel();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot open file, please see log file (GUI.Log) for more information", "Cannot open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    DataDictionary.Generated.ControllersManager.ActivateAllNotifications();
                }

                Refresh();
            }
        }

        #endregion

        #region SaveFile
        /// ------------------------------------------------------
        ///    SAVE OPERATIONS
        /// ------------------------------------------------------

        /// <summary>
        /// A save file configuration
        /// </summary>
        private class SaveOperation : ProgressHandler
        {
            /// <summary>
            /// The form that invoked this progress handler
            /// </summary>
            private MainWindow MainWindow { get; set; }

            /// <summary>
            /// The dictionary to save
            /// </summary>
            private DataDictionary.Dictionary Dictionary { get; set; }

            /// <summary>
            /// The system to save
            /// </summary>
            private DataDictionary.EFSSystem System { get; set; }

            /// <summary>
            /// Constructor used to save a single dictionary
            /// </summary>
            /// <param name="mainWindow"></param>
            /// <param name="dictionary"></param>
            public SaveOperation(MainWindow mainWindow, DataDictionary.Dictionary dictionary)
            {
                MainWindow = mainWindow;
                Dictionary = dictionary;
                System = Dictionary.EFSSystem;
            }

            /// <summary>
            /// Constructor used to save to complete system
            /// </summary>
            /// <param name="mainWindow"></param>
            /// <param name="system"></param>
            public SaveOperation(MainWindow mainWindow, DataDictionary.EFSSystem system)
            {
                MainWindow = mainWindow;
                System = system;
            }

            /// <summary>
            /// Performs the job as a background task
            /// </summary>
            /// <param name="arg"></param>
            public override void ExecuteWork()
            {
                DataDictionary.Util.UnlockAllFiles();

                try
                {
                    if (Dictionary != null)
                    {
                        Dictionary.save();
                    }
                    else
                    {
                        // Save all dictionaries
                        foreach (DataDictionary.Dictionary dictionary in System.Dictionaries)
                        {
                            dictionary.save();
                        }
                    }
                }
                finally
                {
                    DataDictionary.Util.LockAllFiles();
                    System.ShouldSave = false;
                    MainWindow.Invoke((MethodInvoker)delegate
                    {
                        MainWindow.UpdateTitle();
                    });
                }
            }
        }


        /// <summary>
        /// Provides the dictionary on which operation should be performed
        /// </summary>
        /// <returns></returns>
        public DataDictionary.Dictionary GetActiveDictionary()
        {
            DataDictionary.Dictionary retVal = null;

            if (EFSSystem != null)
            {
                if (EFSSystem.Dictionaries.Count == 1)
                {
                    retVal = EFSSystem.Dictionaries[0];
                }
                else
                {
                    DictionarySelector.DictionarySelector dictionarySelector = new DictionarySelector.DictionarySelector(EFSSystem);
                    dictionarySelector.ShowDialog(this);

                    if (dictionarySelector.Selected != null)
                    {
                        retVal = dictionarySelector.Selected;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// The tooltip associated to this form
        /// </summary>
        public ToolTip ToolTip { get { return toolTip; } }


        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary activeDictionary = GetActiveDictionary();

            if (activeDictionary != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Saving EFS file " + activeDictionary.Name;
                saveFileDialog.Filter = "EFS files (*.efs)|*.efs|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    activeDictionary.FilePath = saveFileDialog.FileName;
                    SaveOperation saveOperation = new SaveOperation(this, activeDictionary);
                    ProgressDialog dialog = new ProgressDialog("Saving file " + activeDictionary.FilePath, saveOperation);
                    dialog.ShowDialog();
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                SaveOperation saveOperation = new SaveOperation(this, dictionary);
                ProgressDialog dialog = new ProgressDialog("Saving file " + dictionary.Name, saveOperation);
                dialog.ShowDialog();
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                SaveOperation saveOperation = new SaveOperation(this, dictionary);
                ProgressDialog dialog = new ProgressDialog("Saving file " + dictionary.Name, saveOperation);
                dialog.ShowDialog();
            }
        }
        #endregion

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EFSSystem.ShouldSave)
            {
                DialogResult result = MessageBox.Show("Model has been changed, do you want to save it", "Model changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        ProgressDialog dialog = new ProgressDialog("Saving files", new SaveOperation(this, EFSSystem));
                        dialog.ShowDialog();
                        break;

                    case System.Windows.Forms.DialogResult.No:
                        break;

                    case System.Windows.Forms.DialogResult.Cancel:
                        return;
                }
            }

            this.Close();
        }

        /// <summary>
        /// The rich text box currently selected
        /// </summary>
        public EditorTextBox SelectedRichTextBox { get; set; }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRichTextBox != null)
            {
                SelectedRichTextBox.Undo();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRichTextBox != null)
            {
                SelectedRichTextBox.Redo();
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRichTextBox != null)
            {
                SelectedRichTextBox.Cut();
            }
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRichTextBox != null)
            {
                SelectedRichTextBox.Copy();
            }
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRichTextBox != null)
            {
                SelectedRichTextBox.Paste();
            }
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        #region Check model
        private class CheckModelHandler : Utils.ProgressHandler
        {
            /// <summary>
            /// The system on which the check is performed
            /// </summary>
            private EFSSystem EFSSystem { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="system"></param>
            public CheckModelHandler(EFSSystem system)
            {
                EFSSystem = system;
            }

            /// <summary>
            /// Generates the file in the background thread
            /// </summary>
            /// <param name="arg"></param>
            public override void ExecuteWork()
            {
                DataDictionary.Generated.ControllersManager.DesactivateAllNotifications();
                try
                {
                    foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
                    {
                        dictionary.CheckRules();
                    }
                }
                finally
                {
                    DataDictionary.Generated.ControllersManager.ActivateAllNotifications();
                }
            }
        }

        private void checkModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckModelHandler checkModelHandler = new CheckModelHandler(EFSSystem);
            ProgressDialog dialog = new ProgressDialog("Check model", checkModelHandler);
            dialog.ShowDialog();
            Refresh();
        }
        #endregion

        private void implementedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.Specifications.CheckImplementation();
            }
            Refresh();
        }

        private void implementationRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.MarkUnimplementedItems();
            }
            Refresh();
        }

        private void verificationRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.MarkNotVerifiedRules();
            }
            Refresh();
        }

        private void verifiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.Specifications.CheckReview();
                Refresh();
            }
        }

        /// ------------------------------------------------------
        ///    MARKS OPERATIONS
        /// ------------------------------------------------------

        private void clearMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.ClearMessages();
            }
            Refresh();
        }

        private void markRequirementsWhereMoreInfoIsRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                if (dictionary.Specifications != null)
                {
                    dictionary.Specifications.CheckMoreInfo();
                }
            }
            Refresh();
        }

        private void markImplementedButNoFunctionalTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                if (dictionary.Specifications != null)
                {
                    dictionary.Specifications.CheckImplementedWithNoFunctionalTest();
                }
            }
            Refresh();
        }

        private void markNotImplementedButImplementationExistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                if (dictionary.Specifications != null)
                {
                    dictionary.Specifications.CheckNotImplementedButImplementationExists();
                }
            }
            Refresh();
        }

        private void markApplicableParagraphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                if (dictionary.Specifications != null)
                {
                    dictionary.Specifications.CheckApplicable();
                }
            }
            Refresh();
        }

        private void markImplementationRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.MarkUnimplementedTests();
            }

            Refresh();
        }

        private void markNotTranslatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.MarkNotTranslatedTests();
            }

            Refresh();
        }

        private void markNotImplementedTranslationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.MarkNotImplementedTranslations();
            }

            Refresh();
        }

        private void markNonApplicableRequirementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                if (dictionary.Specifications != null)
                {
                    dictionary.Specifications.CheckNonApplicable();
                }
            }
            Refresh();
        }

        private void markSpecIssuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                if (dictionary.Specifications != null)
                {
                    dictionary.Specifications.CheckSpecIssues();
                }
            }
            Refresh();
        }

        /// ------------------------------------------------------
        ///    IMPORT SPEC OPERATIONS
        /// ------------------------------------------------------

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Open original specification file";
                openFileDialog.Filter = "RTF Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string OriginalFileName = openFileDialog.FileName;
                    openFileDialog.Title = "Open new specification file";
                    openFileDialog.Filter = "RTF Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
                    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        string NewFileName = openFileDialog.FileName;

                        string baseFileName = createBaseFileName(OriginalFileName, NewFileName);

                        /// Perform the importation
                        Importers.RtfDeltaImporter.Importer importer = new Importers.RtfDeltaImporter.Importer(OriginalFileName, NewFileName, dictionary.Specifications);
                        ProgressDialog dialog = new ProgressDialog("Opening file", importer);
                        dialog.ShowDialog();

                        /// Creates the report based on the importation result
                        Reports.Importer.DeltaImportReportHandler reportHandler = new Reports.Importer.DeltaImportReportHandler(dictionary, importer.NewDocument, baseFileName);
                        dialog = new ProgressDialog("Opening file", reportHandler);
                        dialog.ShowDialog();

                        RefreshModel();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the base file name for the report
        /// </summary>
        /// <param name="OriginalFileName"></param>
        /// <param name="NewFileName"></param>
        /// <returns></returns>
        private string createBaseFileName(string OriginalFileName, string NewFileName)
        {
            string baseFileName = "";
            for (int i = 0; i < OriginalFileName.Length && i < NewFileName.Length; i++)
            {
                if (OriginalFileName[i] == NewFileName[i])
                {
                    baseFileName += OriginalFileName[i];
                }
                else
                {
                    break;
                }
            }
            if (baseFileName.IndexOf("\\") > 0)
            {
                baseFileName = baseFileName.Substring(baseFileName.LastIndexOf("\\") + 1);
            }

            if (baseFileName.IndexOf("v") > 0)
            {
                baseFileName = baseFileName.Substring(0, baseFileName.LastIndexOf("v"));
            }

            return baseFileName;
        }

        #region Import test database
        /// ------------------------------------------------------
        ///    IMPORT TEST DATABASE OPERATIONS
        /// ------------------------------------------------------
        private class ImportTestDataBaseHandler : Utils.ProgressHandler
        {
            /// <summary>
            /// The name of the frame for the subset 76
            /// </summary>
            private static string SUBSET_076 = "Subset-076";

            /// <summary>
            /// The password requireed to access the database
            /// </summary>
            private static string DB_PASSWORD = "papagayo";

            /// <summary>
            /// The dictionary in which the database should be imported
            /// </summary>
            private DataDictionary.Dictionary Dictionary;

            /// <summary>
            /// The name of the database to import
            /// </summary>
            private string FileName { get; set; }

            /// <summary>
            /// Should we import a file, or a directory containing a set of files?
            /// </summary>
            public enum Mode { File, Directory };

            /// <summary>
            /// The import mode
            /// </summary>
            private Mode ImportMode { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="dictionary"></param>
            public ImportTestDataBaseHandler(string fileName, Dictionary dictionary, Mode mode)
            {
                FileName = fileName;
                Dictionary = dictionary;
                ImportMode = mode;
            }

            /// <summary>
            /// Generates the file in the background thread
            /// </summary>
            /// <param name="arg"></param>
            public override void ExecuteWork()
            {
                DataDictionary.Tests.Frame frame = Dictionary.findFrame(SUBSET_076);
                if (frame == null)
                {
                    frame = (DataDictionary.Tests.Frame)DataDictionary.Generated.acceptor.getFactory().createFrame();
                    frame.Name = SUBSET_076;
                    Dictionary.appendTests(frame);
                }

                if (ImportMode == Mode.File)
                {
                    Importers.TestImporter importer = new Importers.TestImporter(FileName, DB_PASSWORD);
                    importer.Import(frame);
                }
                else
                {
                    foreach (string fName in System.IO.Directory.GetFiles(FileName, "*.mdb"))
                    {
                        Importers.TestImporter importer = new Importers.TestImporter(fName, DB_PASSWORD);
                        importer.Import(frame);
                    }
                }
            }
        }

        private void importDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Open test sequence database";
                openFileDialog.Filter = "Access Files (*.mdb)|*.mdb";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    ImportTestDataBaseHandler importTestDataBaseHandler = new ImportTestDataBaseHandler(openFileDialog.FileName, dictionary, ImportTestDataBaseHandler.Mode.File);
                    ProgressDialog dialog = new ProgressDialog("Import database", importTestDataBaseHandler);
                    dialog.ShowDialog();

                    // Updates the test tree view data
                    if (TestWindow != null)
                    {
                        TestWindow.TreeView.RefreshModel();
                        Refresh();
                    }
                }
            }
        }

        private void importFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                FolderBrowserDialog selectFolderDialog = new FolderBrowserDialog();
                if (selectFolderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    ImportTestDataBaseHandler importTestDataBaseHandler = new ImportTestDataBaseHandler(selectFolderDialog.SelectedPath, dictionary, ImportTestDataBaseHandler.Mode.Directory);
                    ProgressDialog dialog = new ProgressDialog("Import database directory", importTestDataBaseHandler);
                    dialog.ShowDialog();

                    // Updates the test tree view data
                    if (TestWindow != null)
                    {
                        TestWindow.TreeView.RefreshModel();
                        Refresh();
                    }
                }
            }
        }
        #endregion

        private void showTranslationRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddChildWindow(TranslationWindow);
        }


        private void showShortcutsViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddChildWindow(ShortcutsWindow);
        }

        private void showTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EFSSystem != null)
            {
                Form testWindow = TestWindow;
                if (testWindow == null)
                {
                    AddChildWindow(new TestRunnerView.Window(EFSSystem));
                }
                else
                {
                    testWindow.Select();
                }
            }
        }

        private void showModelViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                AddChildWindow(new DataDictionaryView.Window(dictionary));
            }
        }

        private void showSpecificationViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                AddChildWindow(new SpecificationView.Window(dictionary));
            }
        }

        /// ------------------------------------------------------
        ///    CREATE REPORT OPERATIONS
        /// ------------------------------------------------------

        private void specCoverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                Report.SpecReport aReport = new Report.SpecReport(dictionary);
                aReport.ShowDialog(this);
            }
        }

        private void testsCoverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                Report.TestReport aReport = new Report.TestReport(dictionary);
                aReport.ShowDialog(this);
            }
        }

        private void generateDynamicCoverageReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                Report.TestReport aReport = new Report.TestReport(dictionary);
                aReport.ShowDialog(this);
            }
        }

        private void generateCoverageReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                Report.SpecReport aReport = new Report.SpecReport(dictionary);
                aReport.ShowDialog(this);
            }
        }

        private void generateSpecIssuesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                Report.SpecIssuesReport aReport = new Report.SpecIssuesReport(dictionary);
                aReport.ShowDialog(this);
            }
        }

        private void generateDataDictionaryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                Report.ModelReport aReport = new Report.ModelReport(dictionary);
                aReport.ShowDialog(this);
            }
        }

        private void searchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SearchDialog.SearchDialog dialog = new SearchDialog.SearchDialog();
            dialog.Initialise(EFSSystem);
            dialog.ShowDialog(this);
        }

        private void refreshWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure the system has been compiled
            EFSSystem efsSystem = EFSSystem.INSTANCE;
            DataDictionary.Interpreter.Compiler compiler = new DataDictionary.Interpreter.Compiler(efsSystem, efsSystem.ShouldRebuild);
            compiler.Compile();
            efsSystem.ShouldRebuild = false;

            RefreshModel();
        }

        private void exportFunctionalBlocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                DataDictionary.Specification.FunctionalBlockExporter fbExporter = new DataDictionary.Specification.FunctionalBlockExporter(dictionary.Specifications);
                fbExporter.Export("../FunctionalBlocks.csv");
            }
        }

        private void showRulePerformancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RulePerformances.RulesPerformances rulePerformances = new RulePerformances.RulesPerformances(EFSSystem);
            AddChildWindow(rulePerformances);
        }

        /// <summary>
        /// ReInit counters in rules
        /// </summary>
        private class ResetTimeStamps : DataDictionary.Generated.Visitor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="efsSystem"></param>
            public ResetTimeStamps(DataDictionary.EFSSystem efsSystem)
            {
                foreach (DataDictionary.Dictionary dictionary in efsSystem.Dictionaries)
                {
                    visit(dictionary, true);
                }
            }

            public override void visit(DataDictionary.Generated.Rule obj, bool visitSubNodes)
            {
                DataDictionary.Rules.Rule rule = obj as DataDictionary.Rules.Rule;

                rule.ExecutionTimeInMilli = 0;
                rule.ExecutionCount = 0;

                base.visit(obj, visitSubNodes);
            }

            public override void visit(DataDictionary.Generated.Function obj, bool visitSubNodes)
            {
                DataDictionary.Functions.Function function = obj as DataDictionary.Functions.Function;

                function.ExecutionTimeInMilli = 0;
                function.ExecutionCount = 0;

                base.visit(obj);
            }

            public override void visit(DataDictionary.Generated.Frame obj, bool visitSubNodes)
            {
                // No rules in frames
            }
        }

        private void resetCountersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EFSSystem != null)
            {
                ResetTimeStamps reset = new ResetTimeStamps(EFSSystem);
            }
        }

        private void showFunctionsPerformancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FunctionsPerformances.FunctionsPerformances functionsPerformances = new FunctionsPerformances.FunctionsPerformances(EFSSystem);
            AddChildWindow(functionsPerformances);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creates a new dictionary
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Create new dictionary. Select dictionary file location";
            openFileDialog.Filter = "EFS Files (*.efs)|*.efs";
            openFileDialog.CheckFileExists = false;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                DataDictionary.Dictionary dictionary = new DataDictionary.Dictionary();
                dictionary.FilePath = filePath;
                dictionary.Name = Path.GetFileNameWithoutExtension(filePath);
                EFSSystem.AddDictionary(dictionary);
                RefreshModel();

                // Open a data dictionary window if none is yet present
                bool found = false;
                foreach (IBaseForm form in SubWindows)
                {
                    if (form is DataDictionaryView.Window)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    AddChildWindow(new DataDictionaryView.Window(dictionary));
                }
            }
        }

        private void markParagraphsFromNewRevisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.Specifications.CheckNewRevision();
                Refresh();
            }
        }

        private void generateERTMSAcademyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            if (dictionary != null)
            {
                Report.ERTMSAcademyReport aReport = new Report.ERTMSAcademyReport(dictionary);
                aReport.ShowDialog(this);
            }
        }

        private void compareWithToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open ERTMS Formal Spec file";
            openFileDialog.Filter = "EFS Files (*.efs)|*.efs|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Open the dictionary but do not store it in the EFS System
                OpenFileOperation openFileOperation = new OpenFileOperation(openFileDialog.FileName, null);
                ProgressDialog dialog = new ProgressDialog("Opening file", openFileOperation);
                dialog.ShowDialog();

                // Compare the files
                if (openFileOperation.Dictionary != null)
                {
                    DataDictionary.Dictionary dictionary = GetActiveDictionary();
                    DataDictionary.Compare.VersionDiff versionDiff = new DataDictionary.Compare.VersionDiff();
                    DataDictionary.Compare.Comparer.compareDictionary(dictionary, openFileOperation.Dictionary, versionDiff);
                    versionDiff.markVersionChanges(dictionary);
                }
                else
                {
                    MessageBox.Show("Cannot open file, please see log file (GUI.Log) for more information", "Cannot open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Refresh();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options.Options optionForm = new Options.Options();
            optionForm.Setup(EFSSystem);
            optionForm.ShowDialog(this);
            optionForm.UpdateSystem(EFSSystem);
        }

        private void compareWithGitRevisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Compares with the active dictionary
            DataDictionary.Dictionary dictionary = GetActiveDictionary();
            string workingDir = Path.GetDirectoryName(dictionary.FilePath);

            // Retrieve the hash tag
            VersionSelector.VersionSelector selector = new VersionSelector.VersionSelector(dictionary);
            selector.ShowDialog();
            Commit selected = selector.Selected;
            if (selected != null)
            {
                // Create the temp directory to store alternate version of the subset file
                string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDirectory);
                try
                {
                    // Retrieve the archive of the selected version
                    {
                        ProcessStartInfo _processStartInfo = new ProcessStartInfo();
                        // _processStartInfo.WorkingDirectory = "c:\\ertms-repositories\\ERTMSFormalSpecs";
                        _processStartInfo.WorkingDirectory = workingDir;
                        _processStartInfo.FileName = "git";
                        _processStartInfo.Arguments = "archive -o " + tempDirectory + "\\specs.zip " + selected.Id.Sha + " .";
                        _processStartInfo.CreateNoWindow = true;
                        _processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        Process myProcess = Process.Start(_processStartInfo);
                        myProcess.WaitForExit();
                    }

                    // Unzip the archive
                    {
                        ICSharpCode.SharpZipLib.Zip.FastZip zip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                        zip.ExtractZip(tempDirectory + "\\specs.zip", tempDirectory, null);
                    }

                    // Open the dictionary but do not store it in the EFS System
                    OpenFileOperation openFileOperation = new OpenFileOperation(tempDirectory + "\\subset-026.efs", null);
                    ProgressDialog dialog = new ProgressDialog("Opening file", openFileOperation);
                    dialog.ShowDialog();

                    // Compare the files
                    if (openFileOperation.Dictionary != null)
                    {
                        DataDictionary.Compare.VersionDiff versionDiff = new DataDictionary.Compare.VersionDiff();
                        DataDictionary.Compare.Comparer.compareDictionary(dictionary, openFileOperation.Dictionary, versionDiff);
                        versionDiff.markVersionChanges(dictionary);
                    }
                    else
                    {
                        MessageBox.Show("Cannot open file, please see log file (GUI.Log) for more information", "Cannot open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Exception raised during operation " + exception.Message, "Cannot perform operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    try
                    {
                        Directory.Delete(tempDirectory, true);
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show("Exception raised during operation " + exception2.Message, "Cannot perform operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                Refresh();
            }
        }
    }
}