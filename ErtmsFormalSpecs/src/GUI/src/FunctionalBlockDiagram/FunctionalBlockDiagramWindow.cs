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
using DataDictionary;
using DataDictionary.Constants;
using DataDictionary.Rules;
using DataDictionary.Types;
using DataDictionary.Variables;
using Utils;
using GUI.BoxArrowDiagram;
using DataDictionary.Specification;

namespace GUI.FunctionalBlockDiagram
{
    public partial class FunctionalDiagramWindow : BoxArrowWindow<FunctionalBlock, FunctionalBlockDependance>
    {
        /// <summary>
        /// The system for which the functional diagram is built
        /// </summary>
        public EFSSystem EFSSystem { get; private set; }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="system"></param>
        public FunctionalDiagramWindow()
            : base()
        {
        }

        /// <summary>
        /// The panel used to display the state diagram
        /// </summary>
        private FunctionalBlockPanel FunctionalBlockPanel { get { return (FunctionalBlockPanel)BoxArrowContainerPanel; } }

        /// <summary>
        /// Sets the state machine type
        /// </summary>
        /// <param name="stateMachine"></param>
        public void SetSystem(EFSSystem system)
        {
            EFSSystem = system;

            FunctionalBlockPanel.EFSSystem = EFSSystem;
            FunctionalBlockPanel.RefreshControl();
        }

        public override BoxArrowPanel<FunctionalBlock, FunctionalBlockDependance> createPanel()
        {
            BoxArrowPanel<FunctionalBlock, FunctionalBlockDependance> retVal = new FunctionalBlockPanel();

            return retVal;
        }

        /// <summary>
        /// A box editor
        /// </summary>
        protected class FunctionalBlockEditor : BoxEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="control"></param>
            public FunctionalBlockEditor(BoxControl<FunctionalBlock, FunctionalBlockDependance> control)
                : base(control)
            {
            }
        }

        /// <summary>
        /// Factory for BoxEditor
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        protected override BoxEditor createBoxEditor(BoxControl<FunctionalBlock, FunctionalBlockDependance> control)
        {
            BoxEditor retVal = new FunctionalBlockEditor(control);

            return retVal;
        }

        protected class InternalFunctionalBlockTypeConverter : Converters.FunctionalBlockTypeConverter
        {
            public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
            {
                TransitionEditor instance = (TransitionEditor)context.Instance;
                FunctionalBlockPanel panel = (FunctionalBlockPanel)instance.control.BoxArrowPanel;
                return GetValues(panel.EFSSystem);
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
            public TransitionEditor(ArrowControl<FunctionalBlock, FunctionalBlockDependance> control)
                : base(control)
            {
            }

            [Category("Description"), TypeConverter(typeof(InternalFunctionalBlockTypeConverter))]
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
                    FunctionalDependanceControl transitionControl = (FunctionalDependanceControl)control;
                    FunctionalBlockPanel panel = (FunctionalBlockPanel)transitionControl.Panel;
                    FunctionalBlock functionalBlock = DataDictionary.OverallFunctionalBlockFinder.INSTANCE.findByName(panel.EFSSystem, value);
                    if (functionalBlock != null)
                    {
                        control.SetInitialBox(functionalBlock);
                        control.RefreshControl();
                    }
                }
            }

            [Category("Description"), TypeConverter(typeof(InternalFunctionalBlockTypeConverter))]
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
                    FunctionalDependanceControl transitionControl = (FunctionalDependanceControl)control;
                    FunctionalBlockPanel statePanel = (FunctionalBlockPanel)transitionControl.Panel;
                    FunctionalBlock functionalBlock = DataDictionary.OverallFunctionalBlockFinder.INSTANCE.findByName(statePanel.EFSSystem, value);
                    if (functionalBlock != null)
                    {
                        control.SetTargetBox(functionalBlock);
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
        protected override ArrowEditor createArrowEditor(ArrowControl<FunctionalBlock, FunctionalBlockDependance> control)
        {
            ArrowEditor retVal = new TransitionEditor(control);

            return retVal;
        }
    }
}
