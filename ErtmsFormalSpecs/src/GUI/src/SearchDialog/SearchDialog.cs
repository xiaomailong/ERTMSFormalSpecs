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
using System.Windows.Forms;
using System.Collections.Generic;
using DataDictionary;

namespace GUI.SearchDialog
{
    public partial class SearchDialog : Form
    {
        /// <summary>
        /// The system for which this dialog is built
        /// </summary>
        public DataDictionary.EFSSystem EFSSystem { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchDialog()
        {
            InitializeComponent();
        }

        public MainWindow MDIWindow
        {
            get
            {
                return Owner as MainWindow;
            }
        }

        /// <summary>
        /// Initialises the dialog
        /// </summary>
        /// <param name="efsSystem"></param>
        public void Initialise(DataDictionary.EFSSystem efsSystem)
        {
            EFSSystem = efsSystem;

            searchTextBox.KeyUp += new KeyEventHandler(searchTextBox_KeyUp);
        }

        /// <summary>
        /// Handles specific key actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    e.Handled = true;
                    Close();
                    break;
            }
        }

        /// <summary>
        /// Search Medor, search....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, EventArgs e)
        {
            searchOccurences(searchTextBox.Text);
        }

        /// <summary>
        /// Searches for all occurences of the search string
        /// </summary>
        /// <param name="searchString"></param>
        private void searchOccurences(string searchString)
        {
            List<ModelElement> occurences = new List<ModelElement>();
            foreach (Dictionary dictionary in EFSSystem.Dictionaries)
            {
                DataDictionary.Compare.Comparer.searchDictionary(dictionary, searchString, occurences);
            }

            // Clears all messages and mark the occurences
            foreach (Dictionary dictionary in EFSSystem.Dictionaries)
            {
                dictionary.ClearMessages();
            }
            foreach (ModelElement element in occurences)
            {
                element.AddInfo("Found " + searchString);
            }

            if (occurences.Count == 0)
            {
                MessageBox.Show("Cannot find " + searchString, "Search complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
        }

        /// <summary>
        /// Launch search by pressing Enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchOccurences(searchTextBox.Text);
            }
        }
    }
}
