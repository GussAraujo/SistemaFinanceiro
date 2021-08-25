using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCore.Models
{
    public class TransacaoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe a Data!")]
        public string Data { get; set; }
        public string DataFinal { get; set; }
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Informe o Valor!")]
        public double Valor { get; set; }
        [Required(ErrorMessage = "Informe a Descrição!")]
        public string Descricao { get; set; }
        public int Conta_Id { get; set; }
        public string conta { get; set; }
        public int Plano_Contas_Id { get; set; }
        public string Plano_Conta { get; set; }
        public int Usuario_Id { get; set; }
    }
    public class Dashboard
    {
        public double Total { get; set; }
        public string PlanoConta { get; set; }
    }
}