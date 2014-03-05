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
using DataDictionary.Variables;
using GUI.BoxArrowDiagram;
using DataDictionary.Rules;
using DataDictionary.Specification;

namespace GUI.RequirementSetDiagram
{
    public partial class RequirementSetControl : BoxControl<RequirementSet, RequirementSetDependance>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RequirementSetControl()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public RequirementSetControl(IContainer container)
            : base(container)
        {
        }

        public override void AcceptDrop(Utils.ModelElement element)
        {
            base.AcceptDrop(element);

            // Allows to allocate paragraphs in requirement sets
            Paragraph paragraph = element as Paragraph;
            if (paragraph != null)
            {
                if (!paragraph.BelongsToRequirementSet(Model.Name))
                {
                    RequirementSetReference reference = (RequirementSetReference)DataDictionary.Generated.acceptor.getFactory().createRequirementSetReference();
                    reference.Name = Model.Name;
                    paragraph.appendRequirementSets(reference);
                }
            }
        }

        public override void SelectBox()
        {
            base.SelectBox();

            GUIUtils.MDIWindow.SetCoverageStatus(Model);
        }
    }
}
