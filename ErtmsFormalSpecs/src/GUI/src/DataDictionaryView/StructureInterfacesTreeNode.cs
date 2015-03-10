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
using GUI.SpecificationView;
using NameSpace = DataDictionary.Types.NameSpace;
using Paragraph = DataDictionary.Specification.Paragraph;
using ReqRef = DataDictionary.ReqRef;
using Structure = DataDictionary.Types.Structure;
using StructureElement = DataDictionary.Types.StructureElement;

namespace GUI.DataDictionaryView
{
    public class InterfacesTreeNode : ModelElementTreeNode<Structure>
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
        public InterfacesTreeNode(Structure item, bool buildSubNodes)
            : base(item, buildSubNodes, "Interfaces", true)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            base.BuildSubNodes(buildSubNodes);

            foreach (StructureRef structureRef in Item.InterfaceRefs)
            {
                Nodes.Add(new StructureInterfaceTreeNode(structureRef, buildSubNodes));
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
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add", new EventHandler(AddHandler)));
            retVal.AddRange(base.GetMenuItems());

            return retVal;
        }

        public void AddHandler(object sender, EventArgs args)
        {
            StructureRef structureRef = (StructureRef)acceptor.getFactory().createStructureRef();
            structureRef.Name = "<Interface" + (GetNodeCount(false) + 1) + ">";
            AddInterface(structureRef);
        }

        /// <summary>
        /// Adds a new structure interface to the structure
        /// </summary>
        /// <param name="rule"></param>
        public void AddInterface(StructureRef structureRef)
        {
            Item.appendInterfaces(structureRef);
            Nodes.Add(new StructureInterfaceTreeNode(structureRef, true));
            SortSubNodes();
        }
    }
}
