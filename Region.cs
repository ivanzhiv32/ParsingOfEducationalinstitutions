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
    class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountAllStudents { get; set; }
        public int CountFullTimeStudents { get; set; }
        public int CountFreeFormStudents { get; set; }
        [JsonIgnore]
        public int Year { get; set; }

        public List<Institution> Institutions = new List<Institution>();

        public Region(int id, int year)
        {
            Id = id;
            Year = year;
        }
    }
}
