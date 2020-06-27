using Microsoft.Extensions.DependencyInjection;
using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Services
{
    public class DefaultServiceScopeFactory : IHybridServiceScopeFactory
    {
        protected IServiceScopeFactory Factory { get; }

        public DefaultServiceScopeFactory(IServiceScopeFactory factory)
        {
            Factory = factory;
        }

        public IServiceScope CreateScope()
        {
            return Factory.CreateScope();
        }
    }

}
