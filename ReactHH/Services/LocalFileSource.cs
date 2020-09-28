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

        public async Task<List<string[]>> GetSourceData()
        {
            return (await File.ReadAllLinesAsync(localFile.FullName))
                   .Select(line => line.Split('\t'))
                   .ToList();
        }

        public LocalFileSource(string path)
        {
            localFile = new FileInfo(path);
        }
    }
}
