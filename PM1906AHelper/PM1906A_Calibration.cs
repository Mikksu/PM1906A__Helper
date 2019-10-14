using PM1906AHelper.Core;
using System;

namespace PM1906AHelper
{
    public partial class PM1906A
    {
        /// <summary>
        /// Get the maximum order of the FIR filter supported by the firmware.
        /// </summary>
        public const int MAX_FIR_ORDER = 256;

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
        /// Set the value of the sampling resistors.
        /// </summary>
        /// <param name="Range"></param>
        /// <param name="Res"></param>
        public void SetSamplingResistance(RangeEnum Range, double Res)
        {
            _write($"{CMD_CAL_RES} {(int)Range},{Res}");
        }

        /// <summary>
        /// Set the equation to convert the ADC value to optical power.
        /// </summary>
        /// <param name="Wavelen"></param>
        /// <param name="Range"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="DarkCurrent"></param>
        public void SetFunc(WavelengthEnum Wavelen, RangeEnum Range, double A, double B, double C)
        {
            _write($"{CMD_CAL_FUNC} {(int)Wavelen},{(int)Range},{A},{B},{C}");
        }

        /// <summary>
        /// Set the order of the FIR filter.
        /// </summary>
        /// <param name="Order"> should be 1 - <see cref="MAX_FIR_ORDER"/></param>
        public void SetFIROrder(int Order)
        {
            if (Order > MAX_FIR_ORDER || Order < 1)
                throw new ArgumentOutOfRangeException($"the order should be 1 - {MAX_FIR_ORDER}.");
            else
            {
                _write($"{CMD_CAL_FIR_ORDER} {Order}");
            }
        }

        /// <summary>
        /// Set the coefficient locates in the specific position.
        /// </summary>
        /// <param name="Index">should be 0 - <see cref="MAX_FIR_ORDER"/></param>
        /// <param name="Value"></param>
        public void SetFIRCoefficient(int Index, double Value)
        {
            if(Index > MAX_FIR_ORDER || Index < 0)
                throw new ArgumentOutOfRangeException($"the index should be 1 - {MAX_FIR_ORDER}.");
            else
            {
                _write($"{CMD_CAL_FIR_COEFF} {Index},{Value}");
            }
        }
        
        /// <summary>
        /// Run the process to test the dark-current automatically.
        /// </summary>
        public void TestDarkCurrent()
        {
            _write($"{CMD_DC_TST_AUTO}");
        }
    }
}
