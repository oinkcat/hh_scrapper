using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReactHH.Services
{
    /// <summary>
    /// Vacancies data source accessible by inet
    /// </summary>
    public class WebDataSource : IVacanciesDataSource
    {
        private const int TimeoutSeconds = 3;

        private Uri dataAddress;

        public DateTime FetchDate => DateTime.Now.Date;

        public async Task<IList<string[]>> GetSourceData()
        {
            var client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(TimeoutSeconds)
            };

            return (await client.GetStringAsync(dataAddress))
                .Split('\n')
                .Select(line => line.Split('\t'))
                .ToList();
        }

        public WebDataSource(Uri address)
        {
            dataAddress = address;
        }
    }
}
