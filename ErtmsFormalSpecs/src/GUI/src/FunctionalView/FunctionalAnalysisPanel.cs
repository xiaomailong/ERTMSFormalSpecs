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
using DataDictionary;

namespace GUI.FunctionalView
{
    public class FunctionalAnalysisPanel : BoxArrowPanel<NameSpace, ProcedureOrFunctionCall>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FunctionalAnalysisPanel()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public FunctionalAnalysisPanel(IContainer container)
            : base()
        {
            container.Add(this);
        }

        /// <summary>
        /// Method used to create a box
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override BoxControl<NameSpace, ProcedureOrFunctionCall> createBox(NameSpace model)
        {
            BoxControl<NameSpace, ProcedureOrFunctionCall> retVal = new FunctionalBlockControl();
            retVal.Model = model;

            return retVal;
        }

        /// <summary>
        /// Method used to create an arrow
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ArrowControl<NameSpace, ProcedureOrFunctionCall> createArrow(ProcedureOrFunctionCall model)
        {
            ArrowControl<NameSpace, ProcedureOrFunctionCall> retVal = new FunctionCallControl();
            retVal.Model = model;

            return retVal;
        }

        /// <summary>
        /// The namespace displayed by this panel
        /// </summary>
        public IEnclosesNameSpaces NameSpaceContainer { get; set; }

        /// <summary>
        /// Provides the boxes that need be displayed
        /// </summary>
        /// <returns></returns>
        public override List<NameSpace> getBoxes()
        {
            List<NameSpace> retVal = new List<NameSpace>();

            foreach (NameSpace nameSpace in NameSpaceContainer.NameSpaces)
            {
                retVal.Add(nameSpace);
            }

            return retVal;
        }

        /// <summary>
        /// Provides the arrows that need be displayed
        /// </summary>
        /// <returns></returns>
        public override List<ProcedureOrFunctionCall> getArrows()
        {
            List<ProcedureOrFunctionCall> retVal = new List<ProcedureOrFunctionCall>();

            retVal.AddRange(IEnclosesNameSpacesUtils.getProcedureOrFunctionCalls(NameSpaceContainer));

            return retVal;
        }
    }
}
