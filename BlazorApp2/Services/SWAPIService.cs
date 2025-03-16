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
            var peopleList = new List<People>();
            string nextPageUrl = _apiBaseUrl + "people";

            try
            {
                while (!string.IsNullOrEmpty(nextPageUrl))
                {
                    var response = await _httpClient.GetAsync(nextPageUrl);
                    response.EnsureSuccessStatusCode(); // Exception if no response

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var page = JsonSerializer.Deserialize<SWAPIpage>(responseBody);
                    var personWrapper = JsonSerializer.Deserialize<PersonWrapper>(responseBody);

                    if (personWrapper?.PersonList != null)
                    {
                        peopleList.AddRange(personWrapper.PersonList);
                    }

                    nextPageUrl = page?.NextPage;
                }

                await MapImagesToList(peopleList);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return peopleList;
        }

        //public async Task<List<People>> FilterPeopleList(List<People> peopleList, string nameFilter, string heightFilter, string massFilter)
        //{
        //    List<People> person = peopleList;

        //    var trimmedNameFilter = nameFilter?.Trim().ToLower() ?? string.Empty;
        //    var trimmedHeightFilter = heightFilter?.Trim().ToLower() ?? string.Empty;
        //    var trimmedMassFilter = massFilter?.Trim().ToLower() ?? string.Empty;

        //    var filteredList = person.AsQueryable();

        //    if (!string.IsNullOrEmpty(trimmedNameFilter))
        //    {
        //        filteredList = filteredList.Where(p => p.Name.ToLower().Contains(trimmedNameFilter));
        //    }
        //    if (!string.IsNullOrEmpty(trimmedHeightFilter))
        //    {
        //        filteredList = filteredList.Where(p => p.Height.ToLower().Contains(trimmedHeightFilter));
        //    }
        //    if (!string.IsNullOrEmpty(trimmedMassFilter))
        //    {
        //        filteredList = filteredList.Where(p => p.Mass.ToLower().Contains(trimmedMassFilter));
        //    }
        //    return filteredList.ToList();
        //}

        public async Task<List<People>> SearchPeopleList(List<People> peopleList, string name)
        {
            List<People> person = peopleList;

            var searchName = name.Trim().ToLower() ?? string.Empty;
            var filteredList = person.AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                filteredList = filteredList.Where(p => p.Name.ToLower().Contains(searchName));
            }
            return filteredList.ToList();
        }

        private async Task MapImagesToList(List<People> peopleList)
        {
            string imageFolderPath = Path.Combine("wwwroot", "Images", "StarWarsCharacters");
            string placeholderImagePath = $"/Images/StarWarsCharacters/placeholder.png";

            foreach (var person in peopleList)
            {
                var normalizedPersonName = person.Name.ToLower().Replace(" ", "-");
                var imageFile = Directory.GetFiles(imageFolderPath)
                    .FirstOrDefault(file =>
                    {
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file).ToLower().Replace(" ", "-");
                        return fileNameWithoutExtension == normalizedPersonName;
                    });

                if (imageFile != null)
                {
                    person.ImgSrc = $"/Images/StarWarsCharacters/{Path.GetFileName(imageFile)}";
                }
                else
                {
                    person.ImgSrc = placeholderImagePath;
                }
            }
        }
    }
}

