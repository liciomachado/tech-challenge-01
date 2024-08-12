using System.Text.RegularExpressions;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Domain.Tests.ValueObjects
{
    public class PhoneNumberTests
    {
        public PhoneNumberTests()
        {

        }

        [Trait("ValidationPhoneNumber", "PhoneNumber")]
        [Theory(DisplayName = "Deve criar com sucesso um objeto de valor Phone quando o número de telefone for válido.")]
        [InlineData("(14) 3733-3388")] // Fixo com caracteres especiais e espaços
        [InlineData("(14) 9-4567-1234")] // Celular com caracteres especiais e espaços
        [InlineData("(81) 3738 8896")] // Fixo com caracteres especiais e espaços
        [InlineData("(81) 9 9999 999")] // Celular com caracteres especiais e espaços
        [InlineData("(95)34561234")] // Fixo com caracteres especiais sem espaços
        [InlineData("(95)945671234")] // Celular com caracteres especiais sem espaços
        [InlineData("73945671234")] // Celular sem caracteres especiais ou espaços
        [InlineData("7334561234")] // Fixo sem caracteres especiais ou espaços
        [InlineData("66 93738 8896")] // Celular somente com espaços
        [InlineData("66 3738 8896")] // Fixo somente com espaços
        public void CreatePhoneNumber_WhenNumberIsValid_ShouldReturnSuccess(string number)
        {
            var phoneNumber = new PhoneNumber(number);

            var formattedValue = Regex.Replace(number, "[^0-9]+", "");
            var phoneRegex = new Regex(@"^\d{10,11}$");

            Assert.Matches(phoneRegex, formattedValue);
        }

        [Trait("ValidationPhoneNumber", "PhoneNumber")]
        [Theory(DisplayName = "Deve retornar uma exceção de argumento ao criar um objeto de valor Phone quando o número de telefone for inválido.")]
        [InlineData("")] // Vazio
        [InlineData("(12) 3777 999")] // Tamanho menor do que esperado
        [InlineData("(12) 9 9999 99999")] // Tamanho maior do que esperado
        public void CreatePhoneNumber_WhenPhoneNumberIsInvalid_ShouldReturnArgumentException(string number)
        {
            var result = Assert.Throws<ArgumentException>(() => new PhoneNumber(number));
        }

        [Trait("ValidationPhoneNumber", "PhoneNumber")]
        [Theory(DisplayName = "Deve criar com sucesso um objeto de valor Phone quando o número do DDD for válido.")]
        [InlineData("(11) 9-4567-1234")] // DDD válido
        [InlineData("(73) 9-4567-1234")] // DDD válido
        [InlineData("(12) 9-4567-1234")] // DDD válido
        public void CreatePhoneNumber_WhenDDDIsValid_ShouldReturnSuccess(string number)
        {
            var phoneNumber = new PhoneNumber(number);
            Assert.Contains(int.Parse(phoneNumber.DDD), PhoneNumber.ValidDDDs);
        }

        [Trait("ValidationPhoneNumber", "PhoneNumber")]
        [Theory(DisplayName = "Deve retornar uma exceção de argumento ao criar um objeto de valor Phone quando o número do DDD for inválido.")]
        [InlineData("(073) 9-4567-1234")] // DDD inválido
        [InlineData("(72) 9-4567-1234")] // DDD inválido
        [InlineData("(00) 9-4567-1234")] // DDD inválido
        [InlineData("(1) 4567-1234")] // DDD inválido
        [InlineData("(123) 9-4567-1234")] // DDD inválido
        public void CreatePhoneNumber_WhenDDDIsInvalid_ShouldReturnArgumentException(string number)
        {
            Assert.Throws<ArgumentException>(() => new PhoneNumber(number));
        }
    }
}
