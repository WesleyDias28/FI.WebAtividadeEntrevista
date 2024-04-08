using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Utils
{
    public static class ConverterCpf
    {
        /// <summary>
        ///     Converte a string do CPF para long
        ///     <param name="cpf">CPF</param>
        /// </summary>
        public static long ToLong(string cpf)
        {
            var formattedCpf = cpf.Replace(".", "").Replace("-", "");
            return Convert.ToInt64(formattedCpf);
        }

        /// <summary>
        ///     Converte CPF para ficar em padrao com caracters
        ///     <param name="cpf">CPF</param>
        /// </summary>
        public static string FormatCpf(string cpf)
        {
            return $"{long.Parse(cpf):000\\.000\\.000-00}";
        }
    }
}
