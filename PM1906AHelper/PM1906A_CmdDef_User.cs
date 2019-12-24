namespace PM1906AHelper
{
    /// <summary>
    /// 命令发送格式：命令字符串+回车符。例如*IDN?\r\n。
    /// </summary>
    public partial class PM1906A
    {
        /// <summary>
        /// 系统复位。
        /// </summary>
        private const string CMD_RST = "*RST";

        /// <summary>
        /// 获取错误信息。
        /// 返回值：错误描述字符串
        /// </summary>
        private const string CMD_ERR = "*ERR";

        /// <summary>
        /// 获取设备信息。
        /// </summary>
        private const string CMD_IDN = "*IDN?";

        /// <summary>
        /// 读取功率。
        /// 返回值：功率和单位，以控制作为间隔，例如 -32.1 dBm。
        /// </summary>
        private const string CMD_READ = "READ?";

        /// <summary>
        /// 设置或读取量程。
        /// 设置量程：发送RANGE x，其中x表示量程，范围为1~6。例如RANGE 2表示切换到第2档量程。
        /// 读取量程：发送RANGE?
        /// </summary>
        private const string CMD_RANGE = "RANGE";

        /// <summary>
        /// 设置或读取单位。
        /// 设置量程：发送UNIT x，其中x表示单位，设备支持的单位为：dBm、W、V、A。例如UNIT dBm表示切换单位到dBm。
        /// 读取量程：发送UNIT?
        /// 注意：单位区分大小写，必须按上述列表中的字符串设置单位。
        /// </summary>
        private const string CMD_UNIT = "UNIT";
        /// <summary>
        /// 设置或读取波长。
        /// 设置波长：发送WAV xxxx，其中xxxx表示波长，单位为nm。例如WAV 1310标志切换波长到1310nm。
        /// 读取波长：发送WAV?
        /// 注意：设备支持的波长范围为1270nm~1550nm
        /// </summary>
        private const string CMD_WAV = "WAV";

        /// <summary>
        /// 启动外部触发信号捕获。
        /// 作用：每收到一个外部出发脉冲，采集一次光功率。
        /// </summary>
        private const string CMD_TS_START = "TS:START";

        /// <summary>
        /// 禁止捕获外部触发信号。
        /// </summary>
        private const string CMD_TS_STOP = "TS:STOP";

        /// <summary>
        /// 设置外部触发信号模式。
        /// 【暂不支持该命令】
        /// </summary>
        private const string CMD_TS_MODE = "TS:MODE";

        /// <summary>
        /// 获取触发源状态。
        /// 返回值：
        /// IDLE：已经禁止捕获外部触发信号。通常设备开机后，或执行了TS:STOP命令后，设备工作在此状态。
        /// BUSY：设备正在捕获外部触发信号。通常执行TS:START后，设备工作在此状态。
        /// </summary>
        private const string CMD_TS_STA = "TS:STA";

        /// <summary>
        /// 清空缓冲区，所有已捕获的功率值均被清除。
        /// </summary>
        private const string CMD_TS_CLR = "TS:BUF:CLR";

        /// <summary>
        /// 获取缓冲区内已捕获的功率值的数量。
        /// </summary>
        private const string CMD_TS_GET_LEN = "TS:BUF:LEN";

        /// <summary>
        /// 读取缓冲区。
        /// </summary>
        private const string CMD_TS_READ = "TS:BUF:READ";

    }
}
