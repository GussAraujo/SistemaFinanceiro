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
    public class ContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public ContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            List<ContaModel> lista = new List<ContaModel>();

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT Id, Nome, Saldo, Usuario_Id FROM Conta WHERE Usuario_Id = {id_usuario_logado}";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                lista.Add(new ContaModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nome = Convert.ToString(row["Nome"]),
                    Saldo = Convert.ToDouble(row["Saldo"]),
                    Usuario_Id = Convert.ToInt32(row["Usuario_Id"]),
                });
            }
            return View(lista);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(ContaModel conta)
        {
            if (ModelState.IsValid)
            {
                string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
                string sql = $"INSERT INTO Conta (Nome, Saldo, Usuario_Id) VALUES('{conta.Nome}', '{conta.Saldo}', '{id_usuario_logado}')";

                DAL objDAL = new DAL();
                objDAL.SqlExecute(sql);

                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult ExcluirConta(int id)
        {
            ContaModel conta = new ContaModel();
            string sql = $"DELETE FROM Conta WHERE Id = {id}";
            DAL objDAL = new DAL();
            objDAL.SqlExecute(sql);

            return RedirectToAction("Index");
        }
    }
}
