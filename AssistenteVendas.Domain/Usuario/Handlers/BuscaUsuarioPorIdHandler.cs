using AssistenteVendas.Core.Handlers;
using AssistenteVendas.Domain.Usuario.Commands;
using AssistenteVendas.Domain.Usuario.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AssistenteVendas.Domain.Usuario.Handlers
{
    public class BuscaUsuarioPorIdHandler : BaseHandler, IRequestHandler<BuscaUsuarioPorIdCommand, UsuarioResult>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BuscaUsuarioPorIdHandler(
            IUsuarioRepository usuarioRepository,
            IServiceProvider serviceProvider,
            IMapper mapper,
            ILogger<BuscaUsuarioPorIdHandler> logger
        ) : base(serviceProvider)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UsuarioResult> Handle(BuscaUsuarioPorIdCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _usuarioRepository.Obter(u => u.Id == command.Id);

                return _mapper.Map<UsuarioResult>(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}, {StackTrace}", ex.Message, ex.StackTrace);
                throw new Exception("Não foi possível realizar a pesquisa.");
            }
        }
    }
}