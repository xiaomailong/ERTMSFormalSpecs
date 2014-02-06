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
    public class StateTreeNode : ReqRelatedTreeNode<DataDictionary.Constants.State>
    {
        private class InternalStateTypeConverter : Converters.StateTypeConverter
        {
            public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
            {
                return GetValues(((ItemEditor)context.Instance).Item);
            }
        }

        private class ItemEditor : ReqRelatedEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }

            [Category("Default")]
            public override string Name
            {
                get { return Item.Name; }
                set { Item.Name = value; }
            }

            [Category("Default"), TypeConverter(typeof(InternalStateTypeConverter))]
            public string InitialState
            {
                get { return Item.StateMachine.InitialState; }
                set { Item.StateMachine.InitialState = value; }
            }
        }

        /// <summary>
        /// The sub state machine
        /// </summary>
        public StateSubStatesTreeNode SubStates;
        public StateRulesTreeNode Rules;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public StateTreeNode(DataDictionary.Constants.State item, bool buildSubNodes)
            : base(item, buildSubNodes, null, false, true)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            base.BuildSubNodes(buildSubNodes);

            SubStates = new StateSubStatesTreeNode(Item, buildSubNodes);
            Nodes.Add(SubStates);
            Rules = new StateRulesTreeNode(Item, buildSubNodes);
            Nodes.Add(Rules);
        }

        /// <summary>
        /// Creates the editor for this tree node
        /// </summary>
        /// <returns></returns>
        protected override Editor createEditor()
        {
            return new ItemEditor();
        }

        public void AddStateHandler(object sender, EventArgs args)
        {
            DataDictionary.Constants.State state = (DataDictionary.Constants.State)DataDictionary.Generated.acceptor.getFactory().createState();
            state.Name = "State" + (GetNodeCount(true) + 1);
            Item.StateMachine.appendStates(state);
            Nodes.Add(new StateTreeNode(state, true));
            SortSubNodes();
        }

        /// <summary>
        /// Handles the drop event
        /// </summary>
        /// <param name="SourceNode"></param>
        public override void AcceptDrop(BaseTreeNode SourceNode)
        {
            if (SourceNode is StateMachineTreeNode)
            {
                if (MessageBox.Show("Are you sure you want to override the state machine ? ", "Override state machine", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    StateMachineTreeNode stateMachineTreeNode = (StateMachineTreeNode)SourceNode;
                    DataDictionary.Types.StateMachine stateMachine = stateMachineTreeNode.Item;
                    stateMachineTreeNode.Delete();

                    // Update the model
                    Item.StateMachine = stateMachine;

                    // Update the view
                    TreeNode parent = Parent;
                    parent.Nodes.Remove(this);
                    parent.Nodes.Add(stateMachineTreeNode);
                }
            }

            base.AcceptDrop(SourceNode);
        }

        /// <summary>
        /// Display the associated state diagram
        /// </summary>
        public void ViewDiagram()
        {
            StateDiagram.StateDiagramWindow window = new StateDiagram.StateDiagramWindow();
            GUIUtils.MDIWindow.AddChildWindow(window);
            window.SetStateMachine(Item.StateMachine);
            window.Text = Item.Name + " state diagram";
        }

        protected void ViewStateDiagramHandler(object sender, EventArgs args)
        {
            ViewDiagram();
        }

        public override void DoubleClickHandler()
        {
            ViewDiagram();
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            retVal.Add(new MenuItem("Add sub state", new EventHandler(AddStateHandler)));
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
            retVal.AddRange(base.GetMenuItems());
            retVal.Insert(6, new MenuItem("-"));
            retVal.Insert(7, new MenuItem("View state diagram", new EventHandler(ViewStateDiagramHandler)));

            return retVal;
        }
    }
}
