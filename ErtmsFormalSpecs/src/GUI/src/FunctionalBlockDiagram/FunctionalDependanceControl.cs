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
using DataDictionary.Specification;

namespace GUI.FunctionalBlockDiagram
{
    public partial class FunctionalDependanceControl : ArrowControl<FunctionalBlock, FunctionalBlockDependance>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FunctionalDependanceControl()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public FunctionalDependanceControl(IContainer container)
            : base()
        {
            container.Add(this);
        }
    }
}
