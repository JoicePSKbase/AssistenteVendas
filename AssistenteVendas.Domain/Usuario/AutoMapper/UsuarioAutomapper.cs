using AssistenteVendas.Core.Entities;
using AssistenteVendas.Domain.Usuario.Commands;
using AssistenteVendas.Domain.Usuario.Entities;
using AutoMapper;

namespace AssistenteVendas.Domain.Usuario.AutoMapper
{
    public class UsuarioAutomapper : Profile
    {
        public UsuarioAutomapper()
        {
            CreateMap<UsuarioEntity, UsuarioResult>();
            CreateMap<AdicionarUsuarioCommand, UsuarioEntity>();
          
            CreateMap<ListaPaginada<UsuarioEntity>, ListaPaginada<UsuarioResult>>();
        }
    }
}


