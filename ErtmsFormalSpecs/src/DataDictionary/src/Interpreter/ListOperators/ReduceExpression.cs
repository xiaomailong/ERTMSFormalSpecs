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
using Utils;
using DataDictionary.Interpreter.Filter;

namespace DataDictionary.Interpreter.ListOperators
{
    public class ReduceExpression : ExpressionBasedListExpression, ISubDeclarator
    {
        /// <summary>
        /// The operator for this expression
        /// </summary>
        public static string OPERATOR = "REDUCE";

        /// <summary>
        /// The reduce initial value
        /// </summary>
        public Expression InitialValue { get; private set; }

        /// <summary>
        /// The accumulator variable
        /// </summary>
        public Variables.Variable AccumulatorVariable { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listExpression"></param>
        /// <param name="condition"></param>
        /// <param name="function"></param>
        /// <param name="initialValue"></param>
        /// <param name="root">the root element for which this expression should be parsed</param>
        /// <param name="start">The start character for this expression in the original string</param>
        /// <param name="end">The end character for this expression in the original string</param>
        public ReduceExpression(ModelElement root, ModelElement log, Expression listExpression, Expression condition, Expression function, Expression initialValue, int start, int end)
            : base(root, log, listExpression, condition, function, start, end)
        {
            InitialValue = initialValue;
            InitialValue.Enclosing = this;

            AccumulatorVariable = (Variables.Variable)Generated.acceptor.getFactory().createVariable();
            AccumulatorVariable.Enclosing = this;
            AccumulatorVariable.Name = "RESULT";
            Utils.ISubDeclaratorUtils.AppendNamable(this, AccumulatorVariable);
        }

        /// <summary>
        /// Performs the semantic analysis of the expression
        /// </summary>
        /// <param name="instance">the reference instance on which this element should analysed</param>
        /// <paraparam name="expectation">Indicates the kind of element we are looking for</paraparam>
        /// <returns>True if semantic analysis should be continued</returns>
        public override bool SemanticAnalysis(Utils.INamable instance, BaseFilter expectation)
        {
            bool retVal = base.SemanticAnalysis(instance, expectation);

            if (retVal)
            {
                InitialValue.SemanticAnalysis(instance, AllMatches.INSTANCE);
                StaticUsage.AddUsages(InitialValue.StaticUsage, Usage.ModeEnum.Read);

                AccumulatorVariable.Type = InitialValue.GetExpressionType();
            }

            return retVal;
        }

        public override ICallable getStaticCallable()
        {
            return InitialValue.getStaticCallable();
        }

        /// <summary>
        /// Provides the type of this expression
        /// </summary>
        /// <param name="context">The interpretation context</param>
        /// <returns></returns>
        public override Types.Type GetExpressionType()
        {
            return IteratorExpression.GetExpressionType();
        }

        /// <summary>
        /// Provides the value associated to this Expression
        /// </summary>
        /// <param name="context">The context on which the value must be found</param>
        /// <param name="explain">The explanation to fill, if any</param>
        /// <returns></returns>
        public override Values.IValue GetValue(InterpretationContext context, ExplanationPart explain)
        {
            Values.IValue retVal = null;

            Values.ListValue value = ListExpression.GetValue(context, explain) as Values.ListValue;
            if (value != null)
            {
                int token = PrepareIteration(context);
                context.LocalScope.setVariable(AccumulatorVariable);
                AccumulatorVariable.Value = InitialValue.GetValue(context, explain);

                foreach (Values.IValue v in value.Val)
                {
                    if (v != EFSSystem.EmptyValue)
                    {
                        IteratorVariable.Value = v;
                        if (conditionSatisfied(context, explain))
                        {
                            AccumulatorVariable.Value = IteratorExpression.GetValue(context, explain);
                        }
                    }
                    NextIteration();
                }
                EndIteration(context, token);
                retVal = AccumulatorVariable.Value;
            }
            else
            {
                AddError("Cannot evaluate list value " + ListExpression.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// Provides the callable that is called by this expression
        /// </summary>
        /// <param name="context"></param>
        /// <param name="explain"></param>
        /// <returns></returns>
        public override ICallable getCalled(InterpretationContext context, ExplanationPart explain)
        {
            ICallable retVal = null;

            Functions.Function function = InitialValue.Ref as Functions.Function;
            if (function == null)
            {
                function = InitialValue.getCalled(context, explain) as Functions.Function;
            }

            if (function != null)
            {
                if (function.FormalParameters.Count == 1)
                {
                    int token = context.LocalScope.PushContext();
                    context.LocalScope.setGraphParameter((Parameter)function.FormalParameters[0]);
                    Functions.Graph graph = createGraph(context, (Parameter)function.FormalParameters[0], explain);
                    context.LocalScope.PopContext(token);
                    if (graph != null)
                    {
                        retVal = graph.Function;
                    }
                }
                else if (function.FormalParameters.Count == 2)
                {
                    int token = context.LocalScope.PushContext();
                    context.LocalScope.setSurfaceParameters((Parameter)function.FormalParameters[0], (Parameter)function.FormalParameters[1]);
                    Functions.Surface surface = createSurface(context, (Parameter)function.FormalParameters[0], (Parameter)function.FormalParameters[1], explain);
                    context.LocalScope.PopContext(token);
                    if (surface != null)
                    {
                        retVal = surface.Function;
                    }
                }
                else
                {
                    AddError("Cannot evaluate REDUCE expression to a function");
                }
            }
            else
            {
                AddError("Cannot evaluate REDUCE expression to a function");
            }

            return retVal;
        }

        /// <summary>
        /// Provides the expression text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retVal = OPERATOR + " " + ListExpression.ToString();

            if (Condition != null)
            {
                retVal += " | " + Condition.ToString();
            }

            retVal = retVal + " USING " + IteratorExpression.ToString() + " INITIAL_VALUE " + InitialValue.ToString();

            return retVal;
        }

        /// <summary>
        /// Prepares the iteration on the context provided
        /// </summary>
        /// <param name="context"></param>
        protected override int PrepareIteration(InterpretationContext context)
        {
            int retVal = base.PrepareIteration(context);

            context.LocalScope.setVariable(AccumulatorVariable);

            return retVal;
        }

        /// <summary>
        /// Checks the expression and appends errors to the root tree node when inconsistencies are found
        /// </summary>
        public override void checkExpression()
        {
            base.checkExpression();

            Types.Type initialValueType = InitialValue.GetExpressionType();
            if (initialValueType != null)
            {
                Types.Collection listExpressionType = ListExpression.GetExpressionType() as Types.Collection;
                if (listExpressionType != null)
                {
                    IteratorExpression.checkExpression();
                }
            }
            else
            {
                AddError("Cannot determine initial value expression type for " + ToString());
            }
        }

        /// <summary>
        /// Creates the graph associated to this expression, when the given parameter ranges over the X axis
        /// </summary>
        /// <param name="context">The interpretation context</param>
        /// <param name="parameter">The parameters of *the enclosing function* for which the graph should be created</param>
        /// <param name="explain"></param>
        /// <returns></returns>
        public override Functions.Graph createGraph(InterpretationContext context, Parameter parameter, ExplanationPart explain)
        {
            Functions.Graph retVal = base.createGraph(context, parameter, explain);

            Functions.Graph graph = InitialValue.createGraph(context, parameter, explain);
            if (graph != null)
            {
                Values.ListValue value = ListExpression.GetValue(context, explain) as Values.ListValue;
                if (value != null)
                {
                    int token = PrepareIteration(context);
                    AccumulatorVariable.Value = graph.Function;

                    foreach (Values.IValue v in value.Val)
                    {
                        if (v != EFSSystem.EmptyValue)
                        {
                            IteratorVariable.Value = v;
                            if (conditionSatisfied(context, explain))
                            {
                                AccumulatorVariable.Value = IteratorExpression.GetValue(context, explain);
                            }
                        }
                        NextIteration();
                    }
                    Functions.Function function = AccumulatorVariable.Value as Functions.Function;
                    if (function != null)
                    {
                        retVal = function.Graph;
                    }
                    else
                    {
                        retVal = Functions.Function.createGraphForValue(AccumulatorVariable.Value);
                    }
                    EndIteration(context, token);
                }
            }
            else
            {
                throw new Exception("Cannot create graph for initial value " + InitialValue.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// Provides the surface of this function if it has been statically defined
        /// </summary>
        /// <param name="context">the context used to create the surface</param>
        /// <param name="xParam">The X axis of this surface</param>
        /// <param name="yParam">The Y axis of this surface</param>
        /// <param name="explain"></param>
        /// <returns>The surface which corresponds to this expression</returns>
        public override Functions.Surface createSurface(Interpreter.InterpretationContext context, Parameter xParam, Parameter yParam, ExplanationPart explain)
        {
            Functions.Surface retVal = base.createSurface(context, xParam, yParam, explain);

            Functions.Surface surface = InitialValue.createSurface(context, xParam, yParam, explain);
            if (surface != null)
            {
                Values.ListValue value = ListExpression.GetValue(context, explain) as Values.ListValue;
                if (value != null)
                {
                    int token = PrepareIteration(context);
                    AccumulatorVariable.Value = surface.Function;

                    foreach (Values.IValue v in value.Val)
                    {
                        if (v != EFSSystem.EmptyValue)
                        {
                            IteratorVariable.Value = v;
                            if (conditionSatisfied(context, explain))
                            {
                                AccumulatorVariable.Value = IteratorExpression.GetValue(context, explain);
                            }
                        }
                        NextIteration();
                    }
                    Functions.Function function = AccumulatorVariable.Value as Functions.Function;
                    if (function != null)
                    {
                        retVal = function.Surface;
                    }
                    else
                    {
                        throw new Exception("Expression does not reduces to a function");
                    }
                    EndIteration(context, token);
                }
            }
            else
            {
                throw new Exception("Cannot create surface for initial value " + InitialValue.ToString());
            }
            retVal.XParameter = xParam;
            retVal.YParameter = yParam;

            return retVal;
        }
    }
}
