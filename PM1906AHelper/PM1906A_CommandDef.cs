using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
