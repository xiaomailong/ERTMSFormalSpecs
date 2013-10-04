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
namespace DataDictionary.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utils;

    /// <summary>
    /// Indicates where the model element is used, associated with the usage mode (read / write)
    /// </summary>
    public class Usage : IComparable<Usage>
    {
        /// <summary>
        /// The used element 
        /// </summary>
        public INamable Referenced { get; private set; }

        /// <summary>
        /// The access mode to the element
        /// </summary>
        public enum ModeEnum { Read, Write, ReadAndWrite, Call, Type };

        /// <summary>
        /// Provides the usage mode for this element
        /// </summary>
        public ModeEnum? Mode { get; set; }

        /// <summary>
        /// Provides the element that uses the model
        /// </summary>
        public IModelElement User { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="referenced"></param>
        /// <param name="user"></param>
        public Usage(INamable referenced, IModelElement user, ModeEnum? mode)
        {
            Referenced = referenced;
            User = user;
            Mode = mode;
        }

        /// <summary>
        /// Provides the display name of this usage
        /// </summary>
        /// <returns></returns>
        public string DisplayName()
        {
            string retVal = null;

            IModelElement current = User;
            while (current != null && retVal == null)
            {
                if (current is Rules.RuleCondition ||
                    current is Functions.Function ||
                    current is Functions.Procedure ||
                    current is Types.Type ||
                    current is Tests.TestCase)
                {
                    retVal = current.Name;
                }

                current = current.Enclosing as IModelElement;
            }

            if (retVal == null)
            {
                retVal = User.Name;
            }

            return retVal;
        }

        public int CompareTo(Usage other)
        {
            int retVal = other.DisplayName().CompareTo(DisplayName());

            if (retVal == 0)
            {
                if (other.Mode != null && Mode != null)
                {
                    ModeEnum m1 = (ModeEnum)Mode;
                    ModeEnum m2 = (ModeEnum)other.Mode;

                    retVal = m1.CompareTo(m2);
                }
                else
                {
                    if (Mode == null && other.Mode == null)
                    {
                        retVal = 0;
                    }
                    else
                    {
                        if (Mode == null)
                        {
                            retVal = -1;
                        }
                        else
                        {
                            retVal = 1;
                        }
                    }
                }
            }

            return retVal;
        }
    }

    /// <summary>
    /// A collection of usages
    /// </summary>
    public class Usages
    {
        /// <summary>
        /// The usages
        /// </summary>
        private List<Usage> AllUsages { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Usages()
        {
            AllUsages = new List<Usage>();
        }

        /// <summary>
        /// Adds another list of usage to this list, and sets the mode if not yet set
        /// </summary>
        /// <param name="other"></param>
        /// <param name="mode"></param>
        public void AddUsages(Usages other, Usage.ModeEnum? mode)
        {
            foreach (Usage usage in other.AllUsages)
            {
                if (usage.Mode == null)
                {
                    usage.Mode = mode;
                }
                AllUsages.Add(usage);
            }
        }

        /// <summary>
        /// Adds a new usage to the list
        /// </summary>
        /// <param name="referenced"></param>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        public void AddUsage(INamable referenced, IModelElement user, Usage.ModeEnum? mode)
        {
            if (referenced != null)
            {
                // Do not store namespaces
                if (!(referenced is Types.NameSpace))
                {
                    Usage usage = new Usage(referenced, user, mode);
                    AllUsages.Add(usage);
                }
            }
        }

        /// <summary>
        /// Finds all refernces to the model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<Usage> Find(DataDictionary.ModelElement model)
        {
            List<Usage> retVal = new List<Usage>();

            foreach (Usage usage in AllUsages)
            {
                if (usage.Referenced == model)
                {
                    retVal.Add(usage);
                }
            }

            return retVal;
        }
    }
}
