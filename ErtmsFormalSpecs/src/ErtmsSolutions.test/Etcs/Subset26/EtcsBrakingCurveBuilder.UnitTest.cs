using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErtmsSolutions.Etcs.Subset26.BrakingCurves
{
    [TestClass]
    public class EtcsBrakingCurveBuilder_UnitTest
    {
        [TestMethod]
        public void Build_A_Safe_BackwardTestMethod()
        {
            // TODO : Fill the acceleration surface
            AccelerationSpeedDistanceSurface acceleration = new AccelerationSpeedDistanceSurface();
                        
            for (int i = 0; i < 5; i++)
            {
                double StartDistance = i * 1000;
                double EndDistance = StartDistance + 1000;

                for (int j = 0; j < 4; j++)
                {
                    double StartSpeed = j * 60;
                    double EndSpeed = StartSpeed + 60;

                    acceleration.Tiles.Add(new SurfaceTile(
                        new SiUnits.SiDistance(StartDistance),
                        new SiUnits.SiDistance(EndDistance),
                        new SiUnits.SiSpeed(StartSpeed, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour),
                        new SiUnits.SiSpeed(EndSpeed, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour),
                        new SiUnits.SiAcceleration(-(0.5 + (i-j)/2))
                    ));
                }
            }


            // TODO : Fill the mrsp
            FlatSpeedDistanceCurve mrsp = new FlatSpeedDistanceCurve();
            mrsp.AddSegment(new ConstantCurveSegment<SiUnits.SiDistance,SiUnits.SiSpeed>(
                new SiUnits.SiDistance(0), 
                new SiUnits.SiDistance(650), 
                new SiUnits.SiSpeed(80, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour)
            ));
            mrsp.AddSegment(new ConstantCurveSegment<SiUnits.SiDistance, SiUnits.SiSpeed>(
                new SiUnits.SiDistance(650),
                new SiUnits.SiDistance(1500),
                new SiUnits.SiSpeed(60, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour)
            ));
            mrsp.AddSegment(new ConstantCurveSegment<SiUnits.SiDistance, SiUnits.SiSpeed>(
                new SiUnits.SiDistance(1500),
                new SiUnits.SiDistance(2500),
                new SiUnits.SiSpeed(120, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour)
            ));
            mrsp.AddSegment(new ConstantCurveSegment<SiUnits.SiDistance, SiUnits.SiSpeed>(
                new SiUnits.SiDistance(2500),
                new SiUnits.SiDistance(4000),
                new SiUnits.SiSpeed(55, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour)
            ));
            mrsp.AddSegment(new ConstantCurveSegment<SiUnits.SiDistance,SiUnits.SiSpeed>(
                new SiUnits.SiDistance(4000), 
                new SiUnits.SiDistance(5000),
                new SiUnits.SiSpeed(0)
            ));

            // Compute the deceleration curve using the previous algorithm
            QuadraticSpeedDistanceCurve deceleration = EtcsBrakingCurveBuilder.Build_A_Safe_Backward(acceleration, mrsp);

            // Compute the deceleration curve using the new algorithm
            // TODO : Implement the new algorithm and use it
            QuadraticSpeedDistanceCurve deceleration2 = EtcsBrakingCurveBuilder_NT.Build_A_Safe_Backward(acceleration, mrsp);

            // Compare the deceleration curves
            for (double d = 0.0; d < 5000.0; d += 1)
            {
                Assert.AreEqual(
                    deceleration.GetValueAt(new SiUnits.SiDistance(0.0 + d)), 
                    deceleration2.GetValueAt(new SiUnits.SiDistance(0.0 + d)), 
                    "Value at " + d + " should be equal"
                );
            }
        }
    }
}
