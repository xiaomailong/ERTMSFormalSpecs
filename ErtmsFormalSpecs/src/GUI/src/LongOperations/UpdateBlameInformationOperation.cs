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
    using System.IO;

    public class UpdateBlameInformationOperation : BaseCompareWithRepositoryOperation
    {
        /// <summary>
        /// The latest commit to compare with
        /// </summary>
        private Commit LastCommit { get; set; }

        /// <summary>
        /// The path of the history file
        /// </summary>
        protected string HistoryFilePath { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The location of the history file</param>
        /// <param name="dictionary">The dictionary to compare with</param>
        /// <param name="lastCommit">The last commit used to update the history information</param>
        public UpdateBlameInformationOperation(string path, Dictionary dictionary, Commit lastCommit)
            : base(dictionary)
        {
            HistoryFilePath = path;
            LastCommit = lastCommit;
        }

        /// <summary>
        /// Generates the file in the background thread
        /// </summary>
        /// <param name="arg"></param>
        public override void ExecuteWork()
        {
            // Retrieve the hash tag
            if (LastCommit != null)
            {
                HistoricalData.History history = Dictionary.EFSSystem.History;
                if (history.Commit(LastCommit.Sha) == null)
                {
                    // Processes all commits until the one that has been selected
                    Dictionary currentVersion = Dictionary;
                    Dictionary previousVersion = null;
                    Commit previousCommit = null;
                    foreach (Commit commit in Repository.Commits)
                    {
                        // Always compare the current version with the first commit in the repository 
                        // since changes may have been applied to the current version
                        if (previousVersion == null || !history.CommitExists(commit.Sha))
                        {
                            if (previousCommit == null)
                            {
                                previousVersion = currentVersion;
                            }
                            else
                            {
                                previousVersion = DictionaryByVersion(previousCommit);
                                DataDictionary.Compare.Comparer.ensureGuidDictionary(previousVersion, currentVersion);
                            }
                            currentVersion = DictionaryByVersion(commit);

                            if (currentVersion != null)
                            {
                                DataDictionary.Compare.VersionDiff versionDiff = new DataDictionary.Compare.VersionDiff();
                                versionDiff.setCommitter(commit.Author.Name);
                                versionDiff.setDate(commit.Author.When.ToString());
                                versionDiff.setHash(commit.Sha);
                                versionDiff.setMessage(commit.Message);

                                DataDictionary.Compare.Comparer.ensureGuidDictionary(previousVersion, currentVersion);
                                DataDictionary.Compare.Comparer.compareDictionary(previousVersion, currentVersion, versionDiff);
                                history.AppendOrReplaceCommit(versionDiff);
                                history.save(HistoryFilePath);
                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("Cannot open file for commit\n" + commit.MessageShort + " (" + commit.Sha + ")\nplease see log file (GUI.Log) for more information.\nPress OK to continue.", "Cannot open file", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                if (result == DialogResult.OK)
                                {
                                    // Stick back to the previous version to continue the process
                                    currentVersion = previousVersion;
                                }
                                else
                                {
                                    // Stop the process
                                    break;
                                }
                            }

                            previousCommit = null;
                        }
                        else
                        {
                            previousCommit = commit;
                        }

                        if (commit == LastCommit)
                        {
                            break;
                        }
                    }
                }
                history.UpdateBlame();
            }
        }
    }
}
