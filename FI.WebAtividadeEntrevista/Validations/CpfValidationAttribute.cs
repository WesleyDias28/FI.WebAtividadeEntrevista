
using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Validations
{
    /// <summary>
    /// Classe responsável pela validação dos dados recebidos no campo CPF.
    /// </summary>
    public class CpfValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            return CpfValidation(value.ToString());
        }

        private static bool CpfValidation(string cpfValue)
        {
            cpfValue = RemoveChars(cpfValue);

            if (cpfValue.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpfValue)
                    return false;

            int soma = 0;
            int resto;


            for (int i = 0; i < 9; i++)
                soma += (10 - i) * int.Parse(cpfValue[i].ToString());

            resto = 11 - (soma % 11);

            if (resto == 10 || resto == 11)
                resto = 0;

            if (resto != int.Parse(cpfValue[9].ToString()))
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * int.Parse(cpfValue[i].ToString());

            resto = 11 - (soma % 11);

            if (resto == 10 || resto == 11)
                resto = 0;

            if (resto != int.Parse(cpfValue[10].ToString()))
                return false;

            return true;
        }

        public static string RemoveChars(string stringCpf)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string retorno = regex.Replace(stringCpf, string.Empty);
            return retorno;
        }
    }
}