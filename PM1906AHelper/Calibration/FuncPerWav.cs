using System.ComponentModel;

namespace PM1906AHelper.Calibration
{
    public class FuncPerWav
    {
        [ReadOnly(true)]
        [Browsable(false)]
        /// <summary>
        /// Get or set the wavelength.
        /// </summary>
        public int Wavelength { get; set; }

        [DisplayName("Functions")]
        public FuncPerRange[] Funcs { get; set; }

        public override string ToString()
        {
            return $"{Wavelength}nm";
        }

    }
}
