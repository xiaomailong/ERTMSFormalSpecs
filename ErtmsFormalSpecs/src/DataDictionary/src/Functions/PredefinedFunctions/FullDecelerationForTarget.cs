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
using ErtmsSolutions.Etcs.Subset26.BrakingCurves;
using ErtmsSolutions.SiUnits;

namespace DataDictionary.Functions.PredefinedFunctions
{
    /// <summary>
    /// Returns the full deceleration curve (all the way down to zero speed) for the given target
    /// </summary>
    public class FullDecelerationForTarget : FunctionOnGraph
    {
        /// <summary>
        /// The target used for the deceleration curve
        /// </summary>
        public Parameter Target { get; private set; }

        /// <summary>
        /// The deceleration factor
        /// </summary>
        public Parameter DecelerationFactor { get; private set; }

        public FullDecelerationForTarget(EFSSystem efsSystem)
            : base(efsSystem, "FullDecelerationForTarget")
        {
            Target = (Parameter)Generated.acceptor.getFactory().createParameter();
            Target.Name = "Target";
            Target.Type = EFSSystem.AnyType;
            Target.setFather(this);
            FormalParameters.Add(Target);

            DecelerationFactor = (Parameter)Generated.acceptor.getFactory().createParameter();
            DecelerationFactor.Name = "DecelerationFactor";
            DecelerationFactor.Type = EFSSystem.AnyType;
            DecelerationFactor.setFather(this);
            FormalParameters.Add(DecelerationFactor);
        }

        /// <summary>
        /// Perform additional checks based on the parameter types
        /// </summary>
        /// <param name="root">The element on which the errors should be reported</param>
        /// <param name="context">The evaluation context</param>
        /// <param name="actualParameters">The parameters applied to this function call</param>
        public override void additionalChecks(ModelElement root, Interpreter.InterpretationContext context, Dictionary<string, Interpreter.Expression> actualParameters)
        {
            CheckFunctionalParameter(root, context, actualParameters[Target.Name], 1);
            CheckFunctionalParameter(root, context, actualParameters[DecelerationFactor.Name], 2);
        }

        /// <summary>
        /// Creates a graph for the function
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override Graph createGraph(Interpreter.InterpretationContext context, Parameter parameter)
        {
            Graph retVal = new Graph();

            Values.StructureValue LocationStruct = context.findOnStack(Target).Value as Values.StructureValue;

            SiDistance location;
            SiSpeed speed;

            if (LocationStruct != null)
            {
                Variables.Variable Location = LocationStruct.Val["Location"] as Variables.Variable;
                location =  new SiDistance((Location.Value as Values.DoubleValue).Val);
                Variables.Variable Speed = LocationStruct.Val["Speed"] as Variables.Variable;
                speed = new SiSpeed((Speed.Value as Values.DoubleValue).Val, SiSpeed_SubUnits.KiloMeter_per_Hour);

                Function decelerationFactor = context.findOnStack(DecelerationFactor).Value as Function;
                if (decelerationFactor != null)
                {
                    Surface DecelerationSurface = decelerationFactor.createSurface(context);
                    if (DecelerationSurface != null)
                    {
                        AccelerationSpeedDistanceSurface accelerationSurface = DecelerationSurface.createAccelerationSpeedDistanceSurface(double.MaxValue, double.MaxValue);

                        QuadraticSpeedDistanceCurve BrakingCurve = null;

                        try
                        {
                            BrakingCurve = EtcsBrakingCurveBuilder.Build_Deceleration_Curve(accelerationSurface, speed, location);
                        }
                        catch (System.Exception e)
                        {
                            retVal.addSegment(new Graph.Segment(0, double.MaxValue, new Graph.Segment.Curve(0, 0, 0)));
                        }


                        for (int i = 0; i < BrakingCurve.SegmentCount; i++)
                        {
                            QuadraticCurveSegment segment = BrakingCurve[i];

                            Graph.Segment newSegment = new Graph.Segment(
                                segment.X.X0.ToUnits(),
                                segment.X.X1.ToUnits(),
                                new Graph.Segment.Curve(
                                    segment.A.ToSubUnits(SiAcceleration_SubUnits.Meter_per_SecondSquare),
                                    segment.V0.ToSubUnits(SiSpeed_SubUnits.KiloMeter_per_Hour),
                                    segment.D0.ToSubUnits(SiDistance_SubUnits.Meter)
                                    )
                                );
                            retVal.addSegment(newSegment);
                        }
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Provides the value of the function
        /// </summary>
        /// <param name="context">The evaluation context</param>
        /// <param name="actuals">The parametes applied to this function call</param>
        /// <returns></returns>
        public override Values.IValue Evaluate(Interpreter.InterpretationContext context, Dictionary<Variables.Actual, Values.IValue> actuals)
        {
            Values.IValue retVal = null;

            int token = context.LocalScope.PushContext();
            AssignParameters(context, actuals);

            Function function = (Function)Generated.acceptor.getFactory().createFunction();
            function.Name = "FullDecelerationForTarget ( Target => " + getName(Target) + ", DecelerationFactor => " + getName(DecelerationFactor) + " )";
            function.Enclosing = EFSSystem;
            Parameter parameter = (Parameter)Generated.acceptor.getFactory().createParameter();
            parameter.Name = "X";
            parameter.Type = EFSSystem.DoubleType;
            function.appendParameters(parameter);
            function.ReturnType = EFSSystem.DoubleType;
            function.Graph = createGraph(context, parameter);

            retVal = function;
            context.LocalScope.PopContext(token);

            return retVal;
        }
    }
}
