using AssistenteVendas.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace AssistenteVencas.Core.WebApi.Controllers
{
    [ApiController, Authorize]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IUser AppUser;

        protected Guid UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }

        protected BaseController(IUser appUser)
        {
            AppUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UsuarioId = appUser.GetUserId();
                UsuarioAutenticado = true;
            }
        }

        protected ActionResult CustomResponse<T>(T result = default, bool semRetornoPadrao = false)
        {
            try
            {
                if (semRetornoPadrao)
                {
                    return Ok(result);
                }

                return Ok(new RetornoPadrao<T>
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoPadrao<T>
                {
                    Success = false,
                    Errors = new List<string>()
                });
            }

        }

        protected ActionResult CustomResponseComDataNoErro<T>(T result = default, bool semRetornoPadrao = false)
        {
            try
            {
                if (semRetornoPadrao)
                {
                    return Ok(result);
                }

                return Ok(new RetornoPadrao<T>
                {
                    Success = true,
                    Data = result
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoPadrao<T>
                {
                    Success = false,
                    Errors = new List<string>()
                });
            }
        }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                NotificarErroModelInvalida(modelState);

            return CustomResponse(true);
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
        }

    }
}
