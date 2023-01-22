using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ParsingOfEducationalinstitutions
{
    class DataBase
    {
        string connection_params = "server=s2.kts.tu-bryansk.ru;port=3306;username=IAS18.ZHivII;password=3q%Md=Q2/4;database=IAS18_ZHivII";
        //public void OpenConnection()
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed)
        //        connection.Open();
        //    Console.WriteLine("Connection is opened");
        //}
        //public void CloseConnection()
        //{
        //    if (connection.State == System.Data.ConnectionState.Open)
        //        connection.Close();
        //    Console.WriteLine("Connection is closed");
        //}
        //public MySqlConnection GetConnection()
        //{
        //    return connection;
        //}
        //public List<string> getLinksReady()
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand("SELECT link FROM parser_institution_report_year union SELECT link FROM parser_region_report_year union SELECT link FROM parser_years_reports", 
        //        connection);

        //    var reader = command.ExecuteReader();

        //    List<string> linksReady = new List<string>();
        //    while (reader.Read())
        //    {
        //        if (reader[0].ToString() == "") continue;
        //        linksReady.Add(reader[0].ToString());
        //    }

        //    return linksReady;
        //}
        public List<string> GetLinksReady()
        {
            //if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT link FROM parser_links_ready_data", connection); 

            var reader = command.ExecuteReader();

            List<string> linksReady = new List<string>();
            while (reader.Read())
            {
                if (reader[0].ToString() == "") continue;
                linksReady.Add(reader[0].ToString());
            }

            reader.Close();
            connection.Close();

            return linksReady;
        }

        public int AddYearReport(YearReport yearReport) //Готовый
        {
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_year", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_year", yearReport.Year));
            command.Parameters.Add(new MySqlParameter("_count_all_students", yearReport.CountAllStudents));
            command.Parameters.Add(new MySqlParameter("_count_fulltime_students", yearReport.CountFullTimeStudents));
            command.Parameters.Add(new MySqlParameter("_count_freeform_students", yearReport.CountFreeFormStudents));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();

            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }
        //public int CallProcedure(string nameProcedure, Dictionary<string, int> parameters)
        //{
        //    MySqlCommand command = new MySqlCommand(nameProcedure, connection);
        //    command.CommandType = CommandType.StoredProcedure;
        //    foreach (string key in parameters.Keys)
        //    {
        //        command.Parameters.Add(new MySqlParameter(key, parameters[key]));
        //    }
        //    command.Parameters["?out_id"].Direction = ParameterDirection.Output;

        //    command.ExecuteNonQuery();
        //    int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
        //    return id;
        //}
        //public int GetIdYearReport(int year)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM parser_years_reports WHERE year = {0}", year), connection);

        //    var id_year = Convert.ToInt32(command.ExecuteScalar());

        //    return id_year;
        //}
        //public void AddYear(YearReport yearReport)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("INSERT INTO parser_years_reports (year, count_all_students, count_fulltime_students, count_freeform_students) VALUES ({0}, {1}, {2}, {3})",
        //        yearReport.Year, yearReport.CountAllStudents, yearReport.CountFullTimeStudents, yearReport.CountFreeFormStudents), connection);
        //    command.ExecuteNonQuery();
        //}
        public int AddRegion(Region region) //Готовый
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_region", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_name", region.Name));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }

        //public int GetIdRegion(string name)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM parser_region WHERE name = '{0}'", name), connection);
        //    var id_year = Convert.ToInt32(command.ExecuteScalar());

        //    return id_year;
        //}
        //public void AddRegion(Region region)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("INSERT INTO parser_region (name) VALUES ('{0}')", region.Name), connection);
        //    command.ExecuteNonQuery();

        //}
        public int AddRegionReport(Region region, int idRegion, int idYearReport) //Готовый
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_region_report", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_id_region", idRegion));
            command.Parameters.Add(new MySqlParameter("_id_year", idYearReport));
            command.Parameters.Add(new MySqlParameter("_count_all_students", region.CountAllStudents));
            command.Parameters.Add(new MySqlParameter("_count_fulltime_students", region.CountFullTimeStudents));
            command.Parameters.Add(new MySqlParameter("_count_freeform_students", region.CountFreeFormStudents));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }
        //public int GetIdRegionReport(int idRegion, int idYearReport)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM parser_region_report_year WHERE id_region = {0} AND id_year = {1}", idRegion, idYearReport), connection);
        //    var idRegionReport = Convert.ToInt32(command.ExecuteScalar());

        //    return idRegionReport;
        //}
        //public void AddRegionReport(Region region, int idRegion, int idYearReport)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("INSERT INTO parser_region_report_year (id_region, id_year, count_all_students, count_fulltime_students, count_freeform_students) " +
        //        "VALUES ({0}, {1}, {2}, {3}, {4})", idRegion, idYearReport, region.CountAllStudents, region.CountFullTimeStudents, region.CountFreeFormStudents), connection);
        //    command.ExecuteNonQuery();

        //}

        public int AddInstitution(Institution institution, int idRegion) //Готовый
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_institution", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_link", institution.Site));
            command.Parameters.Add(new MySqlParameter("_name", institution.Name));
            command.Parameters.Add(new MySqlParameter("_adress", institution.Adress));
            command.Parameters.Add(new MySqlParameter("_founder", institution.Founder));
            command.Parameters.Add(new MySqlParameter("_department", institution.Department));
            command.Parameters.Add(new MySqlParameter("_id_region", idRegion));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }
        //public int GetIdInstitution(string name)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM parser_institution WHERE name = '{0}'", name), connection);
        //    var idInstitution = Convert.ToInt32(command.ExecuteScalar());

        //    return idInstitution;
        //}
        //public void AddInstitution(Institution institution, int IdRegion)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("INSERT INTO parser_institution (name, adress, founder, department, id_region) " +
        //        "VALUES ('{0}', '{1}', '{2}', '{3}', {4})", institution.Name, institution.Adress, institution.Founder, institution.Department, IdRegion), connection);
        //    command.ExecuteNonQuery();
        //}
        public int AddInstitutionReport(int idInstitution, int idYearReport) //Готовый
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_institution_report", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_id_institution", idInstitution));
            command.Parameters.Add(new MySqlParameter("_id_year", idYearReport));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }
        public int AddUnitMeasure(string name) //Готовый
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_unit_measure", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_name", name));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }

        //public int GetIdInstitutionReport(int idInstitution, int idYearReport)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM parser_institution_report_year WHERE id_institution = {0} AND id_year = {1}", 
        //        idInstitution, idYearReport), connection);
        //    var idInstitutionReport = Convert.ToInt32(command.ExecuteScalar());

        //    return idInstitutionReport;
        //}
        //public void AddInstitutionReport(int idInstitution, int idYearReport)
        //{
        //    if (connection.State == System.Data.ConnectionState.Closed) throw new Exception("Подкючение к базе данных закрыто");

        //    MySqlCommand command = new MySqlCommand(String.Format("INSERT INTO parser_institution_report_year (id_institution, id_year ) " +
        //        "VALUES ({0}, {1})", idInstitution, idYearReport), connection);
        //    command.ExecuteNonQuery();
        //}

        public int AddNameIndicator(string name, double number, int idUnitMeasure) //Готовый
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_name_indicator", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_name", name));
            command.Parameters.Add(new MySqlParameter("_number", number));
            command.Parameters.Add(new MySqlParameter("_id_unit_measure", idUnitMeasure));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }
        public int AddValueIndicator(int idInstitutionReport, int idNameIndicator, double value) //
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_value_indicator", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new MySqlParameter("_id_institution_report_year", idInstitutionReport));
            command.Parameters.Add(new MySqlParameter("_id_name_indicator", idNameIndicator));
            command.Parameters.Add(new MySqlParameter("_value", value));
            command.Parameters.Add(new MySqlParameter("out_id", 0));
            command.Parameters["?out_id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            connection.Close();

            int id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            return id;
        }

        public void AddLinkReady(string link)
        {
            Thread.Sleep(1000);
            MySqlConnection connection = new MySqlConnection(connection_params);
            connection.Open();

            MySqlCommand command = new MySqlCommand("add_link_ready", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new MySqlParameter("_link", link));

            command.ExecuteNonQuery();
            connection.Close();
        }
        public void AddLinksReady(List<string> links)
        {
            foreach (var link in links)
            {
                Thread.Sleep(500);
                MySqlConnection connection = new MySqlConnection(connection_params);
                connection.Open();

                MySqlCommand command = new MySqlCommand("add_link_ready", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("_link", link));

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
