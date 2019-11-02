using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Audit
{
    class Program
    {
        static void Main(string[] args)
        {
			const string url = "http://jsonplaceholder.typicode.com/photos";
			
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.ContentType = "application/json; charset=utf-8";
			
            HttpWebResponse response = request.GetResponseAsync().Result as HttpWebResponse;

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                string body = reader.ReadToEnd();
                List<Photo> results = JsonConvert.DeserializeObject<List<Photo>>(body);

                List<Photo> sortedResults = SortList(results);

                CreateCSVTextFile(sortedResults);

                TopTenWords(sortedResults);
            }

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
        }

        /// <summary>
        /// Sorts the list by the picture title.
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="data">Data.</param>
        public static List<Photo> SortList(List<Photo> data) {
            Console.WriteLine("Sorting the list by name...");
            List<Photo> SortedList = data.OrderBy(p => p.title).ToList();

            return SortedList;
        }

		/// <summary>
		/// Creates the CSV file on the users desktop, adding the headers and the sorted response from the API.
		/// </summary>
		/// <param name="data">Data.</param>
		public static void CreateCSVTextFile(List<Photo> data)
		{
            Console.WriteLine("Retrieving desktop path...");
            string pathToCSV = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			Console.WriteLine("Retrieving file properties...");
            var properties = typeof(Photo).GetProperties();
			var result = new StringBuilder();

            string[] arrayHeader = new string[] { "ID", "ALBUMID", "TITLE", "URL", "THUMBNAILURL" };

            Console.WriteLine("Adding the CSV headers...");
            var header = string.Join(",", arrayHeader);
			result.AppendLine(header);

            Console.WriteLine("Seperating values with a comma...");
			foreach (var row in data)
			{
				var values = properties.Select(p => p.GetValue(row, null));
				var line = string.Join(",", values);
				result.AppendLine(line);
			}

            Console.WriteLine("Saving CSV file...");
            File.WriteAllText(pathToCSV + "/results.csv", result.ToString());
		}

        /// <summary>
        /// Prints the top ten recurring words in a list.
        /// </summary>
        /// <param name="data">Data.</param>
        public static void TopTenWords(List<Photo> data) {
            Console.WriteLine("Retrieving the titles...");
            string[] titleList = data.Select(p => p.title).ToArray();
            string titles = string.Join("", titleList);

            Console.WriteLine("Tidying the titles for analysis...");
			//removing commas
            titles = titles.Replace(",", "");
			//removing full stops
            titles = titles.Replace(".", "");

            Console.WriteLine("Retrieving the words in the titles...");
			//creating an array of words by adding a space between each one
            string[] arr = titles.Split(' ');

			Dictionary<string, int> dictionary = new Dictionary<string, int>();

            Console.WriteLine("Looping through all the words...");
			foreach (string word in arr) //let's loop over the words
			{
				//check if word is at least 3 letters long
				if (word.Length >= 3) 
				{
					//check if it's in the dictionary
					if (dictionary.ContainsKey(word))
                    {
						//increment the count
						dictionary[word] = dictionary[word] + 1; 
                    }
                    else
                    {
						//add it to the dictionary with a count of 1
						dictionary[word] = 1; 
                    }
				}
			}

            Console.WriteLine("Sorting the dictionary by the number of occurances...");
            var ordered = dictionary.OrderByDescending(x => x.Value);

            Console.WriteLine("Retrieving top 10 occurances...");
			//loop through the dictionary
            foreach (KeyValuePair<string, int> pair in ordered.Take(10))
            { 
                Console.WriteLine("Word: {0}, Occurances: {1}", pair.Key, pair.Value);
            }
        } 

        /// <summary>
        /// Photo data structure.
        /// Used in combination with a List.
        /// Provides simple solutions.
        /// Easy to serialise.
        /// Provides a number of built in functions for sorting. 
        /// </summary>
		public class Photo
		{
			public int id { get; set; }
            public int albumId { get; set; }
			public string title { get; set; }
			public string url { get; set; }
			public string thumbnailUrl { get; set; }
		}
    }
}
