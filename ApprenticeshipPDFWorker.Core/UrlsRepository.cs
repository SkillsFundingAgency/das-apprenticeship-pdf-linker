using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

    public class DatabaseRepository : IUrlsRepository
    {
        List<Urls> updateList = new List<Urls>();
        public void DisplayData(IEnumerable<Urls> linkUris)
        {
            var newDataList = linkUris.AsList();
            var databaseStandardUrlList = new List<string>();
            var databaseAssessmentUrlList = new List<string>();
            var connection = new SqlConnection("Server=.\\SQLEXPRESS;Database=GovUkApprenticeships;Trusted_Connection=True;MultipleActiveResultSets=True;");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM TestTable";
            command.CommandType = CommandType.Text;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                databaseStandardUrlList.Add(reader[1].ToString());
                databaseAssessmentUrlList.Add(reader[2].ToString());
            }
            if (CompareStandardUrls(databaseStandardUrlList, newDataList)|| CompareAssessmentUrls(databaseAssessmentUrlList, newDataList))
            {
                Console.WriteLine("Needs updating");
                UpdateDatabase(newDataList);
            }
            else
            {
                Console.WriteLine("doesn't need updating");
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
                    updateList.Add(newLink.ElementAt(i));
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
                    updateList.Add(newLink.ElementAt(i));
                }
            }
            return needsUpdating;
        }

        public void UpdateDatabase(IEnumerable<Urls> linkUris)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString);
            connection.Open();
            foreach (var source in updateList)    
            {
                int standardCode = Convert.ToInt32(source.StandardCode);
                //the update code doesn't current;y work, will fix tomorrow
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE TestTable SET StandardUrl=@stdUrl, AssessmentUrl = @assUrl WHERE StandardId =@stdId";
                    command.Parameters.AddWithValue("@stdUrl", source.StandardUrl);
                    command.Parameters.AddWithValue("@assUrl", source.AssessmentUrl);
                    command.Parameters.AddWithValue("@stdId", source.StandardCode);

                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
        public void Save(IEnumerable<Urls> linkUris)
        {
            //This comment block contains the code to originally populate the database. Should probably put some conditional
            //  code in place to check if the db has been populated, and run the correct code.

            //var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString);
            //connection.Open();
            //foreach (var source in linkUris)
            //{
            //    connection.Execute("INSERT INTO TestTable (StandardID, StandardUrl, AssessmentUrl) VALUES (@StandardCode, @StandardUrl, @AssessmentUrl)", source);
            //}
            //connection.Close();

            DisplayData(linkUris);
        }
    }
}