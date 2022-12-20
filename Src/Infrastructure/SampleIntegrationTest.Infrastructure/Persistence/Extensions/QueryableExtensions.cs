using System;
using System.Linq;

namespace SampleIntegrationTest.Infrastructure.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TDestination> Apply<TSource, TDestination>(
            this IQueryable<TSource> source,
            Func<IQueryable<TSource>, IQueryable<TDestination>> builder)
        {
            return builder(source);
        }

    }
}
