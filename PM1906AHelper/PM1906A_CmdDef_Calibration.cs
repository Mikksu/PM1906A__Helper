namespace PM1906AHelper
{
    public partial class PM1906A
    {
        /// <summary>
        /// Set the order of the FIR filter.
        /// <para>Order:1 - 256</para>
        /// </summary>
        private string CMD_CAL_FIR_ORDER = "__CAL_FIR_ORDER";

        /// <summary>
        /// Set the coefficient of the FIR filter.
        /// <para>[index],[value]</para>
        /// </summary>
        private string CMD_CAL_FIR_COEFF = "__CAL_FIR_COEFF";

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
        private string CMD_CAL_FUNC = "__CAL_FUNC";

        /// <summary>
        /// Test the dark current automatically.
        /// </summary>
        private string CMD_DC_TST_AUTO = "__CAL_DC_TST";
    }
}
