using SampleIntegrationTest.SharedKernel.SeedWork;
using System;

namespace SampleIntegrationTest.SharedKernel.EventProcessing.DomainEvent
{
    [Serializable]
    public class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            OccurredOn = DateTime.Now;
        }

        public DateTimeOffset OccurredOn { get; }
    }
}
