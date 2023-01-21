using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();

            Region belgorod = new Region(10501, 2021); 
            Region chuvash = new Region(10405, 2021);
            parser.ParseRegion(belgorod);
            parser.ParseRegion(chuvash);
            //Console.WriteLine(region.Name);

            //YearReport yearReport = new YearReport(2021);
            //parser.ParseYearReport(yearReport);

            File.WriteAllText("two_regions.json", string.Empty);
            File.AppendAllText("two_regions.json", JsonConvert.SerializeObject(belgorod));
            File.AppendAllText("two_regions.json", JsonConvert.SerializeObject(chuvash));

            //Console.ReadKey();
        }
    }
}
