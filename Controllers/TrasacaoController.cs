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
    public class TransacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public TransacaoController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            List<TransacaoModel> lista = new List<TransacaoModel>();

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT t.Id, t.Data, t.Descricao as historico, t.Tipo, t.Valor, t.Conta_Id, " +
                        "c.Nome as conta, t.Plano_Contas_Id, " +
                        "p.Descricao as plano_conta " +
                        "FROM Transacao " +
                        "AS t inner join Conta c " +
                        "on t.Conta_Id = c.Id inner join Plano_Contas as p " +
                        "on t.Plano_Contas_Id = p.Id " +
                        $"where t.Usuario_Id = {id_usuario_logado} order by t.Data desc";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                lista.Add(new TransacaoModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Data = DateTime.Parse(row["Data"].ToString()).ToString("dd/MM/yyyy"),
                    Valor = Convert.ToDouble(row["Valor"]),
                    Descricao = Convert.ToString(row["historico"]),
                    Tipo = Convert.ToString(row["Tipo"]),
                    Conta_Id = Convert.ToInt32(row["Conta_Id"]),
                    conta = Convert.ToString(row["conta"]),
                    Plano_Contas_Id = Convert.ToInt32(row["Plano_Contas_Id"]),
                    Plano_Conta = Convert.ToString(row["Plano_Conta"])
                });
            }

            return View(lista);
        }

        [HttpGet]
        public IActionResult Registrar(int? id)
        {
            if (id != null)
            {
                TransacaoModel planoConta = new TransacaoModel();
                ViewBag.Registo = CarregarRegistro(id);
            }

            ViewBag.ListaContas = CarregarRegistroConta();
            ViewBag.ListaPlanoContas = CarregarPlanos();

            return View();
        }
        [HttpPost]
        public IActionResult Registrar(TransacaoModel transacao, int? id)
        {
            if (ModelState.IsValid)
            {
                string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
                string sql = "";
                if (id == 0)
                {
                    sql = $"SET DATEFORMAT ymd; INSERT INTO Transacao (Data, Tipo, Valor, Descricao, Conta_Id, Plano_Contas_Id, Usuario_Id)" +
                          $"VALUES('{DateTime.Parse(transacao.Data).ToString("yyyy/MM/dd")}', '{transacao.Tipo}', '{transacao.Valor}'," +
                          $"'{transacao.Descricao}','{transacao.Conta_Id}','{transacao.Plano_Contas_Id}','{id_usuario_logado}')";
                }
                else
                {
                    sql = $"SET DATEFORMAT ymd;  UPDATE Transacao SET Data = '{DateTime.Parse(transacao.Data).ToString("yyyy/MM/dd")}', " +
                          $"Tipo = '{transacao.Tipo}', Valor = '{transacao.Valor}', Descricao = '{transacao.Descricao}', " +
                          $"Conta_Id = '{transacao.Conta_Id}', Plano_Contas_Id = '{transacao.Plano_Contas_Id}' " +
                          $"WHERE Usuario_Id = '{id_usuario_logado}' AND Id = '{id}'";
                }

                DAL objDAL = new DAL();
                objDAL.SqlExecute(sql);

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ExcluirTransacao(int id)
        {
            TransacaoModel planoConta = new TransacaoModel();
            ViewBag.Registo = CarregarRegistro(id);
            return View();
        }
        public IActionResult Excluir(int id)
        {
            PlanoContaModel conta = new PlanoContaModel();
            string sql = $"DELETE FROM Transacao WHERE Id = {id}";
            DAL objDAL = new DAL();
            objDAL.SqlExecute(sql);

            return RedirectToAction("Index");
        }
        [HttpGet]
        [HttpPost]
        public IActionResult Extrato(TransacaoModel transacao)
        {
            ViewBag.ListaTransacao = Filtros(transacao.Data, transacao.DataFinal, transacao.Tipo, transacao.Conta_Id);
            ViewBag.ListaContas = CarregarRegistroConta();
            return View();
        }
        public IActionResult Dashboard()
        {
            List<Dashboard> DadosGrafico = new List<Dashboard>();
            string sql = $"SELECT SUM(t.Valor) as Total, p.Descricao FROM Transacao as t " +
                $"INNER JOIN Plano_Contas as p on t.Plano_Contas_Id = p.Id WHERE t.Tipo = 'Despesa' " +
                $"group by p.Descricao";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                DadosGrafico.Add(new Dashboard
                {
                    Total = Convert.ToDouble(row["Total"]),
                    PlanoConta = Convert.ToString(row["Descricao"])
                });
            }

            string Valores = "";
            string Labels = "";
            string Cores = "";

            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));

            for (int i=0; i<DadosGrafico.Count; i++)
            {
                Valores += DadosGrafico[i].Total.ToString() + ",";
                Labels += DadosGrafico[i].Total.ToString() + ",";
                Cores += "'" + String.Format("#{0:X6}", random.Next(0x1000000)) + "',";
            }

            ViewBag.Valores = Valores;
            ViewBag.Labels = Labels;
            ViewBag.Cores = Cores;

            return View();
        }
        // ----------------------------------------------------------------------------------//
        // Carregamentos //
        // ----------------------------------------------------------------------------------//
        public TransacaoModel CarregarRegistro(int? id)
        {
            TransacaoModel item;

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT t.Id, t.Data, t.Descricao as historico, t.Tipo, t.Valor, t.Conta_Id, " +
                        "c.Nome as conta, t.Plano_Contas_Id, " +
                        "p.Descricao as plano_conta " +
                        "FROM Transacao " +
                        "AS t inner join Conta c " +
                        "on t.Conta_Id = c.Id inner join Plano_Contas as p " +
                        "on t.Plano_Contas_Id = p.Id " +
                        $"where t.Usuario_Id = {id_usuario_logado} and t.Id = {id}";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            item = new TransacaoModel();
            item.Id = Convert.ToInt32(table.Rows[0]["Id"]);
            item.Data = DateTime.Parse(table.Rows[0]["Data"].ToString()).ToString("dd/MM/yyyy");
            item.Valor = Convert.ToDouble(table.Rows[0]["Valor"]);
            item.Descricao = Convert.ToString(table.Rows[0]["historico"]);
            item.Tipo = Convert.ToString(table.Rows[0]["Tipo"]);
            item.Conta_Id = Convert.ToInt32(table.Rows[0]["Conta_Id"]);
            item.conta = Convert.ToString(table.Rows[0]["conta"]);
            item.Plano_Contas_Id = Convert.ToInt32(table.Rows[0]["Plano_Contas_Id"]);
            item.Plano_Conta = Convert.ToString(table.Rows[0]["Plano_Conta"]);

            return item;
        }
        public List<ContaModel> CarregarRegistroConta()
        {
            List<ContaModel> lista = new List<ContaModel>();

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT Id, Nome, Saldo, Usuario_Id FROM Conta WHERE Usuario_Id = 1";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                lista.Add(new ContaModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nome = Convert.ToString(row["Nome"]),
                    Saldo = Convert.ToDouble(row["Saldo"]),
                    Usuario_Id = Convert.ToInt32(row["Usuario_Id"])
                });
            }
            return lista;
        }
        public List<PlanoContaModel> CarregarPlanos()
        {
            List<PlanoContaModel> lista = new List<PlanoContaModel>();

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT Id, Descricao, Tipo, Usuario_Id FROM Plano_Contas WHERE Usuario_Id = {id_usuario_logado}";
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
        public List<TransacaoModel> CarregarTransacao()
        {
            List<TransacaoModel> lista = new List<TransacaoModel>();

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT t.Id, t.Data, t.Descricao as historico, t.Tipo, t.Valor, t.Conta_Id, " +
                        "c.Nome as conta, t.Plano_Contas_Id, " +
                        "p.Descricao as plano_conta " +
                        "FROM Transacao " +
                        "AS t inner join Conta c " +
                        "on t.Conta_Id = c.Id inner join Plano_Contas as p " +
                        "on t.Plano_Contas_Id = p.Id " +
                        $"where t.Usuario_Id = {id_usuario_logado} order by t.Data desc";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                lista.Add(new TransacaoModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Data = DateTime.Parse(row["Data"].ToString()).ToString("dd/MM/yyyy"),
                    Valor = Convert.ToDouble(row["Valor"]),
                    Descricao = Convert.ToString(row["historico"]),
                    Tipo = Convert.ToString(row["Tipo"]),
                    Conta_Id = Convert.ToInt32(row["Conta_Id"]),
                    conta = Convert.ToString(row["conta"]),
                    Plano_Contas_Id = Convert.ToInt32(row["Plano_Contas_Id"]),
                    Plano_Conta = Convert.ToString(row["Plano_Conta"])
                });
            }

            return lista;
        }
        public List<TransacaoModel> Filtros(String Data, String DataFinal, String Tipo, int Conta_Id)
        {
            List<TransacaoModel> lista = new List<TransacaoModel>();

            // View Extrato
            string filtro = "";

            if ((Data != null))
            {
                filtro += $" and t.Data >= '{DateTime.Parse(Data).ToString("yyyy/MM/dd")}' " +
                        $"and t.Data <= '{DateTime.Parse(DataFinal).ToString("yyyy/MM/dd")}'";
            }

            if (Tipo != null)
            {
                if (Tipo != "Ambos")
                {
                    filtro += $" and t.Tipo = '{Tipo}' ";
                }
            }

            if (Conta_Id != 0)
            {
                filtro += $" and t.Conta_Id = '{Conta_Id}' ";
            }
            // Fim

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SET DATEFORMAT ymd; SELECT t.Id, t.Data, t.Descricao as historico, t.Tipo, t.Valor, t.Conta_Id, " +
                        "c.Nome as conta, t.Plano_Contas_Id, " +
                        "p.Descricao as plano_conta " +
                        "FROM Transacao " +
                        "AS t inner join Conta c " +
                        "on t.Conta_Id = c.Id inner join Plano_Contas as p " +
                        "on t.Plano_Contas_Id = p.Id " +
                        $"where t.Usuario_Id = {id_usuario_logado} {filtro} order by t.Data desc";
            DAL objDAL = new DAL();
            DataTable table = objDAL.SqlSelect(sql);

            foreach (DataRow row in table.Rows)
            {
                lista.Add(new TransacaoModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Data = DateTime.Parse(row["Data"].ToString()).ToString("dd/MM/yyyy"),
                    Valor = Convert.ToDouble(row["Valor"]),
                    Descricao = Convert.ToString(row["historico"]),
                    Tipo = Convert.ToString(row["Tipo"]),
                    Conta_Id = Convert.ToInt32(row["Conta_Id"]),
                    conta = Convert.ToString(row["conta"]),
                    Plano_Contas_Id = Convert.ToInt32(row["Plano_Contas_Id"]),
                    Plano_Conta = Convert.ToString(row["Plano_Conta"])
                });
            }

            return lista;
        }
    }
}