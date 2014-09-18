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
using DataDictionary.Functions;
using System.Drawing.Design;

namespace GUI.DataDictionaryView
{
    public class FunctionTreeNode : ReqRelatedTreeNode<Function>
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

        private class ItemEditor : TypeEditor
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
            public DataDictionary.Functions.Function Type
            {
                get { return Item; }
                set
                {
                    Item = value;
                    RefreshNode();
                }
            }

            /// <summary>
            /// Indicates that the function result can be cached, from one cycle to the other
            /// </summary>
            [Category("Description")]
            public bool IsCacheable
            {
                get { return Item.getCacheable(); }
                set
                {
                    Item.setCacheable(value);
                }
            }
        }

        private CasesTreeNode Cases;
        private ParametersTreeNode Parameters;


        /// <summary>
        /// The editor for message variables
        /// </summary>
        protected class TypeEditor : ReqRelatedEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public TypeEditor()
                : base()
            {
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public FunctionTreeNode(Function item, bool buildSubNodes)
            : base(item, buildSubNodes)
        {
        }

        /// <summary>
        /// Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates that subnodes of the nodes built should also </param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            base.BuildSubNodes(buildSubNodes);

            Parameters = new ParametersTreeNode(Item, buildSubNodes);
            Nodes.Add(Parameters);
            Cases = new CasesTreeNode(Item, buildSubNodes);
            Nodes.Add(Cases);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public FunctionTreeNode(Function item, bool buildSubNodes, string name, bool isFolder = false, bool addRequirements = true)
            : base(item, buildSubNodes, name, isFolder, addRequirements)
        {
        }

        /// <summary>
        /// Creates the editor for this tree node
        /// </summary>
        /// <returns></returns>
        protected override Editor createEditor()
        {
            return new ItemEditor();
        }

        public void AddParameterHandler(object sender, EventArgs args)
        {
            DataDictionaryTreeView treeView = BaseTreeView as DataDictionaryTreeView;
            if (treeView != null)
            {
                DataDictionary.Parameter parameter = (DataDictionary.Parameter)DataDictionary.Generated.acceptor.getFactory().createParameter();
                parameter.Name = "Parameter" + (Item.FormalParameters.Count + 1);
                Parameters.AddParameter(parameter);
            }
        }

        public void AddCaseHandler(object sender, EventArgs args)
        {
            DataDictionaryTreeView treeView = BaseTreeView as DataDictionaryTreeView;
            if (treeView != null)
            {
                DataDictionary.Functions.Case aCase = (DataDictionary.Functions.Case)DataDictionary.Generated.acceptor.getFactory().createCase();
                aCase.Name = "Case" + (Item.Cases.Count + 1);
                Cases.AddCase(aCase);
            }
        }

        public void DisplayHandler(object sender, EventArgs args)
        {
            GraphView.GraphView view = new GraphView.GraphView();
            GUIUtils.MDIWindow.AddChildWindow(view);
            view.Functions.Add(Item);
            view.Refresh();
        }

        /// <summary>
        /// The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            MenuItem newItem = new MenuItem("Add...");
            newItem.MenuItems.Add(new MenuItem("Parameter", new EventHandler(AddParameterHandler)));
            newItem.MenuItems.Add(new MenuItem("Case", new EventHandler(AddCaseHandler)));
            retVal.Add(newItem);
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
            retVal.AddRange(base.GetMenuItems());

            bool previousSilentMode = ModelElement.BeSilent;
            try
            {
                ModelElement.BeSilent = true;
                DataDictionary.Interpreter.InterpretationContext context = new DataDictionary.Interpreter.InterpretationContext(Item);
                if (Item.FormalParameters.Count == 1)
                {
                    Parameter parameter = (Parameter)Item.FormalParameters[0];
                    DataDictionary.Functions.Graph graph = Item.createGraph(context, parameter, null);
                    if (graph != null && graph.Segments.Count != 0)
                    {
                        retVal.Insert(7, new MenuItem("Display", new EventHandler(DisplayHandler)));
                        retVal.Insert(8, new MenuItem("-"));
                    }
                }
                else if (Item.FormalParameters.Count == 2)
                {
                    DataDictionary.Functions.Surface surface = Item.createSurface(context, null);
                    if (surface != null && surface.Segments.Count != 0)
                    {
                        retVal.Insert(7, new MenuItem("Display", new EventHandler(DisplayHandler)));
                        retVal.Insert(8, new MenuItem("-"));
                    }
                }
            }
            finally
            {
                ModelElement.BeSilent = previousSilentMode;
            }

            return retVal;
        }
    }
}
