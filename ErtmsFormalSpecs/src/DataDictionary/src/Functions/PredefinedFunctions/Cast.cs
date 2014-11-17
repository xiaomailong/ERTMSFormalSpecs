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
using System.Collections.Generic;
using DataDictionary.Interpreter;

namespace DataDictionary.Functions.PredefinedFunctions
{
    /// <summary>
    /// Casts a value as a new value of range type 
    /// </summary>
    public class Cast : PredefinedFunction
    {
        /// <summary>
        /// The target type for which the cast is performed
        /// </summary>
        public Types.Type TargetType { get; private set; }

        /// <summary>
        /// The value which is casted
        /// </summary>
        public Parameter Value { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type in which the cast is performed</param>
        public Cast(Types.Type type)
            : base(type.EFSSystem, type.Name)
        {
            TargetType = type;

            Value = (Parameter)Generated.acceptor.getFactory().createParameter();
            Value.Name = "Value";
            Value.Type = EFSSystem.AnyType;
            Value.setFather(this);
            FormalParameters.Add(Value);
        }

        /// <summary>
        /// The return type of the cast function
        /// </summary>
        public override Types.Type ReturnType
        {
            get { return TargetType; }
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
            Values.IValue value = context.findOnStack(Value).Value;
            if (value is Function)
            {
                retVal = value;
            }
            else
            {
                retVal = TargetType.convert(value);
            }
            context.LocalScope.PopContext(token);

            return retVal;
        }

        /// <summary>
        /// Provides the graph of this function if it has been statically defined
        /// </summary>
        /// <param name="context">the context used to create the graph</param>
        /// <param name="parameter"></param>
        /// <param name="explain"></param>
        /// <returns></returns>
        public override Graph createGraph(Interpreter.InterpretationContext context, Parameter parameter, ExplanationPart explain)
        {
            Graph retVal = null;

            Variables.IVariable variable = context.findOnStack(Value);

            if (variable != null)
            {
                retVal = Graph.createGraph(Functions.Function.getDoubleValue(variable.Value), parameter);
            }
            else
            {
                AddError("Cannot find variable " + Value + " on the stack");
            }

            return retVal;
        }
    }
}
