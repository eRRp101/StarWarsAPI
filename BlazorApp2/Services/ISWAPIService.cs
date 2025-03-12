using BlazorApp2.Model;

namespace BlazorApp2.Services
{
    public interface ISWAPIService
    {
        public Task<List<People>> GetPeopleList();
        public Task<List<People>> FilterPeopleList(string nameFilter, string heightFilter, string massFilter);
    }
}
