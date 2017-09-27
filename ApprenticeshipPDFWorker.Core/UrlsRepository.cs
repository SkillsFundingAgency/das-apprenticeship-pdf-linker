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
                Console.WriteLine(item.StandardCode);
                Console.WriteLine(item.StandardUrl);
                Console.WriteLine(item.AssessmentUrl);
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
        // updateList is used later for creating a list of entries which need updating.
        List<Urls> updateList = new List<Urls>();

        // Entry method for class. If the database is empty, it gets populated. Otherwise, it checks for updates.
        public void Save(IEnumerable<Urls> linkUris)
        {
            if (IsDatabaseEmpty())
                PopulateEnptyDatabase(linkUris);
            else
                ProcessData(linkUris);
        }

        // Method to check for database entries. Probably a better way to do it, currently just counts the number of rows and checks for a zero result
        public bool IsDatabaseEmpty()
        {
            int count;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString))
            {
                string numberOfRows = "select count(*) from PdfTable";

                SqlCommand myCommand = new SqlCommand(numberOfRows, connection);
                myCommand.Connection.Open();
                count = (int)myCommand.ExecuteScalar();
                connection.Close();
            }
            if (count != 0)
                return false;
            else
                return true;
        }

        // Method to populate a database. It's only called if the database is empty.
        public void PopulateEnptyDatabase(IEnumerable<Urls> linkUris)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString);
            connection.Open();

            // The following 3 lines completely clears the database table. This is to reset the auto-incrementaion of the primary key to 1.
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

        // Method called if the database table has entries. It checks to see if updates are required and acts accordingly.
        public void ProcessData(IEnumerable<Urls> linkUris)
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
                // reader[2] refers to the Standard pdf url.
                databaseStandardUrlList.Add(reader[2].ToString());
                // reader[3] refers to the Assessment pdf url.
                databaseAssessmentUrlList.Add(reader[3].ToString());
            }

            // Compares the links stored in the database to those pulled from the csv. Any differences triggers a table update.
            if (CompareStandardUrls(databaseStandardUrlList, newDataList)|| CompareAssessmentUrls(databaseAssessmentUrlList, newDataList))
            {
                UpdateDatabase(newDataList);
            }
            connection.Close();
        }

        // Method to compare the Standard pdf urls from the database and the csv file.
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

        // Method to compare the Assessment pdf urls from the database and the csv file.
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

        // Method to update the database table if any links require it.
        public void UpdateDatabase(IEnumerable<Urls> linkUris)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString);
            connection.Open();

            // updateList is populated in the comparison methods, and so only the entries 
            // which are different between the csv file and the database are updated.
            foreach (var source in updateList)
            {
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
        
    }
}