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
using System.ComponentModel;
using System.Windows.Forms;

namespace GUI.DataDictionaryView
{
    public class ProcedureTreeNode : ReqRelatedTreeNode<DataDictionary.Functions.Procedure>
    {
        private class ItemEditor : ReqRelatedEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }

            /// <summary>
            /// The item name
            /// </summary>
            [Category("Description")]
            public override string Name
            {
                get { return base.Name; }
                set { base.Name = value; }
            }
        }

        /// <summary>
        /// The rules associates to this procedure
        /// </summary>
        public ProcedureRulesTreeNode rules;

        /// <summary>
        /// The parameters of this procedure
        /// </summary>
        public ProcedureParametersTreeNode parameters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public ProcedureTreeNode(DataDictionary.Functions.Procedure item, bool buildSubNodes)
            : base(item, buildSubNodes)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public ProcedureTreeNode(DataDictionary.Functions.Procedure item, bool buildSubNodes, string name, bool isFolder, bool addRequirements)
            : base(item, buildSubNodes, name, isFolder, addRequirements)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        protected override void BuildSubNodes(bool buildSubNodes)
        {
            rules = new ProcedureRulesTreeNode(Item, buildSubNodes);
            parameters = new ProcedureParametersTreeNode(Item, buildSubNodes);
            Nodes.Add(parameters);
            Nodes.Add(rules);

            base.BuildSubNodes(buildSubNodes);
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

            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
            retVal.AddRange(base.GetMenuItems());

            return retVal;
        }
    }
}
