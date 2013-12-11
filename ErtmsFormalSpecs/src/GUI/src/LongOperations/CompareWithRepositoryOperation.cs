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
namespace GUI.LongOperations
{
    using DataDictionary;
    using System.Windows.Forms;
    using LibGit2Sharp;

    public class CompareWithRepositoryOperation : BaseCompareWithRepositoryOperation
    {
        /// <summary>
        /// The commit to compare with
        /// </summary>
        private Commit Commit { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary">The dictionary to be compared</param>
        /// <param name="commit">The commit to compare with</param>
        public CompareWithRepositoryOperation(Dictionary dictionary, Commit commit)
            : base(dictionary)
        {
            Commit = commit;
        }

        /// <summary>
        /// Generates the file in the background thread
        /// </summary>
        /// <param name="arg"></param>
        public override void ExecuteWork()
        {
            Dictionary otherVersion = DictionaryByVersion(Commit);

            // Compare the two dictionaries and mark the differences
            if (otherVersion != null)
            {
                DataDictionary.Compare.VersionDiff versionDiff = new DataDictionary.Compare.VersionDiff();
                DataDictionary.Compare.Comparer.ensureGuidDictionary(Dictionary, otherVersion);
                DataDictionary.Compare.Comparer.compareDictionary(Dictionary, otherVersion, versionDiff);
                versionDiff.markVersionChanges(Dictionary);
            }
            else
            {
                MessageBox.Show("Cannot open file, please see log file (GUI.Log) for more information", "Cannot open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
