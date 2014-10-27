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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataDictionary;
using DataDictionary.Values;
using System.Collections;
using DataDictionary.Variables;
using DataDictionary.Interpreter;

namespace GUI.StructureValueEditor
{
    public partial class Window : BaseForm
    {
        /// <summary>
        /// The variable currently being displayed, if any
        /// </summary>
        private IVariable Variable { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Window()
        {
            InitializeComponent();

            // The text to get for each column
            structureTreeListView.GetColumn(0).AspectGetter = CustomizeTreeView.FieldColumnStringonizer;
            structureTreeListView.GetColumn(1).AspectGetter = CustomizeTreeView.ValueColumnStringonizer;
            structureTreeListView.GetColumn(2).AspectGetter = CustomizeTreeView.DescriptionColumnStringonizer;
            structureTreeListView.FormatCell += new EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(CustomizeTreeView.FormatCell);

            // Tree structure
            structureTreeListView.CanExpandGetter = CustomizeTreeView.HasChildren;
            structureTreeListView.ChildrenGetter = CustomizeTreeView.GetChildren;

            // Contextual menu
            structureTreeListView.CellRightClick += new EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(CustomizeTreeView.CreateContextualMenu);

            // Edition
            structureTreeListView.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(CustomizeTreeView.HandleCellEditStarting);
            structureTreeListView.CellEditValidating += new BrightIdeasSoftware.CellEditEventHandler(CustomizeTreeView.HandleCellEditValidating);
            structureTreeListView.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(CustomizeTreeView.HandleCellEditFinishing);

            structureTreeListView.ItemDrag += new ItemDragEventHandler(structureTreeListView_ItemDrag);

            FormClosed += new FormClosedEventHandler(Window_FormClosed);
        }

        void structureTreeListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            GUIUtils.MDIWindow.HandleSubWindowClosed(this);
        }

        /// <summary>
        /// Sets the model for this tree view
        /// </summary>
        /// <param name="model"></param>
        public void SetModel(IValue model)
        {
            List<IValue> ObjectModel = new List<IValue>();

            ListValue listValue = model as ListValue;
            if (listValue != null)
            {
                foreach (IValue value in listValue.Val)
                {
                    if (value != DataDictionary.EFSSystem.INSTANCE.EmptyValue)
                    {
                        ObjectModel.Add(value);
                    }
                }
            }
            else
            {
                ObjectModel.Add(model);
            }

            structureTreeListView.SetObjects(ObjectModel);
        }

        /// <summary>
        /// Sets the variable as data source for this window
        /// </summary>
        /// <param name="variable"></param>
        public void SetVariable(IVariable variable)
        {
            Variable = variable;

            Text = Variable.FullName;
            List<IVariable> ObjectModel = new List<IVariable>();
            ObjectModel.Add(variable);
            structureTreeListView.SetObjects(ObjectModel);
        }

        /// <summary>
        /// Refresh the window contents when the model may have changed
        /// </summary>
        public void RefreshAfterStep()
        {
            if (Variable != null)
            {
                Expression expression = EFSSystem.INSTANCE.Parser.Expression(Utils.EnclosingFinder<Dictionary>.find(Variable), Variable.FullName);
                IVariable variable = expression.GetVariable(new InterpretationContext());
                if (variable != Variable)
                {
                    SetVariable(variable);
                }
                else
                {
                    structureTreeListView.RefreshObject(Variable);
                    structureTreeListView.Refresh();
                }
            }
        }
    }
}
