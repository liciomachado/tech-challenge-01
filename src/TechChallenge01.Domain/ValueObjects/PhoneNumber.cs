using System.Text.RegularExpressions;

namespace TechChallenge01.Domain.ValueObjects
{
    public class PhoneNumber
    {
        /// <summary>
        /// Construtor valida se o número de telefone é válido e se o DDD fornecido está na lista de DDDs válidos. 
        /// Se o número de telefone não for válido ou se o DDD não estiver na lista, uma exceção do tipo ArgumentException é lançada.
        /// </summary>
        /// <param name="value">Número de telefone completo com DDD.</param>
        /// <exception cref="ArgumentException"></exception>
        public PhoneNumber(string value)
        {
            var formattedValue = Regex.Replace(value, "[^0-9]+", "");

            if (!IsValidPhone(formattedValue))
                throw new ArgumentException("Número de telefone informado incorretamente, Modelo esperado: (dd) 99999-9999.");
            else
                this.Value = formattedValue;

            int ddd = int.Parse(this.Value.Substring(0, 2));
            if (!ValidDDDs.Contains(ddd))
            {
                throw new ArgumentException("DDD inválido.");
            }
            else
            {
                DDD = ddd.ToString();
            }
        }

        public string Value { get; private set; }
        public string DDD { get; private set; }

        public override string ToString()
        {
            return this.Value;
        }

        /// <summary>
        ///  HashSet é usado para armazenar os DDDs válidos, pois oferece busca rápida.
        /// </summary>
        public static readonly HashSet<int> ValidDDDs = new HashSet<int>
        {
            11, 12, 13, 14, 15, 16, 17, 18, 19, // São Paulo
            21, 22, 24, // Rio de Janeiro
            27, 28, // Espírito Santo
            31, 32, 33, 34, 35, 37, 38, // Minas Gerais
            41, 42, 43, 44, 45, 46, // Paraná
            47, 48, 49, // Santa Catarina
            51, 53, 54, 55, // Rio Grande do Sul
            61, // Distrito Federal
            62, 64, // Goiás
            63, // Tocantins
            65, 66, // Mato Grosso
            67, // Mato Grosso do Sul
            68, // Acre
            69, // Rondônia
            71, 73, 74, 75, 77, // Bahia
            79, // Sergipe
            81, 87, // Pernambuco
            82, // Alagoas
            83, // Paraíba
            84, // Rio Grande do Norte
            85, 88, // Ceará
            86, 89, // Piauí
            91, 93, 94, // Pará
            92, 97, // Amazonas
            95, // Roraima
            96, // Amapá
            98, 99 // Maranhão
        };

        public static bool IsValidPhone(string phone)
        {
            var phoneRegex = new Regex(@"^\d{10,11}$");
            return phoneRegex.IsMatch(phone);
        }
    }
}
