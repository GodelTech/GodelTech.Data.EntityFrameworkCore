using System;
using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Demo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Demo.Data
{
    public class DemoUnitOfWork : UnitOfWork, IDemoUnitOfWork
    {
        public DemoUnitOfWork(
            Func<DbContext, IRepository<PersonEntity, int>> personRepository,
            DbContextOptions dbContextOptions,
            string schemaName)
            : base(new DemoDbContext(dbContextOptions, schemaName))
        {
            RegisterRepository(personRepository(DbContext));
        }

        public IRepository<PersonEntity, int> PersonRepository => GetRepository<PersonEntity, int>();
    }
}
