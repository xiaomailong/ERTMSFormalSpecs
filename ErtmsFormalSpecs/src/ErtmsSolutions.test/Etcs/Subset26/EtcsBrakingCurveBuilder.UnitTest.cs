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
        private StringBuilder TestOutput;
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
                        new SiUnits.SiAcceleration(-(0.5 + (i-j)/8))
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
            QuadraticSpeedDistanceCurve deceleration = EtcsBrakingCurveBuilder_Obsolete.Build_A_Safe_Backward(acceleration, mrsp);

            // Compute the deceleration curve using the new algorithm
            // TODO : Implement the new algorithm and use it
            QuadraticSpeedDistanceCurve deceleration2 = EtcsBrakingCurveBuilder.Build_A_Safe_Backward(acceleration, mrsp);

            TestOutput = new StringBuilder();

            // Compare the deceleration curves
            for (double d = 0.0; d < 5000.0; d += 1)
            {
                Assert.AreEqual(
                    deceleration.GetValueAt(new SiUnits.SiDistance(0.0 + d)), 
                    deceleration2.GetValueAt(new SiUnits.SiDistance(0.0 + d)), 
                    "Value at " + d + " should be equal"
                );
                TestOutput.Append(d.ToString());
                TestOutput.Append("\t");
                TestOutput.Append(mrsp.GetValueAt(new SiUnits.SiDistance(d)).Value);
                TestOutput.Append("\t");
                TestOutput.Append(deceleration.GetValueAt(new SiUnits.SiDistance(d)).Value);
                TestOutput.Append("\t");
                TestOutput.Append(deceleration2.GetValueAt(new SiUnits.SiDistance(d)).Value);
                TestOutput.Append("\n");
            }
            System.IO.File.WriteAllText("ResultsCompare.csv", TestOutput.ToString());
        }

        [TestMethod]
        public void Build_Deceleration_CurveTestMethod()
        {
            // TODO : Fill the acceleration surface
            AccelerationSpeedDistanceSurface acceleration = new AccelerationSpeedDistanceSurface();

            for (int i = 0; i < 8; i++)
            {
                double StartDistance = i * 625;
                double EndDistance = StartDistance + 625;

                for (int j = 0; j < 6; j++)
                {
                    double StartSpeed = j * 35;
                    double EndSpeed = StartSpeed + 35;

                    acceleration.Tiles.Add(new SurfaceTile(
                        new SiUnits.SiDistance(StartDistance),
                        new SiUnits.SiDistance(EndDistance),
                        new SiUnits.SiSpeed(StartSpeed, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour),
                        new SiUnits.SiSpeed(EndSpeed, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour),
                        new SiUnits.SiAcceleration(-Math.Abs(0.5* ( 1.2 + Math.Pow(-1, i - j) ) ))
                    ));
                }
            }


            // the target
            SiUnits.SiSpeed TargetSpeed = new SiUnits.SiSpeed(50, SiUnits.SiSpeed_SubUnits.KiloMeter_per_Hour);
            SiUnits.SiDistance TargetDistance = new SiUnits.SiDistance(2250);

            // Compute the deceleration curve using the previous algorithm
            QuadraticSpeedDistanceCurve deceleration = EtcsBrakingCurveBuilder.Build_Deceleration_Curve(acceleration, TargetSpeed, TargetDistance);

            TestOutput = new StringBuilder();

            // Compare the deceleration curves
            for (double d = 0.0; d < 5000.0; d += 1)
            {
                double spd = deceleration.GetValueAt(new SiUnits.SiDistance(d)).Value;
                TestOutput.Append(d.ToString());
                TestOutput.Append("\t");
                TestOutput.Append(spd.ToString());
                TestOutput.Append("\t");
                TestOutput.Append(acceleration.GetTileAt(new SiUnits.SiSpeed(spd), new SiUnits.SiDistance(d)).V.Y.Value);
                TestOutput.Append("\n");
            }
            System.IO.File.WriteAllText("Results.csv", TestOutput.ToString());
        }
    }
}
