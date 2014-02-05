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

namespace GUI.TranslationRules
{
    public class SourceTextsTreeNode : ModelElementTreeNode<DataDictionary.Tests.Translations.Translation>
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
        public SourceTextsTreeNode(DataDictionary.Tests.Translations.Translation item, bool buildSubNodes)
            : base(item, buildSubNodes, "Source texts", true)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            Nodes.Clear();

            base.BuildSubNodes(buildSubNodes);

            foreach (DataDictionary.Tests.Translations.SourceText sourceText in Item.SourceTexts)
            {
                Nodes.Add(new SourceTextTreeNode(sourceText, buildSubNodes));
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
        /// Creates a new source text
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SourceTextTreeNode createSourceText(DataDictionary.Tests.Translations.SourceText sourceText)
        {
            SourceTextTreeNode retVal;

            Item.appendSourceTexts(sourceText);
            retVal = new SourceTextTreeNode(sourceText, true);
            Nodes.Add(retVal);
            SortSubNodes();

            return retVal;
        }

        public void AddHandler(object sender, EventArgs args)
        {
            DataDictionary.Tests.Translations.SourceText sourceText = (DataDictionary.Tests.Translations.SourceText)DataDictionary.Generated.acceptor.getFactory().createSourceText();
            sourceText.Name = "<SourceText " + (Item.SourceTexts.Count + 1) + ">";
            createSourceText(sourceText);
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add", new EventHandler(AddHandler)));
            retVal.Add(new MenuItem("-"));
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));

            return retVal;
        }

        /// <summary>
        /// Handles a selection change event
        /// </summary>
        /// <param name="displayStatistics">Indicates that statistics should be displayed in the MDI window</param>
        public override void SelectionChanged(bool displayStatistics)
        {
            base.SelectionChanged(displayStatistics);
            if (BaseTreeView != null && BaseTreeView.RefreshNodeContent)
            {
                IBaseForm baseForm = BaseForm;
                if (baseForm != null)
                {
                    if (baseForm.RequirementsTextBox != null)
                    {
                        baseForm.RequirementsTextBox.Text = Item.getSourceTextExplain();
                    }
                }
            }
        }
    }
}
