
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

        public async Task<List<Person>> GetPeopleList()
        {
            HttpResponseMessage reponse = await _httpClient.GetAsync(_apiBaseUrl + "people");

            if (reponse.IsSuccessStatusCode)
            {
                string responseBody = await reponse.Content.ReadAsStringAsync();
                PersonWrapper person = JsonSerializer.Deserialize<PersonWrapper>(responseBody);
                return person.PersonList;
            }
            return new List<Person>();
        }

        public async Task<List<Person>> FilterPeopleList(string userInput, string filterField)
        {
            List<Person> person = await GetPeopleList();
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
                    return new List<Person>();
            }
        }
        
    }
}
