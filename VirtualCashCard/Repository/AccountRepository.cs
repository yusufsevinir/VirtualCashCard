using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualCashCard.Interfaces;
using VirtualCashCard.Services;
using VirtualCashCard.Types;

namespace VirtualCashCard.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private Account account;
        private readonly object locker = new object();
        private readonly ValidatorService validatorService;

        public AccountRepository(Account account)
        {
            this.account = account;
            this.validatorService = new ValidatorService();
        }

        public async Task<bool> Withdraw(decimal amount)
        {
            if (amount <= 0.00M || amount.Equals(decimal.MaxValue)) return false;
            var isValid = await validatorService.Validate(account);
            if (!isValid) return false;
            lock (locker)
            {
                if (account.Balance < amount) return false;
                account.Balance -= amount;
            }
            return true;
        }

        //change it to Task<bool> when using 3rd party api checks for async operations
        public async Task<bool> Deposit(decimal amount)
        {
            if (amount <= 0.00M || amount.Equals(decimal.MaxValue)) return false;

            var isValid = await validatorService.Validate(account);
            if (!isValid) return false;

            lock (locker)
            {               
                account.Balance += amount;
            }
            return true;
        }
    }
}
