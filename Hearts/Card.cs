using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearts
{
    public struct Card
    {
        public Suite Suite { get; set; }

        public Rank Rank { get; set; }

        public static Card Make(Suite suite, Rank rank) => new() { Suite = suite, Rank = rank };

        #region

        public override string ToString() => $"{Enum.GetName(Rank)} of {Enum.GetName(Suite)}";

        #endregion

        #region implement equality operator

        public static bool operator ==(Card lhs, Card rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Card lhs, Card rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is Card that)
            {
                return this.Suite == that.Suite && this.Rank == that.Rank;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (int)Rank * Enum.GetNames(typeof(Suite)).Length + (int)Suite;
        }

        #endregion
    }
}
