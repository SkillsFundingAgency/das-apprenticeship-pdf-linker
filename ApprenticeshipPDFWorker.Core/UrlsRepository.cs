using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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
            command.CommandText = "SELECT * FROM PdfTable";
            command.CommandType = CommandType.Text;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                databaseStandardUrlList.Add(reader[2].ToString());
                databaseAssessmentUrlList.Add(reader[3].ToString());
            }
            if (CompareStandardUrls(databaseStandardUrlList, newDataList)|| CompareAssessmentUrls(databaseAssessmentUrlList, newDataList))
            {
                UpdateDatabase(newDataList);
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
                if (storedLink[i] != newLink.ElementAt(i).AssessmentUrl)
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
                    command.CommandText = "UPDATE PdfTable SET StandardUrl=@stdUrl, AssessmentUrl = @assUrl, DateSeen = @dateSeen WHERE StandardId =@stdId";
                    command.Parameters.AddWithValue("@stdUrl", source.StandardUrl);
                    command.Parameters.AddWithValue("@assUrl", source.AssessmentUrl);
                    command.Parameters.AddWithValue("@dateseen", DateTime.Now);
                    command.Parameters.AddWithValue("@stdId", source.StandardCode);

                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
        public void Save(IEnumerable<Urls> linkUris)
        {
            if (IsDatabaseEmpty())
                PopulateEnptyDatabase(linkUris);
            else
                DisplayData(linkUris);
        }

        public void PopulateEnptyDatabase(IEnumerable<Urls> linkUris)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString);
            connection.Open();
            string sqlTrunc = "TRUNCATE TABLE " + "PdfTable";
            SqlCommand cmd = new SqlCommand(sqlTrunc, connection);
            cmd.ExecuteNonQuery();
            foreach (var source in linkUris)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO PdfTable (StandardId, StandardUrl, AssessmentUrl, DateSeen) VALUES (@stdCode, @stdUrl, @assUrl, @dateSeen)";
                    command.Parameters.AddWithValue("@stdCode", source.StandardCode);
                    command.Parameters.AddWithValue("@stdUrl", source.StandardUrl);
                    command.Parameters.AddWithValue("@assUrl", source.AssessmentUrl);
                    command.Parameters.AddWithValue("@dateSeen", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }

        public bool IsDatabaseEmpty()
        {
            int count;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString))
            {
                string numberOfRows = "select count(*) from PdfTable";

                SqlCommand myCommand = new SqlCommand(numberOfRows, connection);
                myCommand.Connection.Open();
                count = (int) myCommand.ExecuteScalar();
                connection.Close();
            }
            if (count != 0)
                return false;
            else
                return true;
        }
    }
}