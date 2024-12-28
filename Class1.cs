using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODZ4._1
{ //Для задания 1 с провайдерами
    //провайдеров классы, которые позволяют генерировать программный код доступа к различным источникам данных с минимальными затратами на кодирование.
    internal class Class1
    {
        public interface IDatabaseProvider
        {
            SqlConnection CreateConnection();
        }

        public class SqlServerProvider : IDatabaseProvider
        {
            private string _connectionString;

            public SqlServerProvider(string connectionString)
            {
                _connectionString = connectionString;
            }

            public SqlConnection CreateConnection()
            {
                return new SqlConnection(_connectionString);
            }
        }
        //Этот класс будет использован в основном коде для создания подключения.
    }
}

//Для задания 6 по смене СУБД читал что можно создать интерфейс для пользователя, позволяющий выбрать тип СУБД, четко определив классы-провайдеры для каждой из них. 
//Но не могу понять разницы
