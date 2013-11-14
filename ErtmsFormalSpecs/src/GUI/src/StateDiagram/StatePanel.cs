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

namespace GUI.StateDiagram
{
    public class StatePanel : BoxArrowPanel<State, Transition>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StatePanel()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public StatePanel(IContainer container)
            : base()
        {
            container.Add(this);
        }

        /// <summary>
        /// Method used to create a box
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override BoxControl<State, Transition> createBox(State model)
        {
            BoxControl<State, Transition> retVal = new StateControl();
            retVal.Model = model;

            return retVal;
        }

        /// <summary>
        /// Method used to create an arrow
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ArrowControl<State, Transition> createArrow(Transition model)
        {
            ArrowControl<State, Transition> retVal = new TransitionControl();
            retVal.Model = model;

            return retVal;
        }

        /// <summary>
        /// The state machine displayed by this panel
        /// </summary>
        public StateMachine StateMachine { get; set; }

        /// <summary>
        /// The state machine variable (if any) displayed by this panel
        /// </summary>
        public IVariable StateMachineVariable { get; set; }

        /// <summary>
        /// Provides the boxes that need be displayed
        /// </summary>
        /// <returns></returns>
        public override List<State> getBoxes()
        {
            List<State> retVal = new List<State>();

            foreach (State state in StateMachine.States)
            {
                retVal.Add(state);
            }

            return retVal;
        }

        /// <summary>
        /// Provides the arrows that need be displayed
        /// </summary>
        /// <returns></returns>
        public override List<Transition> getArrows()
        {
            return StateMachine.Transitions;
        }
    }
}
