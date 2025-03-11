
using BlazorApp2.Model;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorApp2.Services
{
    public class SWAPIService : ISWAPIService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://swapi.dev/api/";

        public async Task<List<People>> GetPeopleList()
        {
            List<People> persons = new List<People>();

            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + "people");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                SWAPIpage pages = JsonSerializer.Deserialize<SWAPIpage>(responseBody);

                do
                {
                    PersonWrapper personWrapper = JsonSerializer.Deserialize<PersonWrapper>(responseBody);
                    persons.AddRange(personWrapper.PersonList);

                    if (!string.IsNullOrEmpty(pages.NextPage))
                    {
                        response = await _httpClient.GetAsync(pages.NextPage);
                        responseBody = await response.Content.ReadAsStringAsync(); 

                        if (response.IsSuccessStatusCode)
                        {
                            pages = JsonSerializer.Deserialize<SWAPIpage>(responseBody); 
                        }
                        else
                        {
                            break; 
                        }
                    }
                    else
                    {
                        break; 
                    }
                } while (pages.NextPage != null);
            }
            return persons;
        }

        public async Task<List<People>> FilterPeopleList(string userInput, string filterField)
        {
            List<People> person = await GetPeopleList();
            var trimmedUserInput = userInput?.Trim() ?? string.Empty;

            switch (filterField)
            {
                case "name":
                    return person
                        .Where(p => p.Name.IndexOf(trimmedUserInput, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                case "height":
                    return person
                        .Where(p => p.Height.IndexOf(trimmedUserInput, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                case "mass":
                    return person
                        .Where(p => p.Mass.IndexOf(trimmedUserInput, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                default:
                    return new List<People>();
            }
        }
        
    }
}
