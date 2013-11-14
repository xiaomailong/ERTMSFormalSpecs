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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DataDictionary.Constants;
using DataDictionary.Rules;
using DataDictionary.Types;
using DataDictionary.Variables;
using Utils;
using GUI.BoxArrowDiagram;

namespace GUI.StateDiagram
{
    public partial class StateDiagramWindow : BoxArrowWindow<State, Transition>
    {
        /// <summary>
        /// The state machine currently displayed
        /// </summary>
        public StateMachine StateMachine { get; private set; }

        private System.Windows.Forms.ToolStripMenuItem addStateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTransitionMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addStateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTransitionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addStateMenuItem,
            this.addTransitionMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1});
            // 
            // addStateMenuItem
            // 
            this.addStateMenuItem.Name = "addStateMenuItem";
            this.addStateMenuItem.Size = new System.Drawing.Size(161, 22);
            this.addStateMenuItem.Text = "Add State";
            this.addStateMenuItem.Click += new System.EventHandler(this.addBoxMenuItem_Click);
            // 
            // addTransitionMenuItem
            // 
            this.addTransitionMenuItem.Name = "addTransitionMenuItem";
            this.addTransitionMenuItem.Size = new System.Drawing.Size(161, 22);
            this.addTransitionMenuItem.Text = "Add transition";
            this.addTransitionMenuItem.Click += new System.EventHandler(this.addArrowMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem1.Text = "Delete selected";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.deleteMenuItem1_Click);
        }

        /// <summary>
        /// The state machine variable, if any
        /// </summary>
        public IVariable StateMachineVariable { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StateDiagramWindow()
            : base()
        {
        }

        /// <summary>
        /// The panel used to display the state diagram
        /// </summary>
        private StatePanel StatePanel { get { return (StatePanel)BoxArrowContainerPanel; } }

        /// <summary>
        /// Sets the state machine type
        /// </summary>
        /// <param name="stateMachine"></param>
        public void SetStateMachine(StateMachine stateMachine)
        {
            StateMachine = stateMachine;

            StatePanel.StateMachine = StateMachine;
            StatePanel.RefreshControl();
        }

        /// <summary>
        /// Sets the state machine variable (and type)
        /// </summary>
        /// <param name="stateMachine">The state machine variable to display</param>
        /// <param name="stateMachineType">The state machine type which should be displayed. If null, the default state machine is displayed</param>
        public void SetStateMachine(IVariable stateMachine, StateMachine stateMachineType = null)
        {
            if (stateMachineType == null)
            {
                stateMachineType = stateMachine.Type as StateMachine;
            }

            if (stateMachineType != null)
            {
                StateMachine = stateMachineType;
                StateMachineVariable = stateMachine;
            }

            StatePanel.StateMachine = StateMachine;
            StatePanel.StateMachineVariable = StateMachineVariable;
            StatePanel.RefreshControl();
        }

        public override BoxArrowPanel<State, Transition> createPanel()
        {
            BoxArrowPanel<State, Transition> retVal = new StatePanel();

            return retVal;
        }

        /// <summary>
        /// A box editor
        /// </summary>
        protected class StateEditor : BoxEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="control"></param>
            public StateEditor(BoxControl<State, Transition> control)
                : base(control)
            {
            }
        }

        /// <summary>
        /// Factory for BoxEditor
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        protected override BoxEditor createBoxEditor(BoxControl<State, Transition> control)
        {
            BoxEditor retVal = new StateEditor(control);

            return retVal;
        }

        protected class InternalStateTypeConverter : Converters.StateTypeConverter
        {
            public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
            {
                TransitionEditor instance = (TransitionEditor)context.Instance;
                StatePanel panel = (StatePanel)instance.control.BoxArrowPanel;
                return GetValues(panel.StateMachine);
            }
        }

        /// <summary>
        /// An arrow editor
        /// </summary>
        protected class TransitionEditor : ArrowEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="control"></param>
            public TransitionEditor(ArrowControl<State, Transition> control)
                : base(control)
            {
            }

            [Category("Description"), TypeConverter(typeof(InternalStateTypeConverter))]
            public string Source
            {
                get
                {
                    string retVal = "";

                    if (control.Model.Source != null)
                    {
                        retVal = control.Model.Source.Name;
                    }
                    return retVal;
                }
                set
                {
                    TransitionControl transitionControl = (TransitionControl)control;
                    StatePanel statePanel = (StatePanel)transitionControl.Panel;
                    State state = DataDictionary.OverallStateFinder.INSTANCE.findByName(statePanel.StateMachine, value);
                    if (state != null)
                    {
                        control.SetInitialBox(state);
                        control.RefreshControl();
                    }
                }
            }

            [Category("Description"), TypeConverter(typeof(InternalStateTypeConverter))]
            public string Target
            {
                get
                {
                    string retVal = "";

                    if (control.Model != null && control.Model.Target != null)
                    {
                        retVal = control.Model.Target.Name;
                    }

                    return retVal;
                }
                set
                {
                    TransitionControl transitionControl = (TransitionControl)control;
                    StatePanel statePanel = (StatePanel)transitionControl.Panel;
                    State state = DataDictionary.OverallStateFinder.INSTANCE.findByName(statePanel.StateMachine, value);
                    if (state != null)
                    {
                        control.SetTargetBox(state);
                        control.RefreshControl();
                    }
                }
            }
        }

        /// <summary>
        /// Factory for arrow editor
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        protected override ArrowEditor createArrowEditor(ArrowControl<State, Transition> control)
        {
            ArrowEditor retVal = new TransitionEditor(control);

            return retVal;
        }

        private void addBoxMenuItem_Click(object sender, EventArgs e)
        {
            State state = (State)DataDictionary.Generated.acceptor.getFactory().createState();
            state.Name = "State" + (StateMachine.States.Count + 1);

            if (MDIWindow.DataDictionaryWindow != null)
            {
                DataDictionaryView.StateMachineTreeNode node = MDIWindow.DataDictionaryWindow.FindNode(StateMachine) as DataDictionaryView.StateMachineTreeNode;
                if (node != null)
                {
                    node.AddState(state);
                }
                else
                {
                    StateMachine.appendStates(state);
                }
            }

            BoxArrowContainerPanel.RefreshControl();
        }

        private void addArrowMenuItem_Click(object sender, EventArgs e)
        {
            if (StateMachine.States.Count > 1)
            {
                DataDictionary.ObjectFactory factory = (DataDictionary.ObjectFactory)DataDictionary.Generated.acceptor.getFactory();
                DataDictionary.Rules.Rule rule = (DataDictionary.Rules.Rule)factory.createRule();
                rule.Name = "Rule" + (StateMachine.Rules.Count + 1);

                DataDictionary.Rules.RuleCondition ruleCondition = (DataDictionary.Rules.RuleCondition)factory.createRuleCondition();
                ruleCondition.Name = "RuleCondition" + (rule.RuleConditions.Count + 1);
                rule.appendConditions(ruleCondition);

                DataDictionary.Rules.Action action = (DataDictionary.Rules.Action)factory.createAction();
                action.Expression = "THIS <- " + ((State)StateMachine.States[1]).LiteralName;
                ruleCondition.appendActions(action);

                if (MDIWindow.DataDictionaryWindow != null)
                {
                    DataDictionaryView.StateTreeNode stateNode = MDIWindow.DataDictionaryWindow.FindNode((State)StateMachine.States[0]) as DataDictionaryView.StateTreeNode;
                    DataDictionaryView.RuleTreeNode ruleNode = stateNode.Rules.AddRule(rule);
                }

                BoxArrowContainerPanel.RefreshControl();
                BoxArrowContainerPanel.Refresh();

                ArrowControl<State, Transition> control = BoxArrowContainerPanel.getArrowControl(ruleCondition);
                Select(control);
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

            if (MDIWindow.DataDictionaryWindow != null)
            {
                BaseTreeNode node = MDIWindow.DataDictionaryWindow.FindNode(model);
                if (node != null)
                {
                    node.Delete();
                }
            }
            Select(null);

            BoxArrowContainerPanel.RefreshControl();
            BoxArrowContainerPanel.Refresh();
        }
    }
}
