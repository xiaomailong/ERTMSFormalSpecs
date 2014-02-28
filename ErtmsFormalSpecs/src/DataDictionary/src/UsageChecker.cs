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
using System.Linq;
using DataDictionary.Functions;
using System.Collections;
using DataDictionary.Interpreter;

namespace DataDictionary
{
    /// <summary>
    /// Logs messages on the rules according to the validity of the rule
    /// </summary>
    public class UsageChecker : Generated.Visitor
    {
        /// <summary>
        /// The dictionary used for this visit
        /// </summary>
        private Dictionary dictionary;
        public Dictionary Dictionary
        {
            get { return dictionary; }
            private set { dictionary = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary"></param>
        public UsageChecker(Dictionary dictionary)
        {
            Utils.FinderRepository.INSTANCE.ClearCache();
            Dictionary = dictionary;
        }

        public override void visit(Generated.BaseModelElement obj, bool visitSubNodes)
        {
            checkUsage(obj as ModelElement);

            base.visit(obj, visitSubNodes);
        }

        /// <summary>
        /// Ensures that the element is used somewhere
        /// </summary>
        /// <param name="model"></param>
        private void checkUsage(ModelElement model)
        {
            List<Usage> references;

            Types.Type type = model as Types.Type;
            if (type != null && !(type is ICallable))
            {
                bool checkType = true;
                Types.StateMachine stateMachine = type as Types.StateMachine;
                if (stateMachine != null)
                {
                    checkType = stateMachine.EnclosingStateMachine == null;
                }

                if (checkType)
                {
                    references = EFSSystem.INSTANCE.FindReferences(type);
                    if (references.Count == 0)
                    {
                        model.AddWarning("Type is declared but never used");
                    }
                }
            }
            else
            {
                Variables.IVariable variable = model as Variables.IVariable;
                if (variable != null)
                {
                    references = EFSSystem.INSTANCE.FindReferences(variable as ModelElement);
                    bool read = false;
                    bool written = variable.Mode == Generated.acceptor.VariableModeEnumType.aConstant;

                    foreach (Usage usage in references)
                    {
                        switch (usage.Mode)
                        {
                            case DataDictionary.Interpreter.Usage.ModeEnum.Read:
                                read = true;
                                break;

                            case DataDictionary.Interpreter.Usage.ModeEnum.Write:
                                written = true;
                                break;

                            case DataDictionary.Interpreter.Usage.ModeEnum.ReadAndWrite:
                                read = true;
                                written = true;
                                break;

                            default:
                                break;
                        }
                    }

                    if (!read && !written)
                    {
                        model.AddWarning("Variable is never read nor written");
                    }
                    else
                    {
                        if (!read)
                        {
                            model.AddWarning("Variable is never read");
                        }
                        if (!written)
                        {
                            model.AddWarning("Variable is never written");
                        }
                    }
                }
                else
                {
                    ICallable callable = model as ICallable;
                    if (callable != null)
                    {
                        references = EFSSystem.INSTANCE.FindReferences(callable as ModelElement);
                        bool called = false;

                        foreach (Usage usage in references)
                        {
                            switch (usage.Mode)
                            {
                                case DataDictionary.Interpreter.Usage.ModeEnum.Call:
                                    called = true;
                                    break;
                            }
                        }

                        if (!called)
                        {
                            model.AddWarning("Function or procedure is never called");
                        }
                    }
                }

            }
        }
    }
}
