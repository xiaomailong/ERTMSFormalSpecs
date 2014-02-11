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
        /// Constructor
        /// </summary>
        /// <param name="dictionary">The dictionary to compare with</param>
        /// <param name="lastCommit">The last commit used to update the history information</param>
        public UpdateBlameInformationOperation(Dictionary dictionary, Commit lastCommit)
            : base(dictionary)
        {
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
                Repository repository = getRepository();

                string DictionaryDirectory = Path.GetDirectoryName(Dictionary.FilePath);
                DictionaryDirectory = DictionaryDirectory.Substring(RepositoryPath.Length + 1, DictionaryDirectory.Length - RepositoryPath.Length - 1);

                HistoricalData.History history = Dictionary.EFSSystem.History;
                if (history.Commit(LastCommit.Sha) == null)
                {
                    // Processes all commits until the one that has been selected
                    Dictionary currentVersion = Dictionary;
                    Dictionary previousVersion = null;
                    Commit previousCommit = null;
                    Commit commitToRefer = null;
                    foreach (Commit commit in repository.Commits)
                    {
                        // Always compare the current version with the first commit in the repository 
                        // since changes may have been applied to the current version
                        if (previousVersion == null || !history.CommitExists(commit.Sha))
                        {
                            if (previousCommit == null)
                            {
                                if (previousVersion != Dictionary && currentVersion != previousVersion)
                                {
                                    CleanUpDictionary(previousVersion);
                                }
                                previousVersion = currentVersion;
                            }
                            else
                            {
                                previousVersion = DictionaryByVersion(previousCommit);
                                DataDictionary.Compare.Comparer.ensureGuidDictionary(previousVersion, currentVersion);
                            }

                            bool changesAvailable = true;
                            if (commitToRefer != null)
                            {
                                changesAvailable = false;
                                TreeChanges changes = repository.Diff.Compare(commit.Tree, commitToRefer.Tree);
                                foreach (TreeEntryChanges entry in changes.Modified)
                                {
                                    if (entry.Path.StartsWith(DictionaryDirectory))
                                    {
                                        changesAvailable = true;
                                        break;
                                    }
                                }
                            }

                            if (changesAvailable)
                            {
                                if (commitToRefer != null)
                                {
                                    string message = commitToRefer.Message.Trim();
                                    if (message.Length > 132)
                                    {
                                        message = message.Substring(0, 132) + "...";
                                    }
                                    Dialog.UpdateMessage("Processing " + message + " (" + commitToRefer.Sha + ")");
                                }
                                else
                                {
                                    Dialog.UpdateMessage("Processing current version...");
                                }

                                currentVersion = DictionaryByVersion(commit);

                                if (currentVersion != null)
                                {
                                    DataDictionary.Compare.VersionDiff versionDiff = new DataDictionary.Compare.VersionDiff();
                                    if (commitToRefer != null)
                                    {
                                        versionDiff.setCommitter(commitToRefer.Author.Name);
                                        versionDiff.setDate(commitToRefer.Author.When.ToString());
                                        versionDiff.setHash(commitToRefer.Sha);
                                        versionDiff.setMessage(commitToRefer.Message);
                                    }

                                    DataDictionary.Compare.Comparer.ensureGuidDictionary(previousVersion, currentVersion);
                                    DataDictionary.Compare.Comparer.compareDictionary(previousVersion, currentVersion, versionDiff);
                                    history.AppendOrReplaceCommit(versionDiff);
                                    history.save();
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
                            }
                            else
                            {
                                currentVersion = previousVersion;
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

                        commitToRefer = commit;
                    }
                }
                history.UpdateBlame();
            }
        }

        private class CleanUpCaches : DataDictionary.Generated.Visitor
        {
            public override void visit(DataDictionary.Generated.BaseModelElement obj, bool visitSubNodes)
            {
                Utils.IFinder finder = obj as Utils.IFinder;

                if (finder != null)
                {
                    Utils.FinderRepository.INSTANCE.UnRegister(finder);
                }

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        /// Avoid memory leak : caches hold references to dictionaries
        /// </summary>
        /// <param name="dictionary"></param>
        private static void CleanUpDictionary(Dictionary dictionary)
        {
            if (dictionary != null)
            {
                CleanUpCaches cleaner = new CleanUpCaches();
                cleaner.visit(dictionary);
            }
        }
    }
}
