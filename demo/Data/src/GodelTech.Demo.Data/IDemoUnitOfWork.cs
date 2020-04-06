using GodelTech.Data;
using GodelTech.Demo.Data.Entities;

namespace GodelTech.Demo.Data
{
    public interface IDemoUnitOfWork : IUnitOfWork
    {
        IRepository<PersonEntity, int> PersonRepository { get; }
    }
}
