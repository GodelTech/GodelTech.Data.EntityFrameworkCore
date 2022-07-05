namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeEntity<TKey> : Entity<TKey>
    {
        public string Name { get; set; }
    }
}
