using Autofac;
using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Demo.Data.Entities;

namespace GodelTech.Demo.Data
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Repository<PersonEntity, int>>().As<IRepository<PersonEntity, int>>().InstancePerLifetimeScope();
        }
    }
}
