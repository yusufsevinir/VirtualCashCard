using System;
using NUnit.Framework;
using Moq;
using VirtualCashCard;
using VirtualCashCard.Repository;
using VirtualCashCard.Types;
using System.Threading.Tasks;

namespace VirtualCashCard.Tests
{
    [TestFixture]
    public class Test
    {
        public AccountRepository repository;
        public Account account;

        [SetUp]
        public void Setup()
        {
            account = new Account();
            repository = new AccountRepository(account);

            account.Balance = 100;
            account.Pin = "1234";
            account.CardNumber = "2344324234234";
        }
        
        [Test]
        public async Task AmountShouldBeEqualAfterDeposit()
        {            
            var result = await repository.Deposit(50);
            Assert.That(result, Is.True);
            Assert.AreEqual(150, account.Balance);
        }

        [Test]
        public async Task AmountShouldBeEqualAfterWithDraw()
        {
            var result = await repository.Withdraw(50);
            Assert.That(result, Is.True);
            Assert.AreEqual(50, account.Balance);
        }

        [Test]
        public async Task TransactionShouldFailAfterNegativeDeposit()
        {
            account.Balance = 100;
            account.Pin = "1234";
            account.CardNumber = "2344324234234";

            var result = await repository.Deposit(-5M);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TransactionShouldFailAfterWithDrawBecauseofNotEnoughMoney()
        {
            var result = await repository.Withdraw(150);
            Assert.That(result, Is.False);
            Assert.AreEqual(100, account.Balance);
        }

        [
        TestCase(10),
        TestCase(20),
        TestCase(30),
        TestCase(100)
        ]
        public async Task AmountShouldChangeAfterMultipleDeposits(decimal amount)
        {
            var tempBalance = account.Balance;
            var result = await repository.Deposit(amount);
            Assert.That(result, Is.True);
            Assert.AreEqual(decimal.Add(tempBalance, amount), account.Balance);
        }

        [Test]
        public void MultitaskingTransactionsShouldWorkAfterDepositFromMultiplePlaces()
        {
            var prebalance = account.Balance;

            Task task1 = Task.Factory.StartNew(async () => { await repository.Deposit(100); });
            Task task2 = Task.Factory.StartNew(async () => { await repository.Deposit(200); });
            Task task3 = Task.Factory.StartNew(async () => { await repository.Deposit(300); });

            var task = Task.Factory.ContinueWhenAll(
                new[] { task1, task2, task3 },
                innerTasks =>
                {
                    foreach (var innerTask in innerTasks)
                    {
                        Assert.That(innerTask.IsFaulted, Is.False);
                    }
                    Assert.AreEqual(account.Balance, decimal.Add(prebalance, 600));
                });
        }

        [Test]
        public void MultitaskingTransactionsShouldWorkAfterWithdrawFromMultiplePlaces()
        {
            var prebalance = account.Balance;

            Task task1 = Task.Factory.StartNew(async () => { await repository.Withdraw(5); });
            Task task2 = Task.Factory.StartNew(async () => { await repository.Withdraw(10); });
            Task task3 = Task.Factory.StartNew(async () => { await repository.Withdraw(15); });

            var task = Task.Factory.ContinueWhenAll(
                new[] { task1, task2, task3 },
                innerTasks =>
                {
                    foreach (var innerTask in innerTasks)
                    {
                        Assert.That(innerTask.IsFaulted, Is.False);
                    }
                    Assert.AreEqual(account.Balance, decimal.Subtract(prebalance, 30));
                });
        }
    }

}
