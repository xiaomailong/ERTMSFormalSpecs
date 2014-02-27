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
    public class BaseRefactorTree : Interpreter.Visitor
    {
        /// <summary>
        /// The textual expression to be refactored
        /// </summary>
        protected string Text { get; private set; }

        /// <summary>
        /// The delta in the indexes to be applied to take care 
        /// of the size difference between the replacement text 
        /// and the original text
        /// </summary>
        private int Delta { get; set; }

        /// <summary>
        /// The tree on which the replacement should occur
        /// </summary>
        private InterpreterTreeNode Tree { get; set; }

        /// <summary>
        /// Replace the text, between locations start and end with the replacement value
        /// Update Delta according to this replacement
        /// </summary>
        /// <param name="text">The replacement text</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>True when a replacement has been performed</returns>
        protected bool ReplaceText(string text, int start, int end)
        {
            start = start + Delta;
            end = end + Delta;

            bool retVal = Text.Substring(start, end - start) != text;
            if (retVal)
            {
                Text = Text.Substring(0, start) + text + Text.Substring(end, Text.Length - end);
                Delta = Delta + (text.Length - (end - start));
            }

            return retVal;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="text"></param>
        public BaseRefactorTree()
        {
            Delta = 0;
        }

        /// <summary>
        /// Executes the update and changes the corresponding Text field
        /// </summary>
        public void PerformUpdate(IExpressionable expressionable)
        {
            if (expressionable != null)
            {
                Text = expressionable.ExpressionText;

                if (expressionable.Tree != null)
                {
                    VisitInterpreterTreeNode(expressionable.Tree);
                    if (Text != expressionable.ExpressionText)
                    {
                        if (expressionable.checkValidExpression(Text))
                        {
                            expressionable.ExpressionText = Text;
                        }
                        else
                        {
                            ModelElement model = expressionable as ModelElement;
                            model.AddError("Refactoring aborded for expression " + expressionable.ExpressionText);
                        }
                    }
                }
            }
        }
    }
}
