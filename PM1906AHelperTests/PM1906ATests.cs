using PM1906AHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using PM1906AHelper.Core;

namespace PM1906AHelper.Tests
{
    [TestClass()]
    public class PM1906ATests
    {
        private string PortName = "COM13";
        private int BaudRate = 921600;

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
    }
}