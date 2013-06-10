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

using DataDictionary;
using DataDictionary.Interpreter;
using DataDictionary.Values;
using DataDictionary.Variables;
using Utils;

namespace Interpreter
{
    class Program
    {
        /// <summary>
        /// Interprets the EFS model 
        /// </summary>
        /// <param name="args"></param>
        /// <returns>the error code of the program</returns>
        static int Main(string[] args)
        {
            int retVal = 0;

            try
            {

                Console.Out.WriteLine("EFS Interpreter");

                // Load the dictionaries provided as parameters
                EFSSystem efsSystem = EFSSystem.INSTANCE;
                foreach (string arg in args)
                {
                    Console.Out.WriteLine("Loading dictionary " + arg);
                    Dictionary dictionary = Util.load(arg, efsSystem);
                    if (dictionary == null)
                    {
                        Console.Out.WriteLine("Cannot load dictionary " + arg);
                        return -1;
                    }
                }

                // TODO : Make it clean
                Dictionary subset26 = null;
                foreach (Dictionary dictionary in EFSSystem.INSTANCE.Dictionaries)
                {
                    subset26 = dictionary;
                    break;

                }

                // This class is used to execute the model
                InterpreterRunner runner = new InterpreterRunner();

                // Here, the EFS system is up and running

                // To set up a variable, several steps are required.
                // 1. Determine the type of the variable
                // 2. Create a value to be set to the variable
                // 3. Set the variable value

                // 1. Determine the type of the variable
                // This code is used to retrieve the type of the structure we will instanciate
                Parser parser = EFSSystem.INSTANCE.Parser;
                Expression type = parser.Expression(subset26, "Odometry.OdometerMulticastMessage", Filter.IsStructure);
                DataDictionary.Types.Structure structureType = type.Ref as DataDictionary.Types.Structure;
                StructureValue value = new StructureValue(structureType, subset26);
                {
                    // 2. Create the value to be set to the variable
                    Expression Speed = parser.Expression(subset26, "Default.BaseTypes.Speed", Filter.IsType);
                    DataDictionary.Types.Range SpeedType = Speed.Ref as DataDictionary.Types.Range;

                    value.getVariable("V_MAX").Value = new DoubleValue(SpeedType, 12.21);
                    value.getVariable("V_MIN").Value = new DoubleValue(SpeedType, 11.59);
                    // ... and so on for all fields of the structure
                }
                {
                    // 3. Set the variable value
                    Expression variable = parser.Expression(subset26, "Odometry.Message", Filter.IsVariable);
                    Variable var = variable.Ref as Variable;
                    var.Value = value;
                }

                // When all variables have been set up, let's run the system.
                runner.ExecuteOnePriority(DataDictionary.Generated.acceptor.RulePriority.aVerification);
                runner.ExecuteOnePriority(DataDictionary.Generated.acceptor.RulePriority.aUpdateINTERNAL);
                runner.ExecuteOnePriority(DataDictionary.Generated.acceptor.RulePriority.aProcessing);
                runner.ExecuteOnePriority(DataDictionary.Generated.acceptor.RulePriority.aUpdateOUT);

                // After running the system, let's get back the output variables
                Expression variableExpression = parser.Expression(subset26, "Kernel.XYZ", Filter.IsVariable);
                IVariable outVariable = variableExpression.Ref as IVariable;
                if (outVariable != null)
                {
                    DoubleValue outValue = outVariable.Value as DoubleValue;
                    if (outValue != null)
                    {
                        // Do whatever you wish with that value.
                        System.Console.WriteLine(outValue.Val);
                    }
                }

                // And clean up.
                runner.ExecuteOnePriority(DataDictionary.Generated.acceptor.RulePriority.aCleanUp);

            }
            finally
            {
                DataDictionary.Util.UnlockAllFiles();
            }

            return retVal;
        }
    }
}
