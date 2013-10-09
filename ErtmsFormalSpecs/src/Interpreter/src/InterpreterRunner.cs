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
namespace Interpreter
{
    using System.Collections.Generic;
    using DataDictionary;
    using DataDictionary.Tests.Runner;

    /// <summary>
    /// 
    /// </summary>
    public class InterpreterRunner : Runner
    {
        public override EFSSystem EFSSystem { get { return EFSSystem.INSTANCE; } }

        // Constructor
        public InterpreterRunner()
            : base(null, false)
        {
            Step = 100;
        }

        /// <summary>
        /// Executes the interpretation machine for one priority
        /// </summary>
        /// <param name="priority"></param>
        public void ExecuteOnePriority(DataDictionary.Generated.acceptor.RulePriority priority)
        {
            try
            {
                DataDictionary.Generated.ControllersManager.DesactivateAllNotifications();

                LastActivationTime = Time;

                Utils.ModelElement.Errors = new Dictionary<Utils.ModelElement, List<Utils.ElementLog>>();

                // Clears the cache of functions
                FunctionCacheCleaner.ClearCaches();

                // Activates the processing engine
                HashSet<Activation> activations = new HashSet<Activation>();
                foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
                {
                    foreach (DataDictionary.Types.NameSpace nameSpace in dictionary.NameSpaces)
                    {
                        SetupNameSpaceActivations(priority, activations, nameSpace, null);
                    }
                }

                ApplyActivations(activations);

                // Clears the cache of functions
                FunctionCacheCleaner.ClearCaches();
            }
            finally
            {
                DataDictionary.Generated.ControllersManager.ActivateAllNotifications();
            }

            EventTimeLine.CurrentTime += Step;

        }
    }
}
