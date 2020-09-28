using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactHH.Models
{
    /// <summary>
    /// Vacancy information
    /// </summary>
    public class Vacancy
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Information page URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Vacany title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Date of first ocurrence
        /// </summary>
        public DateTime FirstFoundDate { get; set; }

        /// <summary>
        /// Latest ocurrence date
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Salary
        /// </summary>
        public string Salary { get; set; }

        /// <summary>
        /// Additional tags
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Is vacancy still active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Create vacancy information from data from table data
        /// </summary>
        /// <param name="csvData">Data loaded from CSV file</param>
        /// <param name="date">Date when info was loaded</param>
        /// <returns>Vacancy info</returns>
        public static Vacancy CreateFromCsv(IList<string[]> csvData, DateTime date)
        {
            if (csvData == null || csvData.Count <= 0)
                return null;

            var infoRow = csvData.First();
            if (infoRow.Length <= 1)
                return null;

            var vacancy = new Vacancy(int.Parse(infoRow[0]), infoRow[1], infoRow[4])
            {
                CompanyName = infoRow[5],
                FirstFoundDate = DateTime.Parse(infoRow[2]),
                LastUpdateDate = DateTime.Parse(infoRow[3]),
                Salary = infoRow[7]
            };

            vacancy.IsActive = date <= vacancy.LastUpdateDate;

            string[] info = null;

            do
            {
                info = csvData[0];
                vacancy.Tags.Add(info[8]);
                csvData.RemoveAt(0);
            }
            while (csvData.Count > 0 && csvData.First()[0].Equals(infoRow[0]));

            return vacancy;
        }

        /// <summary>
        /// Get string representation of vacancy info
        /// </summary>
        /// <returns>Vacancy title and company name</returns>
        public override string ToString()
        {
            return $"{Title} - {CompanyName}";
        }

        public Vacancy(int id, string url, string name)
        {
            Id = id;
            Url = url;
            Title = name;
            Tags = new List<string>();
        }
    }
}
