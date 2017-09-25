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
            var newDataList = linkUris.AsList();
            var databaseLinksList = new List<string>();
            var linkList = new List<string>();
            var connection = new SqlConnection("Server=.\\SQLEXPRESS;Database=GovUkApprenticeships;Trusted_Connection=True;MultipleActiveResultSets=True;");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM TestTable";
            command.CommandType = CommandType.Text;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                databaseLinksList.Add(reader[1].ToString());
            }
            if (CompareStandardUrls(databaseLinksList, newDataList))
            {
                Console.WriteLine("Needs updating");
            }

            databaseLinksList.Clear();
            while (reader.Read())
            {
                databaseLinksList.Add(reader[2].ToString());
            }

            if (CompareAssessmentUrls(databaseLinksList, newDataList))
            {
                Console.WriteLine("Needs updating");
            }

            connection.Close();
        }

        public Boolean CompareStandardUrls(List<string> storedLink, IEnumerable<Urls> newLink)
        {
            Boolean needsUpdating = false;
            for (int i = 0; i < storedLink.Count; i++)
            {
                if (storedLink[i] != newLink.ElementAt(i).StandardUrl)
                {
                    needsUpdating = true;
                }
            }
            return needsUpdating;   
        }

        public Boolean CompareAssessmentUrls(List<string> storedLink, IEnumerable<Urls> newLink)
        {
            Boolean needsUpdating = false;
            for (int i = 0; i < storedLink.Count; i++)
            {
                if (storedLink[i] != newLink.ElementAt(i).StandardUrl)
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