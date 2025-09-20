using StarWarsAPI.Model;

namespace StarWarsAPI.Services
{
    public interface ISwApiService
    {
        public Task<List<People>> GetPeopleList();
        public Task<List<People>> FilterPeopleList(List<People> peopleList, string nameFilter, string heightFilter, string massFilter);
    }
}
