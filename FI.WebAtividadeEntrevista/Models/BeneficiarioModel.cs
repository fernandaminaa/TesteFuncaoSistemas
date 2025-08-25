using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WebAtividadeEntrevista.Validations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel 
    {
        #region Private Properties
        private string _cpf;
        #endregion Private Properties

        public long? Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [CPFValidate(ErrorMessage = "CPF do beneficiário inválido")]
        public string CPF
        {
            get => _cpf;
            set => _cpf = Regex.Replace(value, @"\D", "");
        }
    }
}