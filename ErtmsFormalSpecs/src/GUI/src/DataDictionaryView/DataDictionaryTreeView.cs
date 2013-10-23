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
using System.Collections.Generic;
namespace GUI.DataDictionaryView
{
    public class DataDictionaryTreeView : TypedTreeView<DataDictionary.Dictionary>
    {
        protected override void BuildModel()
        {
            Nodes.Clear();
            Nodes.Add(new RuleDisablingsTreeNode(Root));
            Nodes.Add(new NameSpacesTreeNode(Root));
        }

        /// <summary>
        /// Recursively refreshes a node
        /// </summary>
        /// <param name="node"></param>
        private void RefreshAfterStepNode(BaseTreeNode node)
        {
            foreach (BaseTreeNode subNode in node.Nodes)
            {
                VariableTreeNode variableTreeNode = subNode as VariableTreeNode;
                if (variableTreeNode != null)
                {
                    HashSet<DataDictionary.Types.Type> encounteredTypes = new HashSet<DataDictionary.Types.Type>();
                    SubVariablesTreeNode tmp = new SubVariablesTreeNode(variableTreeNode.Item, encounteredTypes);
                    variableTreeNode.subVariables.Nodes.Clear();
                    foreach (BaseTreeNode tt in tmp.Nodes)
                    {
                        variableTreeNode.subVariables.Nodes.Add(tt);
                    }
                }
                else
                {
                    RefreshAfterStepNode(subNode);
                }
            }
        }

        /// <summary>
        /// Refreshes the view after a step has been executed
        /// </summary>
        public void RefreshAfterStep()
        {
            foreach (BaseTreeNode node in Nodes)
            {
                RefreshAfterStepNode(node);
            }
        }
    }
}
