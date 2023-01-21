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

    }
}
