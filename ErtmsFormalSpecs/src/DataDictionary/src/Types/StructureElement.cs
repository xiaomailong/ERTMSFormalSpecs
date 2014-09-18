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

namespace DataDictionary.Types
{
    public class StructureElement : Generated.StructureElement, ITypedElement, Utils.ISubDeclarator, TextualExplain, IDefaultValueElement
    {
        public NameSpace NameSpace
        {
            get { return EnclosingNameSpaceFinder.find(this); }
        }

        /// <summary>
        /// Provides the mode of the structure element
        /// </summary>
        public DataDictionary.Generated.acceptor.VariableModeEnumType Mode
        {
            get { return getMode(); }

            set { setMode(value); }
        }

        /// <summary>
        /// Initialises the declared elements 
        /// </summary>
        public void InitDeclaredElements()
        {
        }

        /// <summary>
        /// Provides all the values that can be stored in this structure
        /// </summary>
        public Dictionary<string, List<Utils.INamable>> DeclaredElements
        {
            get
            {
                Dictionary<string, List<Utils.INamable>> retVal = new Dictionary<string, List<Utils.INamable>>();

                if (Type is Structure)
                {
                    Structure structure = Type as Structure;

                    if (structure.DeclaredElements == null)
                    {
                        structure.InitDeclaredElements();
                    }
                    retVal = structure.DeclaredElements;
                }

                return retVal;
            }
        }

        /// <summary>
        /// Appends the INamable which match the name provided in retVal
        /// </summary>
        /// <param name="name"></param>
        /// <param name="retVal"></param>
        public void Find(string name, List<Utils.INamable> retVal)
        {
            if (Type is Structure)
            {
                Structure structure = Type as Structure;
                structure.Find(name, retVal);
            }
        }

        /// <summary>
        /// Provides the type name of the structure element
        /// </summary>
        public string TypeName
        {
            get
            {
                return getTypeName();
            }
            set
            {
                Type = null;
                setTypeName(value);

                // Ensure types and typename are synchronized
                __type = Type;
            }
        }

        /// <summary>
        /// The cached type
        /// </summary>
        private Type __type;

        /// <summary>
        /// The type associated to this structure element
        /// </summary>
        public Type Type
        {
            get
            {
                Type retVal = __type;

                if (retVal == null)
                {
                    // Find the corresponding state machine in the structure's state machines
                    Structure structure = (Structure)Enclosing;
                    List<Utils.INamable> tmp = new List<Utils.INamable>();
                    structure.Find(getTypeName(), tmp);
                    foreach (Utils.INamable namable in tmp)
                    {
                        StateMachine stateMachine = namable as StateMachine;
                        if (stateMachine != null)
                        {
                            retVal = stateMachine;
                            break;
                        }
                    }

                    if (retVal == null)
                    {
                        retVal = EFSSystem.findType(NameSpace, getTypeName());
                    }
                }

                return retVal;
            }
            set
            {
                __type = value;
                if (value != null)
                {
                    setTypeName(value.getName());
                }
                else
                {
                    setTypeName(null);
                }
            }
        }

        /// <summary>
        /// The enclosing structure
        /// </summary>
        public Structure Structure
        {
            get { return (Structure)getFather(); }
        }

        /// <summary>
        /// The enclosing collection
        /// </summary>
        public override System.Collections.ArrayList EnclosingCollection
        {
            get { return Structure.Elements; }
        }

        /// <summary>
        /// The default value
        /// </summary>
        public string Default
        {
            get { return getDefault(); }
            set { setDefault(value); }
        }

        public override string ExpressionText
        {
            get
            {
                string retVal = Default;

                if (retVal == null)
                {
                    retVal = "";
                }

                return retVal;
            }
            set
            {
                Default = value;
                __expression = null;
            }
        }

        /// <summary>
        /// Provides the expression tree associated to this action's expression
        /// </summary>
        private Interpreter.Expression __expression;
        public Interpreter.Expression Expression
        {
            get
            {
                if (__expression == null)
                {
                    __expression = EFSSystem.Parser.Expression(this, ExpressionText);
                }

                return __expression;
            }
            set
            {
                __expression = value;
            }
        }

        public Interpreter.InterpreterTreeNode Tree { get { return Expression; } }

        /// <summary>
        /// Clears the expression tree to ensure new compilation
        /// </summary>
        public void CleanCompilation()
        {
            Expression = null;
        }

        /// <summary>
        /// Creates the tree according to the expression text
        /// </summary>
        public Interpreter.InterpreterTreeNode Compile()
        {
            // Side effect, builds the statement if it is not already built
            return Tree;
        }


        /// <summary>
        /// Indicates that the expression is valid for this IExpressionable
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool checkValidExpression(string expression)
        {
            bool retVal = false;

            Interpreter.Expression tree = EFSSystem.Parser.Expression(this, expression, null, false, null, true);
            retVal = tree != null;

            return retVal;
        }

        /// <summary>
        /// Provides the variable's default value
        /// </summary>
        public Values.IValue DefaultValue
        {
            get
            {
                Values.IValue retVal = null;

                if (Type != null)
                {
                    if (Utils.Utils.isEmpty(Default))
                    {
                        retVal = Type.DefaultValue;
                    }
                    else
                    {
                        retVal = Type.getValue(Default);

                        if (retVal == null)
                        {
                            if (Expression != null)
                            {
                                retVal = Expression.GetValue(new Interpreter.InterpretationContext(this), null);
                                if (retVal != null && !Type.Match(retVal.Type))
                                {
                                    AddError("Default value type (" + retVal.Type.Name + ")does not match variable type (" + Type.Name + ")");
                                    retVal = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    AddError("Cannot find type of variable (" + getTypeName() + ")");
                }

                if (retVal == null)
                {
                    AddError("Cannot create default value");
                }

                return retVal;
            }
        }

        /// <summary>
        /// Adds a model element in this model element
        /// </summary>
        /// <param name="copy"></param>
        public override void AddModelElement(Utils.IModelElement element)
        {
            base.AddModelElement(element);
        }

        /// <summary>
        /// Provides an explanation of the structure element
        /// </summary>
        /// <param name="indentLevel">the number of white spaces to add at the beginning of each line</param>
        /// <returns></returns>
        public string getExplain(int indentLevel)
        {
            string retVal = TextualExplainUtilities.Comment(this, indentLevel);

            retVal += TextualExplainUtilities.Pad(Name + " : " + TypeName, indentLevel);

            return retVal;
        }

        /// <summary>
        /// Provides an explanation of the range
        /// </summary>
        /// <param name="explainSubElements">Precises if we need to explain the sub elements (if any)</param>
        /// <returns></returns>
        public string getExplain(bool explainSubElements)
        {
            string retVal = getExplain(0);

            return TextualExplainUtilities.Encapsule(retVal);
        }

    }
}
