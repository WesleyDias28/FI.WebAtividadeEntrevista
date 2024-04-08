using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo Beneficiario
        /// </summary>
        /// <param name="Beneficiario">Objeto de Beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiario.DaoBeneficiario cli = new DAL.Beneficiario.DaoBeneficiario();
            return cli.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiario.DaoBeneficiario cli = new DAL.Beneficiario.DaoBeneficiario();
            cli.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public List<DML.Beneficiario> Consultar(long id)
        {
            DAL.Beneficiario.DaoBeneficiario cli = new DAL.Beneficiario.DaoBeneficiario();
            return cli.Consultar(id);
        }
        /// <summary>
        /// Verifica se o beneficiario ja existe na lista
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        /// <param name="cliente">Objeto de cliente</param>
        public bool VerificarInconsistencia(DML.Beneficiario beneficiario, List<DML.Beneficiario> beneficiarios)
        {
            if (beneficiarios.Where(x => x.CPF == beneficiario.CPF).Count() > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Excluir o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.Beneficiario.DaoBeneficiario cli = new DAL.Beneficiario.DaoBeneficiario();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<DML.Beneficiario> Listar(long idCliente)
        {
            DAL.Beneficiario.DaoBeneficiario cli = new DAL.Beneficiario.DaoBeneficiario();
            var beneficiarios = cli.Listar(idCliente);
            return beneficiarios;
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(long idCliente, string CPF)
        {
            DAL.Beneficiario.DaoBeneficiario cli = new DAL.Beneficiario.DaoBeneficiario();
            return cli.VerificarExistencia(idCliente, CPF);
        }
    }
}
