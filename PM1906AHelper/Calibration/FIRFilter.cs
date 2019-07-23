using PM1906AHelper.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM1906AHelper.Calibration
{
    public class FIRFilter: NotifiableClassBase
    {
        #region Constructors

        public FIRFilter()
        {
            Coefficient = new double[1];
            Order = 128;
            Fs = 2000;
        }

        #endregion

        /// <summary>
        /// Get or set the order of the FIR filter.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Get or set the sampling frequency.
        /// </summary>
        [ReadOnly(true)]
        public int Fs { get; set; }

        /// <summary>
        /// Get or set the coefficient.
        /// </summary>
        public double[] Coefficient { get; set; }


        #region Methods

        public void ZeroCoefficient()
        {
            for (int i = 0; i < Coefficient.Length; i++)
            {
                Coefficient[i] = 0;
            }
        }

        /// <summary>
        /// Flush to update the value displayed on the window.
        /// </summary>
        public void Flush()
        {
            OnPropertyChanged("Coefficient");
        }


        public override string ToString()
        {
            return "FIR Filter Parameters";
        }

        #endregion

    }
}
