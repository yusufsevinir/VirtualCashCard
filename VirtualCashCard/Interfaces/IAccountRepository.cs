using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualCashCard.Types;

namespace VirtualCashCard.Interfaces
{
        public interface IAccountRepository
        {
            Task<bool> Withdraw(decimal amount);
            Task<bool> Deposit(decimal amount);
        }
}
