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
using DataDictionary.Interpreter;

namespace DataDictionary.Types
{
    /// <summary>
    /// This is an element which has a default value
    /// </summary>
    public interface IDefaultValueElement : IExpressionable
    {
        string Default { get; set; }
    }

    /// <summary>
    /// This is an element which has a type
    /// </summary>
    public interface ITypedElement : Utils.INamable, Utils.IEnclosed, Utils.IModelElement
    {
        /// <summary>
        /// The namespace related to the typed element
        /// </summary>
        NameSpace NameSpace { get; }

        /// <summary>
        /// Provides the type name of the element
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// The type of the element
        /// </summary>
        Type Type { get; set; }

        /// <summary>
        /// Provides the mode of the typed element
        /// </summary>
        DataDictionary.Generated.acceptor.VariableModeEnumType Mode { get; }

        /// <summary>
        /// Provides the default value of the typed element
        /// </summary>
        string Default { get; set; }
    }

    /// <summary>
    /// This is a type which can enumerate its possible values
    /// </summary>
    public interface IEnumerateValues
    {
        /// <summary>
        /// Provides all constant values from this type
        /// </summary>
        /// <param name="scope">the current scope to identify the constant</param>
        /// <paramparam name="retVal">the dictionary to fill which maps name->value</paramparam>
        void Constants(string scope, Dictionary<string, object> retVal);

        /// <summary>
        /// Provides the values whose name matches the name provided
        /// </summary>
        /// <param name="index">the index in names to consider</param>
        /// <param name="names">the simple value names</param>
        Values.IValue findValue(string[] names, int index);
    }

    /// <summary>
    /// A type. All types must inherit from this class
    /// </summary>
    public class Type : Generated.Type, IDefaultValueElement, IGraphicalDisplay
    {
        /// <summary>
        /// Provides the enclosing namespace
        /// </summary>
        public NameSpace NameSpace
        {
            get
            {
                NameSpace retVal = EnclosingNameSpaceFinder.find(this);

                if (retVal == null && Dictionary != null)
                {
                    // This can be the case for artificial types
                    retVal = Dictionary.findNameSpace("Default");
                }

                return retVal;
            }
        }

        /// <summary>
        /// Gets a value based on its image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public virtual Values.IValue getValue(string image)
        {
            //            Log.ErrorFormat("Value is not available for base type: {0}", Name);
            return null;
        }

        /// <summary>
        /// The default value, as string
        /// </summary>
        public virtual string Default
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

            Expression tree = EFSSystem.Parser.Expression(this, expression, null, false, null, true);
            retVal = tree != null;

            return retVal;
        }

        /// <summary>
        /// The default value
        /// </summary>
        public virtual Values.IValue DefaultValue
        {
            get
            {
                Values.IValue retVal = null;

                try
                {
                    if (!Utils.Utils.isEmpty(Default))
                    {
                        retVal = getValue(Default);

                        if (retVal == null)
                        {
                            if (Expression != null)
                            {
                                retVal = Expression.GetValue(new InterpretationContext(this), null);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    AddException(e);
                }

                return retVal;
            }
        }

        /// <summary>
        /// Finds all references to a specific type
        /// </summary>
        private class TypeUsageFinder : Generated.Visitor
        {
            /// <summary>
            /// The usages of the type
            /// </summary>
            public HashSet<ITypedElement> Usages { get; private set; }

            /// <summary>
            /// The type looked for
            /// </summary>
            public Types.Type Target { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="target"></param>
            public TypeUsageFinder(Types.Type target)
            {
                Target = target;
                Usages = new HashSet<ITypedElement>();
            }

            public override void visit(Generated.Variable obj, bool visitSubNodes)
            {
                Variables.Variable variable = (Variables.Variable)obj;

                if (variable.Type == Target)
                {
                    Usages.Add(variable);
                }

                base.visit(obj, visitSubNodes);
            }

            public override void visit(Generated.StructureElement obj, bool visitSubNodes)
            {
                Types.StructureElement element = (Types.StructureElement)obj;

                if (element.Type == Target)
                {
                    Usages.Add(element);
                }

                base.visit(obj, visitSubNodes);
            }

        }


        /// <summary>
        /// Provides the set of typed elements which uses this type
        /// </summary>
        /// <param name="type">the type to be referenced by the typed elements</param>
        /// <returns>the set of typed elements which have 'type' as type</returns>
        public static HashSet<ITypedElement> ElementsOfType(Types.Type type)
        {
            TypeUsageFinder visitor = new TypeUsageFinder(type);

            EFSSystem efsSystem = Utils.EnclosingFinder<EFSSystem>.find(type);
            if (efsSystem != null)
            {
                foreach (Dictionary dictionary in efsSystem.Dictionaries)
                {
                    visitor.visit(dictionary);
                }
            }

            return visitor.Usages;
        }

        /// <summary>
        /// Performs the arithmetic operation based on the type of the result
        /// </summary>
        /// <param name="context">The context used to perform this operation</param>
        /// <param name="left"></param>
        /// <param name="Operation"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual Values.IValue PerformArithmericOperation(Interpreter.InterpretationContext context, Values.IValue left, BinaryExpression.OPERATOR Operation, Values.IValue right)  // left +/-/*/div/exp right
        {
            Values.IValue retVal = null;

            Functions.Function leftFunction = left as Functions.Function;
            Functions.Function rigthFunction = right as Functions.Function;

            if (leftFunction != null)
            {
                if (rigthFunction == null)
                {
                    if (leftFunction.Graph != null)
                    {
                        Functions.Graph graph = Functions.Graph.createGraph(Functions.Function.getDoubleValue(right));
                        rigthFunction = graph.Function;
                    }
                    else
                    {
                        Functions.Surface surface = Functions.Surface.createSurface(Functions.Function.getDoubleValue(right), leftFunction.Surface.XParameter, leftFunction.Surface.YParameter);
                        rigthFunction = surface.Function;
                    }
                }

                if (leftFunction.Graph != null)
                {
                    Functions.Graph tmp = null;
                    switch (Operation)
                    {
                        case BinaryExpression.OPERATOR.ADD:
                            tmp = leftFunction.Graph.AddGraph(rigthFunction.Graph);
                            break;

                        case BinaryExpression.OPERATOR.SUB:
                            tmp = leftFunction.Graph.SubstractGraph(rigthFunction.Graph);
                            break;

                        case BinaryExpression.OPERATOR.MULT:
                            tmp = leftFunction.Graph.MultGraph(rigthFunction.Graph);
                            break;

                        case BinaryExpression.OPERATOR.DIV:
                            tmp = leftFunction.Graph.DivGraph(rigthFunction.Graph);
                            break;
                    }
                    retVal = tmp.Function;
                }
                else
                {
                    Functions.Surface rightSurface = rigthFunction.getSurface(leftFunction.Surface.XParameter, leftFunction.Surface.YParameter);
                    Functions.Surface tmp = null;
                    switch (Operation)
                    {
                        case BinaryExpression.OPERATOR.ADD:
                            tmp = leftFunction.Surface.AddSurface(rightSurface);
                            break;

                        case BinaryExpression.OPERATOR.SUB:
                            tmp = leftFunction.Surface.SubstractSurface(rightSurface);
                            break;

                        case BinaryExpression.OPERATOR.MULT:
                            tmp = leftFunction.Surface.MultiplySurface(rightSurface);
                            break;

                        case BinaryExpression.OPERATOR.DIV:
                            tmp = leftFunction.Surface.DivideSurface(rightSurface);
                            break;
                    }
                    retVal = tmp.Function;
                }
            }

            return retVal;
        }

        public virtual bool CompareForEquality(Values.IValue left, Values.IValue right)  // left == right
        {
            return left == right;
        }

        public virtual bool Less(Values.IValue left, Values.IValue right)  // left < right
        {
            throw new TypeInconsistancyException("Cannot compare " + left.ToString() + " with " + right.ToString());
        }

        public virtual bool Greater(Values.IValue left, Values.IValue right)  // left > right
        {
            throw new TypeInconsistancyException("Cannot compare " + left.ToString() + " with " + right.ToString());
        }

        public virtual bool Contains(Values.IValue right, Values.IValue left)  // left in right
        {
            throw new TypeInconsistancyException("Variable of type " + GetType() + " cannot contain a variable of type " + left.GetType());
        }

        /// <summary>
        /// Indicates that the other type can be placed in variables of this type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public virtual bool Match(Type otherType)
        {
            if (otherType is AnyType)
            {
                return true;
            }
            else
            {
                return this == otherType;
            }
        }

        /// <summary>
        /// Indicates that binary operation is valid for this type and the other type 
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public virtual bool ValidBinaryOperation(BinaryExpression.OPERATOR operation, Type otherType)
        {
            bool retVal;

            if (operation == BinaryExpression.OPERATOR.IN || operation == BinaryExpression.OPERATOR.NOT_IN)
            {
                Collection collectionType = otherType as Collection;
                if (collectionType != null)
                {
                    retVal = Match(collectionType.Type);
                }
                else
                {
                    retVal = Match(otherType);
                }
            }
            else
            {
                retVal = Match(otherType);
            }

            return retVal;
        }

        /// <summary>
        /// Indicates if the type is double
        /// </summary>
        /// <param name="root"></param>
        /// <param name="expression"></param>
        /// <param name="parameter"></param>
        public bool IsDouble()
        {
            bool retVal = false;

            Types.Range range = this as Types.Range;
            if (range != null)
            {
                retVal = range.getPrecision() == Generated.acceptor.PrecisionEnum.aDoublePrecision;
            }
            else
            {
                retVal = this == EFSSystem.DoubleType;
            }

            return retVal;
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
        /// Combines two types to create a new one
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual Type CombineType(Type right, BinaryExpression.OPERATOR Operator)
        {
            return null;
        }

        /// <summary>
        /// The X position
        /// </summary>
        public int X
        {
            get { return getX(); }
            set { setX(value); }
        }

        /// <summary>
        /// The Y position
        /// </summary>
        public int Y
        {
            get { return getY(); }
            set { setY(value); }
        }

        /// <summary>
        /// The width
        /// </summary>
        public int Width
        {
            get { return getWidth(); }
            set { setWidth(value); }
        }

        /// <summary>
        /// The height
        /// </summary>
        public int Height
        {
            get { return getHeight(); }
            set { setHeight(value); }
        }

        /// <summary>
        /// The name to be displayed
        /// </summary>
        public virtual string GraphicalName { get { return Name; } }

        /// <summary>
        /// Indicates whether the namespace is hidden
        /// </summary>
        public bool Hidden
        {
            get { return getHidden(); }
            set { setHidden(value); }
        }

        /// <summary>
        /// Indicates that the element is pinned
        /// </summary>
        public bool Pinned { get { return getPinned(); } set { setPinned(value); } }

        /// <summary>
        /// Provides an explanation of the range
        /// </summary>
        /// <param name="explainSubElements">Precises if we need to explain the sub elements (if any)</param>
        /// <returns></returns>
        public virtual string getExplain(bool explainSubElements)
        {
            return Name + " {\\b : STATE MACHINE}";
        }
    }

    /// <summary>
    /// Anything
    /// </summary>
    public class AnyType : Type
    {
        public override string Name
        {
            get { return "AnyType"; }
            set { }
        }

        public override string FullName
        {
            get { return Name; }
        }

        /// <summary>
        /// Constrcutor
        /// </summary>
        public AnyType(EFSSystem efsSystem)
        {
            Enclosing = efsSystem;
        }

        public override Values.IValue PerformArithmericOperation(InterpretationContext context, Values.IValue left, BinaryExpression.OPERATOR Operation, Values.IValue right)
        {
            throw new Exception("Cannot perform arithmetic operation between " + left.LiteralName + " and " + right.LiteralName);
        }

        /// <summary>
        /// Indicates that the other type can be placed in variables of this type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool Match(Type otherType)
        {
            if (otherType is NoType)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Nothing
    /// </summary>
    public class NoType : Type
    {
        public override string Name
        {
            get { return "NoType"; }
            set { }
        }

        public override string FullName
        {
            get { return Name; }
        }

        /// <summary>
        /// Constrcutor
        /// </summary>
        public NoType(EFSSystem efsSystem)
        {
            Enclosing = efsSystem;
        }

        public override Values.IValue PerformArithmericOperation(InterpretationContext context, Values.IValue left, BinaryExpression.OPERATOR Operation, Values.IValue right)
        {
            throw new Exception("Cannot perform arithmetic operation between " + left.LiteralName + " and " + right.LiteralName);
        }

        /// <summary>
        /// Indicates that the other type can be placed in variables of this type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool Match(Type otherType)
        {
            return false;
        }
    }
}
