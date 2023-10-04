using System;
using System.Threading;

public class BankAccount
{
    private readonly Mutex _mutex = new();

    private decimal _balance;
    private bool _isOpen;

    public void Open() => _isOpen = true;

    public void Close() => _isOpen = false;

    public decimal Balance => _isOpen ? _balance : throw new InvalidOperationException();

    public void UpdateBalance(decimal change)
    {
        if (!_isOpen)
            throw new InvalidOperationException("Account is closed");

        _mutex.WaitOne();

        try
        {
            _balance += change;
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }
}
