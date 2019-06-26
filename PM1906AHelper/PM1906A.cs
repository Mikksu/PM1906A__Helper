using PM1906AHelper.Core;
using System;
using System.IO.Ports;
using System.Threading;

namespace PM1906AHelper
{
    public partial class PM1906A : IDisposable
    {

        #region Variables

        SerialPort port;

        FirFilter fir_filter;

        #endregion

        #region Constructors

        public PM1906A(string PortName, int BaudRate)
        {
            port = new SerialPort(PortName, BaudRate, Parity.None, 8, StopBits.One);
            port.ReadTimeout = 2000;

            fir_filter = new FirFilter(10);
        }

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        private void _write(string command)
        {
            port.WriteLine(command);
            Thread.Sleep(10);
        }

        private string _query(string command)
        {
            // clear the recevie buffer.
            port.ReadExisting();

            port.WriteLine(command);
            var str = port.ReadLine();
            return str.Replace("\r", "").Replace("\n", "");
        }

        private void _throw_excpetion(string ret_string)
        {
            throw new Exception($"the format of the returned string `{ret_string}` is error.");
        }

        #endregion

        #region Public Methods

        public void Open()
        {
            port.Open();
        }

        public void Close()
        {
            if (port != null && port.IsOpen)
                port.Close();
        }

        public void RST()
        {
            port.WriteLine(CMD_RST);
        }

        public string ERR()
        {
            return _query(CMD_ERR);
        }

        public string IDN()
        {
            return _query(CMD_IDN);
        }

        public void Read(out double Value, out UnitEnum Unit)
        {
            Value = double.NaN;
            Unit = UnitEnum.dBm;

            var monkey = _query(CMD_READ);

            // split the value and the unit.
            var pig = monkey.Split(' ');

            try
            {

                var dog = Convert.ToDouble(pig[0]);
                Unit = (UnitEnum)Enum.Parse(typeof(UnitEnum), pig[1]);

                // filter the power.
                Value = fir_filter.Filter(dog);

                //if (Unit != UnitEnum.dBm)
                //    Value /= 1000;
            }
            catch
            {
                _throw_excpetion(monkey);
            }

        }

        public void GetRange(out RangeEnum Range)
        {
            Range = RangeEnum.RANGE1;
            var monkey = _query($"{CMD_RANGE}?");
            try
            {
                Range = (RangeEnum)Enum.Parse(typeof(RangeEnum), monkey);
            }
            catch
            {
                _throw_excpetion(monkey);
            }
        }

        public void SetRange(RangeEnum Range)
        {
            var cmd = $"{CMD_RANGE} {((int)Range + 1)}";
            port.WriteLine(cmd);

            Thread.Sleep(10);
        }

        public void GetUnit(out UnitEnum Unit)
        {
            Unit = UnitEnum.dBm;
            var monkey = _query($"{CMD_UNIT}?");
            try
            {
                Unit = (UnitEnum)Enum.Parse(typeof(UnitEnum), monkey);
            }
            catch
            {
                _throw_excpetion(monkey);
            }
        }

        public void SetUnit(UnitEnum Unit)
        {
            var cmd = $"{CMD_UNIT} {Unit}";
            _write(cmd);
        }

        public void GetWavelength(out int Wav)
        {
            Wav = 0;
            var monkey = _query($"{CMD_WAV}?");
            try
            {
                var pig = monkey.Split(' ');
                Wav = Convert.ToInt32(pig[0]);
            }
            catch
            {
                _throw_excpetion(monkey);
            }
        }

        public void SetWavelength(int Wav)
        {
            var cmd = $"{CMD_WAV} {Wav}";
            _write(cmd);
        }

        /// <summary>
        /// Format the optical power value accorrding to the Unit and Range.
        /// </summary>
        /// <param name="Unit"></param>
        /// <param name="Range"></param>
        /// <param name="Power"></param>
        /// <param name="Formatted"></param>
        public static void FormatOutputPower(UnitEnum Unit, RangeEnum Range, double Power, out string Formatted)
        {
            Formatted = "";

            switch (Unit)
            {
                case UnitEnum.dBm:
                    Formatted = Power.ToString("F3");
                    break;

                case UnitEnum.W:
                case UnitEnum.A:
                case UnitEnum.V:
                    var tmp = Power.ToString("F10");
                    Formatted = tmp.Remove(tmp.Length - 1 - (int)Range);
                    break;
            }
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PM1906A()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
