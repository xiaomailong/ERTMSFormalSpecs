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
    /// Provides the first distance where the function reaches the speed provided
    /// </summary>
    public class DistanceForSpeed : PredefinedFunction
    {
        /// <summary>
        /// The function speed -> distance
        /// </summary>
        public Parameter Function { get; private set; }

        /// <summary>
        /// The speed to reach
        /// </summary>
        public Parameter Speed { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="efsSystem"></param>
        /// <param name="name">the name of the cast function</param>
        public DistanceForSpeed(EFSSystem efsSystem)
            : base(efsSystem, "DistanceForSpeed")
        {
            Function = (Parameter)Generated.acceptor.getFactory().createParameter();
            Function.Name = "Function";
            Function.Type = EFSSystem.AnyType;
            Function.setFather(this);
            FormalParameters.Add(Function);

            Speed = (Parameter)Generated.acceptor.getFactory().createParameter();
            Speed.Name = "Speed";
            Speed.Type = EFSSystem.DoubleType;
            Speed.setFather(this);
            FormalParameters.Add(Speed);
        }

        /// <summary>
        /// The return type of the function
        /// </summary>
        public override Types.Type ReturnType
        {
            get { return EFSSystem.DoubleType; }
        }

        /// <summary>
        /// Perform additional checks based on the parameter types
        /// </summary>
        /// <param name="root">The element on which the errors should be reported</param>
        /// <param name="context">The evaluation context</param>
        /// <param name="actualParameters">The parameters applied to this function call</param>
        public override void additionalChecks(ModelElement root, Interpreter.InterpretationContext context, Dictionary<string, Interpreter.Expression> actualParameters)
        {
            CheckFunctionalParameter(root, context, actualParameters[Function.Name], 1);
        }

        /// <summary>
        /// Provides the graph of this function if it has been statically defined
        /// </summary>
        /// <param name="context">the context used to create the graph</param>
        /// <returns></returns>
        public override Graph createGraph(Interpreter.InterpretationContext context, Parameter parameter, ExplanationPart explain)
        {
            Graph retVal = null;

            Graph graph = createGraphForValue(context, context.findOnStack(Function).Value, explain, parameter);
            if (graph != null)
            {
                double speed = Functions.Function.getDoubleValue(context.findOnStack(Speed).Value);
                double solutionX = graph.SolutionX(speed);
                if (solutionX == double.MaxValue)
                {
                    // No value found, return Unknown
                    Types.Range distanceType = (Types.Range)EFSSystem.findByFullName("Default.BaseTypes.Distance");
                    Constants.EnumValue unknownDistance = distanceType.findEnumValue("Unknown");
                    retVal = Graph.createGraph(distanceType.getValueAsDouble(unknownDistance));
                }
                else
                {
                    // Create the graph for this solution
                    retVal = Graph.createGraph(solutionX);
                }
            }
            else
            {
                Log.Error("Cannot create graph for " + Function.ToString());
            }

            return retVal;
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
            Functions.Function function = context.findOnStack(Function).Value as Functions.Function;
            if (function != null)
            {
                double speed = Functions.Function.getDoubleValue(context.findOnStack(Speed).Value);

                Parameter parameter = (Parameter)function.FormalParameters[0];
                int token2 = context.LocalScope.PushContext();
                context.LocalScope.setGraphParameter(parameter);
                Graph graph = function.createGraph(context, (Parameter)function.FormalParameters[0], explain);
                context.LocalScope.PopContext(token2);
                double solutionX = graph.SolutionX(speed);
                if ( solutionX == double.MaxValue )
                {
                    Types.Range distanceType = (Types.Range)EFSSystem.findByFullName("Default.BaseTypes.Distance");
                    retVal = distanceType.findEnumValue("Unknown");
                }
                else
                {
                    retVal = new Values.DoubleValue(EFSSystem.DoubleType, solutionX);                
                }
            }
            else
            {
                Log.Error("Cannot get function for " + Function.ToString());
            }
            context.LocalScope.PopContext(token);

            return retVal;
        }
    }
}
