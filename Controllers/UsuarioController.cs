using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjetoCore.Models;
using ProjetoCore.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCore.Controllers
{
    public class UsuarioController : Controller
    {

        [HttpGet]
        public IActionResult Login(int? id)
        {
            if(id != null)
            {
                if(id == 0)
                {
                    HttpContext.Session.SetString("NomeUsuarioLogado", string.Empty);
                    HttpContext.Session.SetString("IdUsuarioLogado", string.Empty);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult ValidarLogin(UsuarioModel usuario)
        {
            string sql = $"SELECT Id, Nome, Data_Nasc FROM Usuario WHERE Email ='{usuario.Email}' and Senha = '{usuario.Senha}'";
            DAL obj = new DAL();

            DataTable dt = obj.SqlSelect(sql);

            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    usuario.Id = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                    usuario.Nome = dt.Rows[0]["Nome"].ToString();
                    usuario.Data_Nasc = dt.Rows[0]["Data_Nasc"].ToString();
                    
                    HttpContext.Session.SetString("NomeUsuarioLogado", usuario.Nome);
                    HttpContext.Session.SetString("IdUsuarioLogado", usuario.Id.ToString());

                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    TempData["LoginInvalido"] = "Dados de login inválidos!";
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Login");
        }

        
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                string sql = $"insert into Usuario(Nome, Email, Senha, Data_Nasc) values('{usuario.Nome}'," +
                    $" '{usuario.Email}', '{usuario.Senha}', '{usuario.Data_Nasc}')";
                DAL obj = new DAL();

                obj.SqlExecute(sql);

                return RedirectToAction("Login");
            }

            return RedirectToAction("Registrar");
        }
    }
}