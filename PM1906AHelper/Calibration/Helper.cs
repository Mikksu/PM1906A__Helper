using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM1906AHelper.Calibration
{
    public class Helper
    {
        public double ADBackgroudNoise { get; set; }

        public double PDDarkCurrent { get; set; }

        public double[] Res { get; set; }

        public FuncPerWav[] CalFactors { get; set; }

    }
}
