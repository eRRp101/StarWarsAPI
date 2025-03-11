
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
        private readonly string _pagesBaseUrl = "https://swapi.dev/api/people/?page=1";

        public async Task<List<People>> GetPeopleList()
        {
            List<People> persons = new List<People>();

            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + "people");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                using (_httpClient.GetAsync(_pagesBaseUrl))
                {
                    SWAPIpage pages = JsonSerializer.Deserialize<SWAPIpage>(responseBody);

                    while (pages.NextPage != null)
                    {
                        response = await _httpClient.GetAsync(pages.NextPage);
                        if (response.IsSuccessStatusCode)
                        {
                            pages = JsonSerializer.Deserialize<SWAPIpage>(responseBody);

                            PersonWrapper personWrapper = JsonSerializer.Deserialize<PersonWrapper>(responseBody);
                            persons.AddRange(personWrapper.PersonList);

                            responseBody = await response.Content.ReadAsStringAsync();
                            pages = JsonSerializer.Deserialize<SWAPIpage>(responseBody);
                        }
                    }
                }
                return persons;
            }
            return new List<People>();

            //if (peopleReponse.IsSuccessStatusCode)
            //{
            //    string responseBody = await peopleReponse.Content.ReadAsStringAsync();
            //    PersonWrapper person = JsonSerializer.Deserialize<PersonWrapper>(responseBody);
            //    return person.PersonList;
            //}
            //return new List<Person>();
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
