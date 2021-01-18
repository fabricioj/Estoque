using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Estoque.Negocio.Dominios
{
    public enum TipoProduto
    {
        [Display(Name = "Não informado")]
        NaoInformado = 0,

        [Display(Name = "Revenda")]
        Revenda = 1,

        [Display(Name = "Acabado")]
        Acabado = 2,

        [Display(Name = "Matéria prima")]
        MateriaPrima = 3,

        [Display(Name = "Embagem")]
        Embalagem = 4
    }
}
