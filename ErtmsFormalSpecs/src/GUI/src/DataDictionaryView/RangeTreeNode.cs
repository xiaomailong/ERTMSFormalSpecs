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
using System.Drawing.Design;

namespace GUI.DataDictionaryView
{
    public class RangeTreeNode : TypeTreeNode<DataDictionary.Types.Range>
    {
        /// <summary>
        /// The editor for Range types
        /// </summary>
        private class ItemEditor : TypeEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }

            [Category("Description"), TypeConverter(typeof(Converters.RangePrecisionConverter))]
            public DataDictionary.Generated.acceptor.PrecisionEnum Precision
            {
                get { return Item.getPrecision(); }
                set { Item.setPrecision(value); }
            }

            [Category("Description")]
            public string MinValue
            {
                get { return Item.getMinValue(); }
                set { Item.setMinValue(value); }
            }

            [Category("Description")]
            public string MaxValue
            {
                get { return Item.getMaxValue(); }
                set { Item.setMaxValue(value); }
            }

            /// <summary>
            /// The range default value
            /// </summary>
            [Category("Description")]
            [System.ComponentModel.Editor(typeof(Converters.DefaultValueUITypedEditor), typeof(UITypeEditor))]
            [System.ComponentModel.TypeConverter(typeof(Converters.DefaultValueUITypeConverter))]
            public DataDictionary.Types.Range DefaultValue
            {
                get { return Item; }
                set
                {
                    Item = value;
                    RefreshNode();
                }
            }
        }

        RangeValuesTreeNode specialValues;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public RangeTreeNode(DataDictionary.Types.Range item, bool buildSubNodes)
            : base(item, buildSubNodes)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public RangeTreeNode(DataDictionary.Types.Range item, bool buildSubNodes, string name, bool isFolder, bool addRequirements)
            : base(item, buildSubNodes, name, isFolder, addRequirements)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            specialValues = new RangeValuesTreeNode(Item, buildSubNodes);
            Nodes.Add(specialValues);
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

        public virtual void AddSpecialValueHandler(object sender, EventArgs e)
        {
            specialValues.AddSpecialValueHandler(sender, e);
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add special value", new EventHandler(AddSpecialValueHandler)));
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
            retVal.AddRange(base.GetMenuItems());

            return retVal;
        }
    }
}
