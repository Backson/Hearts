using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearts
{
    public struct Card : IEqualityComparer<Card>
    {
        public Card(Suite suite, Rank rank)
        {
            Suite = suite;
            Rank = rank;
        }
        
        public Suite Suite { get; set; }

        public Rank Rank { get; set; }


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

        #endregion

        public override bool Equals(object? obj)
        {
            return obj is Card other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Suite, (int)Rank);
        }
        
        public bool Equals(Card x, Card y)
        {
            return x.Suite == y.Suite && x.Rank == y.Rank;
        }

        public int GetHashCode(Card obj)
        {
            return HashCode.Combine((int)obj.Suite, (int)obj.Rank);
        }
        public bool Equals(Card other)
        {
            return Suite == other.Suite && Rank == other.Rank;
        }
    }
}
