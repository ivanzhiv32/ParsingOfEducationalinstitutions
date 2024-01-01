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
        string connection_params = "server=127.0.0.1;port=3306;username=root;password=dgiva4444;database=new_schema";
        //string connection_params = "server=s2.kts.tu-bryansk.ru;port=3306;username=IAS18.ZHivII;password=3q%Md=Q2/4;database=IAS18_ZHivII";

        public List<string> GetLinksReady()
        {
            List<string> linksReady = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("SELECT link FROM links_ready", connection)) //links_ready_data
            {
                connection.Open();

                var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    if (reader[0].ToString() == "") continue;
                    linksReady.Add(reader[0].ToString());
                }

                reader.Close();
                connection.Close();
            }

            return linksReady;
        }
        public int AddYearReport(YearReport yearReport)
        {
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_year", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_year", yearReport.Year));
                command.Parameters.Add(new MySqlParameter("_count_all_students", yearReport.CountAllStudents));
                command.Parameters.Add(new MySqlParameter("_count_fulltime_students", yearReport.CountFullTimeStudents));
                command.Parameters.Add(new MySqlParameter("_count_freeform_students", yearReport.CountFreeFormStudents));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddRegion(Region region)
        {
            Thread.Sleep(5000);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_region", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_name", region.Name));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddRegionReport(Region region, int idRegion, int idYearReport)
        {
            Thread.Sleep(5000);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_region_report", connection))
            {
                connection.Open();

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

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddInstitution(Institution institution, int idRegion)
        {
            Thread.Sleep(5000);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_institution", connection))
            {
                connection.Open();

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

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddInstitutionReport(int idInstitution, int idYearReport)
        {
            Thread.Sleep(1000);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_institution_report", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_id_institution", idInstitution));
                command.Parameters.Add(new MySqlParameter("_id_year", idYearReport));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddUnitMeasure(string name)
        {
            Thread.Sleep(100);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_unit_measure", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_name", name));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddNameIndicator(string name, string number, int idUnitMeasure) //Готовый
        {
            Thread.Sleep(100);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_name_indicator", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_name", name));
                command.Parameters.Add(new MySqlParameter("_number", number));
                command.Parameters.Add(new MySqlParameter("_id_unit_measure", idUnitMeasure));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }
            
            return id;
        }
        public int AddValueIndicator(int idInstitutionReport, int idNameIndicator, double value) 
        {
            Thread.Sleep(100);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_value_indicator", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_id_institution_report_year", idInstitutionReport));
                command.Parameters.Add(new MySqlParameter("_id_name_indicator", idNameIndicator));
                command.Parameters.Add(new MySqlParameter("_value", value));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }
            
            return id;
        }
        public int AddUgn(string name)
        {
            Thread.Sleep(100);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_ugn", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_name", name));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddDistributionUgn(int idInstitution, int idUgn, int countStudents)
        {
            Thread.Sleep(2000);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_distribution_ugn", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_id_institution", idInstitution));
                command.Parameters.Add(new MySqlParameter("_id_ugn", idUgn));
                command.Parameters.Add(new MySqlParameter("_count_students", countStudents));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }

        public int AddBranchScience(string name)
        {
            Thread.Sleep(100);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_branch_science", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_name", name));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public int AddDistributionBranch(int idInstitutionReport, int idBranch, int countStudents)
        {
            Thread.Sleep(2000);
            int id;
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_distribution_branches", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_id_institution_report", idInstitutionReport));
                command.Parameters.Add(new MySqlParameter("_id_branch", idBranch));
                command.Parameters.Add(new MySqlParameter("_count_students", countStudents));
                command.Parameters.Add(new MySqlParameter("out_id", 0));
                command.Parameters["?out_id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                connection.Close();

                id = Convert.ToInt32(command.Parameters["?out_id"].Value);
            }

            return id;
        }
        public void AddLinkReady(string link)
        {
            //Thread.Sleep(5000);
            using (MySqlConnection connection = new MySqlConnection(connection_params))
            using (MySqlCommand command = new MySqlCommand("add_link_ready", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("_link", link));

                command.ExecuteNonQuery();
                connection.Close();
            }            
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
