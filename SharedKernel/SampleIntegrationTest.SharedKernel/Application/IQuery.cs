using MediatR;

namespace SampleIntegrationTest.SharedKernel.Application
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}
