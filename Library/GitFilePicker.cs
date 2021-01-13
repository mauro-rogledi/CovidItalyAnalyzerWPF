using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WPFCovidItalyAnalizer.Library
{
    public static class GitFilePicker
    {
        static readonly HttpClient httpClient = new HttpClient();
        public static async Task<string> GetFilesAsync(string file)
        {
            var url = $"https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/{file}";
            return await GetDataFromGitAsync(url);
        }

        private static async Task<string> GetDataFromGitAsync(string url)
        {
            return await httpClient.GetStringAsync(url);
        }
    }
}
