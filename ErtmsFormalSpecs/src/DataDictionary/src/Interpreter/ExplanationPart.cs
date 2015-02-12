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
using DataDictionary.Functions;
using DataDictionary.Rules;
using DataDictionary.Values;
using Utils;

namespace DataDictionary.Interpreter
{
    /// <summary>
    /// Part of the explanation of an evaluation
    /// </summary>
    public class ExplanationPart
    {
        /// <summary>
        /// The explanation message
        /// </summary>
        private string _message;

        public string Message
        {
            get
            {
                string retVal = _message;
                if (retVal == null)
                {
                    retVal = "";
                }

                if (Change != null)
                {
                    if (Change.NewValue != null)
                    {
                        retVal += Change.Variable.FullName + " <- " + explainNamable(Change.NewValue);
                    }
                    else
                    {
                        retVal += Change.Variable.FullName + " <- <cannot evaluate value>";
                    }
                }

                if (Expression != null)
                {
                    retVal += Expression.ToString();
                }

                if (Namable != null)
                {
                    retVal += explainNamable(Namable);
                }

                return retVal;
            }
            set { _message = value; }
        }

        /// <summary>
        /// The list of sub explanations
        /// </summary>
        public List<ExplanationPart> SubExplanations { get; private set; }

        /// <summary>
        /// The model element for which the explanation is created
        /// </summary>
        public ModelElement Element { get; private set; }

        /// <summary>
        /// The (optional) change for which this explanation part is created
        /// </summary>
        public Change Change { get; private set; }

        /// <summary>
        /// The (optional) expression for which this explanation part is created
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// The (optional) value for which this explanation part is created
        /// </summary>
        public INamable Namable { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element">The element for which this explanation part is created</param>
        public ExplanationPart(ModelElement element, string message, INamable namable = null)
        {
            Element = element;
            Message = message;
            Namable = namable;
            SubExplanations = new List<ExplanationPart>();
        }

        /// <summary>
        /// Constructor for an explanation, based on a change
        /// </summary>
        /// <param name="element">The element for which this explanation part is created</param>
        /// <param name="change">The change performed</param>
        public ExplanationPart(ModelElement element, Change change)
        {
            Element = element;
            Change = change;
            SubExplanations = new List<ExplanationPart>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element">The element for which this explanation part is created</param>
        public ExplanationPart(ModelElement element, Expression expression)
        {
            Element = element;
            Expression = expression;
            SubExplanations = new List<ExplanationPart>();
        }


        /// <summary>
        /// Provides the textual representation of the namable provided
        /// </summary>
        /// <param name="namable"></param>
        /// <returns></returns>
        private string explainNamable(INamable namable)
        {
            string retVal = "";

            if (namable != null)
            {
                retVal = namable.Name;

                Function fonction = namable as Function;
                if (fonction != null)
                {
                    if (fonction.Graph != null)
                    {
                        retVal = fonction.Graph.ToString();
                    }
                    else if (fonction.Surface != null)
                    {
                        retVal = fonction.Surface.ToString();
                    }
                }
                else
                {
                    IValue value = namable as IValue;
                    if (value != null)
                    {
                        retVal = value.LiteralName;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Adds a sub explanation for the explain provided as parameter
        /// </summary>
        /// <param name="explain"></param>
        /// <param name="subExplain"></param>
        public static void AddSubExplanation(ExplanationPart explain, ExplanationPart subExplain)
        {
            if (explain != null)
            {
                explain.SubExplanations.Add(subExplain);
            }
        }

        /// <summary>
        /// Creates a sub explanation for the explain provided as parameter
        /// </summary>
        /// <param name="explain"></param>
        /// <param name="subExpression"></param>
        /// <returns></returns>
        public static ExplanationPart CreateSubExplanation(ExplanationPart explain, Expression subExpression)
        {
            ExplanationPart retVal = null;

            if (explain != null)
            {
                retVal = new ExplanationPart(explain.Element, subExpression);
                explain.SubExplanations.Add(retVal);
            }

            return retVal;
        }

        /// <summary>
        /// Creates a sub explanation for the explain provided as parameter
        /// </summary>
        /// <param name="explain"></param>
        /// <param name="change"></param>
        /// <returns></returns>
        public static ExplanationPart CreateSubExplanation(ExplanationPart explain, ModelElement root, Change change)
        {
            ExplanationPart retVal = null;

            if (explain != null)
            {
                retVal = new ExplanationPart(root, change);
                explain.SubExplanations.Add(retVal);
            }

            return retVal;
        }

        /// <summary>
        /// Creates a sub explanation for the explain provided as parameter
        /// </summary>
        /// <param name="explain"></param>
        /// <param name="name"></param>
        /// <param name="namable"></param>
        /// <returns></returns>
        public static ExplanationPart CreateSubExplanation(ExplanationPart explain, string name)
        {
            ExplanationPart retVal = null;

            if (explain != null)
            {
                retVal = new ExplanationPart(explain.Element, name);
                explain.SubExplanations.Add(retVal);
            }

            return retVal;
        }

        /// <summary>
        /// Sets the value associated to the explanation
        /// </summary>
        /// <param name="explain"></param>
        /// <param name="namable"></param>
        public static void SetNamable(ExplanationPart explain, INamable namable)
        {
            if (explain != null)
            {
                explain.Namable = namable;
            }
        }
    }
}