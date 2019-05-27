using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM1906AHelper
{
    public partial class PM1906A
    {
        /// <summary>
        /// Read the calibration param from the device.
        /// </summary>
        private string CMD_CAL_GET = "__CAL_GET";

        /// <summary>
        /// Reset the calibration params to the default value.
        /// </summary>
        private string CMD_CAL_DEF = "__CAL_DEF";

        /// <summary>
        /// Save the calibration param.
        /// </summary>
        private string CMD_CAL_SAV = "__CAL_SAV";

        /// <summary>
        /// Set the resistance of the sampling resistor of each range.
        /// </summary>
        private string CMD_CAL_RES = "__CAL_RES";

        /// <summary>
        /// Set the factors of the calibration function of each range of each wavelength
        /// </summary>
        private string CMD_CAL_SET = "__CAL_SET";

        /// <summary>
        /// Set the dark current of the PD in uA.
        /// </summary>
        private string CMD_CAL_PDDC = "__CALPDDC";

        /// <summary>
        /// Set the background noise of the ADC in mV.
        /// </summary>
        private string CMD_CAL_ADBN = "__CALADBN";
    }
}
