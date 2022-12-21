using SampleIntegrationTest.SharedKernel.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleIntegrationTest.SharedKernel.Shared
{
    public interface IRepository<TEntity, Tkey> where TEntity : Entity<Tkey>, IAggregateRoot
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> GetAsync(Tkey id, CancellationToken cancellationToken);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : Entity<Guid>, IAggregateRoot
    {

    }
}
