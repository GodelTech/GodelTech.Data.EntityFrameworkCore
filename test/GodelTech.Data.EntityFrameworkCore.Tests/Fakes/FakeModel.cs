namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeModel<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }
    }
}