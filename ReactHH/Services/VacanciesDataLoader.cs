using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ReactHH.Models;

namespace ReactHH.Services
{
    /// <summary>
    /// Loads vacancies list from text file
    /// </summary>
    public class VacanciesDataLoader
    {
        private IVacanciesDataSource dataSource;

        /// <summary>
        /// Load vacancies from source file
        /// </summary>
        /// <returns>List of loaded vacancies</returns>
        public async Task<List<Vacancy>> LoadAsync()
        {
            var csvInfo = await dataSource.GetSourceData();

            var loadedVacancies = new List<Vacancy>();
            var fetchDate = dataSource.FetchDate;

            while(csvInfo.Count > 0)
            {
                var vacancyInfo = Vacancy.CreateFromCsv(csvInfo, fetchDate);

                if(vacancyInfo != null)
                {
                    loadedVacancies.Add(vacancyInfo);
                }
                else
                {
                    break;
                }
            }

            return loadedVacancies;
        }

        public VacanciesDataLoader(string path)
        {
            dataSource = new LocalFileSource(path);
        }

        public VacanciesDataLoader(Uri address)
        {
            dataSource = new WebDataSource(address);
        }
    }
}
