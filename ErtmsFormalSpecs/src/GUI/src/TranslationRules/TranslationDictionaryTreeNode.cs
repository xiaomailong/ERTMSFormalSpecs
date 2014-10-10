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
using System.Windows.Forms;
using DataDictionary.Tests.Translations;

namespace GUI.TranslationRules
{
    public class TranslationDictionaryTreeNode : ModelElementTreeNode<DataDictionary.Tests.Translations.TranslationDictionary>
    {
        private class ItemEditor : NamedEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public TranslationDictionaryTreeNode(DataDictionary.Tests.Translations.TranslationDictionary item, bool buildSubNodes)
            : base(item, buildSubNodes, "Dictionary", true)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            base.BuildSubNodes(buildSubNodes);

            foreach (DataDictionary.Tests.Translations.Folder folder in Item.Folders)
            {
                Nodes.Add(new FolderTreeNode(folder, buildSubNodes));
            }
            foreach (DataDictionary.Tests.Translations.Translation translation in Item.Translations)
            {
                Nodes.Add(new TranslationTreeNode(translation, buildSubNodes));
            }
            SortSubNodes();
        }

        /// <summary>
        /// Creates the editor for this tree node
        /// </summary>
        /// <returns></returns>
        protected override Editor createEditor()
        {
            return new ItemEditor();
        }

        /// <summary>
        /// Creates a new folder
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FolderTreeNode createFolder(DataDictionary.Tests.Translations.Folder folder)
        {
            FolderTreeNode retVal = new FolderTreeNode(folder, true);

            Item.appendFolders(folder);
            Nodes.Add(retVal);
            SortSubNodes();

            return retVal;
        }

        public void AddFolderHandler(object sender, EventArgs args)
        {
            DataDictionary.Tests.Translations.Folder folder = (DataDictionary.Tests.Translations.Folder)DataDictionary.Generated.acceptor.getFactory().createFolder();
            folder.Name = "<Folder " + (Item.Folders.Count + 1) + ">";
            createFolder(folder);
        }

        /// <summary>
        /// Creates a new translation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TranslationTreeNode createTranslation(DataDictionary.Tests.Translations.Translation translation)
        {
            TranslationTreeNode retVal = null;

            Translation existingTranslation = null;
            foreach (SourceText sourceText in translation.SourceTexts)
            {
                existingTranslation = Item.FindExistingTranslation(sourceText);
                if (existingTranslation != null)
                {
                    break;
                }
            }

            if (existingTranslation != null)
            {
                DialogResult dialogResult = MessageBox.Show("Translation already exists. Do you want to create a new one (Cancel will select the existing translation) ?", "Already existing translation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    existingTranslation = null;
                }
                else
                {
                    retVal = (TranslationTreeNode)BaseTreeView.Select(existingTranslation);
                }
            }

            if (existingTranslation == null)
            {
                Item.appendTranslations(translation);
                retVal = new TranslationTreeNode(translation, true);
                Nodes.Add(retVal);
                SortSubNodes();

                BaseTreeView.Select(retVal);
            }

            return retVal;
        }

        public void AddTranslationHandler(object sender, EventArgs args)
        {
            DataDictionary.Tests.Translations.Translation translation = (DataDictionary.Tests.Translations.Translation)DataDictionary.Generated.acceptor.getFactory().createTranslation();
            translation.Name = "<Translation " + (Item.Translations.Count + 1) + ">";
            createTranslation(translation);
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add folder", new EventHandler(AddFolderHandler)));
            retVal.Add(new MenuItem("Add translation", new EventHandler(AddTranslationHandler)));

            return retVal;
        }

        /// <summary>
        /// Sorts the sub nodes of this node
        /// </summary>
        public override void SortSubNodes()
        {
            List<BaseTreeNode> folders = new List<BaseTreeNode>();
            List<BaseTreeNode> translations = new List<BaseTreeNode>();

            foreach (BaseTreeNode node in Nodes)
            {
                if (node is FolderTreeNode)
                {
                    folders.Add(node);
                }
                else if (node is TranslationTreeNode)
                {
                    translations.Add(node);
                }
            }
            folders.Sort();
            translations.Sort();

            Nodes.Clear();

            foreach (BaseTreeNode node in folders)
            {
                Nodes.Add(node);
            }
            foreach (BaseTreeNode node in translations)
            {
                Nodes.Add(node);
            }
        }

        /// <summary>
        /// Creates a new translation based on a step
        /// </summary>
        /// <param name="step"></param>
        private void createTranslation(DataDictionary.Tests.Step step)
        {
            Translation translation = (Translation)DataDictionary.Generated.acceptor.getFactory().createTranslation();
            translation.appendSourceTexts(step.createSourceText());
            createTranslation(translation);
        }

        /// <summary>
        /// Accepts a drop event
        /// </summary>
        /// <param name="SourceNode"></param>
        public override void AcceptDrop(BaseTreeNode SourceNode)
        {
            base.AcceptDrop(SourceNode);
            if (SourceNode is TestRunnerView.StepTreeNode)
            {
                TestRunnerView.StepTreeNode step = SourceNode as TestRunnerView.StepTreeNode;

                createTranslation(step.Item);
            }
            else if (SourceNode is TranslationTreeNode)
            {
                TranslationTreeNode translation = SourceNode as TranslationTreeNode;
                DataDictionary.Tests.Translations.Translation otherTranslation = translation.Item;
                translation.Delete();
                createTranslation(otherTranslation);
            }
        }
    }
}
