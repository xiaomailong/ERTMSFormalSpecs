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
namespace EFSIPCInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DataDictionary.Tests.Runner;
    using DataDictionary;

    public class EFSService : IEFSService
    {
        /// <summary>
        /// Provides the value of a specific variable
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public Value GetVariableValue(string variableName)
        {
            return null;
        }

        /// <summary>
        /// Sets the value of a specific variable
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        public void SetVariableValue(string variableName, Value value)
        {
        }

        /// <summary>
        /// Activates the execution of a single cycle, as the given priority level
        /// </summary>
        /// <param name="priority"></param>
        public void Cycle(Priority priority)
        {
            EFSSystem efsSystem = EFSSystem.INSTANCE;
            if (efsSystem.Runner == null)
            {
                bool explain = false;
                bool logEvents = true;
                efsSystem.Runner = new Runner(explain, logEvents, 100, 10000);
            }

            efsSystem.Runner.ExecuteOnePriority(convertPriority(priority));
        }

        /// <summary>
        /// Converts an interface priority to a Runner priority
        /// </summary>
        /// <param name="priority"></param>
        private DataDictionary.Generated.acceptor.RulePriority convertPriority(Priority priority)
        {
            DataDictionary.Generated.acceptor.RulePriority retVal = DataDictionary.Generated.acceptor.RulePriority.defaultRulePriority;

            switch (priority)
            {
                case Priority.Verification:
                    retVal = DataDictionary.Generated.acceptor.RulePriority.aVerification;
                    break;

                case Priority.UpdateInternal:
                    retVal = DataDictionary.Generated.acceptor.RulePriority.aUpdateINTERNAL;
                    break;

                case Priority.Process:
                    retVal = DataDictionary.Generated.acceptor.RulePriority.aProcessing;
                    break;

                case Priority.UpdateOutput:
                    retVal = DataDictionary.Generated.acceptor.RulePriority.aUpdateOUT;
                    break;

                case Priority.CleanUp:
                    retVal = DataDictionary.Generated.acceptor.RulePriority.aCleanUp;
                    break;
            }

            return retVal;
        }
    }
}
