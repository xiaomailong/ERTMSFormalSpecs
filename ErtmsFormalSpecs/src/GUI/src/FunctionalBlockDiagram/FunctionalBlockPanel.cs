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
using System.Drawing;
using System.Windows.Forms;
using DataDictionary.Constants;
using DataDictionary.Types;
using GUI.BoxArrowDiagram;
using DataDictionary.Rules;
using DataDictionary.Variables;
using Utils;
using DataDictionary;
using DataDictionary.Specification;

namespace GUI.FunctionalBlockDiagram
{
    public class FunctionalBlockPanel : BoxArrowPanel<FunctionalBlock, FunctionalBlockDependance>
    {
        private System.Windows.Forms.ToolStripMenuItem addFunctionalBlockMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDependanceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectRequirementsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;

        /// <summary>
        /// Initializes the start menu
        /// </summary>
        public void InitializeStartMenu()
        {
            addFunctionalBlockMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addDependanceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            selectRequirementsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // addStateMenuItem
            // 
            addFunctionalBlockMenuItem.Name = "addFunctionalBlockMenuItem";
            addFunctionalBlockMenuItem.Size = new System.Drawing.Size(161, 22);
            addFunctionalBlockMenuItem.Text = "Add functional block";
            addFunctionalBlockMenuItem.Click += new System.EventHandler(addBoxMenuItem_Click);
            // 
            // addTransitionMenuItem
            // 
            addDependanceMenuItem.Name = "addDependanceMenuItem";
            addDependanceMenuItem.Size = new System.Drawing.Size(161, 22);
            addDependanceMenuItem.Text = "Add functional block dependance";
            addDependanceMenuItem.Click += new System.EventHandler(addArrowMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator.Name = "toolStripSeparator1";
            toolStripSeparator.Size = new System.Drawing.Size(158, 6);
            // 
            // select requirements
            // 
            selectRequirementsMenuItem.Name = "selectRequirementsMenuItem";
            selectRequirementsMenuItem.Size = new System.Drawing.Size(161, 22);
            selectRequirementsMenuItem.Text = "Select requirements";
            selectRequirementsMenuItem.Click += new System.EventHandler(selectRequirements_Click);
            // 
            // toolStripMenuItem1
            // 
            deleteMenuItem.Name = "toolStripMenuItem1";
            deleteMenuItem.Size = new System.Drawing.Size(153, 22);
            deleteMenuItem.Text = "Delete selected";
            deleteMenuItem.Click += new System.EventHandler(deleteMenuItem1_Click);

            contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                addFunctionalBlockMenuItem,
                addDependanceMenuItem,
                toolStripSeparator,
                selectRequirementsMenuItem,
                toolStripSeparator,
                deleteMenuItem});
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FunctionalBlockPanel()
            : base()
        {
            InitializeStartMenu();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public FunctionalBlockPanel(IContainer container)
            : base()
        {
            container.Add(this);

            InitializeStartMenu();
        }

        /// <summary>
        /// Method used to create a box
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override BoxControl<FunctionalBlock, FunctionalBlockDependance> createBox(FunctionalBlock model)
        {
            BoxControl<FunctionalBlock, FunctionalBlockDependance> retVal = new FunctionalBlockControl();
            retVal.Model = model;

            return retVal;
        }

        /// <summary>
        /// Method used to create an arrow
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ArrowControl<FunctionalBlock, FunctionalBlockDependance> createArrow(FunctionalBlockDependance model)
        {
            ArrowControl<FunctionalBlock, FunctionalBlockDependance> retVal = new FunctionalDependanceControl();
            retVal.Model = model;

            return retVal;
        }

        /// <summary>
        /// The EFSSystem for which this panel is built
        /// </summary>
        public EFSSystem EFSSystem { get; set; }

        /// <summary>
        /// Provides the boxes that need be displayed
        /// </summary>
        /// <returns></returns>
        public override List<FunctionalBlock> getBoxes()
        {
            return EFSSystem.FunctionalBlocks;
        }

        /// <summary>
        /// Provides the arrows that need be displayed
        /// </summary>
        /// <returns></returns>
        public override List<FunctionalBlockDependance> getArrows()
        {
            List<FunctionalBlockDependance> retVal = new List<FunctionalBlockDependance>();

            foreach (FunctionalBlock functionalBlock in EFSSystem.FunctionalBlocks)
            {
                foreach (FunctionalBlockDependance dependance in functionalBlock.Dependances)
                {
                    retVal.Add(dependance);
                }
            }

            return retVal;
        }

        private void addBoxMenuItem_Click(object sender, EventArgs e)
        {
            FunctionalBlock functionalBlock = (FunctionalBlock)DataDictionary.Generated.acceptor.getFactory().createFunctionalBlock();
            functionalBlock.Name = "<functional block " + (EFSSystem.FunctionalBlocks.Count + 1) + ">";

            DataDictionary.Dictionary dictionary;
            if (EFSSystem.Dictionaries.Count > 1)
            {
                DictionarySelector.DictionarySelector selector = new DictionarySelector.DictionarySelector(EFSSystem);
                selector.ShowDialog();
                dictionary = selector.Selected;
            }
            else
            {
                dictionary = EFSSystem.Dictionaries[0];
            }

            if (dictionary != null)
            {
                dictionary.appendFunctionalBlocks(functionalBlock);
            }

            RefreshControl();
        }

        private void addArrowMenuItem_Click(object sender, EventArgs e)
        {
            if (EFSSystem.FunctionalBlocks.Count > 1)
            {
                FunctionalBlock source = null;
                FunctionalBlock target = null;
                FunctionalBlockControl sourceControl = Selected as FunctionalBlockControl;
                if (sourceControl != null)
                {
                    source = sourceControl.Model;
                    target = EFSSystem.FunctionalBlocks[0];
                    if (target == source)
                    {
                        target = EFSSystem.FunctionalBlocks[1];
                    }
                }
                else
                {
                    source = EFSSystem.FunctionalBlocks[0];
                    target = EFSSystem.FunctionalBlocks[1];
                }

                FunctionalBlockDependance dependance = (FunctionalBlockDependance)DataDictionary.Generated.acceptor.getFactory().createFunctionalBlockDependance();
                dependance.setTarget(target.Name);
                source.appendDependances(dependance);

                RefreshControl();
                Refresh();

                ArrowControl<FunctionalBlock, FunctionalBlockDependance> control = getArrowControl(dependance);
                Select(control, false);
            }
        }

        private void selectRequirements_Click(object sender, EventArgs e)
        {
            FunctionalBlockControl control = Selected as FunctionalBlockControl;

            if ( control != null )
            {
                EFSSystem.MarkRequirementsForFunctionalBlock(control.Model);
            }
        }

        private void deleteMenuItem1_Click(object sender, EventArgs e)
        {
            IModelElement model = null;

            if (Selected is BoxControl<State, Transition>)
            {
                model = (Selected as BoxControl<State, Transition>).Model;
            }
            else if (Selected is ArrowControl<State, Transition>)
            {
                ArrowControl<State, Transition> control = Selected as ArrowControl<State, Transition>;
                RuleCondition ruleCondition = control.Model.RuleCondition;
                Rule rule = ruleCondition.EnclosingRule;
                if (rule.countConditions() == 1)
                {
                    model = rule;
                }
                else
                {
                    model = ruleCondition;
                }

            }

            if (GUIUtils.MDIWindow.DataDictionaryWindow != null)
            {
                BaseTreeNode node = GUIUtils.MDIWindow.DataDictionaryWindow.FindNode(model);
                if (node != null)
                {
                    node.Delete();
                }
            }
            Select(null, false);

            RefreshControl();
            Refresh();
        }
    }
}
