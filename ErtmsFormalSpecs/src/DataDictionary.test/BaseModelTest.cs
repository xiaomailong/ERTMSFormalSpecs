using DataDictionary.Generated;
using DataDictionary.Interpreter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using Action = DataDictionary.Rules.Action;
using Function = DataDictionary.Functions.Function;
using NameSpace = DataDictionary.Types.NameSpace;
using PreCondition = DataDictionary.Rules.PreCondition;
using Rule = DataDictionary.Rules.Rule;
using RuleCondition = DataDictionary.Rules.RuleCondition;
using State = DataDictionary.Constants.State;
using StateMachine = DataDictionary.Types.StateMachine;
using Structure = DataDictionary.Types.Structure;
using StructureElement = DataDictionary.Types.StructureElement;
using StructureRef = DataDictionary.Types.StructureRef;
using Variable = DataDictionary.Variables.Variable;

namespace DataDictionary.test
{
    public class BaseModelTest
    {
        /// <summary>
        ///     Initializes all tests
        /// </summary>
        [TestInitialize]
        public void Initialise()
        {
            acceptor.setFactory(new ObjectFactory());
        }

        /// <summary>
        ///     Clean up the environment after a test has been executed
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            System.Dictionaries.Clear();
        }

        /// <summary>
        ///     The EFS System to test
        /// </summary>
        protected EFSSystem System
        {
            get { return EFSSystem.INSTANCE; }
        }

        /// <summary>
        ///     The Compiler
        /// </summary>
        protected Compiler Compiler
        {
            get { return System.Compiler; }
        }

        /// <summary>
        ///     The factory used to create elements
        /// </summary>
        protected Factory Factory
        {
            get { return acceptor.getFactory(); }
        }

        /// <summary>
        ///     Performs a refactor
        /// </summary>
        /// <param name="namable"></param>
        /// <param name="newName"></param>
        protected void Refactor(INamable namable, string newName)
        {
            Compiler.Compile_Synchronous(true);
            Compiler.Refactor(namable as ModelElement, newName);
        }

        /// <summary>
        ///     Performs a refactor
        /// </summary>
        /// <param name="namable"></param>
        protected void RefactorAndRelocate(INamable namable)
        {
            Compiler.Compile_Synchronous(true);
            Compiler.RefactorAndRelocate(namable as ModelElement);
        }

        /// <summary>
        ///     Creates a dictionary in the system
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Dictionary CreateDictionary(string name)
        {
            Dictionary retVal = (Dictionary) Factory.createDictionary();

            System.AddDictionary(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a namespace in the dictionary/namespace provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected NameSpace CreateNameSpace(ModelElement enclosing, string name)
        {
            NameSpace retVal = (NameSpace) Factory.createNameSpace();

            Dictionary dictionary = enclosing as Dictionary;
            NameSpace nameSpace = enclosing as NameSpace;
            if (dictionary != null)
            {
                dictionary.appendNameSpaces(retVal);
            }
            else if (nameSpace != null)
            {
                nameSpace.appendNameSpaces(retVal);
            }
            else
            {
                Assert.Fail();
            }
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a variable in the namespace provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected Variable CreateVariable(NameSpace enclosing, string name, string typeName)
        {
            Variable retVal = (Variable) Factory.createVariable();

            enclosing.appendVariables(retVal);
            retVal.Name = name;
            retVal.TypeName = typeName;

            return retVal;
        }

        /// <summary>
        ///     Creates a structure in the namespace provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Structure CreateStructure(NameSpace enclosing, string name)
        {
            Structure retVal = (Structure) Factory.createStructure();

            enclosing.appendStructures(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a structure in the namespace provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected StructureRef CreateStructureRef(Structure enclosing, string name)
        {
            StructureRef retVal = (StructureRef) Factory.createStructureRef();

            enclosing.appendInterfaces(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a state machine in the structure provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected StateMachine CreateStateMachine(Structure enclosing, string name)
        {
            StateMachine retVal = (StateMachine) Factory.createStateMachine();

            enclosing.appendStateMachines(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a state in the state machine provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected State CreateState(StateMachine enclosing, string name)
        {
            State retVal = (State) Factory.createState();

            enclosing.appendStates(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a structure element in the structure provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected StructureElement CreateStructureElement(Structure enclosing, string name, string type)
        {
            StructureElement retVal = (StructureElement) Factory.createStructureElement();

            enclosing.appendElements(retVal);
            retVal.Name = name;
            retVal.TypeName = type;

            return retVal;
        }

        /// <summary>
        ///     Creates a rule and a rule condition in the state machine provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected RuleCondition CreateRuleAndCondition(ModelElement enclosing, string name)
        {
            Rule rule = (Rule) Factory.createRule();

            Structure structure = enclosing as Structure;
            NameSpace nameSpace = enclosing as NameSpace;
            if (structure != null)
            {
                structure.appendRules(rule);
            }
            else if (nameSpace != null)
            {
                nameSpace.appendRules(rule);
            }
            else
            {
                Assert.Fail();
            }
            rule.Name = name;

            RuleCondition retVal = (RuleCondition) Factory.createRuleCondition();
            rule.appendConditions(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a function in the enclosing namespace
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected Function CreateFunction(NameSpace enclosing, string name, string type)
        {
            Function retVal = (Function) Factory.createFunction();
            enclosing.appendFunctions(retVal);
            retVal.Name = name;
            retVal.TypeName = type;

            return retVal;
        }

        /// <summary>
        ///     Creates an action in the enclosing rule condition
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Action CreateAction(RuleCondition enclosing, string name)
        {
            Action retVal = (Action) Factory.createAction();
            enclosing.appendActions(retVal);
            retVal.ExpressionText = name;

            return retVal;
        }

        /// <summary>
        ///     Creates a precondition in the enclosing rule condition
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected PreCondition CreatePreCondition(RuleCondition enclosing, string name)
        {
            PreCondition retVal = (PreCondition) Factory.createPreCondition();
            enclosing.appendPreConditions(retVal);
            retVal.ExpressionText = name;

            return retVal;
        }
    }
}