using System;
using MediatR;

namespace SampleIntegrationTest.SharedKernel.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTimeOffset OccurredOn { get; }
    }
}
