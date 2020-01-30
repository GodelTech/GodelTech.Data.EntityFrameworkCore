using AutoMapper.QueryableExtensions;
using System.Linq;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeDataMapper : IDataMapper
    {
        public IQueryable<TDestination> Map<TDestination>(IQueryable source)
        {
            return source.ProjectTo<TDestination>();
        }
    }
}
