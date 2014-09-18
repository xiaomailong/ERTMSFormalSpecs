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

namespace DataDictionary.Interpreter.ListOperators
{
    public class CountExpression : ConditionBasedListExpression
    {
        /// <summary>
        /// The operator for this expression
        /// </summary>
        public static string OPERATOR = "COUNT";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listExpression"></param>
        /// <param name="condition"></param>
        /// <param name="root">the root element for which this expression should be parsed</param>
        /// <param name="start">The start character for this expression in the original string</param>
        /// <param name="end">The end character for this expression in the original string</param>
        public CountExpression(ModelElement root, ModelElement log, Expression listExpression, Expression condition, int start, int end)
            : base(root, log, listExpression, condition, start, end)
        {
        }

        /// <summary>
        /// Provides the type of this expression
        /// </summary>
        /// <param name="context">The interpretation context</param>
        /// <returns></returns>
        public override Types.Type GetExpressionType()
        {
            return EFSSystem.IntegerType;
        }

        /// <summary>
        /// Provides the value associated to this Expression
        /// </summary>
        /// <param name="context">The context on which the value must be found</param>
        /// <param name="explain">The explanation to fill, if any</param>
        /// <returns></returns>
        public override Values.IValue GetValue(InterpretationContext context, ExplanationPart explain)
        {
            int count = 0;
            Values.ListValue value = ListExpression.GetValue(context, explain) as Values.ListValue;
            if (value != null)
            {
                int token = PrepareIteration(context);
                foreach (Values.IValue v in value.Val)
                {
                    if (v != EFSSystem.EmptyValue)
                    {
                        IteratorVariable.Value = v;
                        if (conditionSatisfied(context, explain))
                        {
                            count += 1;
                        }
                    }
                    NextIteration();
                }
                EndIteration(context, token);
            }

            return new Values.IntValue(EFSSystem.IntegerType, count); ;
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

            return retVal;
        }
    }
}
