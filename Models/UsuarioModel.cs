using ProjetoCore.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCore.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o email!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha a senha!")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Preencha a data de nascimento!")]
        public string Data_Nasc { get; set; }
    }
}
