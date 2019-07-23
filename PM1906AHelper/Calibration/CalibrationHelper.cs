using Newtonsoft.Json;
using PM1906AHelper.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PM1906AHelper.Calibration
{
    public class CalibrationHelper : NotifiableClassBase
    {
        #region Constructors

        public CalibrationHelper()
        {
            Properties = new ObservableCollection<Property>();
            Properties.Add(new Property() { Name = "ADBackgroundNoise" });
            Properties.Add(new Property() { Name = "PDDarkCurrent" });
            Properties.Add(new Property() { CollectionName = "Res" });
            Properties.Add(new Property() { CollectionName = "FuncsPerWavelength" });
           
        }

        #endregion Constructors

        #region Properties

        [Browsable(false)]
        [JsonIgnore]
        public ObservableCollection<Property> Properties { get; set; }

        [Category("Basic")]
        [DisplayName("AD Background Noise(V)")]
        public double ADBackgroundNoise { get; set; }

        [Category("Basic")]
        [DisplayName("Rsistance(ohm)")]
        public double[] Res { get; set; }

        [Category("Advanced")]
        [DisplayName("Equations")]
        public FuncPerWav[] FuncsPerWavelength { get; set; }

        [Category("Filter")]
        [DisplayName("FIR")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public FIRFilter FIR { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string RawJson { get; internal set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Convert the calibration helper object to json string.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Generate the instance of the Calibration Helper object.
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static CalibrationHelper FromJson(string Json)
        {
            var helper = JsonConvert.DeserializeObject<CalibrationHelper>(Json);
            helper.RawJson = Json;
            return helper;
        }

        #endregion
    }
}