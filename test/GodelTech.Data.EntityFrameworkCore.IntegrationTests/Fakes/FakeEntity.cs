using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeEntity<TKey> : Entity<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override TKey Id { get; set; }

        public string Name { get; set; }
    }
}
