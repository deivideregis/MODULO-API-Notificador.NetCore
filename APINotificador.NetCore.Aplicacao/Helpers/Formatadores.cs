using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace APINotificador.NetCore.Aplicacao.Helpers
{
    public static class Formatadores
    {
        public static bool ValidaCPF(this string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma = 0;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace("_", string.Empty).Replace(".", string.Empty).Replace("-", string.Empty);

            if (cpf.Length != 11)
                return false;

            // Caso coloque todos os numeros iguais
            switch (cpf)
            {
                case "11111111111":
                    return false;
                case "00000000000":
                    return false;
                case "22222222222":
                    return false;
                case "33333333333":
                    return false;
                case "44444444444":
                    return false;
                case "55555555555":
                    return false;
                case "66666666666":
                    return false;
                case "77777777777":
                    return false;
                case "88888888888":
                    return false;
                case "99999999999":
                    return false;
            }

            tempCpf = cpf.Substring(0, 9);
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        public static bool ValidaCNPJ(this string vrCNPJ)
        {

            string CNPJ = vrCNPJ.Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
            int[] resultado = new int[2] { 0, 0 };
            bool[] CNPJOk = new bool[2] { false, false };
            int[] soma = new int[2] { 0, 0 };
            int[] digitos = new int[14];

            int nrDig;
            string ftmt = "6543298765432";

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                        CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig + 1, 1)));
                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (
                         resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == (
                        11 - resultado[nrDig]));
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidaData(this string Data)
        {
            DateTime result;
            return DateTime.TryParse(Data, out result);
        }

        public static bool ValidaData(this object Valor)
        {
            DateTime dataOut = new DateTime();
            return DateTime.TryParse(Valor.ToString(), out dataOut);
        }

        public static bool validaEmail(this string email)
        {
            email = email.Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(email))
                return false;

            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            return (rg.IsMatch(email));
        }

        public static string Mesclar(this object[] Array1)
        {
            string Retorno = string.Empty;

            Array.ForEach(Array1, item => Retorno += Retorno.Equals(string.Empty) ? item.ToString() : String.Format(",{0}", item));

            return Retorno;
        }

        public static string Mesclar(this object[] Array1, string separador)
        {
            string Retorno = string.Empty;

            Array.ForEach(Array1, item => Retorno += Retorno.Equals(string.Empty) ? item.ToString() : String.Format("{0}{1}", separador, item));

            return Retorno;
        }

        public static string RemoveAcentuacao(this string entrada)
        {
            return new string(entrada.Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static string RemoveAcento(this string texto)
        {
            try
            {
                if (texto == null)
                    return "";

                string s = texto.Normalize(NormalizationForm.FormD);

                StringBuilder sb = new StringBuilder();

                for (int k = 0; k < s.Length; k++)
                {
                    UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                    if (uc != UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(s[k]);
                    }
                }
                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string RemoveSimbolos(this string Texto)
        {
            try
            {
                if (Texto == null)
                    return "";

                string Retorno = string.Empty;

                foreach (char item in Texto)
                {
                    if (!char.IsSymbol(item) && item != '&' && item != 'º' && item != 'ª')
                    {
                        Retorno += item.ToString();
                    }
                }

                return Retorno;
            }
            catch
            {
                return "";
            }
        }

        public static string RemovePontuacao(this string Texto)
        {
            string Retorno = string.Empty;

            foreach (char item in Texto)
            {
                if (!char.IsPunctuation(item))
                {
                    Retorno += item.ToString();
                }
            }

            return Retorno;
        }
    }
}
