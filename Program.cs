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
            List<string> linksReady = new List<string>() { "https://monitoring.miccedu.ru/iam/2021/_vpo/inst.php?id=24", "https://monitoring.miccedu.ru/iam/2021/_vpo/inst.php?id=25" };
            var parser = new Parser(linksReady);
            Region region = new Region(10303, 2021);
            parser.ParseRegion(region);

            //DataBase db = new DataBase();
            //MySqlConnection connection = db.GetConnection();
            //db.OpenConnection();
            //MySqlCommand command = new MySqlCommand(String.Format("insert into `parser_institution`(`link`,`name`,`adress`,`founder`,`department`,`id_region`) values('{0}', '{1}', '{2}', '{3}', '{4}', {5})", 
            //    institution.Site, institution.Name, institution.Adress, institution.Founder, institution.Department, 1), connection);
            //command.ExecuteNonQuery();
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
