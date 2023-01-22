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
            Parser parser = new Parser();
            parser.Start();

            //SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            //Console.WriteLine("CTRL+C,CTRL+BREAK or suppress the application to exit");
            //while (!isclosing);

            //AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            //List<string> linksReady = new List<string>() { "https://monitoring.miccedu.ru/iam/2021/_vpo/inst.php?id=24", "https://monitoring.miccedu.ru/iam/2021/_vpo/inst.php?id=25" };
            //var parser = new Parser(linksReady);
            //Region region = new Region(10303, 2021);
            //parser.ParseRegion(region);

            //DataBase db = new DataBase();
            //MySqlConnection connection = db.GetConnection();
            //db.OpenConnection();

            //YearReport yearReport = new YearReport(2021);
            //yearReport.CountFullTimeStudents = 2000;
            //yearReport.CountAllStudents = 10000;
            //yearReport.CountFreeFormStudents = 5000;
            //int id_year = db.AddYearReport(yearReport);

            ////MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM parser_years_reports WHERE year = {0}", yearReport.Year), connection);
            //var id_year = db.GetIdYearReport(yearReport.Year);
            //if (id_year == 0)
            //{
            //    db.AddYear(yearReport);
            //    id_year = db.GetIdYearReport(yearReport.Year);
            //}
            //else Console.WriteLine("Данные года уже содержатся");

            //Dictionary<string, int> parameters = new Dictionary<string, int>();
            //parameters.Add("_year", yearReport.Year);
            //parameters.Add("_count_all_students", yearReport.CountAllStudents);
            //parameters.Add("_count_fulltime_students", yearReport.CountFullTimeStudents);
            //parameters.Add("_count_freeform_students", yearReport.CountFreeFormStudents);
            //parameters.Add("out_id", 0);

            //int id_year = db.AddYearReport(yearReport);

            //Region region = new Region(22, 2021);
            //region.Name = "Bryansk obl";
            ////int id_region = db.GetIdRegion(region.Name);
            ////if(id_region == 0)
            ////{
            ////    db.AddRegion(region);
            ////    id_region = db.GetIdRegion(region.Name);
            ////}
            ////else Console.WriteLine("Данные региона уже содержатся");
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
            //int idRegionReport = db.GetIdRegionReport(id_region, id_year);
            //if(idRegionReport == 0)
            //{
            //    db.AddRegionReport(region, id_region, id_year);
            //}
            //else Console.WriteLine("Данные отчета региона уже содержатся");

            //db.CloseConnection();

            //var parser = new Parser(db.getLinksReady());
            //Region region = new Region(10303, 2021);
            //parser.ParseRegion(region);

            //MySqlCommand command = new MySqlCommand(String.Format("insert into `parser_institution`(`link`,`name`,`adress`,`founder`,`department`,`id_region`) values('{0}', '{1}', '{2}', '{3}', '{4}', {5})",
            //    institution.Site, institution.Name, institution.Adress, institution.Founder, institution.Department, 1), connection);

            //MySqlCommand command = new MySqlCommand("SELECT link FROM parser_institution", connection);
            //var reader = command.ExecuteReader();

            //while (reader.Read())
            //{
            //    // элементы массива [] - это значения столбцов из запроса SELECT
            //    Console.WriteLine(reader[0].ToString());
            //}
            //db.CloseConnection();

            //Проверка парсера
            //var parser = new Parser();

            //Region belgorod = new Region(10501, 2021); 
            //Region chuvash = new Region(10405, 2021);
            //parser.ParseRegion(belgorod);
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
