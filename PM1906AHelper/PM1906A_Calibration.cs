using Newtonsoft.Json;
using System.Threading;

namespace PM1906AHelper
{
    public partial class PM1906A
    {
        /// <summary>
        /// Reset the calibration parameters to the default values.
        /// </summary>
        public void ResetCalParamToDefault()
        {
            _write(CMD_CAL_DEF);
        }

        /// <summary>
        /// Read the calibration parameters from the device as the JSON string.
        /// </summary>
        /// <param name="CalHelper"></param>
        public void ReadCalParam(out Calibration.Helper CalHelper)
        {
            var json = _query(CMD_CAL_GET);

            CalHelper = JsonConvert.DeserializeObject<Calibration.Helper>(json);
        }

        /// <summary>
        /// Save the calibration parameters to the flash.
        /// </summary>
        public void SaveCalParam()
        {
            _write(CMD_CAL_SAV);
        }

        /// <summary>
        /// Set the dark current.
        /// </summary>
        /// <param name="DC_uA">Dark Current in uA</param>
        public void SetDarkCurrent(double DC_uA)
        {
            _write($"{CMD_CAL_PDDC} {DC_uA}");
        }

        /// <summary>
        /// Set the background noise of the inner ADC.
        /// </summary>
        /// <param name="BN_mV">Background Noise in mV.</param>
        public void SetADCBackgroundNoise(double BN_mV)
        {
            _write($"{CMD_CAL_ADBN} {BN_mV}"); 
        }

        public void SetSamplingResistance(RangeEnum Range, double Res)
        {
            _write($"{CMD_CAL_RES} {(int)Range},{Res}");
        }

        public void SetFunc(WavelengthEnum Wavelen, RangeEnum Range, double A, double B)
        {
            _write($"{CMD_CAL_SET} {(int)Wavelen},{(int)Range},{A},{B}");
        }
    }
}
