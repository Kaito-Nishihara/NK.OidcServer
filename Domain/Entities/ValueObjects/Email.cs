using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entities.ValueObjects
{
    public sealed class Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.");

            var regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(value, regex))
                throw new ArgumentException("Invalid email format.");

            return new Email(value.Trim().ToLowerInvariant());
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) =>
            obj is Email other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
