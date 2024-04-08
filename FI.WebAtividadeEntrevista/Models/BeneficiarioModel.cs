using System.ComponentModel.DataAnnotations;
using WebAtividadeEntrevista.Validations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {

        public long Id { get; set; }

        /// <summary>
        /// Beneficiario CPF
        /// </summary>
        [Required]
        [MaxLength(14)]
        [CpfValidationAttribute(ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }

        /// <summary>
        /// Beneficiario Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Beneficiario Id Cliente
        /// </summary>
        [Required]
        public long IdCliente { get; set; }
    }
}