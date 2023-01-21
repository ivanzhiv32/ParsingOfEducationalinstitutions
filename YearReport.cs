using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    class YearReport
    {
        
        public int Year { get; set; }
        public int CountAllStudents { get; set; }
        public int CountFullTimeStudents { get; set; }
        public int CountFreeFormStudents { get; set; }
        public List<Region> Regions = new List<Region>();

        public YearReport(int year)
        {
            if (year > 2022 || year < 2015) throw new Exception("Веб-ресурс не содержит данные за выбранную дату");
            Year = year;
        }
    }
}
