using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactHH.Services
{
    /// <summary>
    /// Represents vacancies table data source
    /// </summary>
    internal interface IVacanciesDataSource
    {
        /// <summary>
        /// Date when list was last fetched
        /// </summary>
        DateTime FetchDate { get; }

        /// <summary>
        /// Retreive vacancies list
        /// </summary>
        /// <returns>Table data</returns>
        Task<List<string[]>> GetSourceData();
    }
}
