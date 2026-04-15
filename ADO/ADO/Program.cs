using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;

//ADO.NET
//ADO - ActiveX Data Objects
//.NET
//System.Data
//System.Data.SqlClient

namespace PV_522_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string connection_string = ConfigurationManager.ConnectionStrings["Movies"].ConnectionString;
            Connector connector = new Connector(connection_string);

            //connector.Select("SELECT * FROM Directors");
            //connector.Select("title,first_name,last_name", "Movies,Directors", "director=director_id");





            ////вставка в Directors
            //connector.InsertDirector("Quentin", "Tarantino");

            ////вставка в Movies
            //connector.InsertMovie("Pulp Fiction", "1994", "Quentin", "Tarantino");




        }


    }
}