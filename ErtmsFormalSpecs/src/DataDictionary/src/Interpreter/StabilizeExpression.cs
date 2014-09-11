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
using DataDictionary.Interpreter.Filter;

namespace DataDictionary.Interpreter
{
    class StabilizeExpression : Expression, Utils.ISubDeclarator
    {
        /// <summary>
        /// The expression to stabilize
        /// </summary>
        public Expression Expression { get; private set; }

        /// <summary>
        /// The initial value for the stabilisation algorithm
        /// </summary>
        public Expression InitialValue { get; private set; }

        /// <summary>
        /// The condition which indicates that the stabilization is complete
        /// </summary>
        public Expression Condition { get; private set; }

        /// <summary>
        /// The value of the last iteration
        /// </summary>
        private Variables.Variable LastIteration { get; set; }

        /// <summary>
        /// The value of the current iteration
        /// </summary>
        private Variables.Variable CurrentIteration { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="root"></param>
        /// <param name="expression">The expression to stabilize</param>
        /// <param name="initialValue">The initial value for this stabilisation computation</param>
        /// <param name="condition">The condition which indicates that the stabilisation is not complete</param>
        /// <param name="start">The start character for this expression in the original string</param>
        /// <param name="end">The end character for this expression in the original string</param>
        public StabilizeExpression(ModelElement root, ModelElement log, Expression expression, Expression initialValue, Expression condition, int start, int end)
            : base(root, log, start, end)
        {
            Expression = expression;
            Expression.Enclosing = this;

            InitialValue = initialValue;
            InitialValue.Enclosing = this;

            Condition = condition;
            Condition.Enclosing = this;

            LastIteration = (Variables.Variable)Generated.acceptor.getFactory().createVariable();
            LastIteration.Enclosing = this;
            LastIteration.Name = "PREVIOUS";

            CurrentIteration = (Variables.Variable)Generated.acceptor.getFactory().createVariable();
            CurrentIteration.Enclosing = this;
            CurrentIteration.Name = "CURRENT";

            InitDeclaredElements();
        }

        /// <summary>
        /// Initialises the declared elements 
        /// </summary>
        public void InitDeclaredElements()
        {
            DeclaredElements = new Dictionary<string, List<Utils.INamable>>();

            Utils.ISubDeclaratorUtils.AppendNamable(this, LastIteration);
            Utils.ISubDeclaratorUtils.AppendNamable(this, CurrentIteration);
        }

        /// <summary>
        /// The elements declared by this declarator
        /// </summary>
        public Dictionary<string, List<Utils.INamable>> DeclaredElements { get; private set; }

        /// <summary>
        /// Appends the INamable which match the name provided in retVal
        /// </summary>
        /// <param name="name"></param>
        /// <param name="retVal"></param>
        public void Find(string name, List<Utils.INamable> retVal)
        {
            Utils.ISubDeclaratorUtils.Find(this, name, retVal);
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
                // InitialValue
                InitialValue.SemanticAnalysis(instance, IsRightSide.INSTANCE);
                StaticUsage.AddUsages(InitialValue.StaticUsage, Usage.ModeEnum.Read);

                // Expression
                Expression.SemanticAnalysis(instance, AllMatches.INSTANCE);
                StaticUsage.AddUsages(Expression.StaticUsage, Usage.ModeEnum.Read);

                // Condition
                Condition.SemanticAnalysis(instance, AllMatches.INSTANCE);
                StaticUsage.AddUsages(Condition.StaticUsage, Usage.ModeEnum.Read);

                LastIteration.Type = InitialValue.GetExpressionType();
                CurrentIteration.Type = InitialValue.GetExpressionType();
                StaticUsage.AddUsage(InitialValue.GetExpressionType(), Root, Usage.ModeEnum.Type);
            }

            return retVal;
        }

        /// <summary>
        /// Provides the type of this expression
        /// </summary>
        /// <param name="context">The interpretation context</param>
        /// <returns></returns>
        public override Types.Type GetExpressionType()
        {
            return InitialValue.GetExpressionType();
        }

        /// <summary>
        /// Provides the value associated to this Expression
        /// </summary>
        /// <param name="context">The context on which the value must be found</param>
        /// <returns></returns>
        public override Values.IValue GetValue(InterpretationContext context)
        {
            ExplanationPart previous = SetupExplanation();

            LastIteration.Value = InitialValue.GetValue(context);

            int i = 0;
            bool stop = false;
            while (!stop)
            {
                i = i + 1;
                ExplanationPart previous2 = SetupExplanation();

                ExplanationPart value = SetupExplanation();
                int token = context.LocalScope.PushContext();
                context.LocalScope.setVariable(LastIteration);
                CurrentIteration.Value = Expression.GetValue(context);
                if (explain)
                {
                    CompleteExplanation(value, "Iteration expression value = ", CurrentIteration.Value);
                }

                ExplanationPart stopExpression = SetupExplanation();
                context.LocalScope.setVariable(CurrentIteration);
                Values.BoolValue stopCondition = Condition.GetValue(context) as Values.BoolValue;
                if (stopCondition != null)
                {
                    stop = stopCondition.Val;
                }
                else
                {
                    AddError("Cannot evaluate condition " + Condition.ToString());
                    stop = true;
                }
                if (explain)
                {
                    CompleteExplanation(stopExpression, "Stop expression value = ", stopCondition);
                }
                context.LocalScope.PopContext(token);
                LastIteration.Value = CurrentIteration.Value;
                if (explain)
                {
                    CompleteExplanation(previous2, "Iteration " + i);
                }
            }

            if (explain)
            {
                CompleteExplanation(previous, ToString() + " = ", CurrentIteration.Value);
            }

            return CurrentIteration.Value;
        }

        /// <summary>
        /// Fills the list provided with the element matching the filter provided
        /// </summary>
        /// <param name="retVal">The list to be filled with the element matching the condition expressed in the filter</param>
        /// <param name="filter">The filter to apply</param>
        public override void fill(List<Utils.INamable> retVal, BaseFilter filter)
        {
            Expression.fill(retVal, filter);
            InitialValue.fill(retVal, filter);
            Condition.fill(retVal, filter);
        }

        /// <summary>
        /// Provides the string representation of the expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retVal = "STABILIZE " + Expression.ToString() + " INITIAL_VALUE " + InitialValue.ToString() + " STOP_CONDITION " + Condition.ToString();

            return retVal;
        }

        /// <summary>
        /// Checks the expression and appends errors to the root tree node when inconsistencies are found
        /// </summary>
        /// <param name="context">The interpretation context</param>
        public override void checkExpression()
        {
            base.checkExpression();

            InitialValue.checkExpression();
            Types.Type initialValueType = InitialValue.GetExpressionType();
            if (initialValueType != null)
            {
                Expression.checkExpression();
                Types.Type expressionType = Expression.GetExpressionType();
                if (expressionType != null)
                {
                    if (expressionType != initialValueType)
                    {
                        AddError("Expression " + Expression + " has not the same type (" + expressionType.FullName + " than initial value " + InitialValue + " type " + initialValueType.FullName);
                    }
                }
                else
                {
                    AddError("Cannot determine type of expression " + Expression);
                }

                Types.Type conditionType = Condition.GetExpressionType();
                if (conditionType != null)
                {
                    if (!(conditionType is Types.BoolType))
                    {
                        AddError("Condition " + Condition + " does not evaluate to a boolean");
                    }
                }
                else
                {
                    AddError("Cannot determine type of condition " + Condition);
                }
            }
            else
            {
                AddError("Cannot determine type of the initial value " + InitialValue);
            }
        }

        /// <summary>
        /// Provides the graph of this function if it has been statically defined
        /// </summary>
        /// <param name="context">the context used to create the graph</param>
        /// <returns></returns>
        public override Functions.Graph createGraph(Interpreter.InterpretationContext context, Parameter parameter)
        {
            Functions.Graph retVal = base.createGraph(context, parameter);

            retVal = Functions.Graph.createGraph(GetValue(context), parameter);

            return retVal;
        }
    }
}
