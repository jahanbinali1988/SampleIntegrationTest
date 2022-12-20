using Sample.SharedKernel.EventProcessing.DomainEvent;
using Sample.SharedKernel.SeedWork;

namespace SampleIntegrationTest.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SampleDbContext _dbContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        public UnitOfWork(SampleDbContext dbContext,
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            _dbContext = dbContext;
            _domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(CancellationToken.None))
            {
                try
                {
                    var result = await _dbContext.SaveChangesAsync(cancellationToken);
                    await _domainEventsDispatcher.DispatchEventsAsync();

                    //ignore cancellation token
                    await transaction.CommitAsync(CancellationToken.None);
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(CancellationToken.None);
                    throw;
                }
            }
        }
    }
}
