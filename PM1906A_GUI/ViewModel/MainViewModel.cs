using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PM1906AHelper;
using PM1906AHelper.Calibration;
using PM1906AHelper.Core;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PM1906A_GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Variables

        PM1906A pm;
        readonly object pmLocker = new object();

        IProgress<(double Power, UnitEnum Unit)> progress;
        CancellationTokenSource cts;

        private string _portname;
        private int _baudrate = 921600;
        private bool _is_opened = false;
        private double _current_power = -61.2;
        private int _current_wav = 1310;
        private RangeEnum _current_range = RangeEnum.RANGE1;
        private UnitEnum _current_unit = UnitEnum.dBm;
        private CalibrationHelper _cal_helper;
        private string _current_power_formatted = "";

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            // generate fake data to debug
            this.CalibrationHelper = new CalibrationHelper();
            this.CalibrationHelper.ADBackgroundNoise = 12.5;
            this.CalibrationHelper.Res = new double[] { 14567897986.1 };
            this.CalibrationHelper.FuncsPerWavelength = new FuncPerWav[]
            {
                new FuncPerWav()
                {
                    Wavelength = 1310,
                    Funcs = new FuncPerRange[]
                    {
                        new FuncPerRange()
                        {
                            Range = 1,
                            A = 131,
                            B = 12,
                            C = 0.12
                        }
                    }
                }
            };
        }

        #region Properties

        public string[] PortNameList
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }

        public string PortName
        {
            get
            {
                return _portname;
            }
            set
            {
                _portname = value;
                RaisePropertyChanged();
            }
        }

        public int BaudRate
        {
            get
            {
                return _baudrate;
            }
            set
            {
                _baudrate = value;
                RaisePropertyChanged();
            }
        }

        public bool IsOpened
        {
            get
            {
                return _is_opened;
            }
            private set
            {
                _is_opened = value;
                RaisePropertyChanged();
            }
        }
       
        public double CurrentPower
        {
            get
            {
                return _current_power;
            }
            private set
            {
                _current_power = value;
                RaisePropertyChanged();

                PM1906A.FormatOutputPower(CurrentUnit, CurrentRange, value, out string formatted);
                CurrentPowerFormatted = formatted;
            }
        }

        public string CurrentPowerFormatted
        {
            get
            {
                return _current_power_formatted;
            }
            private set
            {
                _current_power_formatted = value;
                RaisePropertyChanged();
            }
        }

        public RangeEnum CurrentRange
        {
            get
            {
                return _current_range;
            }
            set
            {
                if (this.IsOpened)
                {
                    // set the range to the device.
                    try
                    {
                        lock (pmLocker)
                        {
                            pm.SetRange(value);
                            pm.GetRange(out RangeEnum range);
                            if (range == value)
                                _current_range = value;
                        }
                        

                    }
                    catch
                    {

                    }
                }
                else
                {
                    _current_range = value;

                }
                RaisePropertyChanged();
            }
        }

        public UnitEnum CurrentUnit
        {
            get
            {
                return _current_unit;
            }
            set
            {
                if (this.IsOpened)
                {
                    try
                    {
                        lock (pmLocker)
                        {
                            pm.SetUnit(value);
                            pm.GetUnit(out UnitEnum unit);
                            if (unit == value)
                                _current_unit = value;
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    _current_unit = value;
                }
                RaisePropertyChanged();
            }
        }

        public int CurrentWavelength
        {
            get
            {
                return _current_wav;
            }
            private set
            {
                _current_wav = value;
                RaisePropertyChanged();
            }
        }

        public CalibrationHelper CalibrationHelper
        {
            get
            {
                return _cal_helper;
            }
            private set
            {
                _cal_helper = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        private void OnPowerReadBack((double Power, UnitEnum unit) result)
        {
            this.CurrentPower = result.Power;
        }

        private void ReadInitStatus()
        {
            pm.GetRange(out RangeEnum range);
            this.CurrentRange = range;

            pm.GetUnit(out UnitEnum unit);
            this.CurrentUnit = unit;

            pm.GetWavelength(out int wav);
            this.CurrentWavelength = wav;

            try
            {
                pm.ReadCalParam(out CalibrationHelper calhelper);
                this.CalibrationHelper = calhelper;
            }
            catch(Exception)
            {

            }

        }

        private void StartToReadPower(IProgress<(double Power, UnitEnum Unit)> ProgressReporter, CancellationToken cancelToken)
        {
            Task.Run(() =>
            {
                while(true)
                {
                    if (cancelToken.IsCancellationRequested)
                    {
                        ProgressReporter.Report((double.NaN, this.CurrentUnit));
                        return;
                    }

                    try
                    {
                        lock (pmLocker)
                        {
                            pm.Read(out double power, out UnitEnum unit);
                            ProgressReporter.Report((power, unit));
                        }
                    }
                    catch(Exception)
                    {
                        Thread.Sleep(500);
                    }
                    finally
                    {
                        Thread.Sleep(2);
                    }

                }
            });
        }

        #endregion

        #region Commands

        public RelayCommand OpenPMCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (pm != null)
                        {
                            try
                            {
                                pm.Close();
                            }
                            catch
                            {

                            }
                        }

                        pm = new PM1906A(PortName, BaudRate);

                        pm.Open();

                        ReadInitStatus();

                        this.IsOpened = true;

                        progress = new Progress<(double Power, UnitEnum unit)>(OnPowerReadBack);

                        cts = new CancellationTokenSource();

                        StartToReadPower(progress, cts.Token);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public RelayCommand ClosePMCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (pm != null)
                        {
                            try
                            {
                                cts.Cancel();
                                lock (pmLocker)
                                {
                                    pm.Close();
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        pm = null;
                        this.IsOpened = false;

                        cts.Cancel();

                    }
                });
            }
        }

        public RelayCommand<string> SetWavCommand
        {
            get
            {
                return new RelayCommand<string>(wavstr =>
                {
                    try
                    {
                        lock (pmLocker)
                        {
                            var wav = Convert.ToInt32(wavstr);
                            pm.SetWavelength(wav);
                            pm.GetWavelength(out int chk);
                            if (wav == chk)
                                this.CurrentWavelength = wav;
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public RelayCommand ReloadCalParamsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    lock(pmLocker)
                    {
                        pm.ReadCalParam(out CalibrationHelper calhelper);
                        this.CalibrationHelper = calhelper;
                    }
                });
            }
        }

        public RelayCommand<CalibrationHelper> SetCalParamsCommand
        {
            get
            {
                return new RelayCommand<CalibrationHelper>(param =>
                {
                    try
                    {
                        lock(pmLocker)
                        {
                            // write AD back-noise
                            pm.SetADCBackgroundNoise(param.ADBackgroundNoise);
                            
                            // write Resistors
                            foreach (RangeEnum range in Enum.GetValues(typeof(RangeEnum)))
                                pm.SetSamplingResistance(range, param.Res[(int)range]);

                            // write functions
                            foreach (var fpwave in param.FuncsPerWavelength)
                            {
                                var wav = (WavelengthEnum)fpwave.Wavelength;
                                foreach (var fun in fpwave.Funcs)
                                {
                                    pm.SetFunc(wav, (RangeEnum)(fun.Range - 1), fun.A, fun.B, fun.C, fun.DC);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public RelayCommand SaveCalParamToFlashCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        lock(pmLocker)
                        {
                            pm.SaveCalParam();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public RelayCommand SetToDefault
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        lock (pmLocker)
                        {
                            pm.ResetCalParamToDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public RelayCommand ExportCalParamsToJsonCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        var json = this.CalibrationHelper.ToJson();

                        SaveFileDialog dlg = new SaveFileDialog();
                        dlg.Filter = "Json|*.json";
                        dlg.DefaultExt = ".json";
                        dlg.AddExtension = true;
                        var ret = dlg.ShowDialog();
                        if(ret.HasValue && ret.Value)
                        {
                            File.WriteAllText(dlg.FileName, json);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                });
            }
        }

        public RelayCommand ImportCalParamsFromJson
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.Filter = "Json|*.json";
                        dlg.Multiselect = false;
                        var ret = dlg.ShowDialog();
                        if(ret.HasValue && ret.Value)
                        {
                            try
                            {
                                var json = File.ReadAllText(dlg.FileName);
                                this.CalibrationHelper = CalibrationHelper.FromJson(json);
                            }
                            catch(Exception)
                            {
                                MessageBox.Show($"Unrecognized json file opened.");                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        #endregion
    }
}