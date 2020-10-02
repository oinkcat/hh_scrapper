using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private VacanciesDataLoader dataLoader;

        /// <summary>
        /// Load and return list of available vacancies
        /// </summary>
        [HttpGet("list")]
        public async Task<ListViewModel> GetVacancies()
        {
            var vacancies = await dataLoader.LoadAsync();
            return new ListViewModel(vacancies, dataLoader.IsOffline);
        }

        public VacanciesDataController(VacanciesDataLoader dataLoader)
        {
            this.dataLoader = dataLoader;
        }
    }
}