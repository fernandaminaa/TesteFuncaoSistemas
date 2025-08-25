using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FI.WebAtividadeEntrevista.Validations
{
    public class ValidateCPF : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cpf = value as string;

            if (string.IsNullOrWhiteSpace(cpf))
                return new ValidationResult("CPF é obrigatório");

            // Remove pontos e traços
            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11)
                return new ValidationResult("O CPF deve conter 11 dígitos");

            if (IsAllDigitsSame(cpf))
                return new ValidationResult("CPF inválido");

            if (!IsValidCPF(cpf))
                return new ValidationResult("CPF inválido");

            return ValidationResult.Success;
        }

        private bool IsValidCPF(string cpf)
        {
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string cpfBase = cpf.Substring(0, 9);
            string digitosInformados = cpf.Substring(9, 2);

            int digito1 = CalcularDigitoVerificador(cpfBase, multiplicadores1);
            int digito2 = CalcularDigitoVerificador(cpfBase + digito1, multiplicadores2);

            string digitosCalculados = digito1.ToString() + digito2.ToString();

            return digitosInformados == digitosCalculados;
        }

        private int CalcularDigitoVerificador(string cpfBase, int[] multiplicadores)
        {
            int soma = 0;

            for (int i = 0; i < multiplicadores.Length; i++)
                soma += int.Parse(cpfBase[i].ToString()) * multiplicadores[i];

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        private bool IsAllDigitsSame(string cpf)
        {
            return cpf.All(c => c == cpf[0]);
        }
    }
}