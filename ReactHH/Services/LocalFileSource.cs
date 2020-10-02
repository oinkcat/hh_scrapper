using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ReactHH.Services
{
    /// <summary>
    /// Data source from local file
    /// </summary>
    public class LocalFileSource : IVacanciesDataSource
    {
        private FileInfo localFile;

        public DateTime FetchDate => localFile.CreationTime.Date;

        public async Task<IList<string[]>> GetSourceData()
        {
            return (await File.ReadAllLinesAsync(localFile.FullName))
                   .Select(line => line.Split('\t'))
                   .ToList();
        }

        /// <summary>
        /// Update local data cache
        /// </summary>
        /// <param name="dataCsv">Data to cache</param>
        public async Task UpdateCache(IList<string[]> dataCsv)
        {
            using var cacheFile = File.CreateText(localFile.FullName);
            
            foreach(var csvLine in dataCsv)
            {
                await cacheFile.WriteLineAsync(String.Join('\t', csvLine));
            }
        }

        public LocalFileSource(string path)
        {
            localFile = new FileInfo(path);
        }
    }
}
