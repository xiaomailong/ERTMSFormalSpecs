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
using System.Reflection;
using DataDictionary.Generated;
using DataDictionary.Interpreter;
using DataDictionary.Interpreter.Statement;
using DataDictionary.Rules;
using DataDictionary.Tests.Runner.Events;
using DataDictionary.Values;
using DataDictionary.Variables;
using log4net;
using Utils;
using Action = DataDictionary.Rules.Action;
using Collection = DataDictionary.Types.Collection;
using NameSpace = DataDictionary.Types.NameSpace;
using Rule = DataDictionary.Generated.Rule;
using RuleCondition = DataDictionary.Rules.RuleCondition;
using State = DataDictionary.Constants.State;
using StateMachine = DataDictionary.Types.StateMachine;
using Structure = DataDictionary.Types.Structure;
using Variable = DataDictionary.Generated.Variable;
using Visitor = DataDictionary.Generated.Visitor;

namespace DataDictionary.Tests.Runner
{
    public class Runner
    {
        /// <summary>
        ///     The Logger
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The event time line for this runner
        /// </summary>
        public EventTimeLine EventTimeLine { get; private set; }

        /// <summary>
        ///     Indicates whether events should be logged using log4net
        /// </summary>
        public bool LogEvents { get; set; }

        /// <summary>
        ///     Indicates whether an explanation should be provided to all actions
        /// </summary>
        public bool Explain { get; set; }

        /// <summary>
        ///     The data dictionary
        /// </summary>
        public virtual EFSSystem EFSSystem
        {
            get { return EFSSystem.INSTANCE; }
        }

        /// <summary>
        ///     The test case for which this runner has been created
        /// </summary>
        public SubSequence SubSequence { get; private set; }

        /// <summary>
        ///     The step between two activations
        /// </summary>
        private double step = 0.1;

        public double Step
        {
            get { return step; }
            set { step = value; }
        }

        /// <summary>
        ///     The current time
        /// </summary>
        public double Time
        {
            get { return EventTimeLine.CurrentTime; }
            set { EventTimeLine.CurrentTime = value; }
        }

        /// <summary>
        ///     The last time when activation has been performed
        /// </summary>
        public double LastActivationTime { get; set; }

        /// <summary>
        ///     Event when the execution is terminated
        /// </summary>
        /// <param name="priority"></param>
        public delegate void CycleExecutionTerminatedDelegate(Runner runner, acceptor.RulePriority priority);

        public static event CycleExecutionTerminatedDelegate CycleExecutionTerminated;

        /// <summary>
        ///     Launches the event CycleExecutionTerminated
        /// </summary>
        /// <param name="priority"></param>
        public virtual void OnCycleExecutionTerminated(acceptor.RulePriority priority)
        {
            if (CycleExecutionTerminated != null)
            {
                CycleExecutionTerminated(this, priority);
            }
        }

        /// <summary>
        ///     Indicates that clients should wait
        /// </summary>
        public bool PleaseWait { get; set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="subSequence"></param>
        /// <param name="explain"></param>
        /// <param name="logEvents">Indicates whether events should be logged</param>
        /// <param name="ensureCompilation">Indicates that the runner should make sure that the system is compiled</param>
        public Runner(SubSequence subSequence, bool explain, bool logEvents, bool ensureCompilation)
        {
            EventTimeLine = new EventTimeLine();
            SubSequence = subSequence;
            EFSSystem.Runner = this;
            LogEvents = logEvents;
            Explain = explain;

            if (ensureCompilation)
            {
                // Compile everything
                EFSSystem.Compiler.Compile_Synchronous(EFSSystem.ShouldRebuild);
                EFSSystem.ShouldRebuild = false;
            }

            Setup();
            PleaseWait = true;
        }

        /// <summary>
        ///     A simple runner
        /// </summary>
        public Runner(bool explain, bool logEvents, int step = 100, int storeEventCount = 0)
        {
            EventTimeLine = new EventTimeLine();
            SubSequence = null;
            Step = 100;
            EventTimeLine.MaxNumberOfEvents = storeEventCount;
            EFSSystem.Runner = this;
            LogEvents = logEvents;
            Explain = explain;

            // Compile everything
            EFSSystem.Compiler.Compile_Synchronous(EFSSystem.ShouldRebuild);
            EFSSystem.ShouldRebuild = false;

            Setup();
        }

        /// <summary>
        ///     Sets up all variables before any execution on the system
        /// </summary>
        private class Setuper : Visitor
        {
            /// <summary>
            ///     The EFS system for which this setuper is created
            /// </summary>
            public EFSSystem EFSSystem { get; private set; }

            /// <summary>
            ///     Constructor
            /// </summary>
            /// <param name="efsSystem"></param>
            public Setuper(EFSSystem efsSystem)
            {
                EFSSystem = efsSystem;
            }

            /// <summary>
            ///     Sets the default values to each variable
            /// </summary>
            /// <param name="variable">The variable to set</param>
            /// <param name="subNodes">Indicates whether sub nodes should be considered</param>
            public override void visit(Variable variable, bool subNodes)
            {
                Variables.Variable var = (Variables.Variable) variable;

                var.Value = var.DefaultValue;

                base.visit(variable, subNodes);
            }

            /// <summary>
            ///     Indicates which rules are not active
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="visitSubNodes"></param>
            public override void visit(Rule obj, bool visitSubNodes)
            {
                Rules.Rule rule = obj as Rules.Rule;
                if (rule != null)
                {
                    rule.ActivationPriorities = null;
                }

                base.visit(obj, visitSubNodes);
            }

            /// <summary>
            ///     Clear the cache of all functions
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="visitSubNodes"></param>
            public override void visit(Function obj, bool visitSubNodes)
            {
                Functions.Function function = obj as Functions.Function;

                if (function != null)
                {
                    function.ClearCache();
                }

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        ///     Initializes the execution time for functions and rules
        /// </summary>
        private class ExecutionTimeInitializer : Visitor
        {
            public override void visit(Function obj, bool visitSubNodes)
            {
                Functions.Function function = obj as Functions.Function;

                function.ExecutionCount = 0;
                function.ExecutionTimeInMilli = 0L;

                base.visit(obj, visitSubNodes);
            }

            public override void visit(Rule obj, bool visitSubNodes)
            {
                Rules.Rule rule = obj as Rules.Rule;

                rule.ExecutionCount = 0;
                rule.ExecutionTimeInMilli = 0L;

                base.visit(obj, visitSubNodes);
            }
        }

        /// <summary>
        ///     Sets up the runner before performing a test case
        /// </summary>
        public void Setup()
        {
            try
            {
                ControllersManager.DesactivateAllNotifications();
                // Setup the execution environment
                Setuper setuper = new Setuper(EFSSystem);
                ExecutionTimeInitializer executionTimeInitializer = new ExecutionTimeInitializer();
                foreach (Dictionary dictionary in EFSSystem.Dictionaries)
                {
                    setuper.visit(dictionary);
                    executionTimeInitializer.visit(dictionary);
                }

                // Clears all caches
                FinderRepository.INSTANCE.ClearCache();

                // Setup the step
                if (SubSequence != null)
                {
                    Expression expression = SubSequence.Frame.CycleDuration;
                    IValue value = expression.GetValue(new InterpretationContext(SubSequence.Frame), null);
                    Step = Functions.Function.getDoubleValue(value);
                }

                PleaseWait = false;
            }
            finally
            {
                ControllersManager.ActivateAllNotifications();
            }
        }

        public class Activation
        {
            /// <summary>
            ///     The action to activate
            /// </summary>
            public RuleCondition RuleCondition { get; private set; }

            /// <summary>
            ///     The instance on which the action is applied
            /// </summary>
            public IModelElement Instance { get; private set; }

            /// <summary>
            ///     The explanation why this activation has been performed
            /// </summary>
            public ExplanationPart Explanation { get; private set; }

            /// <summary>
            ///     Constructor
            /// </summary>
            /// <param name="ruleCondition">The rule condition which leads to this activation</param>
            /// <param name="instance">The instance on which this rule condition's preconditions are evaluated to true</param>
            public Activation(RuleCondition ruleCondition, IModelElement instance, ExplanationPart explanation)
            {
                RuleCondition = ruleCondition;
                Instance = instance;
                Explanation = explanation;
            }

            /// <summary>
            ///     Indicates that two Activations are the same when they share the action and,
            ///     if specified, the instance on which they are applied
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                bool retVal = false;

                Activation other = obj as Activation;
                if (other != null)
                {
                    retVal = RuleCondition.Equals(other.RuleCondition);
                    if (retVal && Instance != null)
                    {
                        if (other.Instance != null)
                        {
                            retVal = retVal && Instance.Equals(other.Instance);
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
                return retVal;
            }

            /// <summary>
            ///     The hash code, according to Equal operator.
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                int retVal = RuleCondition.GetHashCode();

                if (Instance != null)
                {
                    retVal = retVal + Instance.GetHashCode();
                }

                return retVal;
            }
        }

        /// <summary>
        ///     Provides the order in which rules should be activated
        /// </summary>
        public static acceptor.RulePriority[] PRIORITIES_ORDER =
        {
            acceptor.RulePriority.aVerification,
            acceptor.RulePriority.aUpdateINTERNAL,
            acceptor.RulePriority.aProcessing,
            acceptor.RulePriority.aUpdateOUT,
            acceptor.RulePriority.aCleanUp,
        };

        /// <summary>
        ///     The current priority
        /// </summary>
        public acceptor.RulePriority? CurrentPriority { get; private set; }

        /// <summary>
        ///     Activates the rules in the dictionary until stabilisation
        /// </summary>
        public void Cycle()
        {
            try
            {
                CurrentPriority = null;

                if (LogEvents)
                {
                    Log.Info("New cycle");
                }
                ControllersManager.DesactivateAllNotifications();

                LastActivationTime = Time;

                Utils.ModelElement.Errors = new Dictionary<Utils.ModelElement, List<ElementLog>>();

                foreach (acceptor.RulePriority priority in PRIORITIES_ORDER)
                {
                    innerExecuteOnePriority(priority);
                }
                CurrentPriority = null;

                RegisterErrors(Utils.ModelElement.Errors);

                EventTimeLine.GarbageCollect();
            }
            finally
            {
                ControllersManager.ActivateAllNotifications();
            }

            EventTimeLine.CurrentTime += Step;
        }

        /// <summary>
        ///     Executes a single rule priority (shared version of the method)
        /// </summary>
        /// <param name="priority"></param>
        private void innerExecuteOnePriority(acceptor.RulePriority priority)
        {
            CurrentPriority = priority;
            if (LogEvents)
            {
                Log.Info("Priority=" + priority);
            }

            // Activates the processing engine
            HashSet<Activation> activations = new HashSet<Activation>();
            foreach (Dictionary dictionary in EFSSystem.Dictionaries)
            {
                foreach (NameSpace nameSpace in dictionary.NameSpaces)
                {
                    SetupNameSpaceActivations(priority, activations, nameSpace, this);
                }
            }

            List<VariableUpdate> updates = new List<VariableUpdate>();
            EvaluateActivations(activations, priority, ref updates);
            ApplyUpdates(updates);
            CheckExpectationsState(priority);

            // Indicates that the execution of this cycle priority is complete
            OnCycleExecutionTerminated(priority);
        }

        /// <summary>
        ///     Executes the interpretation machine for one priority
        /// </summary>
        /// <param name="priority"></param>
        public void ExecuteOnePriority(acceptor.RulePriority priority)
        {
            try
            {
                ControllersManager.NamableController.DesactivateNotification();
                LastActivationTime = Time;

                Utils.ModelElement.Errors = new Dictionary<Utils.ModelElement, List<ElementLog>>();

                // Executes a single rule priority
                innerExecuteOnePriority(priority);

                EventTimeLine.GarbageCollect();
            }
            finally
            {
                ControllersManager.NamableController.ActivateNotification();
            }

            if (priority == acceptor.RulePriority.aCleanUp)
            {
                EventTimeLine.CurrentTime += Step;
            }
        }

        /// <summary>
        ///     Determines the set of rules in a specific namespace to be applied.
        /// </summary>
        /// <param name="priority">The priority for which this activation is requested</param>
        /// <param name="activations">The set of activations to be filled</param>
        /// <param name="nameSpace">The namespace to consider</param>
        /// <param name="explanation">The explanation part to be filled</param>
        /// <param name="runner"></param>
        /// <returns></returns>
        protected void SetupNameSpaceActivations(acceptor.RulePriority priority, HashSet<Activation> activations,
            NameSpace nameSpace, Runner runner)
        {
            // Finds all activations in sub namespaces
            foreach (NameSpace subNameSpace in nameSpace.NameSpaces)
            {
                SetupNameSpaceActivations(priority, activations, subNameSpace, runner);
            }

            foreach (Rules.Rule rule in nameSpace.Rules)
            {
                // We only apply rules that have not been updated
                if (rule.UpdatedBy.Count == 0)
                {
                    ExplanationPart explanation = new ExplanationPart(rule, "Rule evaluation");
                    rule.Evaluate(this, priority, rule, activations, explanation);
                }
                
            }

            foreach (IVariable variable in nameSpace.Variables)
            {
                EvaluateVariable(priority, activations, variable,
                    new ExplanationPart(variable as ModelElement, "Evaluating variable"), runner);
            }
        }

        /// <summary>
        ///     Evaluates the rules associated to a single variable
        /// </summary>
        /// <param name="priority">The priority in which this variable is evaluated</param>
        /// <param name="activations">The activation list result of this evaluation</param>
        /// <param name="variable">The variable to evaluate</param>
        /// <param name="explanation">The explanation part to be filled</param>
        /// <param name="runner"></param>
        private void EvaluateVariable(acceptor.RulePriority priority, HashSet<Activation> activations,
            IVariable variable, ExplanationPart explanation, Runner runner)
        {
            if (variable != null && variable.Value != EFSSystem.EmptyValue)
            {
                if (variable.Type is Structure)
                {
                    Structure structure = variable.Type as Structure;
                    foreach (Rules.Rule rule in structure.Rules)
                    {
                        rule.Evaluate(this, priority, variable, activations, explanation);
                    }

                    StructureValue value = variable.Value as StructureValue;
                    if (value != null)
                    {
                        foreach (IVariable subVariable in value.SubVariables.Values)
                        {
                            EvaluateVariable(priority, activations, subVariable, explanation, runner);
                        }
                    }
                }
                else if (variable.Type is StateMachine)
                {
                    EvaluateStateMachine(activations, priority, variable, explanation, runner);
                }
                else if (variable.Type is Collection)
                {
                    Collection collectionType = variable.Type as Collection;
                    if (variable.Value != EFSSystem.EmptyValue)
                    {
                        ListValue val = variable.Value as ListValue;

                        if (val != null)
                        {
                            Variables.Variable tmp = new Variables.Variable();
                            tmp.Name = "list_entry";
                            tmp.Type = collectionType.Type;

                            foreach (IValue subVal in val.Val)
                            {
                                tmp.Value = subVal;
                                EvaluateVariable(priority, activations, tmp, explanation, runner);
                            }
                        }
                        else
                        {
                            ModelElement element = variable as ModelElement;
                            if (element != null)
                            {
                                element.AddError("Variable " + variable.Name + " does not hold a collection but " +
                                                 variable.Value);
                            }
                            else
                            {
                                throw new Exception("Variable " + variable.Name + " does not hold a collection but " +
                                                    variable.Value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Try to find a rule, in this state machine, or in a sub state machine
        ///     which
        /// </summary>
        /// <param name="ruleConditions">The rule conditions activated during the evaluation of this state machine</param>
        /// <param name="priority">The priority when this evaluation occurs</param>
        /// <param name="currentStateVariable">The variable which holds the current state of the procedure</param>
        /// <param name="explanation">The explanation part to be filled</param>
        /// <param name="runner"></param>
        private void EvaluateStateMachine(HashSet<Activation> activations, acceptor.RulePriority priority,
            IVariable currentStateVariable, ExplanationPart explanation, Runner runner)
        {
            if (currentStateVariable != null)
            {
                State currentState = currentStateVariable.Value as State;
                StateMachine currentStateMachine = currentState.StateMachine;
                while (currentStateMachine != null)
                {
                    foreach (Rules.Rule rule in currentStateMachine.Rules)
                    {
                        rule.Evaluate(this, priority, currentStateVariable, activations, explanation);
                    }
                    currentStateMachine = currentStateMachine.EnclosingStateMachine;
                }
            }
        }

        /// <summary>
        ///     Indicates that the changes performed should be checked for compatibility
        /// </summary>
        private bool CheckForCompatibleChanges = false;

        /// <summary>
        ///     Applies the selected actions and update the system state
        /// </summary>
        /// <param name="updates"></param>
        public void EvaluateActivations(HashSet<Activation> activations, acceptor.RulePriority priority,
            ref List<VariableUpdate> updates)
        {
            Dictionary<IVariable, Change> changes = new Dictionary<IVariable, Change>();
            Dictionary<Change, VariableUpdate> traceBack = new Dictionary<Change, VariableUpdate>();

            foreach (Activation activation in activations)
            {
                if (LogEvents)
                {
                    Log.Info("Activating " + activation.RuleCondition.FullName);
                }
                if (activation.RuleCondition.Actions.Count > 0)
                {
                    // Register the fact that a rule has been triggered
                    RuleFired ruleFired = new RuleFired(activation, priority);
                    EventTimeLine.AddModelEvent(ruleFired, this, true);
                    ExplanationPart changesExplanation = ExplanationPart.CreateSubExplanation(activation.Explanation,
                        "Changes");

                    // Registers all model updates due to this rule triggering
                    foreach (Action action in activation.RuleCondition.Actions)
                    {
                        if (action.Statement != null)
                        {
                            VariableUpdate variableUpdate = new VariableUpdate(action, activation.Instance, priority);
                            variableUpdate.ComputeChanges(false, this);
                            EventTimeLine.AddModelEvent(variableUpdate, this, false);
                            ruleFired.AddVariableUpdate(variableUpdate);
                            if (changesExplanation != null)
                            {
                                changesExplanation.SubExplanations.Add(variableUpdate.Explanation);
                            }
                            updates.Add(variableUpdate);

                            if (CheckForCompatibleChanges)
                            {
                                ChangeList actionChanges = variableUpdate.Changes;
                                if (variableUpdate.Action.Statement is ProcedureCallStatement)
                                {
                                    Dictionary<IVariable, Change> procedureChanges = new Dictionary<IVariable, Change>();

                                    foreach (Change change in variableUpdate.Changes.Changes)
                                    {
                                        procedureChanges[change.Variable] = change;
                                    }

                                    actionChanges = new ChangeList();
                                    foreach (Change change in procedureChanges.Values)
                                    {
                                        actionChanges.Add(change, false, this);
                                    }
                                }

                                foreach (Change change in actionChanges.Changes)
                                {
                                    IVariable variable = change.Variable;
                                    if (changes.ContainsKey(change.Variable))
                                    {
                                        Change otherChange = changes[change.Variable];
                                        Action otherAction = traceBack[otherChange].Action;
                                        if (!variable.Type.CompareForEquality(otherChange.NewValue, change.NewValue))
                                        {
                                            string action1 = ((INamable) action.Enclosing).FullName + " : " +
                                                             variableUpdate.Action.FullName;
                                            string action2 = ((INamable) otherAction.Enclosing).FullName + " : " +
                                                             traceBack[otherChange].Action.FullName;
                                            variableUpdate.Action.AddError(
                                                "Simultaneous change of the same variable with different values. Conflit between " +
                                                action1 + " and " + action2);
                                        }
                                    }
                                    else
                                    {
                                        changes.Add(change.Variable, change);
                                        traceBack.Add(change, variableUpdate);
                                    }
                                }
                            }
                        }
                        else
                        {
                            action.AddError("Cannot parse action statement");
                        }
                    }
                }
            }

            // Handles the leave & enter state rules
            List<VariableUpdate> updatesToProcess = updates;
            updates = new List<VariableUpdate>();

            while (updatesToProcess.Count > 0)
            {
                List<VariableUpdate> newUpdates = new List<VariableUpdate>();

                foreach (VariableUpdate update in updatesToProcess)
                {
                    updates.Add(update);

                    foreach (Change change in update.Changes.Changes)
                    {
                        if (change.Variable.Type is StateMachine)
                        {
                            HandleLeaveState(priority, newUpdates, change.Variable, (State) change.Variable.Value,
                                (State) change.NewValue);
                            HandleEnterState(priority, newUpdates, change.Variable, (State) change.Variable.Value,
                                (State) change.NewValue);
                        }
                    }
                }

                updatesToProcess = newUpdates;
            }
        }

        /// <summary>
        ///     Add actions when entering a state
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="updates"></param>
        /// <param name="leaveState"></param>
        /// <param name="enterState"></param>
        private void HandleEnterState(acceptor.RulePriority priority, List<VariableUpdate> updates, IVariable variable,
            State leaveState, State enterState)
        {
            if (!enterState.getStateMachine().Contains(enterState, leaveState))
            {
                if (enterState.getEnterAction() != null)
                {
                    Rules.Rule rule = (Rules.Rule) enterState.getEnterAction();
                    ExplanationPart explanation = new ExplanationPart(rule, "Rule evaluation");
                    HashSet<Activation> newActivations = new HashSet<Activation>();
                    List<VariableUpdate> newUpdates = new List<VariableUpdate>();
                    rule.Evaluate(this, priority, variable, newActivations, explanation);
                    EvaluateActivations(newActivations, priority, ref newUpdates);
                    updates.AddRange(newUpdates);
                }

                if (enterState.EnclosingState != null)
                {
                    HandleEnterState(priority, updates, variable, leaveState, enterState.EnclosingState);
                }
            }
        }

        /// <summary>
        ///     Add actions when leaving a state
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="updates"></param>
        /// <param name="leaveState"></param>
        /// <param name="enterState"></param>
        private void HandleLeaveState(acceptor.RulePriority priority, List<VariableUpdate> updates, IVariable variable,
            State leaveState, State enterState)
        {
            if (!leaveState.getStateMachine().Contains(leaveState, enterState))
            {
                if (leaveState.getLeaveAction() != null)
                {
                    Rules.Rule rule = (Rules.Rule) leaveState.getLeaveAction();
                    ExplanationPart explanation = new ExplanationPart(rule, "Rule evaluation");
                    HashSet<Activation> newActivations = new HashSet<Activation>();
                    List<VariableUpdate> newUpdates = new List<VariableUpdate>();
                    rule.Evaluate(this, priority, variable, newActivations, explanation);
                    EvaluateActivations(newActivations, priority, ref newUpdates);
                    updates.AddRange(newUpdates);
                }

                if (leaveState.EnclosingState != null)
                {
                    HandleLeaveState(priority, updates, variable, leaveState.EnclosingState, enterState);
                }
            }
        }

        /// <summary>
        ///     Applies the updates on the system
        /// </summary>
        /// <param name="updates"></param>
        private void ApplyUpdates(List<VariableUpdate> updates)
        {
            foreach (VariableUpdate update in updates)
            {
                update.Apply(this);
            }
        }

        /// <summary>
        ///     Setups the sub-step by applying its actions and adding its expects in the expect list
        /// </summary>
        public void SetupSubStep(SubStep subStep)
        {
            try
            {
                ControllersManager.DesactivateAllNotifications();
                LogInstance = subStep;

                // No setup can occur when some expectations are still active
                if (!EventTimeLine.ContainsSubStep(subStep))
                {
                    EventTimeLine.AddModelEvent(new SubStepActivated(subStep, CurrentPriority), this, true);
                }
            }
            finally
            {
                ControllersManager.ActivateAllNotifications();
            }
        }

        /// <summary>
        ///     Provides the still active expectations
        /// </summary>
        /// <returns></returns>
        public HashSet<Expect> ActiveExpectations()
        {
            return new HashSet<Expect>(EventTimeLine.ActiveExpectations);
        }

        /// <summary>
        ///     Provides the still active and blocking expectations
        /// </summary>
        /// <returns></returns>
        public HashSet<Expect> ActiveBlockingExpectations()
        {
            return EventTimeLine.ActiveBlockingExpectations();
        }

        /// <summary>
        ///     Provides the failed expectations
        /// </summary>
        /// <returns></returns>
        public HashSet<ModelEvent> FailedExpectations()
        {
            return EventTimeLine.FailedExpectations();
        }

        /// <summary>
        ///     Updates the expectation state according to the variables' value
        /// </summary>
        /// <param name="priority">The priority for which this check is performed</param>
        private void CheckExpectationsState(acceptor.RulePriority priority)
        {
            // Update the state of the expectation according to system's state
            foreach (Expect expect in ActiveExpectations())
            {
                Expectation expectation = expect.Expectation;

                // Determine if the deadline is reached
                if (expect.TimeOut < EventTimeLine.CurrentTime)
                {
                    switch (expect.Expectation.getKind())
                    {
                        case acceptor.ExpectationKind.aInstantaneous:
                        case acceptor.ExpectationKind.defaultExpectationKind:
                            // Instantaneous expectation who raised its deadling
                            EventTimeLine.AddModelEvent(new FailedExpectation(expect, CurrentPriority, null), this, true);
                            break;

                        case acceptor.ExpectationKind.aContinuous:
                            // Continuous expectation who raised its deadline
                            EventTimeLine.AddModelEvent(new ExpectationReached(expect, CurrentPriority, null), this,
                                true);
                            break;
                    }
                }
                else
                {
                    ExplanationPart explanation = new ExplanationPart(expectation,
                        "Expectation " + expectation.Expression);
                    try
                    {
                        if (expectation.getCyclePhase() == acceptor.RulePriority.defaultRulePriority ||
                            expectation.getCyclePhase() == priority)
                        {
                            switch (expectation.getKind())
                            {
                                case acceptor.ExpectationKind.aInstantaneous:
                                case acceptor.ExpectationKind.defaultExpectationKind:
                                    if (getBoolValue(expectation, expectation.Expression, explanation))
                                    {
                                        // An instantaneous expectation who reached its satisfactory condition
                                        EventTimeLine.AddModelEvent(
                                            new ExpectationReached(expect, priority, explanation), this, true);
                                    }
                                    else
                                    {
                                        expectation.Explain = explanation;
                                    }
                                    break;

                                case acceptor.ExpectationKind.aContinuous:
                                    if (expectation.getCondition() != null)
                                    {
                                        if (!getBoolValue(expectation, expectation.ConditionTree, explanation))
                                        {
                                            // An continuous expectation who reached its satisfactory condition
                                            EventTimeLine.AddModelEvent(
                                                new ExpectationReached(expect, priority, explanation), this, true);
                                        }
                                        else
                                        {
                                            if (!getBoolValue(expectation, expectation.Expression, explanation))
                                            {
                                                // A continuous expectation who reached a case where it is not satisfied
                                                EventTimeLine.AddModelEvent(
                                                    new FailedExpectation(expect, priority, explanation), this, true);
                                            }
                                            else
                                            {
                                                expectation.Explain = explanation;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!getBoolValue(expectation, expectation.Expression, explanation))
                                        {
                                            // A continuous expectation who reached a case where it is not satisfied
                                            EventTimeLine.AddModelEvent(
                                                new FailedExpectation(expect, priority, explanation), this, true);
                                        }
                                        else
                                        {
                                            expectation.Explain = explanation;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        expectation.AddErrorAndExplain(e.Message, explanation);
                    }
                }
            }
        }

        /// <summary>
        ///     Provides the value of the expression provided
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="expression"></param>
        /// <param name="explain"></param>
        /// <returns></returns>
        private bool getBoolValue(ModelElement instance, Expression expression, ExplanationPart explain)
        {
            bool retVal = false;

            InterpretationContext context = new InterpretationContext(instance);
            BoolValue val = expression.GetValue(context, explain) as BoolValue;

            if (val != null)
            {
                retVal = val.Val;
            }
            else
            {
                throw new Exception("Cannot evaluate value of " + expression);
            }

            return retVal;
        }

        /// <summary>
        ///     Runs until all expectations are reached or failed
        /// </summary>
        public void RunForBlockingExpectations(bool performCycle)
        {
            if (performCycle)
            {
                Cycle();
            }

            while (ActiveBlockingExpectations().Count > 0)
            {
                Cycle();
            }
        }

        /// <summary>
        ///     Runs until all expectations are reached or failed
        /// </summary>
        public void RunForExpectations(bool performCycle)
        {
            if (performCycle)
            {
                Cycle();
            }

            while (ActiveBlockingExpectations().Count > 0)
            {
                Cycle();
            }
        }

        /// <summary>
        ///     Indicates that no test has been run yet
        /// </summary>
        private static int TEST_NOT_RUN = -1;

        /// <summary>
        ///     Indicates that the current test case & current step & current sub-step must be rebuilt from the time line
        /// </summary>
        private static int REBUILD_CURRENT_SUB_STEP = -2;

        /// <summary>
        ///     Indicates that the current test case & current step & current sub-step must be rebuilt from the time line
        /// </summary>
        private static int NO_MORE_STEP = -3;

        /// <summary>
        ///     The index of the last activated sub-step in the current test case
        /// </summary>
        private int currentSubStepIndex = TEST_NOT_RUN;

        /// <summary>
        ///     The index of the last activated step in the current test case
        /// </summary>
        private int currentStepIndex = TEST_NOT_RUN;

        /// <summary>
        ///     The index of the test case in which the last activated step belongs
        /// </summary>
        private int currentTestCaseIndex = TEST_NOT_RUN;

        /// <summary>
        ///     Provides the next test case
        /// </summary>
        /// <returns></returns>
        public TestCase CurrentTestCase()
        {
            TestCase retVal = null;

            if (SubSequence != null && currentTestCaseIndex != NO_MORE_STEP)
            {
                if (currentTestCaseIndex >= 0 && currentTestCaseIndex < SubSequence.TestCases.Count)
                {
                    retVal = (TestCase) SubSequence.TestCases[currentTestCaseIndex];
                }
            }

            return retVal;
        }

        /// <summary>
        ///     steps to the next test case
        /// </summary>
        private void NextTestCase()
        {
            if (currentTestCaseIndex != NO_MORE_STEP)
            {
                if (currentTestCaseIndex == REBUILD_CURRENT_SUB_STEP)
                {
                    currentStepIndex = REBUILD_CURRENT_SUB_STEP;
                    NextStep();
                }
                else
                {
                    currentTestCaseIndex += 1;
                    TestCase testCase = CurrentTestCase();
                    while (testCase != null && testCase.Steps.Count == 0 &&
                           currentTestCaseIndex < SubSequence.TestCases.Count)
                    {
                        currentTestCaseIndex += 1;
                        testCase = CurrentTestCase();
                    }

                    if (testCase == null)
                    {
                        currentTestCaseIndex = NO_MORE_STEP;
                        currentStepIndex = NO_MORE_STEP;
                    }
                }
            }
        }

        /// <summary>
        ///     Provides the current test step
        /// </summary>
        /// <returns></returns>
        public Step CurrentStep()
        {
            Step retVal = null;

            if (currentStepIndex != NO_MORE_STEP)
            {
                TestCase testCase = CurrentTestCase();
                if (testCase != null)
                {
                    if (currentStepIndex >= 0 && currentStepIndex < testCase.Steps.Count)
                    {
                        retVal = (Step) testCase.Steps[currentStepIndex];
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        ///     Steps to the next step (either in the current test case, or in the next test case)
        /// </summary>
        private void NextStep()
        {
            if (currentStepIndex != NO_MORE_STEP)
            {
                Step step = CurrentStep();

                do
                {
                    if (currentStepIndex != REBUILD_CURRENT_SUB_STEP)
                    {
                        currentStepIndex += 1;
                        TestCase testCase = CurrentTestCase();
                        if (testCase == null)
                        {
                            NextTestCase();
                            testCase = CurrentTestCase();
                        }

                        if (testCase != null && currentStepIndex >= testCase.Steps.Count)
                        {
                            NextTestCase();
                            testCase = CurrentTestCase();
                            if (testCase != null)
                            {
                                currentStepIndex = 0;
                            }
                            else
                            {
                                currentTestCaseIndex = NO_MORE_STEP;
                                currentStepIndex = NO_MORE_STEP;
                            }
                        }
                        step = CurrentStep();
                    }
                } while (step != null && step.IsEmpty());
            }
        }

        /// <summary>
        ///     Provides the current test step
        /// </summary>
        /// <returns></returns>
        public SubStep CurrentSubStep()
        {
            SubStep retVal = null;

            if (currentSubStepIndex != NO_MORE_STEP)
            {
                if (currentSubStepIndex == REBUILD_CURRENT_SUB_STEP)
                {
                    currentTestCaseIndex = -1;
                    currentStepIndex = -1;
                    currentSubStepIndex = -1;
                    int previousTestCaseIndex = currentTestCaseIndex;
                    int previousStepIndex = currentStepIndex;
                    int previousSubStepIndex = currentSubStepIndex;

                    NextSubStep();
                    retVal = CurrentSubStep();
                    while (retVal != null && EventTimeLine.SubStepActivationCache.ContainsKey(retVal))
                    {
                        previousTestCaseIndex = currentTestCaseIndex;
                        previousStepIndex = currentStepIndex;
                        previousSubStepIndex = currentSubStepIndex;

                        NextSubStep();
                        retVal = CurrentSubStep();
                    }

                    currentTestCaseIndex = previousTestCaseIndex;
                    currentStepIndex = previousStepIndex;
                    currentSubStepIndex = previousSubStepIndex;
                }

                Step step = CurrentStep();
                if (step != null)
                {
                    if (currentSubStepIndex >= 0 && currentSubStepIndex < step.SubSteps.Count)
                    {
                        retVal = (SubStep) step.SubSteps[currentSubStepIndex];
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        ///     Steps to the next sub-step (either in the current test case, or in the next test case)
        /// </summary>
        private void NextSubStep()
        {
            if (currentSubStepIndex != NO_MORE_STEP)
            {
                SubStep subStep = CurrentSubStep();
                Step step;

                do
                {
                    currentSubStepIndex++;
                    step = CurrentStep();
                    if (step == null)
                    {
                        NextStep();
                        step = CurrentStep();
                    }

                    if (step != null && currentSubStepIndex >= step.SubSteps.Count)
                    {
                        NextStep();
                        step = CurrentStep();
                        if (step != null)
                        {
                            currentSubStepIndex = 0;
                        }
                        else
                        {
                            currentTestCaseIndex = NO_MORE_STEP;
                            currentStepIndex = NO_MORE_STEP;
                        }
                    }
                    subStep = CurrentSubStep();
                } while (step != null && (step.IsEmpty() || subStep.IsEmpty()));
            }
        }

        /// <summary>
        ///     Runs the test case until the step provided is encountered
        ///     This does not execute the corresponding step.
        /// </summary>
        /// <param name="target"></param>
        public void RunUntilStep(Step target)
        {
            try
            {
                ControllersManager.DesactivateAllNotifications();

                currentStepIndex = NO_MORE_STEP;
                currentTestCaseIndex = NO_MORE_STEP;

                if (target != null)
                {
                    RunForBlockingExpectations(false);
                }
                else
                {
                    RunForExpectations(false);
                }

                // Run all following steps until the target step is encountered
                foreach (TestCase testCase in SubSequence.TestCases)
                {
                    foreach (Step step in testCase.Steps)
                    {
                        if (step == target)
                        {
                            currentStepIndex = REBUILD_CURRENT_SUB_STEP;
                            currentTestCaseIndex = REBUILD_CURRENT_SUB_STEP;
                            break;
                        }

                        if (!EventTimeLine.ContainsStep(step))
                        {
                            foreach (SubStep subStep in step.SubSteps)
                            {
                                SetupSubStep(subStep);
                                if (!subStep.getSkipEngine())
                                {
                                    if (target != null)
                                    {
                                        RunForBlockingExpectations(true);
                                    }
                                    else
                                    {
                                        RunForExpectations(true);
                                    }
                                }
                                else
                                {
                                    foreach (acceptor.RulePriority priority in PRIORITIES_ORDER)
                                    {
                                        CheckExpectationsState(priority);
                                    }
                                }
                            }

                            while (EventTimeLine.ActiveBlockingExpectations().Count > 0)
                            {
                                Cycle();
                            }
                        }
                    }

                    if (currentTestCaseIndex == REBUILD_CURRENT_SUB_STEP)
                    {
                        break;
                    }
                }
            }
            finally
            {
                ControllersManager.ActivateAllNotifications();
            }
        }

        /// <summary>
        ///     Runs the test case until the step provided is encountered
        ///     This does not execute the corresponding step.
        /// </summary>
        /// <param name="Item"></param>
        public void RunUntilTime(double targetTime)
        {
            while (EventTimeLine.CurrentTime < targetTime)
            {
                SubStep subStep = null;
                if (ActiveBlockingExpectations().Count == 0)
                {
                    NextSubStep();
                    subStep = CurrentSubStep();
                    if (subStep != null)
                    {
                        SetupSubStep(subStep);
                    }
                }

                if (subStep == null || !subStep.getSkipEngine())
                {
                    Cycle();
                }
                else
                {
                    if (subStep.getSkipEngine())
                    {
                        CheckExpectationsState(acceptor.RulePriority.aCleanUp);
                    }
                    EventTimeLine.CurrentTime += Step;
                }
            }
        }

        /// <summary>
        ///     Steps one step backward in this run
        /// </summary>
        public void StepBack()
        {
            EventTimeLine.StepBack(step);
            currentSubStepIndex = REBUILD_CURRENT_SUB_STEP;
            currentStepIndex = REBUILD_CURRENT_SUB_STEP;
            currentTestCaseIndex = REBUILD_CURRENT_SUB_STEP;
        }

        /// <summary>
        ///     Indicates whether a rule condition has been activated at a given time
        /// </summary>
        /// <param name="ruleCondition">The rule condition that should be activated</param>
        /// <param name="time">the time when the rule condition should be activated</param>
        /// <param name="variable">The variable impacted by this rule condition, if any</param>
        /// <returns>true if the corresponding rule condition has been activated at the time provided</returns>
        public bool RuleActivatedAtTime(RuleCondition ruleCondition, double time, IVariable variable)
        {
            return EventTimeLine.RuleActivatedAtTime(ruleCondition, time, variable);
        }

        /// <summary>
        ///     Provides the log instance, an object on which logging should be performed
        /// </summary>
        public ModelElement LogInstance { get; set; }

        /// <summary>
        ///     Terminates the execution of a run
        /// </summary>
        public void EndExecution()
        {
            ExecutionTimeInitializer initializer = new ExecutionTimeInitializer();
            foreach (Dictionary dictionary in EFSSystem.Dictionaries)
            {
                initializer.visit(dictionary);
            }
        }

        /// <summary>
        ///     Provides the next rule priority according to the current one
        /// </summary>
        /// <returns></returns>
        private acceptor.RulePriority NextPriority()
        {
            acceptor.RulePriority retVal = acceptor.RulePriority.defaultRulePriority;

            if (CurrentPriority != null)
            {
                bool currentFound = false;
                foreach (acceptor.RulePriority next in PRIORITIES_ORDER)
                {
                    if (next == CurrentPriority)
                    {
                        currentFound = true;
                    }
                    else if (currentFound)
                    {
                        retVal = next;
                    }
                }
            }

            if (retVal == acceptor.RulePriority.defaultRulePriority)
            {
                retVal = acceptor.RulePriority.aVerification;
            }

            return retVal;
        }

        /// <summary>
        ///     Registers the errors raised during evaluation and create ModelInterpretationFailure for each one of them
        /// </summary>
        /// <param name="errors"></param>
        private void RegisterErrors(Dictionary<Utils.ModelElement, List<ElementLog>> errors)
        {
            foreach (KeyValuePair<Utils.ModelElement, List<ElementLog>> pair in errors)
            {
                foreach (ElementLog log in pair.Value)
                {
                    switch (log.Level)
                    {
                        case ElementLog.LevelEnum.Error:
                            ModelInterpretationFailure modelInterpretationFailure = new ModelInterpretationFailure(log,
                                pair.Key as INamable, null);
                            ModelElement modelElement = pair.Key as ModelElement;
                            if (modelElement != null)
                            {
                                modelInterpretationFailure.Explanation = modelElement.Explain;
                            }
                            EventTimeLine.AddModelEvent(modelInterpretationFailure, this, true);
                            break;

                        case ElementLog.LevelEnum.Warning:
                            break;
                        case ElementLog.LevelEnum.Info:
                            break;
                    }
                }
            }
        }
    }
}