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
namespace DataDictionary.Compare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Stores a difference between two versions of a dictionary
    /// </summary>
    public class Diff
    {
        public enum ActionEnum { Add, Remove, Change };

        /// <summary>
        /// The action performed during the change
        /// </summary>
        public ActionEnum Action { get; private set; }

        /// <summary>
        /// The message associated to this change
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The element affected by that change
        /// </summary>
        public ModelElement Model { get; private set; }

        /// <summary>
        /// The field that is affected, if any
        /// </summary>
        public string Field { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action"></param>
        public Diff(ModelElement model, ActionEnum action, string field = "", string message = "")
        {
            Model = model;
            Action = action;
            Field = field;
            Message = message;
        }
    }

    public class VersionDiff
    {
        /// <summary>
        /// The differences
        /// </summary>
        public List<Diff> Diffs { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public VersionDiff()
        {
            Diffs = new List<Diff>();
        }
    }
}
