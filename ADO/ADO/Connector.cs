using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace PV_522_ADO
{
    class Connector
    {
        SqlConnection connection;
        public Connector(string connection_string)
        {
            connection = new SqlConnection(connection_string);
            Console.WriteLine(connection.ConnectionString);
        }


        //вставка данных в таблицу  ()
        public void Insert(string cmd, Dictionary<string, object> parameters)
        {
            SqlCommand command = new SqlCommand(cmd, connection);

            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }


        //вставка данных в таблицу Directors (или первоисточник) - gthtuheprf
        public void InsertDirector(string firstName, string lastName)
        {

            int directorId = GetDirectorId(firstName, lastName);
            if (directorId != -1)
            {
                Console.WriteLine("Такой режиссёр уже есть в базе.");
                return;
            }


            Insert(
                    "INSERT INTO Directors (first_name, last_name) VALUES (@first, @last)",
                    new Dictionary<string, object>
                    {
                                    { "@first", firstName },
                                    { "@last", lastName }
                    }
            );

        }

        //проверка есть ли уже такой режесер () вернет  ID
        public int GetDirectorId(string firstName, string lastName)
        {
            SqlCommand command = new SqlCommand(
                "SELECT director_id FROM Directors WHERE first_name=@first AND last_name=@last",
                connection);

            command.Parameters.AddWithValue("@first", firstName);
            command.Parameters.AddWithValue("@last", lastName);

            connection.Open();
            object result = command.ExecuteScalar();
            connection.Close();

            if (result == null) return -1;
            return Convert.ToInt32(result);
        }






        //вставка данных в таблицу Movies
        public void InsertMovie(string title, string releaseDate, string firstName, string lastName)
        {
            connection.Open();

            //ищем режисера 
            int directorId = GetDirectorId(firstName, lastName);


            //режесер не найден
            if (directorId == -1)
            {
                SqlCommand insertDirector = new SqlCommand(
                    "INSERT INTO Directors (first_name, last_name) VALUES (@first, @last); SELECT SCOPE_IDENTITY();",
                    connection);
                //SELECT SCOPE_IDENTITY(); - функция вернет ID последней вставленной записи в текущем соединении и текущем scope

                insertDirector.Parameters.AddWithValue("@first", firstName);
                insertDirector.Parameters.AddWithValue("@last", lastName);

                directorId = Convert.ToInt32(insertDirector.ExecuteScalar()); //ExecuteScalar() - вернет 1 строку, первого стобца последнего SELECT
            }
            else 
            { 
                //ПРОВЕРКА НА ДУБЛЬ ФИЛЬМА
            
            }


                SqlCommand insertMovie = new SqlCommand(
                    "INSERT INTO Movies (title, release_date, director) VALUES (@title, @date, @director)",
                    connection);

            insertMovie.Parameters.AddWithValue("@title", title);
            insertMovie.Parameters.AddWithValue("@date", releaseDate);
            insertMovie.Parameters.AddWithValue("@director", directorId);

            insertMovie.ExecuteNonQuery(); //вставляем в таблицу

            connection.Close();
        }







        public void Select(string cmd)
        {
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            //Определяем максимальный размер данных в каждом поле таблицы:
            int[] string_sizes = new int[reader.FieldCount];    //Этот массив хранит максимальные размеры строк для каждого поля.
            int interval = 15;
            //Вычисляем максимальные размеры строк:
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).ToString().Length > string_sizes[i])
                    string_sizes[i] = reader.GetName(i).ToString().Length + interval;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader[i].ToString().Length > string_sizes[i])
                        string_sizes[i] = reader[i].ToString().Length + interval;
                }
            }
            reader.Close();
            //Заново выполняем запрос, и выводим результаты запроса:
            reader = command.ExecuteReader();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i).PadRight(string_sizes[i])}");
                //if (reader.GetName(i).ToString().Length > string_sizes[i]) string_sizes[i] = reader.GetName(i).ToString().Length + 1;
            }
            Console.WriteLine();
            for (int i = 0; i < string_sizes.Sum(); i++) Console.Write("-"); Console.WriteLine();
            while (reader.Read())
            {
                //Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}");
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write(reader[i].ToString().PadRight(string_sizes[i]));
                Console.WriteLine();
            }
            reader.Close();
            connection.Close();
        }
        public void Select(string fields, string tables, string condition = "")
        {
            string cmd = $"SELECT {fields} FROM {tables} ";
            if (condition != "") cmd += $" WHERE {condition}";
            Select(cmd);
        }
        public object Scalar(string cmd)
        {
            object value = null;
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            value = command.ExecuteScalar();
            connection.Close();
            return value;
        }
    }
}