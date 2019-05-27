using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM1906AHelper.Calibration
{
    public class FuncPerWav
    {
        /// <summary>
        /// Get or set the wavelength.
        /// </summary>
        public int WL { get; set; }

        public FuncPerRange[] PARAM { get; set; }

        public override string ToString()
        {
            return $"{WL}nm";
        }

    }
}
