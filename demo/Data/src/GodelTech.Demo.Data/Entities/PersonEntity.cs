using System.Reflection.Metadata.Ecma335;
using GodelTech.Data.EntityFrameworkCore;

namespace GodelTech.Demo.Data.Entities
{
    public class PersonEntity : Entity<int>
    {
        public string Name { get; set; }
    }
}
