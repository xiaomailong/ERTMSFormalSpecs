using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDictionary.Functions;
using DataDictionary.Generated;
using DataDictionary.Interpreter;
using DataDictionary.Types;
using DataDictionary.Values;
using NUnit.Framework;
using NameSpace = DataDictionary.Types.NameSpace;
using State = DataDictionary.Constants.State;
using StateMachine = DataDictionary.Types.StateMachine;

namespace DataDictionary.test.updateModel
{
    [TestFixture]
    class UpdateStateMachineTest: BaseModelTest
    {
        /// <summary>
        /// Tests that a variable of type State Machine is correctly updated
        /// </summary>
        [Test]
        public void TestUpdateStateMachine()
        {
            Dictionary dictionary = CreateDictionary("Test");
            NameSpace nameSpace = CreateNameSpace(dictionary, "N1");
            
            StateMachine stateMachine = CreateStateMachine(nameSpace, "TestSM");
            State state1 = CreateState(stateMachine, "S1");
            State state2 = CreateState(stateMachine, "S2");
            stateMachine.Default = "S1";

            Variable var = CreateVariable(nameSpace, "TheVariable", "TestSM");
            var.setDefaultValue("TestSM.S1");

            Dictionary dictionary2 = CreateDictionary("TestUpdate");
            dictionary2.setUpdates(dictionary.Guid);

            StateMachine updStateMachine = stateMachine.CreateStateMachineUpdate(dictionary2);

            State updState = updStateMachine.findState("S1");
            State subState = CreateState(updState.StateMachine, "bis");
            updState.StateMachine.setDefault("bis");


            Compiler.Compile_Synchronous(true);

            Expression expression = Parser.Expression(dictionary, "N1.TheVariable");
            IValue value = expression.GetValue(new InterpretationContext(), null);
            Assert.AreEqual(value, subState);
        }
    }
}
