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
using DataDictionary.Rules;

namespace DataDictionary.Interpreter.Statement
{
    public class ReplaceStatement : Statement, Utils.ISubDeclarator
    {
        /// <summary>
        /// The value to replace 
        /// </summary>
        public Expression Value { get; private set; }

        /// <summary>
        /// The list on which the value should be inserted
        /// </summary>
        public Expression ListExpression { get; private set; }

        /// <summary>
        /// The condition which indicates which element should be replaced
        /// </summary>
        public Expression Condition { get; private set; }

        /// <summary>
        /// The iterator variable
        /// </summary>
        public Variables.Variable IteratorVariable { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="root">The root element for which this element is built</param>
        /// <param name="value">The value to insert in the list</param>
        /// <param name="listExpression">The list affected by the replace statement</param>
        /// <param name="condition">The condition which indicates the value to be replaced</param>
        /// <param name="start">The start character for this expression in the original string</param>
        /// <param name="end">The end character for this expression in the original string</param>
        public ReplaceStatement(ModelElement root, ModelElement log, Expression value, Expression listExpression, Expression condition, int start, int end)
            : base(root, log, start, end)
        {
            Value = value;
            Value.Enclosing = this;

            ListExpression = listExpression;
            ListExpression.Enclosing = this;

            Condition = condition;
            Condition.Enclosing = this;

            IteratorVariable = (Variables.Variable)Generated.acceptor.getFactory().createVariable();
            IteratorVariable.Enclosing = this;
            IteratorVariable.Name = "X";

            InitDeclaredElements();
        }

        /// <summary>
        /// Initialises the declared elements 
        /// </summary>
        public void InitDeclaredElements()
        {
            DeclaredElements = new Dictionary<string, List<Utils.INamable>>();

            Utils.ISubDeclaratorUtils.AppendNamable(this, IteratorVariable);
        }

        /// <summary>
        /// The elements declared by this declarator
        /// </summary>
        public Dictionary<string, List<Utils.INamable>> DeclaredElements { get; private set; }

        /// <summary>
        /// Appends the INamable which match the name provided in retVal
        /// </summary>
        /// <param name="name"></param>
        /// <param name="retVal"></param>
        public void Find(string name, List<Utils.INamable> retVal)
        {
            Utils.ISubDeclaratorUtils.Find(this, name, retVal);
        }

        /// <summary>
        /// Performs the semantic analysis of the statement
        /// </summary>
        /// <param name="instance">the reference instance on which this element should analysed</param>
        /// <returns>True if semantic analysis should be continued</returns>
        public override bool SemanticAnalysis(Utils.INamable instance)
        {
            bool retVal = base.SemanticAnalysis(instance);

            if (retVal)
            {
                // ListExpression
                ListExpression.SemanticAnalysis(instance);
                StaticUsage.AddUsages(ListExpression.StaticUsage, Usage.ModeEnum.ReadAndWrite);

                Types.Collection collectionType = ListExpression.GetExpressionType() as Types.Collection;
                if (collectionType != null)
                {
                    IteratorVariable.Type = collectionType.Type;
                }

                // Value
                Value.SemanticAnalysis(instance);
                StaticUsage.AddUsages(Value.StaticUsage, Usage.ModeEnum.Read);
                Types.Type valueType = Value.GetExpressionType();
                if (valueType != null)
                {
                    if (!valueType.Match(collectionType.Type))
                    {
                        AddError("Type of " + Value + " does not match collection type " + collectionType);
                    }
                }
                else
                {
                    AddError("Cannot determine type of " + Value);
                }

                // Condition
                if (Condition != null)
                {
                    Condition.SemanticAnalysis(instance);
                    StaticUsage.AddUsages(Condition.StaticUsage, Usage.ModeEnum.Read);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Provides the list of elements read by this statement
        /// </summary>
        /// <param name="retVal">the list to fill</param>
        public override void ReadElements(List<Types.ITypedElement> retVal)
        {
            retVal.AddRange(Value.GetVariables());
            retVal.AddRange(ListExpression.GetVariables());
        }

        /// <summary>
        /// Provides the statement which modifies the variable
        /// </summary>
        /// <param name="variable"></param>
        /// <returns>null if no statement modifies the element</returns>
        public override VariableUpdateStatement Modifies(Types.ITypedElement variable)
        {
            VariableUpdateStatement retVal = null;

            return retVal;
        }

        /// <summary>
        /// Provides the list of update statements induced by this statement
        /// </summary>
        /// <param name="retVal">the list to fill</param>
        public override void UpdateStatements(List<VariableUpdateStatement> retVal)
        {
        }

        /// <summary>
        /// Indicates whether the condition is satisfied with the value provided
        /// Hyp : the value of the iterator variable has been assigned before
        /// </summary>
        /// <param name="context"></param>
        /// <param name="explain"></param>
        /// <returns></returns>
        public bool conditionSatisfied(InterpretationContext context, ExplanationPart explain)
        {
            bool retVal = true;

            if (Condition != null)
            {
                Values.BoolValue b = Condition.GetValue(context, explain) as Values.BoolValue;
                if (b == null)
                {
                    retVal = false;
                }
                else
                {
                    retVal = b.Val;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Checks the statement for semantical errors
        /// </summary>
        public override void CheckStatement()
        {
            if (ListExpression.Ref is Parameter)
            {
                Root.AddError("Cannot change the list value which is a parameter (" + ListExpression.ToString() + ")");
            }

            Value.checkExpression();

            Types.Collection targetListType = ListExpression.GetExpressionType() as Types.Collection;
            if (targetListType != null)
            {
                Types.Type elementType = Value.GetExpressionType();
                if (elementType != targetListType.Type)
                {
                    Root.AddError("Inserted element type does not corresponds to list type");
                }

                Condition.checkExpression();
                Types.BoolType conditionType = Condition.GetExpressionType() as Types.BoolType;
                if (conditionType == null)
                {
                    Root.AddError("Condition does not evaluates to boolean");
                }

            }
            else
            {
                Root.AddError("Cannot determine collection type of " + ListExpression);
            }
        }

        /// <summary>
        /// Provides the changes performed by this statement
        /// </summary>
        /// <param name="context">The context on which the changes should be computed</param>
        /// <param name="changes">The list to fill with the changes</param>
        /// <param name="explanation">The explanatino to fill, if any</param>
        /// <param name="apply">Indicates that the changes should be applied immediately</param>
        /// <param name="runner"></param>
        public override void GetChanges(InterpretationContext context, ChangeList changes, ExplanationPart explanation, bool apply, Tests.Runner.Runner runner)
        {
            int index = context.LocalScope.PushContext();
            context.LocalScope.setVariable(IteratorVariable);

            Variables.IVariable variable = ListExpression.GetVariable(context);
            if (variable != null)
            {
                // HacK : ensure that the value is a correct rigth side
                // and keep the result of the right side operation
                Values.ListValue listValue = variable.Value.RightSide(variable, false, false) as Values.ListValue;
                variable.Value = listValue;
                if (listValue != null)
                {
                    Values.ListValue newListValue = new Values.ListValue(listValue);

                    int i = 0;
                    foreach (Values.IValue current in newListValue.Val)
                    {
                        IteratorVariable.Value = current;
                        if (conditionSatisfied(context, explanation))
                        {
                            break;
                        }
                        i += 1;
                    }

                    if (i < newListValue.Val.Count)
                    {
                        Values.IValue value = Value.GetValue(context, explanation);
                        if (value != null)
                        {
                            newListValue.Val[i] = value;
                            Rules.Change change = new Rules.Change(variable, variable.Value, newListValue);
                            changes.Add(change, apply, runner);

                            if (explanation != null)
                            {
                                explanation.SubExplanations.Add(new ExplanationPart(Root, change));
                            }
                        }
                        else
                        {
                            Root.AddError("Cannot find value for " + Value.ToString());
                        }
                    }
                    else
                    {
                        Root.AddError("Cannot find value in " + ListExpression.ToString() + " which satisfies " + Condition.ToString());
                    }
                }
                else
                {
                    Root.AddError("Variable " + ListExpression.ToString() + " does not contain a list value");
                }
            }
            else
            {
                Root.AddError("Cannot find variable for " + ListExpression.ToString());
            }

            context.LocalScope.PopContext(index);
        }

        public override string ToString()
        {
            return "REPLACE " + Condition.ToString() + " IN " + ListExpression.ToString() + " BY " + Value.ToString();
        }

        /// <summary>
        /// Provides a real short description of this statement
        /// </summary>
        /// <returns></returns>
        public override string ShortShortDescription()
        {
            return ListExpression.ToString();
        }

        /// <summary>
        /// Provides the main model elemnt affected by this statement
        /// </summary>
        /// <returns></returns>
        public override ModelElement AffectedElement()
        {
            return ListExpression.Ref as ModelElement;
        }
    }
}
