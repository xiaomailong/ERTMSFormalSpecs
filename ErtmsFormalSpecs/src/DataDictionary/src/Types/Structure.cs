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

using System.Collections;
using System.Collections.Generic;
using DataDictionary.Functions;
using DataDictionary.Interpreter;
using DataDictionary.Rules;
using DataDictionary.Values;
using DataDictionary.Variables;
using Utils;

namespace DataDictionary.Types
{
    public class Structure : Generated.Structure, ISubDeclarator, IFinder, TextualExplain
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Structure()
        {
            FinderRepository.INSTANCE.Register(this);
        }

        /// <summary>
        /// The structure elements 
        /// </summary>
        public ArrayList Elements
        {
            get
            {
                if (allElements() == null)
                {
                    setAllElements(new ArrayList());
                }
                return allElements();
            }
        }

        /// <summary>
        /// The structure procedures
        /// </summary>
        public ArrayList Procedures
        {
            get
            {
                if (allProcedures() == null)
                {
                    setAllProcedures(new ArrayList());
                }
                return allProcedures();
            }
        }

        /// <summary>
        /// The state machines
        /// </summary>
        public ArrayList StateMachines
        {
            get
            {
                if (allStateMachines() == null)
                {
                    setAllStateMachines(new ArrayList());
                }
                return allStateMachines();
            }
        }

        public void ClearCache()
        {
        }

        /// <summary>
        /// Initialises the declared elements 
        /// </summary>
        public void InitDeclaredElements()
        {
            DeclaredElements = new Dictionary<string, List<INamable>>();

            foreach (StructureElement element in Elements)
            {
                ISubDeclaratorUtils.AppendNamable(this, element);
            }
            foreach (Procedure procedure in Procedures)
            {
                ISubDeclaratorUtils.AppendNamable(this, procedure);
            }

            foreach (StateMachine stateMachine in StateMachines)
            {
                ISubDeclaratorUtils.AppendNamable(this, stateMachine);
            }
        }

        /// <summary>
        /// The declared elements of the structure
        /// </summary>
        public Dictionary<string, List<INamable>> DeclaredElements { get; set; }

        /// <summary>
        /// Appends the INamable which match the name provided in retVal
        /// </summary>
        /// <param name="name"></param>
        /// <param name="retVal"></param>
        public void Find(string name, List<INamable> retVal)
        {
            ISubDeclaratorUtils.Find(this, name, retVal);
        }

        /// <summary>
        /// The structure rules
        /// </summary>
        public ArrayList Rules
        {
            get
            {
                if (allRules() == null)
                {
                    setAllRules(new ArrayList());
                }
                return allRules();
            }
        }

        /// <summary>
        /// Provides the structure element which corresponds to the name provided
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StructureElement findStructureElement(string name)
        {
            return (StructureElement) INamableUtils.findByName(name, Elements);
        }

        public override ArrayList EnclosingCollection
        {
            get { return NameSpace.Structures; }
        }

        /// <summary>
        /// Provides the default value for this structure
        /// </summary>
        public override IValue DefaultValue
        {
            get
            {
                StructureValue retVal = new StructureValue(this);

                return retVal;
            }
        }

        /// <summary>
        /// Adds a model element in this model element
        /// </summary>
        /// <param name="copy"></param>
        public override void AddModelElement(IModelElement element)
        {
            {
                StructureElement item = element as StructureElement;
                if (item != null)
                {
                    appendElements(item);
                }
            }
            {
                Procedure item = element as Procedure;
                if (item != null)
                {
                    appendProcedures(item);
                }
            }
            {
                Rule item = element as Rule;
                if (item != null)
                {
                    appendRules(item);
                }
            }
            {
                StateMachine item = element as StateMachine;
                if (item != null)
                {
                    appendStateMachines(item);
                }
            }

            base.AddModelElement(element);
        }

        /// <summary>
        /// Indicates that binary operation is valid for this type and the other type 
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool ValidBinaryOperation(BinaryExpression.OPERATOR operation, Type otherType)
        {
            bool retVal;

            if (operation == BinaryExpression.OPERATOR.LESS || operation == BinaryExpression.OPERATOR.LESS_OR_EQUAL || operation == BinaryExpression.OPERATOR.GREATER || operation == BinaryExpression.OPERATOR.GREATER_OR_EQUAL)
            {
                retVal = false;
            }
            else
            {
                retVal = base.ValidBinaryOperation(operation, otherType);
            }

            return retVal;
        }

        public override bool CompareForEquality(IValue left, IValue right) // left == right
        {
            bool retVal = base.CompareForEquality(left, right);

            if (!retVal)
            {
                if (left.Type == right.Type)
                {
                    StructureValue leftValue = left as StructureValue;
                    StructureValue rightValue = right as StructureValue;

                    if (left != null && right != null)
                    {
                        retVal = true;

                        foreach (KeyValuePair<string, IVariable> pair in leftValue.SubVariables)
                        {
                            IVariable leftVar = pair.Value;
                            IVariable rightVar = rightValue.getVariable(pair.Key);

                            if (leftVar.Type != null)
                            {
                                retVal = leftVar.Type.CompareForEquality(leftVar.Value, rightVar.Value);
                                if (!retVal)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Provides an explanation of the structure
        /// </summary>
        /// <param name="indentLevel">the number of white spaces to add at the beginning of each line</param>
        /// <returns></returns>
        public string getExplain(int indentLevel)
        {
            string retVal = TextualExplainUtilities.Comment(this, indentLevel);

            retVal += TextualExplainUtilities.Pad("{\\b STRUCTURE }" + Name + " \\par ", indentLevel);
            foreach (StructureElement element in Elements)
            {
                retVal += element.getExplain(indentLevel + 2) + "\\par ";
            }

            retVal += "\\par ";
            retVal += TextualExplainUtilities.Comment("-------------------", indentLevel + 2);
            retVal += TextualExplainUtilities.Comment("Procedures", indentLevel + 2);
            retVal += TextualExplainUtilities.Comment("-------------------", indentLevel + 2);
            foreach (Procedure procedure in Procedures)
            {
                retVal += procedure.getExplain(indentLevel + 2, false) + "\\par ";
                retVal += "\\par ";
            }
            retVal += TextualExplainUtilities.Pad("{\\b END STRUCTURE }", indentLevel);

            return retVal;
        }

        /// <summary>
        /// Provides an explanation of the range
        /// </summary>
        /// <param name="explainSubElements">Precises if we need to explain the sub elements (if any)</param>
        /// <returns></returns>
        public override string getExplain(bool explainSubElements)
        {
            string retVal = getExplain(0);

            return TextualExplainUtilities.Encapsule(retVal);
        }

        /// <summary>
        /// The explanation of the element
        /// </summary>
        /// <param name="explainSubElements">Precises if we need to explain the sub elements (if any)</param>
        /// <returns></returns>
        public override string getExplain()
        {
            return getExplain(0);
        }
    }
}