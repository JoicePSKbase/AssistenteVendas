using AssistenteVendas.Core.Entities;
using AssistenteVendas.Core.Handlers;
using AssistenteVendas.Domain.Usuario.Commands;
using AssistenteVendas.Domain.Usuario.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AssistenteVendas.Domain.Usuario.Handlers
{
    public class BuscaUsuarioHandler : BaseHandler, IRequestHandler<BuscaUsuarioCommand, ListaPaginada<UsuarioResult>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BuscaUsuarioHandler(
            IUsuarioRepository usuarioRepository,
            IServiceProvider serviceProvider,
            IMapper mapper,
            ILogger<BuscaUsuarioHandler> logger
        ) : base(serviceProvider)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ListaPaginada<UsuarioResult>> Handle(BuscaUsuarioCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var filtros = new[]
                {
                    command.Filtro
                }.Where(x => !string.IsNullOrEmpty(x));

                var filtroCustom = string.Join(" AND ", filtros);

                var listaPaginada = await _usuarioRepository.Buscar(
                    filtroCustom,
                    command.Ordenacao,
                    command.Pagina,
                    command.QtdeRegistros);

                return _mapper.Map<ListaPaginada<UsuarioResult>>(listaPaginada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}, {StackTrace}", ex.Message, ex.StackTrace);
                throw new Exception("Não foi possível realizar a pesquisa.");
            }
        }
    }
}