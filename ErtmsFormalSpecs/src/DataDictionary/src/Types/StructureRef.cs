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
using System.IO;
using System.Collections.Generic;
using Utils;

namespace DataDictionary.Types
{
    public class StructureRef : Generated.StructureRef
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StructureRef()
            : base()
        {
        }

        /// <summary>
        /// The structure referenced by this StructureRef
        /// </summary>
        private Structure structure;

        /// <summary>
        /// The type associated to this StructureRef
        /// </summary>
        public Structure ReferencedStructure
        {
            get
            {
                if (structure == null)
                {
                    Structure enclosingStructure = Enclosing as Structure;
                    if (enclosingStructure != null)
                    {
                        structure = EFSSystem.findType(enclosingStructure.NameSpace, getName()) as Structure;
                    }
                }
                return structure;
            }
            set
            {
                if (value != null)
                {
                    setName(value.getName());
                }
                else
                {
                    setName(null);
                }
                structure = value;
            }
        }

        /// <summary>
        /// Computes the recursive list of the interfaces implemented
        /// by the structure corresponding to this StructureRef
        /// </summary>
        public List<Structure> ImplementedStructures
        {
            get
            {
                List<Structure> result = new List<Structure>();
                if (ReferencedStructure != null)
                {
                    result.Add(ReferencedStructure);
                    foreach (Structure aStructure in ReferencedStructure.Interfaces)
                    {
                        result.AddRange(aStructure.ImplementedStructures);
                    }
                }
                return result;
            }
        }
    }
}