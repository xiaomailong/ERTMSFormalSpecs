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
    public class ReNameSpaceTree : BaseRefactorTree
    {
        /// <summary>
        /// The new location of the element
        /// </summary>
        private Types.NameSpace NewNameSpace { get; set; }

        protected override void VisitDerefExpression(DerefExpression derefExpression)
        {
            bool replaced = false;

            if (!(derefExpression.Ref is Types.StructureElement))
            {
                ModelElement model = derefExpression.Ref as ModelElement;
                if (model != null)
                {
                    ReplaceText(model.ReferenceName(NewNameSpace), derefExpression.Start, derefExpression.End);
                    replaced = true;
                }
                else
                {
                    foreach (Expression expression in derefExpression.Arguments)
                    {
                        if (expression != null)
                        {
                            model = expression.Ref as ModelElement;
                            if (model != null)
                            {
                                ReplaceText(model.ReferenceName(NewNameSpace), derefExpression.Start, expression.End);
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
            ModelElement model = designator.Ref as ModelElement;
            if (model != null && !designator.IsPredefined())
            {
                ReplaceText(model.ReferenceName(NewNameSpace), designator.Start, designator.End);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interpreterTreeNode"></param>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <param name="replacementValue"></param>
        public ReNameSpaceTree(Types.NameSpace newNameSpace)
            : base()
        {
            NewNameSpace = newNameSpace;
        }
    }
}
