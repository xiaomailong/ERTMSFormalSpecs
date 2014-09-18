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
using System.Collections.Generic;
using DataDictionary.Interpreter;


namespace DataDictionary.Functions.PredefinedFunctions
{
    /// <summary>
    /// Creates a new function which converts a double to an integer
    /// </summary>
    public class DoubleToInteger : PredefinedFunction
    {
        /// <summary>
        /// The value to be rounded
        /// </summary>
        public Parameter Value { get; private set; }

        /// <summary>
        /// The return type of the function
        /// </summary>
        public override DataDictionary.Types.Type ReturnType
        {
            get { return EFSSystem.IntegerType; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="efsSystem"></param>
        public DoubleToInteger(EFSSystem efsSystem)
            : base(efsSystem, "DoubleToInteger")
        {
            Value = (Parameter)Generated.acceptor.getFactory().createParameter();
            Value.Name = "Value";
            Value.Type = EFSSystem.DoubleType;
            Value.setFather(this);
            FormalParameters.Add(Value);
        }

        /// <summary>
        /// Provides the value of the function
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actuals">the actual parameters values</param>
        /// <param name="explain"></param>
        /// <returns>The value for the function application</returns>
        public override Values.IValue Evaluate(Interpreter.InterpretationContext context, Dictionary<Variables.Actual, Values.IValue> actuals, ExplanationPart explain)
        {
            Values.IValue retVal = null;

            int token = context.LocalScope.PushContext();
            AssignParameters(context, actuals);

            Values.DoubleValue value = context.findOnStack(Value).Value as Values.DoubleValue;
            if (value != null)
            {
                int res = (int)Math.Round(value.Val);
                retVal = new Values.IntValue(ReturnType, res);
            }

            context.LocalScope.PopContext(token);

            return retVal;
        }
    }
}
