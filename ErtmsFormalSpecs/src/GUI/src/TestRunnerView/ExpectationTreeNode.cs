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
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace GUI.TestRunnerView
{
    public class ExpectationTreeNode : ModelElementTreeNode<DataDictionary.Tests.Expectation>
    {
        /// <summary>
        /// The value editor
        /// </summary>
        private class ItemEditor : Editor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }

            [Category("Description")]
            [System.ComponentModel.Editor(typeof(Converters.ExpressionableUITypedEditor), typeof(UITypeEditor))]
            [System.ComponentModel.TypeConverter(typeof(Converters.ExpressionableUITypeConverter))]
            public DataDictionary.Tests.Expectation Expression
            {
                get { return Item; }
                set
                {
                    Item = value;
                    RefreshNode();
                }
            }

            [Category("Description")]
            public bool Blocking
            {
                get { return Item.getBlocking(); }
                set { Item.setBlocking(value); }
            }

            [Category("Description"), TypeConverter(typeof(Converters.ExpectationKindConverter))]
            [ReadOnly(false)]
            public DataDictionary.Generated.acceptor.ExpectationKind Kind
            {
                get { return Item.getKind(); }
                set
                {
                    Item.setKind(value);
                    UpdateActivation();
                }
            }

            [Category("Description"), DisplayName("Condition"), Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
            [ReadOnly(false)]
            public string Condition
            {
                get { return Item.getCondition(); }
                set { Item.setCondition(value); }
            }

            [Category("Description")]
            public double DeadLine
            {
                get { return Item.DeadLine; }
                set { Item.DeadLine = value; }
            }

            /// <summary>
            /// The item name
            /// </summary>
            [Category("Description"), TypeConverter(typeof(Converters.CyclePhaseConverter))]
            public DataDictionary.Generated.acceptor.RulePriority CyclePhase
            {
                get { return Item.getCyclePhase(); }
                set { Item.setCyclePhase(value); }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public ExpectationTreeNode(DataDictionary.Tests.Expectation item, bool buildSubNodes)
            : base(item, buildSubNodes)
        {
        }

        /// <summary>
        /// Handles a selection change event
        /// </summary>
        /// <param name="displayStatistics">Indicates that statistics should be displayed in the MDI window</param>
        public override void SelectionChanged(bool displayStatistics)
        {
            base.SelectionChanged(displayStatistics);
            if (Item.Translation != null)
            {
                if (BaseTreeView != null && BaseTreeView.RefreshNodeContent)
                {
                    IBaseForm baseForm = BaseForm;
                    if (baseForm != null)
                    {
                        if (baseForm.RequirementsTextBox != null)
                        {
                            baseForm.RequirementsTextBox.Text = Item.Translation.getSourceTextExplain();
                        }
                    }
                }
            }
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

        /// <summary>
        /// Creates sub sequence tree nodes
        /// </summary>
        /// <param name="elements">The elements to be placed in the node</param>
        public static List<BaseTreeNode> CreateExpectations(System.Collections.ArrayList elements)
        {
            List<BaseTreeNode> retVal = new List<BaseTreeNode>();

            foreach (DataDictionary.Tests.Expectation expectation in elements)
            {
                retVal.Add(new ExpectationTreeNode(expectation, true));
            }

            return retVal;
        }
    }
}
