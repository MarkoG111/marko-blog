using Application;

namespace API.Core
{
    public class AnonymousActor : IApplicationActor
    {
        public int Id => 0;
        public string Identity => "Unauthorized user";
        public IEnumerable<int> AllowedUseCases => new List<int> { 1, 5, 6, 12, 13, 23, 24, 28, 29 };
    }
}