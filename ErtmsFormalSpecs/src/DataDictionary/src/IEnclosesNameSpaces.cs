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
namespace DataDictionary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DataDictionary.Types;
    using DataDictionary.Interpreter;
    using Utils;

    /// <summary>
    /// Holds several namespaces
    /// </summary>
    public interface IEnclosesNameSpaces
    {
        /// <summary>
        /// The EFS system in which this container lies
        /// </summary>
        EFSSystem EFSSystem { get; }

        /// <summary>
        /// The namespaces referenced by this 
        /// </summary>
        System.Collections.ArrayList NameSpaces { get; }
    }


    /// <summary>
    /// Utility class to handle INameSpaceContainer
    /// </summary>
    public static class IEnclosesNameSpacesUtils
    {
        /// <summary>
        /// Provides all the function calls related to this namespace
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static SortedSet<ProcedureOrFunctionCall> getProcedureOrFunctionCalls(IEnclosesNameSpaces container)
        {
            SortedSet<ProcedureOrFunctionCall> retVal = new SortedSet<ProcedureOrFunctionCall>();

            foreach (Usage functionCall in container.EFSSystem.FindReferences(Filter.IsCallable))
            {
                ModelElement target = (ModelElement)functionCall.Referenced;
                ModelElement source = functionCall.User;

                NameSpace sourceNameSpace = getCorrespondingNameSpace(source, container);
                NameSpace targetNameSpace = getCorrespondingNameSpace(target, container);

                if (functionCall.Referenced.Name == "BG_To_LRBG")
                {
                    //    System.Diagnostics.Debugger.Break();
                }

                if (considerCall(functionCall, container, sourceNameSpace, targetNameSpace))
                {
                    retVal.Add(new ProcedureOrFunctionCall(sourceNameSpace, targetNameSpace, target));
                }
            }

            return retVal;
        }

        /// <summary>
        /// Indicates whether a call should be considered in the ProcedureOrFunctionCalls
        /// </summary>
        /// <param name="functionCall"></param>
        /// <param name="container"></param>
        /// <param name="sourceNameSpace"></param>
        /// <param name="targetNameSpace"></param>
        /// <returns></returns>
        private static bool considerCall(Usage functionCall, IEnclosesNameSpaces container, NameSpace sourceNameSpace, NameSpace targetNameSpace)
        {
            bool retVal = true;

            // Ignore casting
            retVal = retVal && !(functionCall.Referenced is Range);

            // Only display things that are relevant namespacewise
            retVal = retVal && sourceNameSpace != null && targetNameSpace != null;

            // Do not consider internal calls
            retVal = retVal && sourceNameSpace != targetNameSpace;
            // Only display things that can be displayed in this functional view
            // TODO : also consider sub namespaces in the diagram
            retVal = retVal && (container.NameSpaces.Contains(sourceNameSpace) || container.NameSpaces.Contains(targetNameSpace));

            // Ignore default namespaces
            retVal = retVal && !DefaultNameSpace(sourceNameSpace);
            retVal = retVal && !DefaultNameSpace(targetNameSpace);

            return retVal;
        }

        /// <summary>
        /// Indicates whether the namespace belongs to the namespace "Default"
        /// </summary>
        /// <param name="sourceNameSpace"></param>
        /// <returns></returns>
        private static bool DefaultNameSpace(NameSpace sourceNameSpace)
        {
            bool retVal = false;

            NameSpace current = sourceNameSpace;
            while (!retVal && current != null)
            {
                retVal = retVal || current.Name == "Default";
                current = current.EnclosingNameSpace;
            }

            return retVal;
        }

        /// <summary>
        /// Provides the namespace of the element provided in this container
        /// </summary>
        /// <param name="source">The element from which the namespace should be found</param>
        /// <param name="container">The container which contains the namespace</param>
        /// <returns></returns>
        private static NameSpace getCorrespondingNameSpace(ModelElement source, IEnclosesNameSpaces container)
        {
            NameSpace retVal = null;

            object current = source;
            while (current != null && retVal == null)
            {
                // Retrieves the namespace in which the source belong
                // This namespace should belong to the container
                NameSpace nameSpace = current as NameSpace;
                if (container.NameSpaces.Contains(nameSpace))
                {
                    retVal = nameSpace;
                }

                // If no result has been found, go one step further 
                // in the parent hierarchy
                if (retVal == null)
                {
                    IEnclosed enclosed = current as IEnclosed;
                    if (enclosed != null)
                    {
                        current = enclosed.Enclosing;
                    }
                    else
                    {
                        current = null;
                    }
                }
            }

            return retVal;
        }
    }
}
