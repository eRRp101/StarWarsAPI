using BlazorApp2.Model;
using System.Text.Json;

namespace BlazorApp2.Services
{
    public class SWAPIService : ISWAPIService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://swapi.dev/api/";

        public async Task<List<People>> GetPeopleList()
        {
            List<People> peopleList = new List<People>();
            string nextPageUrl = _apiBaseUrl + "people";  

            try
            {
                while (!string.IsNullOrEmpty(nextPageUrl))
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(nextPageUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        SWAPIpage page = JsonSerializer.Deserialize<SWAPIpage>(responseBody);
                        PersonWrapper personWrapper = JsonSerializer.Deserialize<PersonWrapper>(responseBody);

                        peopleList.AddRange(personWrapper.PersonList);
                        nextPageUrl = page.NextPage;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

            return peopleList;
        }

        public async Task<List<People>> FilterPeopleList(string nameFilter, string heightFilter, string massFilter)
        {
            List<People> person = await GetPeopleList();

            var trimmedNameFilter = nameFilter?.Trim().ToLower() ?? string.Empty;
            var trimmedHeightFilter = heightFilter?.Trim().ToLower() ?? string.Empty;
            var trimmedMassFilter = massFilter?.Trim().ToLower() ?? string.Empty;

            var filteredList = person.AsQueryable();

            if (!string.IsNullOrEmpty(trimmedNameFilter))
            {
                filteredList = filteredList.Where(p => p.Name.ToLower().Contains(trimmedNameFilter));
            }
            if (!string.IsNullOrEmpty(trimmedHeightFilter))
            {
                filteredList = filteredList.Where(p => p.Height.ToLower().Contains(trimmedHeightFilter));
            }
            if (!string.IsNullOrEmpty(trimmedMassFilter))
            {
                filteredList = filteredList.Where(p => p.Mass.ToLower().Contains(trimmedMassFilter));
            }
            return filteredList.ToList();
        }

    }

}

