using System.Threading;
using System.Threading.Tasks;

namespace SampleIntegrationTest.SharedKernel.SeedWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
