using System.Linq;
using AutoMapper;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeDataMapper : IDataMapper
    {
        private readonly IMapper _mapper;

        public FakeDataMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IQueryable<TDestination> Map<TDestination>(IQueryable source)
        {
            return _mapper.ProjectTo<TDestination>(source);
        }
    }
}
