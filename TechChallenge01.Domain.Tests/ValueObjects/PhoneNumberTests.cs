using TechChallenge01.Domain.Tests.Fixtures.PhoneNumberFixtures;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Domain.Tests.ValueObjects
{
    [Collection(nameof(PhoneNumberTestFixtureCollection))]
    public class PhoneNumberTests
    {
        private readonly PhoneNumberTestFixture _phoneNumberTestFixture;

        public PhoneNumberTests(PhoneNumberTestFixture phoneNumberTestFixture)
        {
            _phoneNumberTestFixture = phoneNumberTestFixture;
        }

        [Trait("CreatePhoneNumber_WithValidLength", "PhoneNumber")]
        [Fact(DisplayName = "Deve criar com sucesso um objeto de valor Phone quando o número de telefone tiver a quantidade de dígitos válida.")]
        public void CreatePhoneNumber_WhenDigitLengthIsValid_ShouldReturnSuccess()
        {
            var phoneNumber = new PhoneNumber(_phoneNumberTestFixture.ValidDigitLengthNumber);

            Assert.True(phoneNumber.Value.Length.Equals(11));
        }

        [Trait("CreatePhoneNumber_WithInvalidLength", "PhoneNumber")]
        [Fact(DisplayName = "Deve retornar uma exceção de argumento ao criar um objeto de valor Phone quando a quantidade de dígitos do número de telefone for inválida.")]
        public void CreatePhoneNumber_WhenDigitLengthIsInvalid_ShouldReturnArgumentException()
        {
            var result = Assert.Throws<ArgumentException>(() => new PhoneNumber(_phoneNumberTestFixture.InvalidDigitLengthNumber));
        }

        [Trait("CreatePhoneNumber_WithValidDDD", "PhoneNumber")]
        [Fact(DisplayName = "Deve criar com sucesso um objeto de valor Phone quando o número do DDD for válido.")]
        public void CreatePhoneNumber_WhenDDDIsValid_ShouldReturnSuccess()
        {
            var phoneNumber = new PhoneNumber(_phoneNumberTestFixture.ValidDDDNumber);
            Assert.Contains(int.Parse(phoneNumber.DDD), PhoneNumber.ValidDDDs);
        }

        [Trait("CreatePhoneNumber_WithInvalidDDD", "PhoneNumber")]
        [Fact(DisplayName = "Deve retornar uma exceção de argumento ao criar um objeto de valor Phone quando o número do DDD for inválido.")]
        public void CreatePhoneNumber_WhenDDDIsInvalid_ShouldReturnArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new PhoneNumber(_phoneNumberTestFixture.InvalidDDDNumber));
        }
    }
}
