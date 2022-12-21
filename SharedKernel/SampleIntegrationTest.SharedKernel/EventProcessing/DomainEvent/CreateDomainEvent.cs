using SampleIntegrationTest.SharedKernel.SeedWork;

namespace SampleIntegrationTest.SharedKernel.EventProcessing.DomainEvent
{
    public class CreateDomainEvent<T> : DomainEventBase where T : Entity
    {
        public CreateDomainEvent(T createdObject)
        {
            CreatedObject = createdObject;
        }

        public T CreatedObject { get; set; }
    }

    public class UpdateEntityDomainEvent<T> : DomainEventBase where T : Entity
    {
        public UpdateEntityDomainEvent(T beforeUpdate, T afterUpdate)
        {
            BeforeUpdate = beforeUpdate;
            AfterUpdate = afterUpdate;
        }

        public T BeforeUpdate { get; set; }
        public T AfterUpdate { get; set; }
    }
}
