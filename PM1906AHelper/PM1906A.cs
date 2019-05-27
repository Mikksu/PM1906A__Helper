using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PM1906AHelper
{
    public partial class PM1906A : IDisposable
    {

        #region Variables

        SerialPort port;

        #endregion

        #region Constructors

        public PM1906A(string PortName, int BaudRate)
        {
            port = new SerialPort(PortName, BaudRate, Parity.None, 8, StopBits.One);
            port.ReadTimeout = 2000;
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
                Value = Convert.ToDouble(pig[0]);
                Unit = (UnitEnum)Enum.Parse(typeof(UnitEnum), pig[1]);
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
            var cmd = $"{CMD_UNIT} {((int)Unit + 1)}";
            port.WriteLine(cmd);

            Thread.Sleep(10);
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
