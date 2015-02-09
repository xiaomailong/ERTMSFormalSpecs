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

using DataDictionary.Types;

namespace DataDictionary.Interpreter.Refactor
{
    /// <summary>
    /// This visitor is used to handle refactoring of expressions.
    /// </summary>
    public class RelocateTree : BaseRefactorTree
    {
        /// <summary>
        /// The new location of the element
        /// </summary>
        private ModelElement BaseLocation { get; set; }

        protected override void VisitDerefExpression(DerefExpression derefExpression)
        {
            bool replaced = false;

            if (!(derefExpression.Ref is StructureElement))
            {
                ModelElement model = derefExpression.Ref as ModelElement;
                ModelElement enclosingModel = derefExpression.Arguments[derefExpression.Arguments.Count - 2].Ref as ModelElement;
                if (model != null && enclosingModel != null)
                {
                    ReplaceText(model.ReferenceName(BaseLocation), derefExpression.Start, derefExpression.End);
                    replaced = true;
                }
                else
                {
                    enclosingModel = null;
                    foreach (Expression expression in derefExpression.Arguments)
                    {
                        if (expression != null)
                        {
                            model = expression.Ref as ModelElement;
                            if (model != null && enclosingModel != null)
                            {
                                ReplaceText(model.ReferenceName(BaseLocation), derefExpression.Start, expression.End);
                                replaced = true;
                                break;
                            }
                            enclosingModel = model;
                        }
                    }
                }
            }

            if (!replaced)
            {
                VisitExpression(derefExpression.Arguments[0]);
            }
        }

        protected override void VisitDesignator(Designator designator)
        {
            ModelElement model = designator.Ref as ModelElement;
            if (model != null && !designator.IsPredefined())
            {
                ReplaceText(model.ReferenceName(BaseLocation), designator.Start, designator.End);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interpreterTreeNode"></param>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <param name="replacementValue"></param>
        public RelocateTree(ModelElement baseLocation)
            : base()
        {
            BaseLocation = baseLocation;
        }
    }
}