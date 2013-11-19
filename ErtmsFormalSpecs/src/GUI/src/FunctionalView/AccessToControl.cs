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
using DataDictionary.Rules;
using DataDictionary.Types;
using GUI.BoxArrowDiagram;
using DataDictionary.Constants;
using DataDictionary.Types.AccessMode;

namespace GUI.FunctionalView
{
    public partial class FunctionCallControl : ArrowControl<NameSpace, AccessMode>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FunctionCallControl()
            : base()
        {
            ArrowMode = ArrowModeEnum.Half;
            ArrowFill = ArrowFillEnum.Fill;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public FunctionCallControl(IContainer container)
            : base()
        {
            container.Add(this);
        }

        /// <summary>
        /// Provides the name of the target state
        /// </summary>
        /// <returns></returns>
        public override string getTargetName()
        {
            string retVal = "<Unknown>";

            if (Model.Target != null)
            {
                retVal = Model.Target.FullName;
            }

            return retVal;
        }

    }
}
