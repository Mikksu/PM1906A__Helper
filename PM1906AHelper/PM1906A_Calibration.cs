﻿using Newtonsoft.Json;
using PM1906AHelper.Core;
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
        public void ReadCalParam(out Calibration.CalibrationHelper CalHelper)
        {
            var json = _query(CMD_CAL_GET);

            CalHelper = Calibration.CalibrationHelper.FromJson(json);
        }

        /// <summary>
        /// Save the calibration parameters to the flash.
        /// </summary>
        public void SaveCalParam()
        {
            _write(CMD_CAL_SAV);
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

        public void SetFunc(WavelengthEnum Wavelen, RangeEnum Range, double A, double B, double C, double DarkCurrent)
        {
            _write($"{CMD_CAL_FUNC} {(int)Wavelen},{(int)Range},{A},{B},{C},{DarkCurrent}");
        }
    }
}
