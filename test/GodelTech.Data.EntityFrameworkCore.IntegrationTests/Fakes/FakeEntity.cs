namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeEntity<TKey> : Entity<TKey>
    {
        public string Name { get; set; }
    }
}