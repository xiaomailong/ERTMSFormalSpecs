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
namespace GUI.DataDictionaryView
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class StructureStateMachinesTreeNode : DataTreeNode<DataDictionary.Types.Structure>
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
        /// <param name="name"></param>
        public StructureStateMachinesTreeNode(DataDictionary.Types.Structure item)
            : base(item, "State Machines", true)
        {
            foreach (DataDictionary.Types.StateMachine stateMachine in item.StateMachines)
            {
                Nodes.Add(new StateMachineTreeNode(stateMachine));
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

        public void AddHandler(object sender, EventArgs args)
        {
            DataDictionaryTreeView treeView = BaseTreeView as DataDictionaryTreeView;
            if (treeView != null)
            {
                DataDictionary.Types.StateMachine stateMachine = (DataDictionary.Types.StateMachine)DataDictionary.Generated.acceptor.getFactory().createStateMachine();
                stateMachine.Name = "<StateMachine" + (GetNodeCount(false) + 1) + ">";
                AddStateMachine(stateMachine);
            }
        }

        /// <summary>
        /// Adds a new state machine
        /// </summary>
        /// <param name="collection"></param>
        public StateMachineTreeNode AddStateMachine(DataDictionary.Types.StateMachine stateMachine)
        {
            StateMachineTreeNode retVal = new StateMachineTreeNode(stateMachine);
            Item.appendStateMachines(stateMachine);
            Nodes.Add(retVal);
            SortSubNodes();

            return retVal;
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = base.GetMenuItems();

            retVal.Add(new MenuItem("Add", new EventHandler(AddHandler)));

            return retVal;
        }


        /// <summary>
        /// Accepts drop of a tree node, in a drag & drop operation
        /// </summary>
        /// <param name="SourceNode"></param>
        public override void AcceptDrop(BaseTreeNode SourceNode)
        {
            base.AcceptDrop(SourceNode);

            if (SourceNode is StateMachineTreeNode)
            {
                StateMachineTreeNode stateMachineTreeNode = SourceNode as StateMachineTreeNode;
                DataDictionary.Types.StateMachine stateMachine = stateMachineTreeNode.Item;

                stateMachineTreeNode.Delete();
                AddStateMachine(stateMachine);
            }
            else if (SourceNode is SpecificationView.ParagraphTreeNode)
            {
                SpecificationView.ParagraphTreeNode node = SourceNode as SpecificationView.ParagraphTreeNode;
                DataDictionary.Specification.Paragraph paragaph = node.Item;

                DataDictionary.Types.StateMachine stateMachine = (DataDictionary.Types.StateMachine)DataDictionary.Generated.acceptor.getFactory().createStateMachine();
                stateMachine.Name = paragaph.Name;

                DataDictionary.ReqRef reqRef = (DataDictionary.ReqRef)DataDictionary.Generated.acceptor.getFactory().createReqRef();
                reqRef.Name = paragaph.FullId;
                stateMachine.appendRequirements(reqRef);
                AddStateMachine(stateMachine);
            }
        }
    }
}
