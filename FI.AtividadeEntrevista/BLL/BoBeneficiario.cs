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
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            return daoBeneficiario.Incluir(beneficiario);
        }

        /// <summary>
        /// Lista os beneficiario
        /// </summary>
        public List<DML.Beneficiario> ListarPorCliente(long clienteId)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            return daoBeneficiario.ListarPorCliente(clienteId);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            daoBeneficiario.Alterar(beneficiario);
        }

        /// <summary>
        /// Excluir o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            daoBeneficiario.Excluir(id);
        }
    }
}
