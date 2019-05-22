using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualCashCard.Interfaces;
using VirtualCashCard.Types;

namespace VirtualCashCard.Services
{
    public class ValidatorService 
    {
        
        public async Task<bool> Validate(Account account)
        {
            // can be changed to switch with TransactionResult (Error, Message) but now implementing a simple solution
            if (account == null) return false;
            else {
                if (account.CardNumber == null) return false;
                else {
                    var pinIsValid = await IsPinValid(account.CardNumber, account.Pin); // call validator service     
                }                
            }
            return true;
        }

        private async Task<bool> IsPinValid(string cardNumber, string pin)
        {
            return true;
        }
    }

}
