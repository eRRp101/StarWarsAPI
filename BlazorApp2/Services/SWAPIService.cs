
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

