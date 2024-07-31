namespace TechChallenge01.Domain.Tests.Fixtures.PhoneNumberFixtures
{
    public class PhoneNumberTestFixture
    {
        public PhoneNumberTestFixture()
        {
            
        }

        public string ValidDigitLengthNumber
        {
            get
            {
                var validPhoneNumbers = new string[]
                {
                "(11) 9-4567-1234",
                "abcdefghijklmn!@#$%¨&*()(12) 9-4567-1234 opqrstuvwyxz" // 11 Digits with characters and special characters
                };

                return validPhoneNumbers[new Random().Next(validPhoneNumbers.Length)];
            }
        }

        public string InvalidDigitLengthNumber
        {
            get
            {
                var invalidDigitLengthNumbers = new string[]
                {
                "(12) 9 9999!@#$%¨&*()999 abcdefghijklmnopqrstuvwyxz" // 10 Digits with characters and special characters
                };

                return invalidDigitLengthNumbers[new Random().Next(invalidDigitLengthNumbers.Length)];
            }
        }

        public string ValidDDDNumber
        {
            get
            {
                var validDDDNumbers = new string[]
                {
                "(11) 9-4567-1234",
                "(73) 9-4567-1234",
                "(12) 9-4567-1234",
                };

                return validDDDNumbers[new Random().Next(validDDDNumbers.Length)];
            }
        }

        public string InvalidDDDNumber
        {
            get
            {
                var invalidDDDNumbers = new string[]
                {
                "(073) 9-4567-1234",
                "(72) 9-4567-1234",
                "(00) 9-4567-1234",
                "(1) 9-4567-1234",
                "(123) 9-4567-1234",
                };

                return invalidDDDNumbers[new Random().Next(invalidDDDNumbers.Length)];
            }
        }
    }
}
