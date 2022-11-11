using Gatherly.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatherly.Domain.Shared;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The Specified result value is null.");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }
        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }
    public static bool operator !=(Error? a, Error? b)
    {
        return !(a == b);
    }
    public bool Equals(Error? other)
    {
        if (other is null) { return false; }

        if (other.GetType() != GetType()) { return false; }

        if (other is not Error error) { return false; }

        return error.Code == Code;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Error error)
        {
            return false; 
        }

        return error.Code == Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode() * 24;
    }
}
