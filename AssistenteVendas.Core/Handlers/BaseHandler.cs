namespace AssistenteVendas.Core.Handlers
{
    public abstract class BaseHandler
    {
        private readonly IServiceProvider _serviceProvider;

        protected BaseHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

    }

}