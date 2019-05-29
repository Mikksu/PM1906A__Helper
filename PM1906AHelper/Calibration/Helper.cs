using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace PM1906AHelper.Calibration
{
    public class Helper
    {
        [Category("Basic")]
        [DisplayName("AD Background Noise(mV)")]
        public double ADBackgroundNoise { get; set; }

        [Category("Basic")]
        [DisplayName("PD Dark Current(uA)")]
        public double PDDarkCurrent { get; set; }

        [Category("Basic")]
        [DisplayName("Rsistance(ohm)")]
        public double[] Res { get; set; }

        [Category("Advanced")]
        [DisplayName("Functions")]
        public FuncPerWav[] FuncsPerWavelength { get; set; }

        [Browsable(false)]
        public string RawJson { get; internal set; }

    }
}
