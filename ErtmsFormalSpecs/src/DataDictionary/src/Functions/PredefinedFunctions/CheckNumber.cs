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
using DataDictionary.Generated;
using DataDictionary.Interpreter;
using DataDictionary.Values;
using DataDictionary.Variables;
using Type = DataDictionary.Types.Type;

namespace DataDictionary.Functions.PredefinedFunctions
{
    /// <summary>
    /// Checks the validity of a string
    /// </summary>
    public class CheckNumber : PredefinedFunction
    {
        /// <summary>
        /// The number being checked
        /// </summary>
        public Parameter Number { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="efsSystem"></param>
        public CheckNumber (EFSSystem efsSystem)
            : base(efsSystem, "CheckNumber")
        {
            Number = (Parameter)acceptor.getFactory().createParameter();
            Number.Name = "Number";
            Number.Type = EFSSystem.AnyType;
            Number.setFather(this);
            FormalParameters.Add(Number);
        }

        /// <summary>
        /// The return type of the before function
        /// </summary>
        public override Type ReturnType
        {
            get { return EFSSystem.BoolType; }
        }

        /// <summary>
        /// Provides the value of the function
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actuals">the actual parameters values</param>
        /// <param name="explain"></param>
        /// <returns>The value for the function application</returns>
        public override IValue Evaluate(InterpretationContext context, Dictionary<Actual, IValue> actuals, ExplanationPart explain)
        {
            IValue retVal = EFSSystem.BoolType.False;

            int token = context.LocalScope.PushContext();
            AssignParameters(context, actuals);

            string number = context.findOnStack(Number).Value.ToString();

            if (number != "")
            {
                char[] tmp = number.ToCharArray();

                // Check that the only letters in 'number' are the string of F at the end
                for (int i = 0; i < tmp.Length; i++)
                {
                    bool numberSequence = true;

                    if (System.Char.IsLetter(tmp[i]) &&
                        (numberSequence || tmp[i] == 'F'))
                    {
                        if (!tmp[i].Equals('F'))
                        {
                            break;
                        }
                        else
                        {
                            numberSequence = false;
                        }
                    }

                    if (i == tmp.Length - 1)
                    {
                        retVal = EFSSystem.BoolType.True;
                    }
                }
            }

            context.LocalScope.PopContext(token);

            return retVal;
        }
    }
}
