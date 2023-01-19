using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    class WholeCountry
    {
        public List<GeoArea> GeoAreas { get; set; }

        public WholeCountry()
        {
            GeoAreas = new List<GeoArea>();
        }

        public void Parse(string html)
        {
            Regex regex = new Regex(@"id=(\d{5})'");
            MatchCollection matches = regex.Matches(html);

            foreach (Match match in matches)
            {
                GeoArea geoArea = new GeoArea(Int32.Parse(match.Groups[1].Value));
                GeoAreas.Add(geoArea);
            }
        }
    }
}
