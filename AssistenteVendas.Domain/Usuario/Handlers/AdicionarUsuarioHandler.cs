using AssistenteVendas.Core.Handlers;
using AssistenteVendas.Domain.Usuario.Commands;
using AssistenteVendas.Domain.Usuario.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AssistenteVendas.Domain.Usuario.Handlers
{
    public class AdicionarUsuarioHandler : BaseHandler, IRequestHandler<AdicionarUsuarioCommand, UsuarioResult>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AdicionarUsuarioHandler(
            IServiceProvider serviceProvider,
            IMapper mapper,
            ILogger<AdicionarUsuarioHandler> logger,
            IUsuarioRepository usuarioRepository) : base(serviceProvider)
        {
            _mapper = mapper;
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResult> Handle(AdicionarUsuarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _usuarioRepository.Obter(u => u.Id == request.Id);

                if (usuario is not null)
                {
                    throw new Exception("Usuário informado já cadastrado.");
                }

                usuario = _mapper.Map(request, usuario);

                _usuarioRepository.Adicionar(usuario);
                await _usuarioRepository.SaveChanges(cancellationToken);

                return _mapper.Map<UsuarioResult>(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}, {StackTrace}", ex.Message, ex.StackTrace);
                throw new Exception("Não foi possível adicionar o usuário.");
            }
        }
    }
}
