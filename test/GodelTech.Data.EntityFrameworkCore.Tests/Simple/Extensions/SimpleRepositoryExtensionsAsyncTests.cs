using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Simple.Extensions
{
    public partial class SimpleRepositoryExtensionsAsyncTests
    {
        public static IEnumerable<object[]> TypesMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid)
                },
                // int
                new object[]
                {
                    default(int)
                },
                // string
                new object[]
                {
                    string.Empty
                }
            };
    }
}
