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

namespace DataDictionary.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a requirement set
    /// </summary>
    public class RequirementSetDependance : Generated.RequirementSetDependance, IGraphicalArrow<RequirementSet>
    {
        /// <summary>
        /// The source of the arrow
        /// </summary>
        public RequirementSet Source { get { return Enclosing as RequirementSet; } }

        /// <summary>
        /// Sets the source box for this arrow
        /// </summary>
        /// <param name="initialBox"></param>
        public void SetInitialBox(IGraphicalDisplay initialBox)
        {
            Source.removeDependances(this);
            RequirementSet newSource = (RequirementSet)initialBox;
            newSource.appendDependances(this);
        }

        /// <summary>
        /// The target of the arrow
        /// </summary>
        public RequirementSet Target
        {
            get
            {
                RequirementSet retVal = null;

                foreach (RequirementSet requirementSet in EFSSystem.RequirementSets)
                {
                    if (requirementSet.Name == getTarget())
                    {
                        retVal = requirementSet;
                        break;
                    }
                }
                return retVal;
            }
        }

        /// <summary>
        /// Sets the target box for this arrow
        /// </summary>
        /// <param name="targetBox"></param>
        public void SetTargetBox(IGraphicalDisplay targetBox)
        {
            setTarget(targetBox.Name);
        }

        /// <summary>
        /// The name to be displayed
        /// </summary>
        public string GraphicalName { get { return Name; } }

        /// <summary>
        /// The model element which is referenced by this arrow
        /// </summary>
        public ModelElement ReferencedModel { get { return this; } }
    }
}
