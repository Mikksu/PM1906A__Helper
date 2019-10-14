namespace PM1906AHelper
{
    public partial class PM1906A
    {
        private const string CMD_RST = "*RST";
        private const string CMD_ERR = "*ERR";
        private const string CMD_IDN = "*IDN?";
        private const string CMD_READ = "READ?";
        private const string CMD_RANGE = "RANGE";
        private const string CMD_UNIT = "UNIT";
        private const string CMD_WAV = "WAV";

        private const string CMD_TS_START = "TS:START";
        private const string CMD_TS_STOP = "TS:STOP";
        private const string CMD_TS_MODE = "TS:MODE";
        private const string CMD_TS_STA = "TS:STA";
        private const string CMD_TS_CLR = "TS:BUF:CLR";
        private const string CMD_TS_GET_LEN = "TS:BUF:LEN";
        private const string CMD_TS_READ = "TS:BUF:READ";

    }
}
