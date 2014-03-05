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

namespace GUI.RequirementSetDiagram
{
    public partial class RequirementSetDiagramWindow : BoxArrowWindow<RequirementSet, RequirementSetDependance>
    {
        /// <summary>
        /// The system for which the requirement set diagram is built
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
        public RequirementSetDiagramWindow()
            : base()
        {
        }

        /// <summary>
        /// The panel used to display the state diagram
        /// </summary>
        private RequirementSetPanel Panel { get { return (RequirementSetPanel)BoxArrowContainerPanel; } }

        /// <summary>
        /// Sets the state machine type
        /// </summary>
        /// <param name="stateMachine"></param>
        public void SetSystem(EFSSystem system)
        {
            EFSSystem = system;

            Panel.EFSSystem = EFSSystem;
            Panel.RefreshControl();
        }

        public override BoxArrowPanel<RequirementSet, RequirementSetDependance> createPanel()
        {
            BoxArrowPanel<RequirementSet, RequirementSetDependance> retVal = new RequirementSetPanel();

            return retVal;
        }

        /// <summary>
        /// A box editor
        /// </summary>
        protected class RequirementSetEditor : BoxEditor
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="control"></param>
            public RequirementSetEditor(BoxControl<RequirementSet, RequirementSetDependance> control)
                : base(control)
            {
            }
        }

        /// <summary>
        /// Factory for BoxEditor
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        protected override BoxEditor createBoxEditor(BoxControl<RequirementSet, RequirementSetDependance> control)
        {
            BoxEditor retVal = new RequirementSetEditor(control);

            return retVal;
        }

        protected class InternalRequirementSetTypeConverter : Converters.RequirementSetTypeConverter
        {
            public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
            {
                TransitionEditor instance = (TransitionEditor)context.Instance;
                RequirementSetPanel panel = (RequirementSetPanel)instance.control.BoxArrowPanel;
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
            public TransitionEditor(ArrowControl<RequirementSet, RequirementSetDependance> control)
                : base(control)
            {
            }

            [Category("Description"), TypeConverter(typeof(InternalRequirementSetTypeConverter))]
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
                    RequirementSetDependanceControl transitionControl = (RequirementSetDependanceControl)control;
                    RequirementSetPanel panel = (RequirementSetPanel)transitionControl.Panel;
                    RequirementSet requirementSet = DataDictionary.OverallRequirementSetFinder.INSTANCE.findByName(panel.EFSSystem, value);
                    if (requirementSet != null)
                    {
                        control.SetInitialBox(requirementSet);
                        control.RefreshControl();
                    }
                }
            }

            [Category("Description"), TypeConverter(typeof(InternalRequirementSetTypeConverter))]
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
                    RequirementSetDependanceControl transitionControl = (RequirementSetDependanceControl)control;
                    RequirementSetPanel statePanel = (RequirementSetPanel)transitionControl.Panel;
                    RequirementSet requirementSet = DataDictionary.OverallRequirementSetFinder.INSTANCE.findByName(statePanel.EFSSystem, value);
                    if (requirementSet != null)
                    {
                        control.SetTargetBox(requirementSet);
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
        protected override ArrowEditor createArrowEditor(ArrowControl<RequirementSet, RequirementSetDependance> control)
        {
            ArrowEditor retVal = new TransitionEditor(control);

            return retVal;
        }
    }
}
