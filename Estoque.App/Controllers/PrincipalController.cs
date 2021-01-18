using Estoque.Negocio.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.App.Controllers
{
    public class PrincipalController : Controller
    {

        protected void RetornarMensagem(string mensagem)
        {
            ModelState.AddModelError("", mensagem);
        }

        protected void RetornarModelStateMensagens(IEnumerable<Mensagem> mensagens)
        {
            if (mensagens == null)
            {
                return;
            }

            foreach (var apiMensagem in mensagens)
            {
                ModelState.AddModelError("", apiMensagem.Texto);
            }
        }
    }
}
