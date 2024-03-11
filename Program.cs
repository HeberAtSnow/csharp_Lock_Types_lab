using System.Diagnostics;

public class BankAccount
{
    public BankAccount()
    {
        AcctNumber = Random.Shared.Next() % 10_000_000;//only 7 digits long MAX size - Dups could exist
        CurrentBalance = 0.0M;
    }

    public int AcctNumber { get; set; }
    public decimal CurrentBalance { get; set; }
}
public class BankBranch
{
    public enum ConcurrencyChoice { NoLock, SingleLock, DoubleLock_DeadlockPossible, DoubleLock_Safe };
    public static ConcurrencyChoice cc = ConcurrencyChoice.NoLock; //<<<<<<< Changes which locking strategy you are using

    private static Dictionary<int, BankAccount> AccountMap = new();
    private static void AcctTransfer_DeadlockingPossible(decimal dollarAmt, int fromAccount, int toAccount)
    {
        //REQUIREMENTS:
        //
        //1) fromAccount's balance should += dollarAmt
        //2)   toAccount's balance should -= dollarAmt
        //3) MUST USE TWO LOCKS - one on the fromAccount and the second on toAccount
        //    ensure they have the 'ability' to cause a deadlock
        throw new NotImplementedException();
    }

    private static void AcctTransfer_TimedLock(decimal dollarAmt, int fromAccount, int toAccount)
    {
        //REQUIREMENTS:
        //1) Using the Monitor class,  take a lock on the fromAccount (lock1)
        //2) Using the Monitor class,  TRY to take a lock on toAccount (lock2),
        //                             but give up if you can't get it within timeoutInMilliseconds value (5 in sample code)
        //                             KEEP TRYING until it it works or you exceed numberOfTriesLimit (loop required)
        //3) No matter what type of errors occur you MUST:
        //                            - release lock1 correctly
        //                            - release lock2 correctly
        //4) Abort Message.  If you FAIL to successfully get lock2 (because your loop exceeded numberOfTriesLimit)
        //   then you do not do a transfer.  (requirement #3 is still in effect)
        //   Printout the following to the console:
        //      string msg = $"Thread {Thread.CurrentThread.ManagedThreadId,3} Could not process transaction: dollarAmt={dollarAmt}, from={fromAccount}, to={toAccount}";
        //      if (!transferSuccessful) Console.WriteLine(msg);

        int timeoutInMilliseconds = 5;
        int failureCount = 0;
        int numberOfTriesLimit = 2;
        bool transferSuccessful = false;

        throw new NotImplementedException();

    }

    private static void AccountTransfer_NoLock(decimal dollarAmt, int fromAccount, int toAccount)
    {
        //FREE CODE - this method is provided for you complete.
        //Notice, no mutual-exclusion is going on, no lock protection
        AccountMap[fromAccount].CurrentBalance += dollarAmt;
        AccountMap[toAccount].CurrentBalance -= dollarAmt;
    }
    private static void AccountTransfer_SingleLock(decimal dollarAmt, int fromAccount, int toAccount)
    {
        //REQUIREMENTS
        //1) use 1 lock on the entire AccountMap object
        AccountMap[fromAccount].CurrentBalance += dollarAmt;
        AccountMap[toAccount].CurrentBalance -= dollarAmt;
        throw new NotImplementedException();

    }
    public BankBranch()
    {
        for (int i = 0; i < 1000; i++) //change from 100 to 100_000 for less collisions
        {
            AccountMap[i] = new BankAccount();
        }
    }

    public static void doNTransactions(object n)
    {
        for (int i = 1; i <= (int)n; i++)
        {
            int fromAccount = Random.Shared.Next(0, AccountMap.Count);
            int toAccount = Random.Shared.Next(0, AccountMap.Count);
            if (fromAccount == toAccount)
                if (toAccount > 50)
                    toAccount--;
                else
                    toAccount++;
            decimal theAmmount = (decimal)Math.Round(Random.Shared.NextDouble(), 2);

            switch (cc)
            {
                case ConcurrencyChoice.NoLock:
                    AccountTransfer_NoLock(theAmmount, fromAccount, toAccount); break;
                case ConcurrencyChoice.SingleLock:
                    AccountTransfer_SingleLock(theAmmount, fromAccount, toAccount); break;
                case ConcurrencyChoice.DoubleLock_DeadlockPossible:
                    AcctTransfer_DeadlockingPossible(theAmmount, fromAccount, toAccount); break;
                case ConcurrencyChoice.DoubleLock_Safe:
                    AcctTransfer_TimedLock(theAmmount, fromAccount, toAccount); break;
                default:
                    throw new ArgumentException("Invalid ConcurrencyChoice (cc) setting. FIX your CODE!");
            }

            //Progress Update Line:
            if (i % 100_000 == 0) Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3}: {Math.Round((decimal)i / (int)n, 2) * 100}% done");
        }

    }
    public string getBalanceOfAllAccounts()
    {
        decimal sum = 0m;
        foreach (var a in AccountMap.Keys)
        {
            sum += AccountMap[a].CurrentBalance;
        }
        if (Math.Round(sum, 2) == 0.00M)
            return "ALL EQUAL";
        else return $"BALANCES off by {sum}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        BankBranch bb = new();
        const int numThreads = 16;
        Thread[] myThreads = new Thread[numThreads];

        //TODO:  Change cc to match the type of Concurrency Strategy you are going to use for this run.
        /*       choices:
        BankBranch.ConcurrencyChoice.NoLock
        BankBranch.ConcurrencyChoice.SingleLock
        BankBranch.ConcurrencyChoice.DoubleLock_DeadlockPossible
        BankBranch.ConcurrencyChoice.DoubleLock_Safe
        ***************************************************************************/

        foreach (BankBranch.ConcurrencyChoice x in Enum.GetValues(typeof(BankBranch.ConcurrencyChoice)))
        {
            BankBranch.cc = x;

            //Create Threads
            for (int i = 0; i < numThreads; i++)
                myThreads[i] = new Thread(new ParameterizedThreadStart(BankBranch.doNTransactions));

            Stopwatch watch = new();
            watch.Start();
            //Start Threads at N number of transactions
            for (int i = 0; i < numThreads; i++)
                myThreads[i].Start(20_000_000 / numThreads);

            //Wait until ALL are done
            for (int i = 0; i < numThreads; i++)
                myThreads[i].Join();
            watch.Stop();
            Console.WriteLine(bb.getBalanceOfAllAccounts());
            Console.WriteLine($"Program took {watch.ElapsedMilliseconds} milliseconds to run \"{BankBranch.cc}\" locking mode");
        }

    }
}