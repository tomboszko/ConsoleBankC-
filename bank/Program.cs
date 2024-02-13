// Import necessary system libraries
using System;

// Define a Client class to represent a bank's client
public class Client
{
    // Properties to hold client information
    public int Id { get; set; } // Unique identifier for the client
    public string LastName { get; set; } // Client's last name
    public string FirstName { get; set; } // Client's first name
    public DateTime DateOfBirth { get; set; } // Client's date of birth
    public int PinCode { get; set; } // Personal Identification Number for security
    public DateTime DateJoined { get; set; } // The date when the client joined the bank

    // Constructor to initialize a new client with their details
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

// Define a BankAccount class to manage a client's bank account
public class BankAccount
{
    // Property to hold the account owner's details
    public Client Client { get; set; } // The client who owns the account
    public decimal Balance { get; private set; } // The current balance, private set to restrict direct modification
    public string Type { get; set; } // Type of the bank account (e.g., Savings, Checking)

    // Constructor to initialize a new bank account
    public BankAccount(Client client, string type, decimal startingBalance)
    {
        Client = client;
        Type = type;
        Balance = startingBalance;
    }

    // Method to verify the input PIN against the client's PIN
    private bool VerifyPin(int inputPin)
    {
        return Client.PinCode == inputPin; // Returns true if the pins match, otherwise false
    }

    // Method to check the balance, requires PIN verification
    public decimal CheckBalance(int inputPin)
    {
        if (VerifyPin(inputPin))
        {
            return Balance; // Return the balance if PIN is correct
        }
        else
        {
            throw new Exception("Invalid PIN."); // Throw an exception if PIN is incorrect
        }
    }

    // Method to deposit an amount into the account
    public void Deposit(decimal amount)
    {
        Balance += amount; // Adds the amount to the current balance
    }

    // Method to withdraw an amount from the account, requires PIN verification
    public bool Withdraw(decimal amount, int inputPin)
    {
        if (!VerifyPin(inputPin))
        {
            throw new Exception("Invalid PIN."); // Throw exception if PIN is incorrect
        }

        if (Balance >= amount) // Check if the balance is sufficient
        {
            Balance -= amount; // Deduct the amount from the balance
            return true; // Return true to indicate success
        }
        else
        {
            return false; // Return false if insufficient funds
        }
    }
}

// Main program class
class Program
{
    // Entry point of the program
    static void Main(string[] args)
    {
        // Create a new client instance
        Client client = new Client(1, "Boszko", "Tom", new DateTime(1982, 7, 3), 1234, DateTime.Now);
        // Create a new bank account instance for the client
        BankAccount bankAccount = new BankAccount(client, "Savings", 1000);

        // Prompt the user to enter their PIN
        Console.WriteLine("Please enter your PIN to access your account:");
        int inputPin = ReadPin(); // Call ReadPin method to get the PIN from the user

        try
        {
            // Display the current balance after verifying the PIN
            Console.WriteLine($"Current Balance: {bankAccount.CheckBalance(inputPin)}");

            string action;
            do
            {
                // Prompt the user for their desired action
                Console.WriteLine("What would you like to do? (deposit, withdraw, info, quit):");
                action = Console.ReadLine().ToLower(); // Convert input to lowercase to handle case-insensitive comparison

                switch (action)
                {
                    case "deposit":
                        Console.WriteLine("Enter amount to deposit:");
                        decimal depositAmount = Convert.ToDecimal(Console.ReadLine()); // Convert string input to decimal
                        bankAccount.Deposit(depositAmount); // Deposit the amount
                        Console.WriteLine($"New balance: {bankAccount.CheckBalance(inputPin)}"); // Display the new balance
                        break;
                    case "withdraw":
                        Console.WriteLine("Enter amount to withdraw:");
                        decimal withdrawAmount = Convert.ToDecimal(Console.ReadLine()); // Convert string input to decimal
                        if (bankAccount.Withdraw(withdrawAmount, inputPin)) // Attempt to withdraw the amount
                        {
                            Console.WriteLine($"New balance: {bankAccount.CheckBalance(inputPin)}"); // Display the new balance if successful
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds."); // Inform the user if funds are insufficient
                        }
                        break;
                    case "info":
                        // Display client and account information
                        Console.WriteLine($"Client Information:");
                        Console.WriteLine($"Name: {client.FirstName} {client.LastName}");
                        Console.WriteLine($"Date of Birth: {client.DateOfBirth.ToShortDateString()}");
                        Console.WriteLine($"Account Type: {bankAccount.Type}");
                        Console.WriteLine($"Current Balance: {bankAccount.CheckBalance(inputPin)}");
                        break;
                    case "quit":
                        Console.WriteLine("Exiting."); // Exit the program
                        break;
                    default:
                        // Handle invalid options
                        Console.WriteLine("Invalid option. Please choose deposit, withdraw, info, or quit.");
                        break;
                }
            }
            while (action != "quit"); // Repeat until the user decides to quit
        }
        catch (Exception ex)
        {
            // Catch and display any errors that occur during operation
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    // Method to securely read the PIN from the console
    static int ReadPin()
    {
        string pin = ""; // Initialize pin as an empty string
        ConsoleKeyInfo key; // To capture key press information

        do
        {
            key = Console.ReadKey(true); // Read key input without displaying it

            if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
            {
                // Allow backspace to remove the last digit
                pin = pin[0..^1]; // Remove the last character from pin
                Console.Write("\b \b"); // Erase the last * displayed
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (pin.Length > 0)
                {
                    Console.WriteLine(); // Move to the next line after pressing Enter
                    break; // Exit the loop when Enter key is pressed
                }
            }
            else if (char.IsDigit(key.KeyChar))
            {
                // Only add the character if it's a digit
                pin += key.KeyChar; // Append the digit to pin
                Console.Write("*"); // Display * for each digit entered
            }
            // Loop continues until Enter is pressed and pin is not empty
        } while (true);

        // Try to convert the pin string to an integer and return it
        return int.TryParse(pin, out int pinNumber) ? pinNumber : throw new InvalidOperationException("PIN must be numeric.");
    }
}
