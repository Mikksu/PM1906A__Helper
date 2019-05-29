using System.ComponentModel;
using PM1906AHelper.Core;

namespace PM1906AHelper.Calibration
{
    public class FuncPerRange : NotifiableClassBase
    {
        private double _a;
        private double _b;
        private double _c;

        [ReadOnly(true)]
        public int Range { get; set; }

        public double A
        {
            get
            {
                return _a;
            }
            set
            {
                UpdateProperty(ref _a, value);
            }
        }

        public double B
        {
            get
            {
                return _b;
            }
            set
            {
                UpdateProperty(ref _b, value);
            }
        }

        public double C
        {
            get
            {
                return _c;
            }
            set
            {
                UpdateProperty(ref _c, value);
            }
        }


        public override string ToString()
        {
            // return $"{A} * x^2 + {B} * x + {C}";
            return $"Range{Range}";
        }
    }
}
