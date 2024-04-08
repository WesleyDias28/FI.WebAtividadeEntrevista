using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Utils;

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
            BoCliente bo = new BoCliente();
            var boBeneficiario = new BoBeneficiario();

            if (bo.VerificarExistencia(model.CPF))
            {
                return Json("CPF já cadastrado, não foi possível concluir o cadastro do cliente.");
            }
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                
                model.Id = bo.Incluir(new Cliente()
                {                  
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    CPF = model.CPF,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    Beneficiarios = model.Beneficiarios?.Select(beneficiario => new Beneficiario
                    {
                        IdCliente = model.Id,
                        Nome = beneficiario.Nome,
                        CPF = beneficiario.CPF
                    }).ToList()
                });

                try
                {
                    model.Beneficiarios?.ForEach(beneficiario =>
                    {
                        if (boBeneficiario.VerificarExistencia(model.Id, beneficiario.CPF))
                        {
                            Response.StatusCode = 400;
                            throw new Exception("CPF já cadastrado");
                        }

                        boBeneficiario.Incluir(new Beneficiario
                        {
                            IdCliente = model.Id,
                            Nome = beneficiario.Nome,
                            CPF = beneficiario.CPF
                        });
                    });
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (bo.VerificarExistenciaCPFOutroCliente(model.CPF, model.Id))
                {
                    return Json("CPF já cadastrado para outro cliente, não foi possível efetuar a alteração dos dados do cliente!");
                }

                try
                {
                    var beneficiarios = boBeneficiario.Listar(model.Id) ?? new List<Beneficiario>();

                    beneficiarios.ForEach(beneficiario =>
                    {
                        if (model.Beneficiarios == null)
                            boBeneficiario.Excluir(beneficiario.Id);
                        else if (model.Beneficiarios.All(b => b.Id != beneficiario.Id))
                            boBeneficiario.Excluir(beneficiario.Id);
                    });

                    model.Beneficiarios?.ForEach(beneficiario =>
                    {
                        if (boBeneficiario.VerificarExistencia(model.Id, beneficiario.CPF))
                        {
                            Response.StatusCode = 400;
                            throw new Exception("CPF já cadastrado");
                        }

                        if (beneficiario.Id == 0)
                            boBeneficiario.Incluir(new Beneficiario
                            {
                                IdCliente = model.Id,
                                Nome = beneficiario.Nome,
                                CPF = beneficiario.CPF
                            });
                        else
                            boBeneficiario.Alterar(new Beneficiario
                            {
                                Id = beneficiario.Id,
                                IdCliente = model.Id,
                                Nome = beneficiario.Nome,
                                CPF = beneficiario.CPF
                            });
                    });
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }

                bo.Alterar(new Cliente()
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
                    CPF = model.CPF,
                    Telefone = model.Telefone
                });
                               
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
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
                    CPF = ConverterCpf.FormatCpf(cliente.CPF),
                    Telefone = cliente.Telefone,
                    Beneficiarios = boBeneficiario.Listar(id).Select(beneficiario => new BeneficiarioModel
                    {
                        Id = beneficiario.Id,
                        IdCliente = beneficiario.IdCliente,
                        Nome = beneficiario.Nome,
                        CPF = ConverterCpf.FormatCpf(beneficiario.CPF)
                    }).ToList()
                };

            
            }

            return View(model);
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