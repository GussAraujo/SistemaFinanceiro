using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoCore.Models;
using ProjetoCore.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCore.Controllers
{
    public class PlanoContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public PlanoContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }
        public IActionResult Index()
        {
            List<PlanoContaModel> lista = new List<PlanoContaModel>();

            string sql = $"SELECT Id, Descricao, Tipo, Usuario_Id FROM Plano_Contas WHERE Usuario_Id = {IdUsuarioLogado()}";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                lista.Add(new PlanoContaModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Descricao = Convert.ToString(row["Descricao"]),
                    Tipo = Convert.ToString(row["Tipo"]),
                    Usuario_Id = Convert.ToInt32(row["Usuario_Id"]),
                });
            }
            return View(lista);
        }
        public IActionResult Criar(int? id)
        {
            if (id != null)
            {
                PlanoContaModel planoConta = new PlanoContaModel();
                ViewBag.Registo = CarregarRegistro(id)[0];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Criar(PlanoContaModel conta, int? id)
        {
            if (ModelState.IsValid)
            {
                string sql = "";
                if (id == 0)
                {
                    sql = $"INSERT INTO Plano_Contas (Descricao, Tipo, Usuario_Id) VALUES('{conta.Descricao}', '{conta.Tipo}', '{IdUsuarioLogado()}')";
                }
                else
                {
                    sql = $"UPDATE Plano_Contas SET Descricao = '{conta.Descricao}', Tipo = '{conta.Tipo}' WHERE Usuario_Id = '{IdUsuarioLogado()}' AND Id = '{id}'";
                }

                DAL objDAL = new DAL();
                objDAL.SqlExecute(sql);

                return RedirectToAction("Index");
            }
            return View();
        }
        public List<PlanoContaModel> CarregarRegistro(int? id)
        {
            List<PlanoContaModel> lista = new List<PlanoContaModel>();

            string sql = $"SELECT Id, Descricao, Tipo, Usuario_Id FROM Plano_Contas WHERE Usuario_Id = {IdUsuarioLogado()} AND Id = {id}";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                lista.Add(new PlanoContaModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Descricao = Convert.ToString(row["Descricao"]),
                    Tipo = Convert.ToString(row["Tipo"]),
                    Usuario_Id = Convert.ToInt32(row["Usuario_Id"]),
                });
            }
            return lista;
        }
        public IActionResult ExcluirConta(int id)
        {
            PlanoContaModel conta = new PlanoContaModel();
            string sql = $"DELETE FROM Plano_Contas WHERE Id = {id}";
            DAL objDAL = new DAL();
            objDAL.SqlExecute(sql);

            return RedirectToAction("Index");
        }
    }
}