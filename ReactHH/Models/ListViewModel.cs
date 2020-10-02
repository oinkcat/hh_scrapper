using System;
using System.Collections.Generic;

namespace ReactHH.Models
{
    /// <summary>
    /// All displayed vacancies
    /// </summary>
    public class ListViewModel
    {
        /// <summary>
        /// Vacancies to display
        /// </summary>
        public IList<Vacancy> Vacancies { get; set; }

        /// <summary>
        /// Is currently offline
        /// </summary>
        public bool IsOffline { get; set; }

        public ListViewModel(IList<Vacancy> vacancyList, bool isOffline = false)
        {
            Vacancies = vacancyList;
            IsOffline = isOffline;
        }
    }
}
