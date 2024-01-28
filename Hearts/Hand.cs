using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearts
{
    /// <summary>
    /// The cards in a players hand
    /// </summary>
    public class Hand
    {
        public List<Card> Cards { get; set; } = new();
    }
}
