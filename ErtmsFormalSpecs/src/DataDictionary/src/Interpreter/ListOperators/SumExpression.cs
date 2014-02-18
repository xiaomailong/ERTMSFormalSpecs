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

using DataDictionary.Interpreter.Filter;
namespace DataDictionary.Interpreter.ListOperators
{
    public class SumExpression : ExpressionBasedListExpression, Utils.ISubDeclarator
    {
        /// <summary>
        /// The operator for this expression
        /// </summary>
        public static string OPERATOR = "SUM";

        /// <summary>
        /// The accumulator variable
        /// </summary>
        public Variables.Variable AccumulatorVariable { get; private set; }

        /// <summary>
        /// The accumulation expression, as defined in the statement
        /// </summary>
        private Expression DefinedAccumulator { get; set; }

        /// <summary>
        /// The accumulator expression to be used for evaluation 
        /// </summary>
        public Expression Accumulator { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listExpression"></param>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <param name="root">the root element for which this expression should be parsed</param>
        /// <param name="start">The start character for this expression in the original string</param>
        /// <param name="end">The end character for this expression in the original string</param>
        public SumExpression(ModelElement root, ModelElement log, Expression listExpression, Expression condition, Expression expression, int start, int end)
            : base(root, log, listExpression, condition, expression, start, end)
        {
            AccumulatorVariable = (Variables.Variable)Generated.acceptor.getFactory().createVariable();
            AccumulatorVariable.Enclosing = this;
            AccumulatorVariable.Name = "RESULT";
            Utils.ISubDeclaratorUtils.AppendNamable(this, AccumulatorVariable);

            DefinedAccumulator = expression;
            DefinedAccumulator.Enclosing = this;

            Accumulator = new BinaryExpression(Root, RootLog, DefinedAccumulator, BinaryExpression.OPERATOR.ADD, new UnaryExpression(Root, RootLog, new Term(Root, RootLog, new Designator(Root, RootLog, "RESULT", -1, -1), -1, -1), -1, -1), -1, -1);
            Accumulator.Enclosing = this;
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
                // Accumulator
                AccumulatorVariable.Type = GetExpressionType();

                DefinedAccumulator.SemanticAnalysis(instance, AllMatches.INSTANCE);

                Accumulator.SemanticAnalysis(instance, AllMatches.INSTANCE);
                StaticUsage.AddUsages(Accumulator.StaticUsage, Usage.ModeEnum.Read);
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
            return IteratorExpression.GetExpressionType();
        }

        /// <summary>
        /// Provides the value associated to this Expression
        /// </summary>
        /// <param name="context">The context on which the value must be found</param>
        /// <returns></returns>
        public override Values.IValue GetValue(InterpretationContext context)
        {
            Values.IValue retVal = null;

            Values.ListValue value = ListExpression.GetValue(context) as Values.ListValue;
            if (value != null)
            {
                int token = PrepareIteration(context);
                context.LocalScope.setVariable(AccumulatorVariable);

                Types.Type resultType = GetExpressionType();
                if (resultType != null)
                {
                    AccumulatorVariable.Value = resultType.getValue("0");

                    foreach (Values.IValue v in value.Val)
                    {
                        if (v != EFSSystem.EmptyValue)
                        {
                            IteratorVariable.Value = v;
                            if (conditionSatisfied(context))
                            {
                                AccumulatorVariable.Value = Accumulator.GetValue(context);
                            }
                        }
                        NextIteration();
                    }
                }

                EndIteration(context, token);

                retVal = AccumulatorVariable.Value;
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

            retVal = retVal + " USING " + IteratorExpression.ToString();

            return retVal;
        }

        /// <summary>
        /// Checks the expression and appends errors to the root tree node when inconsistencies are found
        /// </summary>
        public override void checkExpression()
        {
            base.checkExpression();

            Types.Collection listExpressionType = ListExpression.GetExpressionType() as Types.Collection;
            if (listExpressionType != null)
            {
                IteratorExpression.checkExpression();
            }

            Accumulator.checkExpression();
            if (!(DefinedAccumulator.GetExpressionType() is Types.Range))
            {
                AddError("Accumulator expression should be a range");
            }
        }
    }
}
