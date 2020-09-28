using System;
using System.Collections.Generic;

namespace ReactHH.Models
{
    public class ListViewModel
    {
        public IList<Vacancy> Vacancies { get; set; }

        public bool IsFallback { get; set; }

        public ListViewModel(IList<Vacancy> vacancyList, bool isFallback = false)
        {
            Vacancies = vacancyList;
            IsFallback = isFallback;
        }
    }
}
