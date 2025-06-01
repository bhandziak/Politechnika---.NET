using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GutenbergProject
{
    public class WebClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<string> GetPageContentAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode(); // Rzuci wyjątek, jeśli status nie jest 2xx

                string content = await response.Content.ReadAsStringAsync();

                return content;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Błąd podczas pobierania strony: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
