using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using ReactHH.Models;

namespace ReactHH.Services
{
    /// <summary>
    /// Loads vacancies list from text file
    /// </summary>
    public class VacanciesDataLoader
    {
        private IWebHostEnvironment env;

        private IOptions<SourcesConfig> sourceOpts;

        private LocalFileSource cacheSource;

        /// <summary>
        /// Is currently offline
        /// </summary>
        public bool IsOffline { get; private set; }

        /// <summary>
        /// Last data loaded time
        /// </summary>
        public DateTime LastFetchDate { get; private set; }

        /// <summary>
        /// Load vacancies from source file
        /// </summary>
        /// <returns>List of loaded vacancies</returns>
        public async Task<IList<Vacancy>> LoadAsync()
        {
            var vacanciesCsvData = await LoadVacanciesCsv();

            if(!IsOffline)
            {
                await cacheSource.UpdateCache(vacanciesCsvData);
            }

            return ParseVacanciesCsv(vacanciesCsvData);
        }

        // Load raw vacancies data
        private async Task<IList<string[]>> LoadVacanciesCsv()
        {
            var vacanciesCsv = new List<string[]>();

            try
            {
                var listUri = new Uri(sourceOpts.Value.DataListUrl);
                var remoteSource = new WebDataSource(listUri);
                vacanciesCsv.AddRange(await remoteSource.GetSourceData());
                LastFetchDate = remoteSource.FetchDate;
            }
            catch
            {
                vacanciesCsv.AddRange(await cacheSource.GetSourceData());
                LastFetchDate = cacheSource.FetchDate;
                IsOffline = true;
            }

            return vacanciesCsv;
        }

        // Parse vacancy data fields
        private IList<Vacancy> ParseVacanciesCsv(IList<string[]> vacanciesCsv)
        {
            var vacancies = new List<Vacancy>();

            while (vacanciesCsv.Count > 0)
            {
                var vacancyInfo = Vacancy.CreateFromCsv(vacanciesCsv, LastFetchDate);

                if (vacancyInfo != null)
                {
                    vacancies.Add(vacancyInfo);
                }
                else
                {
                    break;
                }
            }

            return vacancies;
        }

        public VacanciesDataLoader (
            IWebHostEnvironment env,
            IOptions<SourcesConfig> sourceOpts
        ) {
            this.env = env;
            this.sourceOpts = sourceOpts;

            string cacheFileName = env.ContentRootPath
                                 + '/'
                                 + sourceOpts.Value.LocalCacheFileName;
            cacheSource = new LocalFileSource(cacheFileName);
        }
    }
}
