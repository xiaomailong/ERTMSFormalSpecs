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

namespace DataDictionary.Interpreter.ListOperators
{
    public class MapExpression : ExpressionBasedListExpression
    {
        /// <summary>
        /// The operator for this expression
        /// </summary>
        public static string OPERATOR = "MAP";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listExpression"></param>
        /// <param name="condition"></param>
        /// <param name="function"></param>
        /// <param name="iteratorVariableName"></param>
        /// <param name="start">The start character for this expression in the original string</param>
        /// <param name="end">The end character for this expression in the original string</param>
        public MapExpression(ModelElement root, ModelElement log, Expression listExpression, string iteratorVariableName, Expression condition, Expression function, int start, int end)
            : base(root, log, listExpression, iteratorVariableName, condition, function, start, end)
        {
        }

        /// <summary>
        /// Provides the type of this expression
        /// </summary>
        /// <param name="context">The interpretation context</param>
        /// <returns></returns>
        public override Types.Type GetExpressionType()
        {
            Types.Type retVal = null;

            Types.Type iteratorType = IteratorExpression.GetExpressionType();
            if (iteratorType != null)
            {
                Types.Collection collection = (Types.Collection)Generated.acceptor.getFactory().createCollection();
                collection.Enclosing = EFSSystem;
                collection.Type = iteratorType;

                retVal = collection;
            }
            else
            {
                AddError("Cannot evaluate iterator type for " + ToString());
            }

            return retVal;
        }

        /// <summary>
        /// Provides the value associated to this Expression
        /// </summary>
        /// <param name="context">The context on which the value must be found</param>
        /// <param name="explain">The explanation to fill, if any</param>
        /// <returns></returns>
        public override Values.IValue GetValue(InterpretationContext context, ExplanationPart explain)
        {
            Values.ListValue retVal = null;

            Values.ListValue value = ListExpression.GetValue(context, explain) as Values.ListValue;
            if (value != null)
            {
                int token = PrepareIteration(context);
                retVal = new Values.ListValue((Types.Collection)GetExpressionType(), new List<Values.IValue>());
                foreach (Values.IValue v in value.Val)
                {
                    if (v != EFSSystem.EmptyValue)
                    {
                        ElementFound = true;
                        IteratorVariable.Value = v;

                        if (conditionSatisfied(context, explain))
                        {
                            MatchingElementFound = true;
                            retVal.Val.Add(IteratorExpression.GetValue(context, explain));
                        }
                    }
                    NextIteration();
                }
                EndIteration(context, explain, token);
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

            retVal = retVal + " USING " + IteratorVariable.Name + " IN " + IteratorExpression.ToString();

            return retVal;
        }

        /// <summary>
        /// Checks the expression and appends errors to the root tree node when inconsistencies are found
        /// </summary>
        /// <param name="context">The interpretation context</param>
        public override void checkExpression()
        {
            base.checkExpression();

            IteratorExpression.checkExpression();
        }
    }
}
