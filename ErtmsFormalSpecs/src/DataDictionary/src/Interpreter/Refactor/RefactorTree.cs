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
    public class RefactorTree : BaseRefactorTree
    {
        /// <summary>
        /// The model element which should be refactored
        /// </summary>
        private ModelElement Ref { get; set; }

        /// <summary>
        /// By what should be replaced the referenced
        /// </summary>
        private string ReplacementValue { get; set; }

        protected override void VisitDerefExpression(DerefExpression derefExpression)
        {
            bool replaced = false;

            if (!(Ref is Types.StructureElement))
            {
                if (derefExpression.Ref == Ref)
                {
                    ReplaceText(ReplacementValue, derefExpression.Start, derefExpression.End);
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
                                ReplaceText(ReplacementValue, derefExpression.Start, expression.End);
                                replaced = true;
                                break;
                            }
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
                ReplaceText(ReplacementValue, designator.Start, designator.End);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interpreterTreeNode"></param>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <param name="replacementValue"></param>
        public RefactorTree(ModelElement reference, string replacementValue)
            : base()
        {
            Ref = reference;
            ReplacementValue = replacementValue;
        }
    }
}
