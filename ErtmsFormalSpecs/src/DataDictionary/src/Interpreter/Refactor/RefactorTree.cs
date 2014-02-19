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
namespace DataDictionary.Interpreter.Refactor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This visitor is used to handle refactoring of expressions.
    /// </summary>
    public class RefactorTree : Interpreter.Visitor
    {
        /// <summary>
        /// The model element which should be refactored
        /// </summary>
        private ModelElement Ref { get; set; }

        /// <summary>
        /// By what should be replaced the referenced
        /// </summary>
        private string ReplacementValue { get; set; }

        /// <summary>
        /// The textual expression to be refactored
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The delta in the indexes to be applied to take care 
        /// of the size difference between the replacement text 
        /// and the original text
        /// </summary>
        private int Delta { get; set; }

        /// <summary>
        /// Replace the text, between locations start and end with the replacement value
        /// Update Delta according to this replacement
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void ReplaceText(int start, int end)
        {
            Text = Text.Substring(0, start) + ReplacementValue + Text.Substring(end, Text.Length - end);

            int len = end - start;
            Delta = Delta + (ReplacementValue.Length - len);
        }

        protected override void VisitDerefExpression(DerefExpression derefExpression)
        {
            bool replaced = false;

            if (derefExpression.Ref == Ref)
            {
                ReplaceText(derefExpression.Start + Delta, derefExpression.End + Delta);
                replaced = true;
            }
            else
            {
                foreach (Expression expression in derefExpression.Arguments)
                {
                    if (expression != null)
                    {
                        if (expression.Ref == Ref)
                        {
                            ReplaceText(derefExpression.Start + Delta, expression.End + Delta);
                            replaced = true;
                            break;
                        }
                    }
                }
            }

            if (!replaced)
            {
                base.VisitDerefExpression(derefExpression);
            }
        }

        protected override void VisitDesignator(Designator designator)
        {
            if (designator.Ref == Ref && designator.Location != Designator.LocationEnum.This)
            {
                ReplaceText(designator.Start + Delta, designator.End + Delta);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interpreterTreeNode"></param>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <param name="replacementValue"></param>
        public RefactorTree(InterpreterTreeNode interpreterTreeNode, string text, ModelElement reference, string replacementValue)
        {
            Text = text;
            Ref = reference;
            ReplacementValue = replacementValue;
            Delta = 0;

            if (interpreterTreeNode != null)
            {
                VisitInterpreterTreeNode(interpreterTreeNode);
            }
        }
    }
}
