using StarWarsAPI.Model;
using System.Text.Json;

namespace StarWarsAPI.Services
{
    public class SwApiService : ISwApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://swapi.dev/api/";
        private readonly ApiExceptionService _apiExceptionService;

        public SwApiService(ApiExceptionService apiExceptionService)
        {
            _apiExceptionService = apiExceptionService;
        }

        public async Task<List<People>> GetPeopleList()
        {
            var peopleList = new List<People>();
            string nextPageUrl = _apiBaseUrl + "people";
            try
            {
                do
                {
                    var response = await _httpClient.GetAsync(nextPageUrl);
                    response.EnsureSuccessStatusCode(); // Throws HttpRequestException on failure

                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var peopleWrapper = await JsonSerializer.DeserializeAsync<PeopleWrapper>(responseStream);

                    if (peopleWrapper?.Results != null)
                    {
                        foreach (var person in peopleWrapper.Results)
                        {
                            
                            if (!string.IsNullOrEmpty(person.HomeworldUrl))
                            {
                                person.Planet = await GetPlanetAsync(person.HomeworldUrl);
                            }

                            if (person.SpeciesUrls != null && person.SpeciesUrls.Any())
                            {

                                person.Species = await GetSpeciesAsync(person.SpeciesUrls);
                            }
                            else
                            {
                                //For some reason, only for humans, MOST contain empty SpeciesUrls. Some don't. 
                                //Swapi Documentation claims Luke Skywalker contains species URL - but postman or hoppscotch confirm he doesn't.
                                person.Species = new List<Species> { new Species { Name = "Human" } };
                            }
                            peopleList.Add(person);
                        }
                    }

                    nextPageUrl = peopleWrapper?.NextPage;
                }
                while (!string.IsNullOrEmpty(nextPageUrl));

                await MapImagesToList(peopleList);

            }
            catch (HttpRequestException ex)
            {
                _apiExceptionService.LogException(ex, "HTTP request failed");
                throw new ApiServiceException("Failed to connect to the API. Please check the endpoint or your internet connection.", ex);
            }
            catch (JsonException ex)
            {
                _apiExceptionService.LogException(ex, "JSON deserialization failed");
                throw new ApiServiceException("Failed to process the API response. The data may be invalid.", ex);
            }
            catch (Exception ex)
            {
                _apiExceptionService.LogException(ex, "Unexpected error");
                throw new ApiServiceException("An unexpected error occurred. Please try again later.", ex);
            }

            return peopleList;
        }

        private async Task<Planet> GetPlanetAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Planet>(json);
        }

        private async Task<List<Species>> GetSpeciesAsync(List<string> urls)
        {
            var speciesList = new List<Species>();

            foreach (var url in urls)
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                speciesList.Add(JsonSerializer.Deserialize<Species>(json));

            }
            return speciesList;
        }

        public async Task<List<People>> FilterPeopleList(List<People> peopleList, string nameFilter, string speciesFilter, string planetFilter)
        {
            List<People> person = peopleList;

            var trimmedNameFilter = nameFilter?.Trim().ToLower() ?? string.Empty;
            var trimmedSpeciesFilter = speciesFilter?.Trim().ToLower() ?? string.Empty;
            var trimmedPlanetFilter = planetFilter?.Trim().ToLower() ?? string.Empty;

            var filteredList = person.AsQueryable();

            if (!string.IsNullOrEmpty(trimmedNameFilter))
            {
                filteredList = filteredList.Where(p => p.Name.ToLower().Contains(trimmedNameFilter));
            }

            if (!string.IsNullOrEmpty(trimmedSpeciesFilter))
            {
                filteredList = filteredList.Where(p => p.Species != null && p.Species.Any(s => s.Name != null && s.Name.ToLower().Contains(trimmedSpeciesFilter)));
            }

            if (!string.IsNullOrEmpty(trimmedPlanetFilter))
            {
                filteredList = filteredList.Where(p => p.Planet != null && p.Planet.Name != null && p.Planet.Name.ToLower().Contains(trimmedPlanetFilter));
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

