using System.Text.RegularExpressions;
using CalculatorLibrary;

namespace CalculatorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;
            string? input = "";
            string choice = "";
            bool previousCalculationsExist = File.Exists("calculation.json");
            // Display title as the C# console calculator app.
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");

            Calculator calculator = new Calculator();
            if (previousCalculationsExist)
            {
                Console.WriteLine("List of previous calculations found. Would you like to keep it? (y/n)");
                do
                {
                    input = Console.ReadLine();
                    if (input != null)
                    {
                        choice = input.ToLower().Trim();
                    }
                    switch (choice)
                    {
                        case "y":
                            Console.WriteLine("Using existing list...");
                            calculator.LoadCalculationJson();
                            break;
                        case "n":
                            Console.WriteLine("Deleting list...");
                            calculator.DeleteJson();
                            break;
                        default:
                            Console.WriteLine("Invalid input.");
                            break;
                    }
                } while (choice != "y" && choice != "n");




            }
            while (!endApp)
            {
                double previousResult = 0;
                double cleanNum1 = 0;
                double cleanNum2 = 0;
                bool num1AlreadyUsed = false;
                bool num2AlreadyUsed = false;

                int chosenOperand = 0;
                previousCalculationsExist = File.Exists("calculation.json");

                if (previousCalculationsExist)
                {
                        calculator.LoadCalculationJson();
                    Console.WriteLine("Would you like to view the list? Type y and enter if yes and Type any other key and enter if no.");
                    input = Console.ReadLine();
                    if (input != null)
                    {
                        choice = input.Trim().ToLower();
                        if (choice == "y")
                        {
                            calculator.ViewPreviousCalculations();
                            Console.WriteLine("Would you like to use any of the results here for your calculation now? Type y if yes and any other key for no.");
                            input = Console.ReadLine();
                            if (input != null)
                            {
                                choice = input.ToLower().Trim();
                                if (choice == "y")
                                {
                                    previousResult = calculator.UseResultAsOperand();
                                    Console.WriteLine("Now please choose where you would like to place the result. Type 1 if Operand 1 or 2 if Operand 2");
                                    input = Console.ReadLine();
                                    while (!int.TryParse(input, out chosenOperand) || (chosenOperand > 2 || chosenOperand < 1))
                                    {
                                        Console.WriteLine("Invalid input. Please type either 1 if Operand 1 or 2 if Operand 2");
                                        input = Console.ReadLine();
                                    }
                                    if (chosenOperand == 1)
                                    {
                                        cleanNum1 = previousResult;
                                        num1AlreadyUsed = true;
                                    }
                                    else if (chosenOperand == 2)
                                    {
                                        cleanNum2 = previousResult;
                                        num2AlreadyUsed = true;
                                    }
                                }
                            }
                        }
                    }
                }
               
                // Ask the user to choose an operator.
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\ta - Add");
                Console.WriteLine("\ts - Subtract");
                Console.WriteLine("\tm - Multiply");
                Console.WriteLine("\td - Divide");
                Console.WriteLine("\te - Taking the power");
                Console.WriteLine("\tsqr - Square Root");
                Console.WriteLine("\t10x - 10 raised to x");
                Console.Write("Your option? ");

                string? op = Console.ReadLine();

                // Validate input is not null, and matches the pattern
                while (op == null || !Regex.IsMatch(op, "^(a|s|m|d|sqr|e|10x)$"))
                {
                    Console.WriteLine("Error: Unrecognized input. Please try again.");
                    op = Console.ReadLine();
                }
                try
                {
                    // Declare variable and set to empty.
                    // Use Nullable types (with ?) to match type of System.Console.ReadLine
                    string? numInput1 = "";
                    string? numInput2 = "";
                    double result = 0;

                    // Ask the user to type the first number.
                    if (!num1AlreadyUsed)
                    {
                        Console.WriteLine("Type a number, then press Enter (Operand 1)");
                        numInput1 = Console.ReadLine();

                        while (!double.TryParse(numInput1, out cleanNum1))
                        {
                            Console.Write("This is not a valid input. Please enter a numeric value:");
                            numInput1 = Console.ReadLine();
                        }

                    }
                    // Ask the user to type the second number.
                    if (!num2AlreadyUsed)
                    {
                        Console.WriteLine("Type a number, then press Enter (Operand 2)");
                        numInput2 = Console.ReadLine();

                        while (!double.TryParse(numInput2, out cleanNum2))
                        {
                            Console.Write("This is not a valid input. Please enter a numeric value:");
                            numInput2 = Console.ReadLine();
                        }

                    }
                    result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else Console.WriteLine("Your result: {0:0.##}\n", result);
                }
                catch (DivideByZeroException e)
                {
                    Console.WriteLine("Oh no! An exception occured trying to do the math.\n - Details: " + e.Message);
                }
                Console.WriteLine($"Calculator used {calculator.calcCounter} times");
                Console.WriteLine("------------------------\n");

                // Wait for the user to respond before closing.
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;

                calculator.SaveCalculationToJSon();
            }
            return;
        }
    }
}
