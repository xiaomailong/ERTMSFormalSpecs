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

namespace DataDictionary.Functions.PredefinedFunctions
{
    /// <summary>
    /// 
    /// </summary>
    public class Discontinuities : PredefinedFunction
    {
        /// <summary>
        /// The MRSP function
        /// </summary>
        public Parameter Targets { get; private set; }


        public override Types.Type ReturnType
        {
            get { return EFSSystem.findType(OverallNameSpaceFinder.INSTANCE.findByName(EFSSystem.Dictionaries[0], "Kernel.SpeedAndDistanceMonitoring.TargetSpeedMonitoring"), "Kernel.SpeedAndDistanceMonitoring.TargetSpeedMonitoring.Targets"); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="efsSystem"></param>
        public Discontinuities(EFSSystem efsSystem)
            : base(efsSystem,"DISCONT")
        {
            Targets = (Parameter)Generated.acceptor.getFactory().createParameter();
            Targets.Name = "Targets";
            Targets.Type = EFSSystem.AnyType;
            Targets.setFather(this);
            FormalParameters.Add(Targets);
        }

        /// <summary>
        /// Perform additional checks based on the parameter type
        /// </summary>
        /// <param name="root">The element on which the errors should be reported</param>
        /// <param name="context">The evaluation context</param>
        /// <param name="actualParameters">The parameters applied to this function call</param>
        public override void additionalChecks(ModelElement root, Interpreter.InterpretationContext context, Dictionary<string, Interpreter.Expression> actualParameters)
        {
            CheckFunctionalParameter(root, context, actualParameters[Targets.Name], 1);
        }

        /// <summary>
        /// Provides the value of the function
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actuals"></param>
        /// <returns></returns>
        public override Values.IValue Evaluate(Interpreter.InterpretationContext context, Dictionary<Variables.Actual, Values.IValue> actuals)
        {
            Values.IValue retVal = null;

            int token = context.LocalScope.PushContext();
            AssignParameters(context, actuals);

            Types.Collection collectionType = (Types.Collection)EFSSystem.findType(OverallNameSpaceFinder.INSTANCE.findByName(EFSSystem.Dictionaries[0], "Kernel.SpeedAndDistanceMonitoring.TargetSpeedMonitoring"), "Kernel.SpeedAndDistanceMonitoring.TargetSpeedMonitoring.Targets");
            Values.ListValue collection = new Values.ListValue(collectionType, new List<Values.IValue>());

            Function function = context.findOnStack(Targets).Value as Functions.Function;
            if (function != null && !function.Name.Equals("EMPTY"))
            {
                Graph graph1 = createGraphForValue(context, function);
                ComputeTargets(graph1.Function, collection);
            }

            context.LocalScope.PopContext(token);

            retVal = collection;
            return retVal;
        }

        /// <summary>
        /// Computes targets from the function and stores them in the collection
        /// </summary>
        /// <param name="function">Function containing targets</param>
        /// <param name="collection">Collection to be filled with targets</param>
        private void ComputeTargets(Function function, Values.ListValue collection)
        {
            bool retVal = true;
            if (function != null)
            {
                Graph graph = function.Graph;
                if (graph != null)
                {
                    for (int i = 0; i < graph.Segments.Count; i++)
                    {
                        Graph.Segment s = graph.Segments[i];
                        Values.StructureValue value = CreateTarget(s.Start, s.End - s.Start, s.Val(s.Start));

                        collection.Val.Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a single target
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        private Values.StructureValue CreateTarget(double start, double length, double speed)
        {
            Types.Structure structureType = (Types.Structure)EFSSystem.findType(OverallNameSpaceFinder.INSTANCE.findByName(EFSSystem.Dictionaries[0], "Kernel.SpeedAndDistanceMonitoring.TargetSpeedMonitoring"), "Kernel.SpeedAndDistanceMonitoring.TargetSpeedMonitoring.Target");
            Values.StructureValue value = new Values.StructureValue(structureType);

            Variables.Variable speedV = (Variables.Variable)DataDictionary.Generated.acceptor.getFactory().createVariable();
            speedV.Type = EFSSystem.findType(OverallNameSpaceFinder.INSTANCE.findByName(EFSSystem.Dictionaries[0], "Default.BaseTypes"), "Default.BaseTypes.Speed");
            speedV.Name = "Speed";
            speedV.Mode = Generated.acceptor.VariableModeEnumType.aInternal;
            speedV.Default = "0.0";
            speedV.Enclosing = value;
            speedV.Value = new Values.DoubleValue(EFSSystem.DoubleType, speed);
            value.set(speedV);

            Variables.Variable location = (Variables.Variable)DataDictionary.Generated.acceptor.getFactory().createVariable();
            location.Type = EFSSystem.findType(OverallNameSpaceFinder.INSTANCE.findByName(EFSSystem.Dictionaries[0], "Default.BaseTypes"), "Default.BaseTypes.Distance");
            location.Name = "Location";
            location.Mode = Generated.acceptor.VariableModeEnumType.aInternal;
            location.Default = "0.0";
            location.Enclosing = value;
            location.Value = new Values.DoubleValue(EFSSystem.DoubleType, start);
            value.set(location);

            Variables.Variable lengthV = (Variables.Variable)DataDictionary.Generated.acceptor.getFactory().createVariable();
            lengthV.Type = EFSSystem.findType(OverallNameSpaceFinder.INSTANCE.findByName(EFSSystem.Dictionaries[0], "Default.BaseTypes"), "Default.BaseTypes.Length");
            lengthV.Name = "Length";
            lengthV.Mode = Generated.acceptor.VariableModeEnumType.aInternal;
            lengthV.Default = "0.0";
            lengthV.Enclosing = value;
            lengthV.Value = new Values.DoubleValue(EFSSystem.DoubleType, length);
            value.set(lengthV);
            return value;
        }
    }
}
