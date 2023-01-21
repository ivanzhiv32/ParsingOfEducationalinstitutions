using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ParsingOfEducationalinstitutions
{
    class DataBase
    {
        MySqlConnection connection = new MySqlConnection("server=s2.kts.tu-bryansk.ru;port=3306;username=IAS18.ZHivII;password=3q%Md=Q2/4;database=IAS18_ZHivII");
        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            Console.WriteLine("Connection is opened");
        }
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
            Console.WriteLine("Connection is closed");
        }
        public MySqlConnection GetConnection()
        {
            return connection;
        }
        public List<string> getLinksReady()
        {
            if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

            MySqlCommand command = new MySqlCommand("SELECT link FROM parser_institution_report_year union SELECT link FROM parser_region_report_year union SELECT link FROM parser_years_reports", 
                connection);

            var reader = command.ExecuteReader();

            List<string> linksReady = new List<string>();
            while (reader.Read())
            {
                if (reader[0].ToString() == "") continue;
                linksReady.Add(reader[0].ToString());
            }

            return linksReady;
        }
        
        public void AddRegion(Region region)
        {
            if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

            MySqlCommand command = new MySqlCommand(String.Format("insert into `parser_region`(`name`) values('{0}')", region.Name), connection);
            command.ExecuteNonQuery();

        }
        public void AddRegionReport(Region region)
        {
            if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

            AddRegion(region);

            foreach (var institution in region.Institutions)
            {
                AddInstitutionReport(institution);
            }

            //MySqlCommand command = new MySqlCommand(String.Format("insert into `parser_region_report_year`(`year`,`count_all_students`,`count_fulltime_students`,`count_freeform_students`) values({0}, {1}, {2}, {3})",
            //    yearReport.Year, yearReport.CountAllStudents, yearReport.CountFullTimeStudents, yearReport.CountFreeFormStudents), connection);
            //command.ExecuteNonQuery();
            //Добавить ссылку на отчет региона, когда он будет полностью готов
        }
        public void AddInstitution(Institution institution)
        {
            if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

            //добавление данных с внешним ключем
            //MySqlCommand command = new MySqlCommand(String.Format("insert into `parser_years_reports`(`year`,`count_all_students`,`count_fulltime_students`,`count_freeform_students`) values({0}, {1}, {2}, {3})",
            //    yearReport.Year, yearReport.CountAllStudents, yearReport.CountFullTimeStudents, yearReport.CountFreeFormStudents), connection);
            //command.ExecuteNonQuery();
        }
        public void AddInstitutionReport(Institution institution)
        {
            if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

            AddInstitution(institution);

            //Добавление имен и значений индикаторов
            //Добавление отчета
            //Добавить ссылку на отчет института, когда он будет полностью готов
        }
        public void AddYear(YearReport yearReport)
        {
            if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

            MySqlCommand command = new MySqlCommand(String.Format("insert into `parser_years_reports`(`year`,`count_all_students`,`count_fulltime_students`,`count_freeform_students`) values({0}, {1}, {2}, {3})",
                yearReport.Year, yearReport.CountAllStudents, yearReport.CountFullTimeStudents, yearReport.CountFreeFormStudents), connection);
            command.ExecuteNonQuery();
        }
        public void AddYearReport(YearReport yearReport)
        {
            AddYear(yearReport);

            foreach (var region in yearReport.Regions)
            {
                AddRegionReport(region);
            }
            
            //Добавить ссылку на годовой отчет, когда он будет полностью готов (update)
        }
        public void AddNameIndicator(Indicator indicator)
        {

        }
        public void AddUnitsMeasure(string unitMeasure)
        {

        }
    }
}
