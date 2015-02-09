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
using DataDictionary.Types;

namespace GUI.DataDictionaryView
{
    public class StructureTreeNode : TypeTreeNode<Structure>
    {
        private class ItemEditor : TypeEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }
        }

        private NameSpaceTreeNode types
        {
            get { return Parent.Parent as NameSpaceTreeNode; }
        }

        private RulesTreeNode rules;
        private StructureElementsTreeNode elements;
        private StructureStateMachinesTreeNode stateMachines;
        private StructureProceduresTreeNode procedures;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public StructureTreeNode(Structure item, bool buildSubNodes)
            : base(item, buildSubNodes)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public StructureTreeNode(Structure item, bool buildSubNodes, string name, bool isFolder, bool addRequirements)
            : base(item, buildSubNodes, name, isFolder, addRequirements)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            base.BuildSubNodes(buildSubNodes);

            rules = new RulesTreeNode(Item, buildSubNodes);
            elements = new StructureElementsTreeNode(Item, buildSubNodes);
            procedures = new StructureProceduresTreeNode(Item, buildSubNodes);
            stateMachines = new StructureStateMachinesTreeNode(Item, buildSubNodes);
            Nodes.Add(procedures);
            Nodes.Add(stateMachines);
            Nodes.Add(elements);
            Nodes.Add(rules);
        }

        /// <summary>
        /// Creates the editor for this tree node
        /// </summary>
        /// <returns></returns>
        protected override Editor createEditor()
        {
            return new ItemEditor();
        }

        public void AddStructureElement(StructureElement element)
        {
            elements.AddElement(element);
        }

        private void AddProcedureHandler(object sender, EventArgs args)
        {
            if (procedures != null)
            {
                procedures.AddHandler(sender, args);
            }
        }

        private void AddStructureElementHandler(object sender, EventArgs args)
        {
            if (elements != null)
            {
                elements.AddStructureElementHandler(sender, args);
            }
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            MenuItem newItem = new MenuItem("Add...");
            newItem.MenuItems.Add(new MenuItem("Procedure", new EventHandler(AddProcedureHandler)));
            newItem.MenuItems.Add(new MenuItem("Structure element", new EventHandler(AddStructureElementHandler)));
            retVal.Add(newItem);
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
            retVal.AddRange(base.GetMenuItems());

            return retVal;
        }
    }
}