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

namespace GUI.DataDictionaryView
{
    public class SubRulesTreeNode : RuleConditionTreeNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public SubRulesTreeNode(DataDictionary.Rules.RuleCondition item, bool buildSubNodes)
            : base(item, buildSubNodes, "Sub rules", true, false)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            Nodes.Clear();

            foreach (DataDictionary.Rules.Rule rule in Item.SubRules)
            {
                Nodes.Add(new RuleTreeNode(rule, buildSubNodes));
            }
            SubNodesBuilt = true;
        }

        private static List<BaseTreeNode> sort(List<BaseTreeNode> nodes)
        {
            nodes.Sort();
            return nodes;
        }

        /// <summary>
        /// Adds a rule in this set of sub rules
        /// </summary>
        /// <param name="rule"></param>
        public RuleTreeNode AddRule(DataDictionary.Rules.Rule rule)
        {
            RuleTreeNode retVal = new RuleTreeNode(rule, true);

            Item.appendSubRules(rule);
            Nodes.Add(retVal);
            SortSubNodes();

            Item.setVerified(false);

            return retVal;
        }

        public void AddHandler(object sender, EventArgs args)
        {
            DataDictionary.Rules.Rule rule = (DataDictionary.Rules.Rule)DataDictionary.Generated.acceptor.getFactory().createRule();
            rule.Name = "<Rule" + (GetNodeCount(false) + 1) + ">";

            DataDictionary.Rules.RuleCondition condition = (DataDictionary.Rules.RuleCondition)DataDictionary.Generated.acceptor.getFactory().createRuleCondition();
            condition.Name = "<Condition1>";
            rule.appendConditions(condition);

            RuleTreeNode node = AddRule(rule);
            node.ExpandAll();
        }

        /// <summary>
        /// Handles a drop event
        /// </summary>
        /// <param name="SourceNode"></param>
        public override void AcceptDrop(BaseTreeNode SourceNode)
        {
            if (SourceNode is RuleTreeNode)
            {
                if (MessageBox.Show("Are you sure you want to move the corresponding rule ?", "Move rule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RuleTreeNode ruleTreeNode = (RuleTreeNode)SourceNode;

                    DataDictionary.Rules.Rule rule = ruleTreeNode.Item;
                    ruleTreeNode.Delete();
                    AddRule(rule);
                }
            }
        }

        /// <summary>
        /// The menu items for this tree node
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
