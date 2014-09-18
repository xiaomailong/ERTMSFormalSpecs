﻿// ------------------------------------------------------------------------------
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
    /// Provides the distance at which the functions intersect
    /// </summary>
    public class IntersectAt : PredefinedFunction
    {
        /// <summary>
        /// The function speed -> distance
        /// </summary>
        public Parameter FunctionA { get; private set; }

        /// <summary>
        /// The function distance -> speed
        /// </summary>
        public Parameter FunctionB { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="efsSystem"></param>
        /// <param name="name">the name of the cast function</param>
        public IntersectAt(EFSSystem efsSystem)
            : base(efsSystem, "IntersectAt")
        {
            FunctionA = (Parameter)Generated.acceptor.getFactory().createParameter();
            FunctionA.Name = "FunctionA";
            FunctionA.Type = EFSSystem.AnyType;
            FunctionA.setFather(this);
            FormalParameters.Add(FunctionA);

            FunctionB = (Parameter)Generated.acceptor.getFactory().createParameter();
            FunctionB.Name = "FunctionB";
            FunctionB.Type = EFSSystem.AnyType;
            FunctionB.setFather(this);
            FormalParameters.Add(FunctionB);
        }


        /// <summary>
        /// The return type of the function
        /// </summary>
        public override Types.Type ReturnType
        {
            get { return EFSSystem.DoubleType; }
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

            AssignParameters(context, actuals);
            Graph graph = createGraphForValue(context, context.findOnStack(FunctionA).Value, explain);
            if (graph != null)
            {
                foreach (Graph.Segment segment in graph.Segments)
                {
                    if (segment.Expression.a == 0.0)
                    {
                        double speed = segment.Expression.v0;

                        Function function = context.findOnStack(FunctionB).Value as Function;
                        if (function.FormalParameters.Count > 0)
                        {
                            Parameter functionParameter = function.FormalParameters[0] as Parameter;
                            Variables.Actual actual = functionParameter.createActual();
                            actual.Value = new Values.DoubleValue(EFSSystem.DoubleType, speed);
                            Dictionary<Variables.Actual, Values.IValue> values = new Dictionary<Variables.Actual, Values.IValue>();
                            values[actual] = new Values.DoubleValue(EFSSystem.DoubleType, speed);
                            Values.IValue solution = function.Evaluate(context, values, explain);
                            double doubleValue = getDoubleValue(solution);

                            if (doubleValue >= segment.Start && doubleValue <= segment.End)
                            {
                                retVal = new Values.DoubleValue(EFSSystem.DoubleType, doubleValue);
                                break;
                            }
                        }
                        else
                        {
                            Log.Error("The FunctionB doesn't have any parameter");
                            break;
                        }
                    }
                    else
                    {
                        Log.Error("The FunctionA is not a step function");
                        break;
                    }
                }
            }
            else
            {
                Log.Error("Cannot create graph for " + FunctionA.ToString());
            }

            if (retVal == null)
            {
                Log.Error("Cannot compute the intersection of " + FunctionA.ToString() + " and " + FunctionB.ToString());
            }

            return retVal;
        }
    }
}
