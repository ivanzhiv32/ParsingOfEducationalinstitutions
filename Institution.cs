using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    
    class Institution
    {
        [JsonProperty("id_institution")]
        public int Id { get; set; }

        [JsonProperty("name_institution")]
        public string Name { get; set; }

        [JsonProperty("adress_institution")]
        public string Adress { get; set; }

        [JsonProperty("department_institution")]
        public string Department { get; set; }

        [JsonProperty("site_institution")]
        public string Site { get; set; }

        [JsonProperty("founder_institution")]
        public string Founder { get; set; }

        [JsonProperty("indicators_institution")]
        public List<Indicator> Indicators = new List<Indicator>();
        [JsonIgnore]
        public int Year { get; set; }

        public Institution(int id, int year)
        {
            Id = id;
            Year = year;
        }
    }
}
