using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ReactHH.Models;
using ReactHH.Services;

namespace ReactHH.Controllers
{
    /// <summary>
    /// Controller for actions with vacancies
    /// </summary>
    [ApiController]
    [Route("api/vacancies")]
    public class VacanciesDataController : Controller
    {
        const string WebSourceAddress = "http://192.168.100.50/vacancies.txt";

        private IWebHostEnvironment env;

        private const string FallbackFilePath = "/Data/vacancies.txt";

        private string FallbackLocalFilePath => env.ContentRootPath + FallbackFilePath;

        /// <summary>
        /// Load and return list of available vacancies
        /// </summary>
        [HttpGet("list")]
        public async Task<ListViewModel> GetVacancies()
        {
            var loader = CreateDataLoader(true);

            try
            {
                return new ListViewModel(await loader.LoadAsync());
            }
            catch
            {
                var fallbackLoader = CreateDataLoader(false);
                return new ListViewModel(await fallbackLoader.LoadAsync(), true);
            }
        }

        // Create remote or local data loader
        private VacanciesDataLoader CreateDataLoader(bool remote)
        {
            if(remote)
            {
                var webSourceUri = new Uri(WebSourceAddress);
                return new VacanciesDataLoader(webSourceUri);
            }
            else
            {
                return new VacanciesDataLoader(FallbackLocalFilePath);
            }
        }

        public VacanciesDataController(IWebHostEnvironment env)
        {
            this.env = env;
        }
    }
}