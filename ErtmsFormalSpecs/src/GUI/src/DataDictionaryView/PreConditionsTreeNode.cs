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
using DataDictionary.Generated;
using Case = DataDictionary.Functions.Case;
using PreCondition = DataDictionary.Rules.PreCondition;

namespace GUI.DataDictionaryView
{
    public class PreConditionsTreeNode : CaseTreeNode
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="children"></param>
        public PreConditionsTreeNode(Case item, bool buildSubNodes)
            : base(item, buildSubNodes, "Pre condition", true)
        {
        }

        /// <summary>
        ///     Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates whether the subnodes of the nodes should also be built</param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            Nodes.Clear();

            foreach (PreCondition preCondition in Item.PreConditions)
            {
                Nodes.Add(new PreConditionTreeNode(preCondition, buildSubNodes));
            }
            SubNodesBuilt = true;
        }

        /// <summary>
        ///     Create structure based on the subsystem structure
        /// </summary>
        /// <param name="SourceNode"></param>
        public override void AcceptDrop(BaseTreeNode SourceNode)
        {
            base.AcceptDrop(SourceNode);

            if (SourceNode is PreConditionTreeNode)
            {
                if (
                    MessageBox.Show("Are you sure you want to move the corresponding function ?", "Move action",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PreConditionTreeNode node = SourceNode as PreConditionTreeNode;
                    PreCondition preCondition = node.Item;
                    node.Delete();
                    AddPreCondition(preCondition);
                }
            }
        }

        public void AddHandler(object sender, EventArgs args)
        {
            PreCondition preCondition = (PreCondition) acceptor.getFactory().createPreCondition();
            preCondition.Condition = "<empty>";
            AddPreCondition(preCondition);
        }

        /// <summary>
        ///     Adds a preCondition to the modelized item
        /// </summary>
        /// <param name="preCondition"></param>
        public void AddPreCondition(PreCondition preCondition)
        {
            Item.appendPreConditions(preCondition);
            Nodes.Add(new PreConditionTreeNode(preCondition, true));
            SortSubNodes();
        }

        /// <summary>
        ///     The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add", new EventHandler(AddHandler)));

            return retVal;
        }
    }
}