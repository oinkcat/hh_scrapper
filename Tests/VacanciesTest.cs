using System;
using Xunit;
using ReactHH.Services;

namespace Tests
{
    public class VacanciesTest
    {
        const string DataFilePath = @"C:\Users\softc\source\repos\" +
                                    @"ReactHH\ReactHH\Data\vacancies.txt";

        /// <summary>
        /// Test loading from file
        /// </summary>
        [Fact]
        public async void TestVacancyLoading()
        {
            var loader = new VacanciesDataLoader(DataFilePath);
            var vacancies = await loader.LoadAsync();

            Assert.NotEmpty(vacancies);
        }
    }
}
