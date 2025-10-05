using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public readonly record struct Result(bool Succeeded, string? Error = null)
    {
        public static Result Success() => new(true, null);
        public static Result Fail(string error) => new(false, error);
    }

    public readonly record struct Result<T>(bool Succeeded, T? Value, string? Error = null)
    {
        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Fail(string error) => new(false, default, error);
    }
}
