using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    List<string> erros = (from item in ModelState.Values
                                          from error in item.Errors
                                          select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(new { Result = "ERROR", Message = string.Join(Environment.NewLine, erros) });
                }

                BoCliente boCliente = new BoCliente();

                if (boCliente.VerificarExistencia(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json(new { Result = "ERROR", Message = "Já existe um cliente cadastrado com esse CPF" });
                }

                List<string> cpfsDuplicados = model.Beneficiarios.GroupBy(b => b.CPF)
                                                                 .Where(g => g.Count() > 1)
                                                                 .Select(g => g.Key)
                                                                 .ToList();

                if (cpfsDuplicados.Any())
                {
                    Response.StatusCode = 400;
                    return Json(new { Result = "ERROR", Message = $"Existem beneficiários com o mesmo CPF: {string.Join(", ", cpfsDuplicados)}" });
                }

                model.Id = boCliente.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                if (model.Beneficiarios != null && model.Beneficiarios.Count > 0)
                {
                    BoBeneficiario boBeneficiario = new BoBeneficiario();

                    foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                    {
                        if (beneficiario == null)
                            continue;

                        beneficiario.Id = boBeneficiario.Incluir(new Beneficiario()
                        {
                            ClienteId = model.Id,
                            Nome = beneficiario.Nome,
                            CPF = beneficiario.CPF
                        });
                    }
                }

                return Json(new { Result = "OK", Message = "Cadastro efetuado com sucesso" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    List<string> erros = (from item in ModelState.Values
                                          from error in item.Errors
                                          select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(new { Result = "ERROR", Message = string.Join(Environment.NewLine, erros) });
                }

                BoBeneficiario boBeneficiario = new BoBeneficiario();

                List<string> cpfsDuplicados = model.Beneficiarios.GroupBy(b => b.CPF)
                                                                 .Where(g => g.Count() > 1)
                                                                 .Select(g => g.Key)
                                                                 .ToList();

                if (cpfsDuplicados.Any())
                {
                    Response.StatusCode = 400;
                    return Json(new { Result = "ERROR", Message = $"Existem beneficiários com o mesmo CPF: {string.Join(", ", cpfsDuplicados)}" });
                }

                List<Beneficiario> beneficiarios = boBeneficiario.ListarPorCliente(model.Id);

                foreach (Beneficiario beneficiario in beneficiarios)
                {
                    if (model.Beneficiarios.Find(x => x.CPF == beneficiario.CPF && x.Id != beneficiario.Id) != null)
                    {
                        Response.StatusCode = 400;
                        return Json(new { Result = "ERROR", Message = $"Beneficiário com o CPF '{beneficiario.CPF}' já cadastrado para este cliente" });
                    }
                }

                BoCliente boCliente = new BoCliente();

                boCliente.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                if (model.Beneficiarios != null && model.Beneficiarios.Count > 0)
                {
                    foreach (var beneficiarioModel in model.Beneficiarios)
                    {
                        if (beneficiarioModel == null)
                            continue;

                        if (beneficiarioModel.Id > 0)
                        {
                            boBeneficiario.Alterar(new Beneficiario()
                            {
                                Id = beneficiarioModel.Id.Value,
                                ClienteId = model.Id,
                                Nome = beneficiarioModel.Nome,
                                CPF = beneficiarioModel.CPF
                            });
                        }
                        else
                        {
                            beneficiarioModel.Id = boBeneficiario.Incluir(new Beneficiario()
                            {
                                ClienteId = model.Id,
                                Nome = beneficiarioModel.Nome,
                                CPF = beneficiarioModel.CPF
                            });
                        }
                    }
                }

                return Json(new { Result = "OK", Message = "Cadastro alterado com sucesso" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            Cliente cliente = new BoCliente().Consultar(id);

            ClienteModel model = null;

            if (cliente != null)
            {
                List<Beneficiario> beneficiarios = new BoBeneficiario().ListarPorCliente(id);

                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = beneficiarios.Select(beneficiario => new BeneficiarioModel
                    {
                        Id = beneficiario.Id,
                        Nome = beneficiario.Nome,
                        CPF = beneficiario.CPF
                    }).ToList()
                };
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Deletar(long id)
        {
            try
            {
                List<Beneficiario> beneficiarios = new BoBeneficiario().ListarPorCliente(id);

                if (beneficiarios != null && beneficiarios.Count > 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { Result = "ERROR", Message = "Não é possível excluir o cliente, pois existem beneficiários vinculados a ele" });
                }

                BoCliente boCliente = new BoCliente();
                boCliente.Excluir(id);
                
                return Json(new { Result = "OK", Message = "Cliente excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeletarBeneficiario(long id)
        {
            try
            {
                BoBeneficiario boBeneficiario = new BoBeneficiario();

                boBeneficiario.Excluir(id);

                return Json(new { Result = "OK", Message = "Beneficiário excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = $"Erro ao excluir beneficiário: {ex.Message}" });
            }
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}