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
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using DataDictionary;
using DataDictionary.Functions;
using DataDictionary.Generated;
using DataDictionary.Interpreter;
using GUI.Converters;
using Utils;
using Case = DataDictionary.Functions.Case;
using Function = DataDictionary.Functions.Function;
using ModelElement = DataDictionary.ModelElement;
using Parameter = DataDictionary.Parameter;

namespace GUI.DataDictionaryView
{
    public class FunctionTreeNode : ReqRelatedTreeNode<Function>
    {
        private class InternalTypesConverter : TypesConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                ItemEditor editor = (ItemEditor) context.Instance;
                return GetValues(editor.Item);
            }
        }

        private class ItemEditor : TypeEditor
        {
            /// <summary>
            ///     Constructor
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
            ///     The variable type
            /// </summary>
            [Category("Description")]
            [Editor(typeof (TypeUITypedEditor), typeof (UITypeEditor))]
            [TypeConverter(typeof (TypeUITypeConverter))]
            public Function Type
            {
                get { return Item; }
                set
                {
                    Item = value;
                    RefreshNode();
                }
            }

            /// <summary>
            ///     Indicates that the function result can be cached, from one cycle to the other
            /// </summary>
            [Category("Description")]
            public bool IsCacheable
            {
                get { return Item.getCacheable(); }
                set { Item.setCacheable(value); }
            }
        }

        private CasesTreeNode Cases;
        private ParametersTreeNode Parameters;


        /// <summary>
        ///     The editor for message variables
        /// </summary>
        protected class TypeEditor : ReqRelatedEditor
        {
            /// <summary>
            ///     Constructor
            /// </summary>
            public TypeEditor()
                : base()
            {
            }
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="item"></param>
        public FunctionTreeNode(Function item, bool buildSubNodes)
            : base(item, buildSubNodes)
        {
        }

        /// <summary>
        ///     Builds the subnodes of this node
        /// </summary>
        /// <param name="buildSubNodes">Indicates whether the subnodes of the nodes should also be built</param>
        public override void BuildSubNodes(bool buildSubNodes)
        {
            base.BuildSubNodes(buildSubNodes);

            Parameters = new ParametersTreeNode(Item, buildSubNodes);
            Nodes.Add(Parameters);
            Cases = new CasesTreeNode(Item, buildSubNodes);
            Nodes.Add(Cases);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public FunctionTreeNode(Function item, bool buildSubNodes, string name, bool isFolder = false,
            bool addRequirements = true)
            : base(item, buildSubNodes, name, isFolder, addRequirements)
        {
        }

        /// <summary>
        ///     Creates the editor for this tree node
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
                Parameter parameter = (Parameter) acceptor.getFactory().createParameter();
                parameter.Name = "Parameter" + (Item.FormalParameters.Count + 1);
                Parameters.AddParameter(parameter);
            }
        }

        public void AddCaseHandler(object sender, EventArgs args)
        {
            DataDictionaryTreeView treeView = BaseTreeView as DataDictionaryTreeView;
            if (treeView != null)
            {
                Case aCase = (Case) acceptor.getFactory().createCase();
                aCase.Name = "Case" + (Item.Cases.Count + 1);
                Cases.AddCase(aCase);
            }
        }


        public void AddUpdate(object sender, EventArgs args)
        {
            // Choose the dictionary you want to add the update to
            // duplicate the function (+ namespace structure) to the
            //     in the new one, set the updates field to the guid of this function

            DataDictionary.Dictionary dictionary = GetPatchDictionary();

            if (dictionary != null)
            {

                if (dictionary.getUpdates() == null)
                {
                    dictionary.setUpdates(Item.Dictionary.Guid);
                }

                INamable patchedNamable = dictionary.findByFullName(Item.FullName);
                if (patchedNamable == null)
                {
                    // If the element does not already exist in the patch, add a copy of the function to it
                    // Get the enclosing namespace (by splitting the fullname and asking a recursive function to provide or make it)
                    String[] thing = Item.FullName.Split('.');
                    thing = thing.Take(thing.Count() - 1).ToArray();
                    NameSpace nameSpace = GetNameSpace(thing, dictionary);

                    // Add a copy of Item to it
                    ModelElement newFunction = Item.Duplicate();
                    newFunction.setUpdates(Item.Guid);
                    if ((newFunction as Function) != null)
                    {
                        nameSpace.appendFunctions(newFunction as Function);
                    }
                }

                // navigate to the function, whether it was created or not
            }
        }


        private DataDictionary.Types.NameSpace GetNameSpace(String[] levels, DataDictionary.Dictionary dictionary)
        {
            DataDictionary.Types.NameSpace nameSpace = OverallNameSpaceFinder.INSTANCE.findByName(dictionary, String.Join(".", levels));

            if (nameSpace == null)
            {
                nameSpace = new DataDictionary.Types.NameSpace();
                if (levels.Count() == 1)
                {
                    nameSpace.setName(levels[0]);
                    nameSpace.setUpdates(Item.Dictionary.findNameSpace(levels[0]).Guid);
                    dictionary.appendNameSpaces(nameSpace);
                }
                else
                {
                    String[] higherLevels = levels.Take(levels.Count() - 1).ToArray();
                    DataDictionary.Types.NameSpace parent = GetNameSpace(higherLevels, dictionary);
                    nameSpace.setName(levels[levels.Count() - 1]);
                    nameSpace.setUpdates(OverallNameSpaceFinder.INSTANCE.findByName(Item.Dictionary, String.Join(".", levels)).Guid);
                    parent.appendNameSpaces(nameSpace);
                }
            }

            return nameSpace;
        }

        
        /// <summary>
        ///     Provides the dictionary on which operation should be performed
        /// </summary>
        /// <returns></returns>
        public DataDictionary.Dictionary GetPatchDictionary()
        {
            DataDictionary.Dictionary retVal = null;

            MainWindow mainWindow = GUIUtils.MDIWindow;
            EFSSystem efsSystem = mainWindow.EFSSystem;
            if (efsSystem != null)
            {

                if (efsSystem.Dictionaries.Count == 1)
                {
                    retVal = efsSystem.Dictionaries[0];
                }
                else if (efsSystem.Dictionaries.Count > 1)
                {
                    DictionarySelector.DictionarySelector dictionarySelector =
                        new DictionarySelector.DictionarySelector(efsSystem);
                    dictionarySelector.ShowDialog(mainWindow);

                    if (dictionarySelector.Selected != null)
                    {
                        retVal = dictionarySelector.Selected;
                    }
                }
            }

            return retVal;
        }

        public void DisplayHandler(object sender, EventArgs args)
        {
            GraphView.GraphView view = new GraphView.GraphView();
            GUIUtils.MDIWindow.AddChildWindow(view);
            view.Functions.Add(Item);
            view.Refresh();
        }

        /// <summary>
        ///     The menu items for this tree node
        /// </summary>
        /// <returns></returns>
        protected override List<MenuItem> GetMenuItems()
        {
            List<MenuItem> retVal = new List<MenuItem>();

            MenuItem newItem = new MenuItem("Add...");
            newItem.MenuItems.Add(new MenuItem("Parameter", new EventHandler(AddParameterHandler)));
            newItem.MenuItems.Add(new MenuItem("Case", new EventHandler(AddCaseHandler)));
            newItem.MenuItems.Add(new MenuItem("Update", new EventHandler(AddUpdate)));
            retVal.Add(newItem);
            retVal.Add(new MenuItem("Delete", new EventHandler(DeleteHandler)));
            retVal.AddRange(base.GetMenuItems());

            bool previousSilentMode = ModelElement.BeSilent;
            try
            {
                ModelElement.BeSilent = true;
                InterpretationContext context = new InterpretationContext(Item);
                if (Item.FormalParameters.Count == 1)
                {
                    Parameter parameter = (Parameter) Item.FormalParameters[0];
                    Graph graph = Item.createGraph(context, parameter, null);
                    if (graph != null && graph.Segments.Count != 0)
                    {
                        retVal.Insert(7, new MenuItem("Display", new EventHandler(DisplayHandler)));
                        retVal.Insert(8, new MenuItem("-"));
                    }
                }
                else if (Item.FormalParameters.Count == 2)
                {
                    Surface surface = Item.createSurface(context, null);
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