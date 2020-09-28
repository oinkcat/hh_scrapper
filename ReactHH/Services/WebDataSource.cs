using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReactHH.Services
{
    /// <summary>
    /// Vacancies data source accessible by inet
    /// </summary>
    public class WebDataSource : IVacanciesDataSource
    {
        private Uri dataAddress;

        public DateTime FetchDate => DateTime.Now.Date;

        public async Task<List<string[]>> GetSourceData()
        {
            var client = new WebClient();

            return (await client.DownloadStringTaskAsync(dataAddress))
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
