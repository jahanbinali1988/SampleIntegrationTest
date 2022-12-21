using System;

namespace SampleIntegrationTest.SharedKernel.Application
{
    public class CommandBase : ICommand
    {
        public Guid CommandId { get; }

        public CommandBase()
        {
            CommandId = Guid.NewGuid();
        }

        protected CommandBase(Guid id)
        {
            CommandId = id;
        }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid CommandId { get; }

        protected CommandBase()
        {
            CommandId = Guid.NewGuid();
        }

        protected CommandBase(Guid id)
        {
            CommandId = id;
        }
    }
}
