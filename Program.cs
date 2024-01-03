using MySql.Data.MySqlClient;
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
            //Запуск полного функционала парсера
            //Parser parser = new Parser();
            //parser.Start();

            //Запуск парсера отзывов
            Parser parser = new Parser();
            parser.ParseReviews();

            //DataBase db = new DataBase();

            //YearReport yearReport = new YearReport(2021);
            //yearReport.CountFullTimeStudents = 2000;
            //yearReport.CountAllStudents = 10000;
            //yearReport.CountFreeFormStudents = 5000;

            //int id_year = db.AddYearReport(yearReport);

            //Region region = new Region(22, 2021);
            //region.Name = "Bryansk obl";
            //int id_region = db.AddRegion(region);

            //region.CountAllStudents = 1000;
            //region.CountFullTimeStudents = 400;
            //region.CountFreeFormStudents = 500;

            //int idRegionReport = db.AddRegionReport(region, id_region, id_year);

            //Institution institution = new Institution(33, 2021);
            //institution.Site = "ekg.ru";
            //institution.Name = "BSTU";
            //institution.Adress = "ul. Kuibisheva";
            //institution.Department = "MO";
            //institution.Founder = "MO";

            //int idInstitution = db.AddInstitution(institution, id_region);

            //int idInstitutionReport = db.AddInstitutionReport(idInstitution, id_year);
            //int idUnitMeasure = db.AddUnitMeasure("тыс.");
            //int idNameIndicator = db.AddNameIndicator("Количество студентов", 1.1, idUnitMeasure);
            //int idValueIndicator = db.AddValueIndicator(idInstitutionReport, idNameIndicator, 1000);
            //List<string> links = new List<string>() { "vk.com", "ok.ru", "https://monitoring.miccedu.ru/?m=vpo&year=2021" };
            //db.AddLinksReady(links);

            //Проверка парсера
            //var parser = new Parser();

            //Region belgorod = new Region(10501, 2021);
            //Region chuvash = new Region(10405, 2021);
            //parser.ParseRegion(belgorod, 1);
            //parser.ParseRegion(chuvash);
            //Console.WriteLine(region.Name);

            //YearReport yearReport = new YearReport(2021);
            //parser.ParseYearReport(yearReport);

            //File.WriteAllText("two_regions.json", string.Empty);
            //File.AppendAllText("two_regions.json", JsonConvert.SerializeObject(belgorod));
            //File.AppendAllText("two_regions.json", JsonConvert.SerializeObject(chuvash));


            Console.ReadKey();
        }
    }
}
