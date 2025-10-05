using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; } = default!;

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode() => Id!.GetHashCode();
    }

}
