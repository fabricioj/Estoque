using Estoque.Negocio.Utilidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dados.Utilidades
{
    public class Transacao : ITransacao
    {
        private readonly EstoqueContext _EstoqueContext;
        public Transacao(EstoqueContext estoqueContext)
        {
            _EstoqueContext = estoqueContext;
        }

        public async Task<IEnumerable<Mensagem>> Salvar()
        {
            List<Mensagem> mensagens = new List<Mensagem>();
            try
            {
                await _EstoqueContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                mensagens.Add(new Mensagem(TipoMensagem.Erro, "Falha durante o processamento da operação"));
            }

            return mensagens;
        }

        public async Task Desfazer()
        {
            //Rollback automatico quando o GC destrói o Context
        }
    }
}
