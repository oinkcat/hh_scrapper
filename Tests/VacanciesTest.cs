using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Xunit;
using Moq;
using ReactHH.Models;
using ReactHH.Services;

namespace Tests
{
    public class VacanciesTest
    {
        const string AppContentRoot = @"../../../../ReactHH";
        readonly string DataFilePath = @$"{AppContentRoot}/Data/vacancies.txt";

        private string localCacheFilePath;

        /// <summary>
        /// Test loading CSV data from cache file
        /// </summary>
        [Fact]
        public async void TestLoadingFromCache()
        {
            var cacheSource = new LocalFileSource(localCacheFilePath);

            var vacanciesCsv = await cacheSource.GetSourceData();

            Assert.NotEmpty(vacanciesCsv);
        }

        /// <summary>
        /// Test loading and writing CSV data to cache
        /// </summary>
        [Fact]
        public async void TestCacheReadAndWrite()
        {
            var cacheSource = new LocalFileSource(localCacheFilePath);

            var vacanciesCsv = await cacheSource.GetSourceData();
            int nVacanciesOnFirstLoad = vacanciesCsv.Count;

            await cacheSource.UpdateCache(vacanciesCsv);

            int nVacanciesOnSecondLoad = (await cacheSource.GetSourceData()).Count;

            Assert.Equal(nVacanciesOnFirstLoad, nVacanciesOnSecondLoad);
        }

        /// <summary>
        /// Test loading vacancies from file
        /// </summary>
        [Fact]
        public async void TestVacanciesLoading()
        {
            var envMock = new Mock<IWebHostEnvironment>();
            envMock.SetupGet(env => env.ContentRootPath).Returns(AppContentRoot);

            var optionsMock = new Mock<IOptions<SourcesConfig>>();

            var configValues = new SourcesConfig {
                DataListUrl = "http://192.168.100.50/vacancies.txt",
                LocalCacheFileName = "Data/vacancies.txt"
            };
            optionsMock.SetupGet(opt => opt.Value).Returns(configValues);

            var loader = new VacanciesDataLoader(envMock.Object, optionsMock.Object);
            var vacancies = await loader.LoadAsync();

            Assert.NotEmpty(vacancies);
            Assert.NotEmpty(vacancies[0].Tags);
        }

        public VacanciesTest()
        {
            localCacheFilePath = $"{Environment.CurrentDirectory}/{DataFilePath}";
        }
    }
}
