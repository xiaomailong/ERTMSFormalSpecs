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
using DataDictionary;
using DataDictionary.Types;
using System.Drawing.Design;

namespace GUI.DataDictionaryView
{
    public class VariableTreeNode : ReqRelatedTreeNode<DataDictionary.Variables.Variable>
    {
        private class InternalTypesConverter : Converters.TypesConverter
        {
            public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
            {
                ItemEditor editor = (ItemEditor)context.Instance;
                return GetValues(editor.Item);
            }
        }

        private class InternalValuesConverter : Converters.ValuesConverter
        {
            public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
            {
                ItemEditor editor = (ItemEditor)context.Instance;
                DataDictionary.Types.NameSpace nameSpace = editor.Item.NameSpace;
                DataDictionary.Types.Type type = editor.Item.Type;

                return GetValues(nameSpace, type);
            }
        }

        /// <summary>
        /// The editor for Train data variables
        /// </summary>
        private class ItemEditor : ReqRelatedEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ItemEditor()
                : base()
            {
            }

            [Category("Description")]
            public override string Name
            {
                get { return base.Name; }
                set { base.Name = value; }
            }

            /// <summary>
            /// The variable type
            /// </summary>
            [Category("Description")]
            [System.ComponentModel.Editor(typeof(Converters.TypeUITypedEditor), typeof(UITypeEditor))]
            [System.ComponentModel.TypeConverter(typeof(Converters.TypeUITypeConverter))]
            public DataDictionary.Variables.Variable Type
            {
                get { return Item; }
                set
                {
                    Item = value;
                    RefreshNode();
                }
            }

            /// <summary>
            /// The variable default value
            /// </summary>
            [Category("Description")]
            [System.ComponentModel.Editor(typeof(Converters.DefaultValueUITypedEditor), typeof(UITypeEditor))]
            [System.ComponentModel.TypeConverter(typeof(Converters.DefaultValueUITypeConverter))]
            public DataDictionary.Variables.Variable DefaultValue
            {
                get { return Item; }
                set
                {
                    Item = value;
                    RefreshNode();
                }
            }

            /// <summary>
            /// The variable mode
            /// </summary>
            [Category("Description"), TypeConverter(typeof(Converters.VariableModeConverter))]
            public DataDictionary.Generated.acceptor.VariableModeEnumType Mode
            {
                get { return Item.getVariableMode(); }
                set { Item.setVariableMode(value); }
            }

            /// <summary>
            /// The variable value
            /// </summary>
            [Category("Description")]
            [System.ComponentModel.Editor(typeof(Converters.VariableValueUITypedEditor), typeof(UITypeEditor))]
            [System.ComponentModel.TypeConverter(typeof(Converters.VariableValueUITypeConverter))]
            public DataDictionary.Variables.Variable Value
            {
                get { return Item; }
                set
                {
                    Item = value;
                    RefreshNode();
                }
            }
        }

        public bool IsASubVariable;
        public SubVariablesTreeNode subVariables;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="children"></param>
        /// <param name="encounteredTypes">the types that have already been encountered in the path to create this variable </param>
        public VariableTreeNode(DataDictionary.Variables.Variable item, bool buildSubNodes, HashSet<DataDictionary.Types.Type> encounteredTypes, bool isASubVariable = false)
            : base(item, buildSubNodes)
        {
            encounteredTypes.Add(item.Type);
            IsASubVariable = isASubVariable;
            subVariables = new SubVariablesTreeNode(item, buildSubNodes, encounteredTypes);
            Nodes.Add(subVariables);
            encounteredTypes.Remove(item.Type);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="children"></param>
        /// <param name="encounteredTypes">the types that have already been encountered in the path to create this variable </param>
        public VariableTreeNode(DataDictionary.Variables.Variable item, bool buildSubNodes, string name, HashSet<DataDictionary.Types.Type> encounteredTypes, bool isASubVariable = false)
            : base(item, buildSubNodes, name, false)
        {
            encounteredTypes.Add(item.Type);
            IsASubVariable = isASubVariable;
            subVariables = new SubVariablesTreeNode(item, buildSubNodes, encounteredTypes);
            Nodes.Add(subVariables);
            encounteredTypes.Remove(item.Type);
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
        /// Display the associated state diagram
        /// </summary>
        public void ViewDiagram()
        {
            if (Item.Type is StateMachine)
            {
                StateDiagram.StateDiagramWindow window = new StateDiagram.StateDiagramWindow();
                GUIUtils.MDIWindow.AddChildWindow(window);
                window.SetStateMachine(Item);
                window.Text = Item.Name + " state diagram";
            }
        }

        protected void ViewStateDiagramHandler(object sender, EventArgs args)
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

            if (!IsASubVariable)
            {
                retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
                retVal.AddRange(base.GetMenuItems());
            }
            else
            {
                retVal.Add(new MenuItem("Refresh", new EventHandler(RefreshNodeHandler)));
            }

            DataDictionary.Functions.Function function = Item.Value as DataDictionary.Functions.Function;
            if (function != null)
            {
                DataDictionary.Interpreter.InterpretationContext context = new DataDictionary.Interpreter.InterpretationContext(Item);
                if (function.FormalParameters.Count == 1)
                {
                    Parameter parameter = (Parameter)function.FormalParameters[0];
                    DataDictionary.Functions.Graph graph = function.createGraph(context, parameter);
                    if (graph != null && graph.Segments.Count != 0)
                    {
                        retVal.Insert(5, new MenuItem("-"));
                        retVal.Insert(6, new MenuItem("Display", new EventHandler(DisplayHandler)));
                    }
                }
                else if (function.FormalParameters.Count == 2)
                {
                    DataDictionary.Functions.Surface surface = function.createSurface(context);
                    if (surface != null && surface.Segments.Count != 0)
                    {
                        retVal.Insert(5, new MenuItem("-"));
                        retVal.Insert(6, new MenuItem("Display", new EventHandler(DisplayHandler)));
                    }
                }
            }

            if (Item.Type is StateMachine)
            {
                retVal.Insert(5, new MenuItem("-"));
                retVal.Insert(6, new MenuItem("View state diagram", new EventHandler(ViewStateDiagramHandler)));
            }

            return retVal;
        }


        public void DisplayHandler(object sender, EventArgs args)
        {
            DataDictionary.Functions.Function function = Item.Value as DataDictionary.Functions.Function;
            if (function != null)
            {
                GraphView.GraphView view = new GraphView.GraphView();
                GUIUtils.MDIWindow.AddChildWindow(view);
                view.Functions.Add(function);
                view.Refresh();
            }
        }

        public override void RefreshNode()
        {
            if (Nodes != null && subVariables != null)
            {
                Nodes.Remove(subVariables);
                subVariables = new SubVariablesTreeNode(Item, true, new HashSet<DataDictionary.Types.Type>());
                Nodes.Add(subVariables);
            }
            base.RefreshNode();
        }
    }
}
