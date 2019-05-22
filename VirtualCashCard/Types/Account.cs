using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualCashCard.Interfaces;

namespace VirtualCashCard.Types
{
    public class Account 
    {
        private readonly object balanceLocker = new object();

        private decimal balance;
        public decimal Balance
        {
            get
            {
                lock (balanceLocker)
                {
                    return balance;
                }
            }

            set { balance = value; }
        }

        public string Pin { get; set; }
        public string CardNumber { get; set; }
    }
}
