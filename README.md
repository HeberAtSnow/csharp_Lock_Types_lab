# csharp_Lock_Types_lab

## Purpose
You have already read about POSIX style locking (lock, try-lock, timed-lock) for mutual exclusivity.  In this lab you will do the C# equivalents for locking.  You may use the reference page on our classroom canvas page for reference:  https://snow.instructure.com/courses/926134/pages/posix-%3Ec-number-reference  

##How to run
1) If you run the code as is (just from the starter code) you will get:
a) NoLock code will run
b) some exceptions thrown (the method or operation is not implemented) (16 times because the threadcount is set to 16)
c) then the program terminates.

##What to do
1) Line 98 (AccountTransfer_SingleLock) needs the method fixed.
        //REQUIREMENTS
        //1) use 1 lock on the entire AccountMap object
    - once fixed the program should run and finish both the 'NoLock' and now your 'SingleLock' modes - and then it will die again
2) Line 100 (AcctTransfer_DeadlockingPossible) {or whatever your new line # is} will now be throwing errors.  Fix it.
        //REQUIREMENTS:
        //
        //1) fromAccount's balance should += dollarAmt
        //2)   toAccount's balance should -= dollarAmt
        //3) MUST USE TWO LOCKS - one on the fromAccount and the second on toAccount
        //    ensure they have the 'ability' to cause a deadlock

        Special Warning:  Your deadlock WILL hang.  So adjust your switch statement to no-longer call your deadlock code
        From the starter code, commenting out line 100 would work - because it takes away the method call AND the break - so it would flow straight into the TimedLock case.

3) Now your error has moved to (AcctTransfer_TimedLock).  Fix it too.
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

Submit your code when you have it all working!