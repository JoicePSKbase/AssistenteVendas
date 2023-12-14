using AssistenteVendas.Core.Handlers;
using AssistenteVendas.Domain.Usuario.Commands;
using AssistenteVendas.Domain.Usuario.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AssistenteVendas.Domain.Usuario.Handlers
{
    public class BuscaUsuarioPorEmailHandler : BaseHandler, IRequestHandler<BuscaUsuarioPorEmailCommand, UsuarioResult>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BuscaUsuarioPorEmailHandler(
            IUsuarioRepository usuarioRepository,
            IServiceProvider serviceProvider,
            IMapper mapper,
            ILogger<BuscaUsuarioPorEmailHandler> logger
        ) : base(serviceProvider)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UsuarioResult> Handle(BuscaUsuarioPorEmailCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _usuarioRepository.Obter(u => u.Email == command.Email);

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