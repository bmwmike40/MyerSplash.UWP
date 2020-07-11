using MyerSplash.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyerSplash.Model
{
    public class PresetSearchWord : ModelBase
    {
        private string _displayText;
        [JsonProperty("display_text")]
        public string DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    RaisePropertyChanged(() => DisplayText);
                }
            }
        }

        [JsonProperty("search_text")]
        public string SearchText { get; set; }
    }
}
