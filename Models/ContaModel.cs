using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCore.Models
{
    public class ContaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o nome da conta!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe o saldo da conta!")]
        public double Saldo { get; set; }
        public int Usuario_Id { get; set; } 
    }
}
