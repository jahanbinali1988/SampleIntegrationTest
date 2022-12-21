using System.Threading.Tasks;

namespace SampleIntegrationTest.SharedKernel.EventProcessing.DomainEvent
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}
