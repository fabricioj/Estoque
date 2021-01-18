using Estoque.Api.Utilities;
using Estoque.Negocio.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Api.Controllers
{
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        protected readonly List<Mensagem> Mensagens;
        
        public PrincipalController()
        {
            Mensagens = new List<Mensagem>();
        }

        protected void RetornarMensagens(IEnumerable<Mensagem> mensagens)
        {
            if (mensagens == null)
            {
                return;
            }

            Mensagens.AddRange(mensagens);
        }

        protected void RetornarMensagem(Mensagem mensagem)
        {
            Mensagens.Add(mensagem);
        }

        protected IActionResult RespostaWS()
        {
            return RespostaWS<object>(null);
        }

        protected IActionResult RespostaWS<TResult>(TResult dados)
        {
            var retorno = new ApiResposta<TResult>();
            retorno.sucesso = Sucesso();
            retorno.mensagens = Mensagens;
            retorno.dados = dados;
            if (retorno.sucesso)
            {
                return Ok(retorno);
            }

            return BadRequest(retorno);
        }

        protected bool Sucesso()
        {
            return !Mensagens.Any(m => m.Tipo == TipoMensagem.Erro);
        }

        protected bool Falhou()
        {
            return !Sucesso();
        }
    }
}
