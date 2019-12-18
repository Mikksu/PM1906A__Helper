using PM1906AHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using PM1906AHelper.Core;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;

namespace PM1906AHelper.Tests
{
    [TestClass()]
    public class PM1906ATests
    {
        private string PortName = "COM7";
        private int BaudRate = 921600;

        [TestMethod()]
        public void TriggerTest()
        {
            using (PM1906A pm = new PM1906A(PortName, BaudRate))
            {
                pm.Open();

                var idn = pm.IDN();

                Debug.WriteLine(idn);

                pm.Trigger_Start();

                Thread.Sleep(4000);

                pm.Trigger_Stop();

                var len = pm.Trigger_GetUsedBuffLen();

                Debug.WriteLine($"There are {len} points in the trigger buffer.");

                var powers = pm.Trigger_ReadBuffer();
            }
        }

        [TestMethod()]
        public void ReadTest()
        {
            using (PM1906A pm = new PM1906A(PortName, BaudRate))
            {
                pm.Open();

                var idn = pm.IDN();

                Trace.WriteLine(idn);

                pm.Read(out double power, out UnitEnum unit);

                Trace.Indent();
                Trace.WriteLine($"Power: {power} {unit}");
                Trace.Unindent();
            }
        }

        [TestMethod()]
        public void RangeOprationTest()
        {
            using (PM1906A pm = new PM1906A(PortName, BaudRate))
            {
                pm.Open();

                var idn = pm.IDN();

                Trace.WriteLine(idn);


                pm.SetRange(RangeEnum.RANGE1);
                pm.GetRange(out RangeEnum range);

                Trace.Indent();
                Trace.WriteLine($"Current Range: {range}");
                Trace.Unindent();

                Assert.AreEqual(range, RangeEnum.RANGE1);

                // switch to range1
                Trace.WriteLine($"Switch range to {RangeEnum.RANGE6}");

                pm.SetRange(RangeEnum.RANGE6);
                pm.GetRange(out range);

                Trace.Indent();
                Trace.WriteLine($"Current Range: {range}");
                Trace.Unindent();

                Assert.AreEqual(range, RangeEnum.RANGE6);
            }
        }

        [TestMethod()]
        public void ReadCalibrationParamTest()
        {
            using (PM1906A pm = new PM1906A(PortName, BaudRate))
            {
                pm.Open();

                var idn = pm.IDN();

                Trace.WriteLine(idn);

                pm.ReadCalParam(out Calibration.CalibrationHelper CalHelper);
            }
        }

        [TestMethod()]
        public void SetSamplingResistanceTest()
        {
            using (PM1906A pm = new PM1906A(PortName, BaudRate))
            {
                pm.Open();

                var idn = pm.IDN();

                Trace.WriteLine(idn);

                
                pm.SetSamplingResistance(RangeEnum.RANGE2, 1.0);
                pm.ReadCalParam(out Calibration.CalibrationHelper calHelper);

                Assert.AreEqual(calHelper.Res[(int)RangeEnum.RANGE2], 1.0);

                pm.SetSamplingResistance(RangeEnum.RANGE2, 166065.0);
                pm.ReadCalParam(out calHelper);

                Assert.AreEqual(calHelper.Res[(int)RangeEnum.RANGE2], 166065.0);
            }
        }

        [TestMethod]
        public void FIR_Filter_Test()
        {

            //Stopwatch sw = new Stopwatch();

            //sw.Start();

            FIRCreateCoeff();

            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";

            // Random r = new Random();

            using (PM1906A pm = new PM1906A(PortName, BaudRate))
            {
                pm.Open();

                var idn = pm.IDN();

                pm.SetUnit(UnitEnum.V);

                using (StreamWriter wr = File.CreateText(filename))
                {
                    while (true)
                    {
                        pm.Read(out double power, out UnitEnum unit);
                        wr.WriteLine(power);
                        // wr.WriteLine($"{sw.ElapsedTicks},{power}");

                        //Thread.Sleep(1);
                    }
                }

                

            }

            
            
        }

        #region FIR Filter Test

        const int FIR_ORDER = 64;
        Queue<double> fir_data = new Queue<double>(FIR_ORDER);
        List<double> fir_coeff = new List<double>(FIR_ORDER);

        private void FIRCreateCoeff()
        {
            for (int i = 0; i < FIR_ORDER; i++)
            {
                fir_coeff.Add(1.0 / (double)FIR_ORDER);
                fir_data.Enqueue(0.0);
            }
        }

        private double Smooth(double Power)
        {
            fir_data.Enqueue(Power);
            if (fir_data.Count > FIR_ORDER)
                fir_data.Dequeue();

            var x = fir_data.ToArray();
            var h = fir_coeff.ToArray();

            double y = 0.0;

            for (int i = 0; i < FIR_ORDER; i++)
            {
                y += h[i] * x[FIR_ORDER - i - 1];
            }

            return y;
        }

        private List<double> FIRFilter(List<double> b, List<double> x)
        {
            //y[n]=b0x[n]+b1x[n-1]+....bmx[n-M]

            var y = new List<double>();

            int M = b.Count;
            int n = x.Count;

            double t = 0.0;

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < M; i++)
                {
                    t += b[i] * x[n - i - 1];
                }
                y.Add(t);
            }

            return y;
        }

        #endregion
    }
}