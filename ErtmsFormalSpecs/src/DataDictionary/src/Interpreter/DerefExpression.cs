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
using System.Collections.Generic;
using Utils;
using DataDictionary.Interpreter.Filter;

namespace DataDictionary.Interpreter
{
    public class DerefExpression : Expression
    {
        /// <summary>
        /// Desig elements of this designator
        /// </summary>
        public List<Expression> Arguments { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="left"></param>
        /// <param name="op"></param>
        /// <param name="right"></param>
        /// <param name="start">The start character for this expression in the original string</param>
        /// <param name="end">The end character for this expression in the original string</param>
        public DerefExpression(ModelElement root, ModelElement log, List<Expression> arguments, int start, int end)
            : base(root, log, start, end)
        {
            Arguments = arguments;

            foreach (Expression expr in Arguments)
            {
                expr.Enclosing = this;
            }
        }

        /// <summary>
        /// The model element referenced by this designator.
        /// </summary>
        public override INamable Ref { get; protected set; }

        /// <summary>
        /// Provides the ICallable referenced by this 
        /// </summary>
        public ICallable Called
        {
            get
            {
                ICallable retVal = Ref as ICallable;

                if (retVal == null)
                {
                    Types.Range range = GetExpressionType() as Types.Range;
                    if (range != null)
                    {
                        retVal = range.CastFunction;
                    }
                }

                return retVal;
            }
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
                Ref = null;

                ReturnValue tmp = Arguments[0].getReferences(instance, AllMatches.INSTANCE, false);
                if (tmp.IsEmpty)
                {
                    tmp = Arguments[0].getReferenceTypes(instance, AllMatches.INSTANCE, false);
                }

                // When variables & parameters are found, only consider the first one
                // which is the one that is closer in the tree
                {
                    ReturnValue tmp2 = tmp;
                    tmp = new ReturnValue();

                    ReturnValueElement variable = null;
                    foreach (ReturnValueElement elem in tmp2.Values)
                    {
                        if (elem.Value is Parameter || elem.Value is Variables.IVariable)
                        {
                            if (variable == null)
                            {
                                variable = elem;
                                tmp.Values.Add(elem);
                            }
                        }
                        else
                        {
                            tmp.Values.Add(elem);
                        }
                    }
                }

                if (!tmp.IsEmpty)
                {
                    for (int i = 1; i < Arguments.Count; i++)
                    {
                        ReturnValue tmp2 = tmp;
                        tmp = new ReturnValue(Arguments[i]);

                        foreach (ReturnValueElement elem in tmp2.Values)
                        {
                            bool last = i == (Arguments.Count - 1);
                            tmp.Merge(elem, Arguments[i].getReferences(elem.Value, AllMatches.INSTANCE, last));
                        }

                        if (tmp.IsEmpty)
                        {
                            AddError("Cannot find " + Arguments[i].ToString() + " in " + Arguments[i - 1].ToString());
                        }
                    }
                }
                else
                {
                    AddError("Cannot evaluate " + Arguments[0].ToString());
                }

                tmp.filter(expectation);
                if (tmp.IsUnique)
                {
                    // Unique element has been found. Reference it and perform the semantic analysis 
                    // on all dereferenced expression, now that the context is known for each expression
                    Ref = tmp.Values[0].Value;
                    StaticUsage.AddUsage(Ref, Root, null);

                    Filter.References referenceFilter;
                    ReturnValueElement current = tmp.Values[0];
                    for (int i = Arguments.Count - 1; i > 0; i--)
                    {
                        referenceFilter = new Filter.References(current.Value);
                        current = current.PreviousElement;
                        Arguments[i].SemanticAnalysis(current.Value, referenceFilter);
                        StaticUsage.AddUsages(Arguments[i].StaticUsage, null);
                        StaticUsage.AddUsage(Arguments[i].Ref, Root, null);
                    }
                    referenceFilter = new Filter.References(current.Value);
                    Arguments[0].SemanticAnalysis(null, referenceFilter);
                    StaticUsage.AddUsages(Arguments[0].StaticUsage, null);
                    StaticUsage.AddUsage(Arguments[0].Ref, Root, null);
                }
                else if (tmp.IsAmbiguous)
                {
                    // Several possible interpretations for this deref expression, not allowed
                    AddError("Expression " + ToString() + " may have several interpretations " + tmp.ToString() + ", please disambiguate");
                }
                else
                {
                    // No possible interpretation for this deref expression, not allowed
                    AddError("Expression " + ToString() + " has no interpretation");
                }
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
            Types.Type retVal = Ref as Types.Type;

            if (retVal == null)
            {
                Types.ITypedElement typedElement = Ref as Types.ITypedElement;
                if (typedElement != null)
                {
                    retVal = typedElement.Type;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Provides the variable referenced by this expression, if any
        /// </summary>
        /// <param name="context">The context on which the variable must be found</param>
        /// <param name="explain"></param>
        /// <returns></returns>
        public override Variables.IVariable GetVariable(InterpretationContext context)
        {
            INamable current = null;

            InterpretationContext ctxt = new InterpretationContext(context);
            for (int i = 0; i < Arguments.Count; i++)
            {
                if (current != null)
                {
                    // Current can be null on several loop iterations when the referenced element
                    // does not references a variable (for instance, when it references a namespace)
                    ctxt.Instance = current;
                }
                current = Arguments[i].GetVariable(ctxt);
                if (current == null)
                {
                    current = Arguments[i].GetValue(ctxt, null);
                }
            }

            return current as Variables.IVariable;
        }

        /// <summary>
        /// Provides the value associated to this Expression
        /// </summary>
        /// <param name="context">The context on which the value must be found</param>
        /// <param name="explain">The explanation to fill, if any</param>
        /// <returns></returns>
        public override Values.IValue GetValue(InterpretationContext context, ExplanationPart explain)
        {
            INamable retVal = Ref as Values.IValue;

            if (retVal == null)
            {
                InterpretationContext ctxt = new InterpretationContext(context);
                for (int i = 0; i < Arguments.Count; i++)
                {
                    if (retVal != null)
                    {
                        ctxt.Instance = retVal;
                    }
                    retVal = Arguments[i].GetValue(ctxt, explain);

                    if (retVal == EFSSystem.EmptyValue)
                    {
                        break;
                    }
                }
            }

            if (retVal == null)
            {
                AddError(ToString() + " does not refer to a value");
            }

            return retVal as Values.IValue;
        }

        /// <summary>
        /// Provides the value of the prefix of the expression
        /// </summary>
        /// <param name="context">The context on which the value must be found</param>
        /// <param name="elementCount">The number of elements to consider</param>
        /// <param name="explain"></param>
        /// <returns></returns>
        public INamable GetPrefixValue(InterpretationContext context, int elementCount, ExplanationPart explain)
        {
            INamable retVal = null;

            InterpretationContext ctxt = new InterpretationContext(context);
            for (int i = 0; i < elementCount; i++)
            {
                if (retVal != null)
                {
                    ctxt.Instance = retVal;
                }
                retVal = Arguments[i].GetValue(ctxt, explain);
                if (retVal == null)
                {
                    retVal = Arguments[i].Ref;
                }

                if (retVal == EFSSystem.EmptyValue)
                {
                    break;
                }
            }

            if (retVal == null)
            {
                AddError(ToString() + " prefix does not refer to a value");
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
            ICallable retVal = Called;

            if (retVal == null)
            {
                AddError("Cannot evaluate call to " + ToString());
            }

            return retVal;
        }

        /// <summary>
        /// Fills the list provided with the element matching the filter provided
        /// </summary>
        /// <param name="retVal">The list to be filled with the element matching the condition expressed in the filter</param>
        /// <param name="filter">The filter to apply</param>
        public override void fill(List<Utils.INamable> retVal, BaseFilter filter)
        {
            if (filter.AcceptableChoice(Ref))
            {
                retVal.Add(Ref);
            }
        }

        /// <summary>
        /// Provides the string representation of the binary expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retVal = "";

            bool first = true;
            foreach (Expression expr in Arguments)
            {
                if (!first)
                {
                    retVal += ".";
                }
                retVal += expr.ToString();

                first = false;
            }

            return retVal;
        }

        /// <summary>
        /// Checks the expression and appends errors to the root tree node when inconsistencies are found
        /// </summary>
        /// <param name="context">The interpretation context</param>
        public override void checkExpression()
        {
            foreach (Expression subExpression in Arguments)
            {
                subExpression.checkExpression();
            }

            base.checkExpression();
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

            retVal = Functions.Graph.createGraph(GetValue(context, explain), parameter, explain);

            if (retVal == null)
            {
                throw new Exception("Cannot create graph for " + ToString());
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

            retVal = Functions.Surface.createSurface(GetValue(context, explain), xParam, yParam);

            if (retVal == null)
            {
                throw new Exception("Cannot create surface for " + ToString());
            }
            retVal.XParameter = xParam;
            retVal.YParameter = yParam;

            return retVal;
        }

    }
}
