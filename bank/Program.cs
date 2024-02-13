﻿using System;

public class Client
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int PinCode { get; set; }
    public DateTime DateJoined { get; set; }

    public Client(int id, string lastName, string firstName, DateTime dateOfBirth, int pinCode, DateTime dateJoined)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
        DateOfBirth = dateOfBirth;
        PinCode = pinCode;
        DateJoined = dateJoined;
    }
}

public class BankAccount
{
    public Client Client { get; set; }
    public decimal Balance { get; private set; }
    public string Type { get; set; }

    public BankAccount(Client client, string type)
    {
        Client = client;
        Type = type;
    }

    private bool VerifyPin(int inputPin)
    {
        return Client.PinCode == inputPin;
    }

    public decimal CheckBalance(int inputPin)
    {
        if (VerifyPin(inputPin))
        {
            return Balance;
        }
        else
        {
            throw new Exception("Invalid PIN.");
        }
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public bool Withdraw(decimal amount, int inputPin)
    {
        if (!VerifyPin(inputPin))
        {
            throw new Exception("Invalid PIN.");
        }

        if (Balance >= amount)
        {
            Balance -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Client client = new Client(1, "Boszko", "Tom", new DateTime(1982, 7, 3), 1234, DateTime.Now);
        BankAccount bankAccount = new BankAccount(client, "Savings");

        Console.WriteLine("Please enter your PIN to access your account:");
        int inputPin = ReadPin();

        try
        {
            bankAccount.CheckBalance(inputPin); // Juste pour vérifier le PIN
            Console.WriteLine("\nPIN verified.");

            string action;
            do
            {
                Console.WriteLine("What would you like to do? (deposit, withdraw, info, quit):");
                action = Console.ReadLine().ToLower();

                switch (action)
                {
                    case "deposit":
                        Console.WriteLine("Enter amount to deposit:");
                        decimal depositAmount = Convert.ToDecimal(Console.ReadLine());
                        bankAccount.Deposit(depositAmount);
                        Console.WriteLine($"New balance: {bankAccount.CheckBalance(inputPin)}");
                        break;
                    case "withdraw":
                        Console.WriteLine("Enter amount to withdraw:");
                        decimal withdrawAmount = Convert.ToDecimal(Console.ReadLine());
                        if (bankAccount.Withdraw(withdrawAmount, inputPin))
                        {
                            Console.WriteLine($"New balance: {bankAccount.CheckBalance(inputPin)}");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds.");
                        }
                        break;
                    case "info":
                        Console.WriteLine($"Client Information:");
                        Console.WriteLine($"Name: {client.FirstName} {client.LastName}");
                        Console.WriteLine($"Date of Birth: {client.DateOfBirth.ToShortDateString()}");
                        Console.WriteLine($"Account Type: {bankAccount.Type}");
                        Console.WriteLine($"Current Balance: {bankAccount.CheckBalance(inputPin)}");
                        break;
                    case "quit":
                        Console.WriteLine("Exiting.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose deposit, withdraw, info, or quit.");
                        break;
                }
            }
            while (action != "quit");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nAn error occurred: " + ex.Message);
        }
    }

    static int ReadPin()
    {
        string pin = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true); //true = pas affiché

            if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
            {
                pin = pin[0..^1]; 
                Console.Write("\b \b"); 
            }
            else if (key.Key != ConsoleKey.Enter)
            {
                pin += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        return int.TryParse(pin, out int pinNumber) ? pinNumber : 0;
    }
}
