using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using ApprenticeshipPDFWorker.Core.Models;
using Dapper;
using Newtonsoft.Json;

namespace ApprenticeshipPDFWorker.Core
{

    public class UrlsRepository : IUrlsRepository
    {
        public void Save(IEnumerable<Urls> linkUris)
        {
            foreach (var item in linkUris)
            {
                System.Console.WriteLine(item.StandardCode);
                System.Console.WriteLine(item.StandardUrl);
                System.Console.WriteLine(item.AssessmentUrl);
            }
        }
    }

    public class FileRepostiory : IUrlsRepository
    {
        public void Save(IEnumerable<Urls> linkUris)
        {
            var json = JsonConvert.SerializeObject(linkUris);
            File.WriteAllText("output.txt", json);
        }
    }

    public class DatabaseRepository : IUrlsRepository {

        // kinda poor method to enter items into the DB
       /* public void Save2(IEnumerable<Urls> linkUris)
        {
            var connection = new SqlConnection("Server=.\\SQLEXPRESS;Database=GovUkApprenticeships;Trusted_Connection=True;");
            connection.Open();
            //connection.Execute("INSERT INTO Test2 (Value) VALUES (@Value)")
            foreach (var source in linkUris.Take(5))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO Test2 (Value) VALUES (@Value)";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("Value", source.StandardUrl);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }*/

        // Method to read from DB and CW all entries
        public void DisplayData(IEnumerable<Urls> linkUris)
        {
            //int incrementer = 0;
            var poop = linkUris.AsList();
            var blah = new List<string>();
            var linkList = new List<string>();
            //for (int i = 0; i < linkUris.AsList().Count; i++)
            //{
            //    linkList.Add(linkUris.ElementAt(i).StandardUrl);
            //}
            var connection = new SqlConnection("Server=.\\SQLEXPRESS;Database=GovUkApprenticeships;Trusted_Connection=True;MultipleActiveResultSets=True;");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM TestTable";
            command.CommandType = CommandType.Text;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string foo = reader[1].ToString();
                blah.Add(foo);
                
                
                
                /*
                string foo = reader[1].ToString();
                string poo = linkUris.ElementAt(incrementer).StandardUrl.ToString();
                 if (poo != foo)
                 {
                     var blah = connection.CreateCommand();
                     blah.CommandType = CommandType.Text;
                     blah.CommandText = //doesn't work
                         "UPDATE TestTable SET [StandardUrl] = @StandardUrl WHERE StandardId = @StandardId ";
                     blah.Parameters.AddWithValue("@StandardUrl", poo);
                     blah.Parameters.AddWithValue("@StandardId", linkUris.ElementAt(incrementer).StandardCode);
                     blah.Connection = connection;
                     blah.ExecuteNonQuery();
                    
                 }
                
                if (reader[2].ToString() != linkUris.ElementAt(incrementer).AssessmentUrl.ToString())
                {
                    SqlCommand blah = new SqlCommand();
                    blah.CommandType = CommandType.Text;
                    blah.CommandText = //probably doesn't work
                        "UPDATE TestTable SET [AssessmentUrl] = @AssessmentUrl WHERE StandardId = @StandardId ";
                    blah.Parameters.AddWithValue("@AssessmentUrl", linkUris.ElementAt(incrementer).AssessmentUrl);
                    blah.Parameters.AddWithValue("@StandardId", linkUris.ElementAt(incrementer).StandardCode);
                    blah.Connection = connection;
                    blah.ExecuteNonQuery();
                    blah.Connection.Close();
                }
                incrementer += 1;
                
             // Console.WriteLine(reader[0]);
             //Console.WriteLine(reader[1]);
             //Console.WriteLine(reader[2]);
             */
            }
            if (CompareData(blah, linkUris))
            {
                Console.WriteLine("Needs updating");
            }
            else
            {
                Console.WriteLine("doesn't need updating");
            }
            connection.Close();
        }

        public Boolean CompareData(List<string> blah, IEnumerable<Urls> linkUris)
        {
            Boolean needsUpdating = false;
            for (int i = 0; i < blah.Count; i++)
            {
                if (blah[i] != linkUris.ElementAt(i).StandardUrl)
                {
                    needsUpdating = true;
                }
            }
            return needsUpdating;   
        }

            //method enters standard code, url and assment url into db
            //IMPORTANT! Will only need calling to setup db, shouldn't be called once data is populated.
        public void Save(IEnumerable<Urls> linkUris)
        {
            /* This comment block contains the code to originally populate the database. Should probably put some conditional
               code in place to check if the db has been populated, and run the correct code.
             
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString);
            connection.Open();
            foreach (var source in linkUris)
            {
                connection.Execute("INSERT INTO TestTable (StandardID, StandardUrl, AssessmentUrl) VALUES (@StandardCode, @StandardUrl, @AssessmentUrl)", source);
            }
            connection.Close();*/
            
            DisplayData(linkUris);
        }


        /* Code for changing a row:
         * UPDATE tableName
         * SET column='newData'
         * WHERE 'conditon';
         * 
         * For example, once confirmed that the url needs updating
         * UPDATE TestTable
         * SET StandardUrl='source.StandardUrl'
         * WHERE StandardId='source.StandardId';
         */
    }
}