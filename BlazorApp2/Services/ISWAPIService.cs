using BlazorApp2.Model;

namespace BlazorApp2.Services
{
    public interface ISWAPIService
    {
        public Task<List<Person>> GetPeopleList();
        public Task<List<Person>> FilterPeopleList(string userInput, string filterField);
    }
}
