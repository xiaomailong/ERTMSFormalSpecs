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

namespace GUI.RequirementSetDiagram
{
    public class RequirementSetPanel : BoxArrowPanel<RequirementSet, RequirementSetDependance>
    {
        private System.Windows.Forms.ToolStripMenuItem addRequirementSetMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDependanceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectRequirementsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;

        /// <summary>
        /// Initializes the start menu
        /// </summary>
        public void InitializeStartMenu()
        {
            addRequirementSetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addDependanceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            selectRequirementsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // addStateMenuItem
            // 
            addRequirementSetMenuItem.Name = "addRequirementSetMenuItem";
            addRequirementSetMenuItem.Size = new System.Drawing.Size(161, 22);
            addRequirementSetMenuItem.Text = "Add requirement set";
            addRequirementSetMenuItem.Click += new System.EventHandler(addBoxMenuItem_Click);
            // 
            // addTransitionMenuItem
            // 
            addDependanceMenuItem.Name = "addDependanceMenuItem";
            addDependanceMenuItem.Size = new System.Drawing.Size(161, 22);
            addDependanceMenuItem.Text = "Add dependance";
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
                addRequirementSetMenuItem,
                addDependanceMenuItem,
                toolStripSeparator,
                selectRequirementsMenuItem,
                toolStripSeparator,
                deleteMenuItem});
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RequirementSetPanel()
            : base()
        {
            InitializeStartMenu();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public RequirementSetPanel(IContainer container)
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
        public override BoxControl<RequirementSet, RequirementSetDependance> createBox(RequirementSet model)
        {
            BoxControl<RequirementSet, RequirementSetDependance> retVal = new RequirementSetControl();
            retVal.Model = model;

            return retVal;
        }

        /// <summary>
        /// Method used to create an arrow
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ArrowControl<RequirementSet, RequirementSetDependance> createArrow(RequirementSetDependance model)
        {
            ArrowControl<RequirementSet, RequirementSetDependance> retVal = new RequirementSetDependanceControl();
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
        public override List<RequirementSet> getBoxes()
        {
            return EFSSystem.RequirementSets;
        }

        /// <summary>
        /// Provides the arrows that need be displayed
        /// </summary>
        /// <returns></returns>
        public override List<RequirementSetDependance> getArrows()
        {
            List<RequirementSetDependance> retVal = new List<RequirementSetDependance>();

            foreach (RequirementSet requirementSet in EFSSystem.RequirementSets)
            {
                foreach (RequirementSetDependance dependance in requirementSet.Dependances)
                {
                    retVal.Add(dependance);
                }
            }

            return retVal;
        }

        private void addBoxMenuItem_Click(object sender, EventArgs e)
        {
            RequirementSet requirementSet = (RequirementSet)DataDictionary.Generated.acceptor.getFactory().createRequirementSet();
            requirementSet.Name = "<set " + (EFSSystem.RequirementSets.Count + 1) + ">";

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
                dictionary.appendRequirementSets(requirementSet);
            }

            RefreshControl();
        }

        private void addArrowMenuItem_Click(object sender, EventArgs e)
        {
            if (EFSSystem.RequirementSets.Count > 1)
            {
                RequirementSet source = null;
                RequirementSet target = null;
                RequirementSetControl sourceControl = Selected as RequirementSetControl;
                if (sourceControl != null)
                {
                    source = sourceControl.Model;
                    target = EFSSystem.RequirementSets[0];
                    if (target == source)
                    {
                        target = EFSSystem.RequirementSets[1];
                    }
                }
                else
                {
                    source = EFSSystem.RequirementSets[0];
                    target = EFSSystem.RequirementSets[1];
                }

                RequirementSetDependance dependance = (RequirementSetDependance)DataDictionary.Generated.acceptor.getFactory().createRequirementSetDependance();
                dependance.setTarget(target.Name);
                source.appendDependances(dependance);

                RefreshControl();
                Refresh();

                ArrowControl<RequirementSet, RequirementSetDependance> control = getArrowControl(dependance);
                Select(control, false);
            }
        }

        private void selectRequirements_Click(object sender, EventArgs e)
        {
            RequirementSetControl control = Selected as RequirementSetControl;

            if (control != null)
            {
                EFSSystem.MarkRequirementsForRequirementSet(control.Model);
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
