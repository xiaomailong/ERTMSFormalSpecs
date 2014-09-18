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
    public abstract class PredefinedFunction : Function
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="efsSystem">The system for which this function is created</param>
        /// <param name="name">The name of the predefined function</param>
        public PredefinedFunction(EFSSystem efsSystem, string name)
        {
            Enclosing = efsSystem;
            Name = name;
        }

        /// <summary>
        /// The enclosing collection of the function
        /// </summary>
        public override System.Collections.ArrayList EnclosingCollection
        {
            get { return null; }
        }

        /// <summary>
        /// The type associated to this function
        /// </summary>
        public override abstract Types.Type ReturnType { get; }

        /// <summary>
        /// Provides the value of the function
        /// </summary>
        /// <param name="instance">the instance on which the function is evaluated</param>
        /// <param name="actuals">the actual parameters values</param>
        /// <param name="localScope">the values of local variables</param>
        /// <returns>The value for the function application</returns>
        public override abstract Values.IValue Evaluate(Interpreter.InterpretationContext context, Dictionary<Variables.Actual, Values.IValue> actuals, ExplanationPart explain);


        /// <summary>
        /// Provides the name associated to the parameter
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string getName(Parameter param)
        {
            string retVal = param.Name;

            return retVal;
        }

        /// <summary>
        /// Ensures that the parameter provided corresponds to a function double->double
        /// </summary>
        /// <param name="root">Element on which the errors shall be attached</param>
        /// <param name="context">The context used to evaluate the expression</param>
        /// <param name="expression">The expression which references the function</param>
        /// <param name="count">the expected number of parameters</param>
        protected virtual void CheckFunctionalParameter(ModelElement root, Interpreter.InterpretationContext context, Interpreter.Expression expression, int count)
        {
            Types.Type type = expression.GetExpressionType();

            Function function = type as Function;
            if (function != null)
            {
                if (function.FormalParameters.Count == count)
                {
                    foreach (Parameter parameter in function.FormalParameters)
                    {
                        if (!parameter.Type.IsDouble())
                        {
                            root.AddError(expression.ToString() + " does not takes a double for parameter " + parameter.Name);
                        }
                    }
                }
                else
                {
                    root.AddError(expression.ToString() + " does not take " + count + "parameter(s) as input");
                }

                if (!function.ReturnType.IsDouble())
                {
                    root.AddError(expression.ToString() + " does not return a double");
                }
            }
            else
            {
                if (!type.IsDouble())
                {
                    root.AddError(expression.ToString() + " type is not double");
                }
            }
        }

        /// <summary>
        /// Creates the graph associated to the parameter provided
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        /// <param name="explain"></param>
        /// <returns></returns>
        protected Graph createGraphForValue(Interpreter.InterpretationContext context, Values.IValue value, ExplanationPart explain, Parameter parameter = null)
        {
            Graph retVal = new Graph();

            Function function = value as Function;
            if (function != null)
            {
                if (parameter == null)
                {
                    parameter = (Parameter)function.FormalParameters[0];
                    int token = context.LocalScope.PushContext();
                    context.LocalScope.setGraphParameter(parameter);
                    retVal = function.createGraph(context, parameter, explain);
                    context.LocalScope.PopContext(token);
                }
                else
                {
                    retVal = function.createGraph(context, parameter, explain);
                }
            }
            else
            {
                Values.DoubleValue val = value as Values.DoubleValue;
                retVal.addSegment(new Graph.Segment(0, double.MaxValue, new Graph.Segment.Curve(0.0, val.Val, 0.0)));
            }

            return retVal;
        }

        /// <summary>
        /// Creates the surface associated to the value provided
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <param name="explain"></param>
        /// <returns></returns>
        protected Surface createSurfaceForValue(Interpreter.InterpretationContext context, Values.IValue value, ExplanationPart explain)
        {
            Surface retVal = null;

            Function function = value as Function;
            if (function != null)
            {
                retVal = function.createSurface(context, explain);
            }
            else
            {
                Values.DoubleValue val = value as Values.DoubleValue;
                Graph graph = new Graph();
                graph.addSegment(new Graph.Segment(0, double.MaxValue, new Graph.Segment.Curve(0, val.Val, 0)));
                retVal = new Functions.Surface(null, null);
                retVal.AddSegment(new Surface.Segment(0, double.MaxValue, graph));
            }

            return retVal;
        }
    }
}
