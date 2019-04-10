namespace Ataoge.EventBus.InMemoryStorage
{
    internal class InMemoryTransaction : TransactionBase
    {
        public InMemoryTransaction(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void Commit()
        { 
            Flush();
        }

        public override void Rollback()
        {
            //Ignore
        }

        public override void Dispose()
        {
        }
    }
}