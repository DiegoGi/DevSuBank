using DevSu.Bank.Customers.Domain.Resources;
using DevSu.Bank.Customers.Domain.ValueObjects;

namespace DevSu.Bank.Customers.Domain.SeedWork
{
    public abstract class Person : Entity
    {
        private const int MinimumAge = 0;
        private const int MaximumAge = 120;

        public int Id { get; protected set; }

        public string Name { get; protected set; } = null!;

        public Gender Gender { get; protected set; }

        public int Age { get; protected set; }

        public string Identification { get; protected set; } = null!;

        public string? Address { get; protected set; }

        public string? Phone { get; protected set; }

        // Required for ORM
        protected Person()
        {
        }

        protected Person(string name, Gender gender, int age, string identification, string? address,
            string? phone)
        {
            Identification = EnsureNotEmpty(identification, nameof(Identification));
            SetPersonalInformation(name, gender, age, address, phone);
        }

        public virtual void UpdatePersonalInformation(string name, Gender gender, int age, string? address,
            string? phone)
        {
            SetPersonalInformation(name, gender, age, address, phone);
        }

        private void SetPersonalInformation(string name, Gender gender, int age, string? address, string? phone)
        {
            Name = EnsureNotEmpty(name, nameof(Name));
            Gender = gender;
            Age = EnsureValidAge(age);
            Address = address?.Trim();
            Phone = phone?.Trim();
        }

        protected static string EnsureNotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainValidationException(string.Format(Generals.NotEmptyOrNullParameter, parameterName));
            }

            return value.Trim();
        }

        private static int EnsureValidAge(int age)
        {
            if (age < MinimumAge)
            {
                throw new DomainValidationException(string.Format(Generals.GreaterOrEqualTo, nameof(Age), MinimumAge));
            }

            if (age > MaximumAge)
            {
                throw new DomainValidationException(string.Format(Generals.LessThanOrEqualThanTo, nameof(Age), MaximumAge));
            }

            return age;
        }
    }
}
