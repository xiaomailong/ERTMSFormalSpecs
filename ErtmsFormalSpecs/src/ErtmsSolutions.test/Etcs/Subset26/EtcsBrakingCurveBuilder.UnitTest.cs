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
            acceleration.Tiles.Add(new SurfaceTile(
                new SiUnits.SiDistance(0), 
                new SiUnits.SiDistance(5000), 
                new SiUnits.SiSpeed(0), 
                new SiUnits.SiSpeed(600), 
                new SiUnits.SiAcceleration(-1)
            ));

            // TODO : Fill the mrsp
            FlatSpeedDistanceCurve mrsp = new FlatSpeedDistanceCurve();
            mrsp.AddSegment(new ConstantCurveSegment<SiUnits.SiDistance,SiUnits.SiSpeed>(
                new SiUnits.SiDistance(0), 
                new SiUnits.SiDistance(1000), 
                new SiUnits.SiSpeed(120, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour)
            ));
            mrsp.AddSegment(new ConstantCurveSegment<SiUnits.SiDistance,SiUnits.SiSpeed>(
                new SiUnits.SiDistance(1000), 
                new SiUnits.SiDistance(5000),
                new SiUnits.SiSpeed(0)
            ));

            // Compute the deceleration curve using the previous algorithm
            QuadraticSpeedDistanceCurve deceleration = EtcsBrakingCurveBuilder.Build_A_Safe_Backward(acceleration, mrsp);

            // Compute the deceleration curve using the new algorithm
            // TODO : Implement the new algorithm and use it
            QuadraticSpeedDistanceCurve deceleration2 =EtcsBrakingCurveBuilder.Build_A_Safe_Backward(acceleration, mrsp);

            // Compare the deceleration curves
            for (double d = 0.0; d < 2000.0; d += 1)
            {
                Assert.AreEqual(
                    deceleration.GetValueAt(new SiUnits.SiDistance(0.0)), 
                    deceleration2.GetValueAt(new SiUnits.SiDistance(0.0)), 
                    "Value at " + d + " should be equal"
                );
            }
        }
    }
}
