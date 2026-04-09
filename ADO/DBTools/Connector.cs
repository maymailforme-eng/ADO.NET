using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DBTools
{
    public class Connector
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

            int directorId = -1;

            //ищем режесера
            SqlCommand findDirector = new SqlCommand(
                "SELECT director_id FROM Directors WHERE first_name=@first AND last_name=@last",
                connection);

            findDirector.Parameters.AddWithValue("@first", firstName);
            findDirector.Parameters.AddWithValue("@last", lastName);

            object result = findDirector.ExecuteScalar();



            //режисер не найден
            if (result == null)
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
                directorId = Convert.ToInt32(result);


                //ищем фильм
                SqlCommand findMovie = new SqlCommand(
                     "SELECT movie_id FROM Movies WHERE title=@title AND release_date=@release_date AND director = @directorId",
                     connection);

                findMovie.Parameters.AddWithValue("@title", title);
                findMovie.Parameters.AddWithValue("@release_date", releaseDate);
                findMovie.Parameters.AddWithValue("@directorId", directorId);

                object resultMovies = findMovie.ExecuteScalar();

                //если такой фильм уже есть
                if (resultMovies != null)
                {
                    Console.WriteLine("Такой фильм уже есть в базе.");
                    connection.Close();
                    return;
                }

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







        public DataTable Select(string cmd)
        {
            DataTable table = new DataTable();

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
                table.Columns.Add(reader.GetName(i));

                Console.Write($"{reader.GetName(i).PadRight(string_sizes[i])}");
                //if (reader.GetName(i).ToString().Length > string_sizes[i]) string_sizes[i] = reader.GetName(i).ToString().Length + 1;
            }
            Console.WriteLine();
            for (int i = 0; i < string_sizes.Sum(); i++) Console.Write("-"); Console.WriteLine();
            while (reader.Read())
            {

                DataRow row = table.NewRow();
                //Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}");
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i].ToString().PadRight(string_sizes[i]));
                    row[i] = reader[i];
  
                }
                Console.WriteLine();

                table.Rows.Add(row);

            }


            reader.Close();
            connection.Close();

            return table;
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




        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////\
        ///  секция UPDATE
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////\


        public void Update(string cmd)
        {
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Update(string table, string fields, string values, string condition)
        {
            string[] s_fields = fields.Split(',');
            string[] s_values = values.Split(',');
            if (s_fields.Length != s_values.Length) return;
            string parsed = " ";
            for (int i = 0; i < s_fields.Length; i++)
            {
                parsed += $"{s_fields[i]}={ParseValue(s_values[i])}";
                if (i != s_values.Length - 1) parsed += ",";
            }
            string cmd = $"UPDATE {table} SET {parsed} WHERE {condition}";
            Update(cmd);
        }
        string ParseValue(string value)
        {
            if (value.Length > 1)
            {
                if (value[0] != 'N' && value[1] != '\'') value = $"N'{value}'";
                /*
				-------------------------
				\ (Backslash) - символ отмены специального значения другого символа.
				-------------------------
				*/
            }
            return value;
        }






    }




}

