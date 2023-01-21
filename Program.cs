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

            //Region region = new Region(10303, 2022);
            //parser.ParseRegion(region);
            //Console.WriteLine(region.Name);

            YearReport yearReport = new YearReport(2021);
            parser.ParseYearReport(yearReport);

            File.WriteAllText("test.json", string.Empty);
            File.AppendAllText("test.json", JsonConvert.SerializeObject(yearReport));

            Console.ReadKey();
        }
    }
}
