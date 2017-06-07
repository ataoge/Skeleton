namespace Ataoge.Services
{
    public abstract class ServiceBase : IService
    {
        protected ServiceBase(IServiceContext serviceContext)
        {
            this.ServiceContext = serviceContext;
        }

        public IServiceContext ServiceContext { get;}
    }
}