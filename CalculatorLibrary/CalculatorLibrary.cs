﻿
using Newtonsoft.Json;
using System.Diagnostics.Metrics;


namespace CalculatorLibrary
{

    public class CalculationLog
    {
        public double Num1 { get; set; }
        public double Num2 { get; set; }
        public double Result { get; set; }
        public string? Operation { get; set; }

    }

    public class CalculatorTracker
    {
        public int Counter = 0;
    }

    public class Calculator
    {  
        string jsonFileLoc = "calculation.json";
        string counterFileLoc = "counter.json";
        List<CalculationLog> calcLogList = new List<CalculationLog>();
        List<CalculationLog> jsonList = new List<CalculationLog>();
        CalculatorTracker counter = new CalculatorTracker();
        public int calcCounter = 0;
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            // Use a switch statement to do the math.
            string operationUsed = "";
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    operationUsed = "Addition";
                    break;
                case "s":
                    result = num1 - num2;
                    operationUsed = "Subtraction";
                    break;
                case "m":
                    result = num1 * num2;
                    operationUsed = "Multiplication";
                    break;
                case "d":
                    //Ask the user to enter a non-zero divisor
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        operationUsed = "Division";
                    }
                    else
                    {
                        throw new DivideByZeroException("Mathematical Error. Dividing by Zero");
                    }
                    break;
                // Return text for an incorrect option entry
                default:
                    break;
            }
            if (!File.Exists(jsonFileLoc))
            {
                calcLogList.Add(new CalculationLog { Num1 = num1, Num2 = num2, Operation = operationUsed, Result = result });
            }
            else
            {
                jsonList.Add(new CalculationLog { Num1 = num1, Num2 = num2, Operation = operationUsed, Result = result });
            }
            if (!File.Exists (counterFileLoc))
            {
                counter.Counter++;
            }
            else
            {
                string jsonCounter = File.ReadAllText(counterFileLoc);
                int index = jsonCounter.IndexOf (": ");
                string number = jsonCounter.Substring(index + 1).Trim().TrimEnd('\r', '\n', '}');
                counter.Counter = Convert.ToInt32 (number);
                counter.Counter++;
            }
            calcCounter = counter.Counter;
            
            return result;
        }

        public void SaveCalculationToJSon()
        {
            if (!File.Exists(jsonFileLoc))
            {
                string calcLogJson = JsonConvert.SerializeObject(calcLogList, Formatting.Indented);
                File.WriteAllText(jsonFileLoc, calcLogJson);
            }
            else
            {
                string calcLogJson = JsonConvert.SerializeObject(jsonList, Formatting.Indented);
                File.WriteAllText(jsonFileLoc, calcLogJson);
            }
            SaveCounterToJson();
            
        }

        private void SaveCounterToJson()
        {
            string counterLogJson = JsonConvert.SerializeObject(counter, Formatting.Indented);
            File.WriteAllText(counterFileLoc, counterLogJson);
        }

        public void LoadCalculationJson()
        {
            if (File.Exists(jsonFileLoc))
            {
                string? input = "";
                string choice = "";
                Console.WriteLine("List of previous calculations found. Would you like to use it? (y/n)");
                do
                {
                    input = Console.ReadLine();
                    if (input != null)
                    {
                        choice = input.ToLower();
                        switch (choice)
                        {
                            case "y":
                                Console.WriteLine("Using existing list...");
                                string jsonFile = File.ReadAllText(jsonFileLoc);
                                jsonList = JsonConvert.DeserializeObject<List<CalculationLog>>(jsonFile);
                                break;
                            case "n":
                                DeleteJson();
                                break;
                            default:
                                Console.WriteLine("Invalid input.");
                                break;
                        }
                    }
                } while (choice != "y" && choice != "n");
               
            }
            
        }
        private void DeleteJson()
        {
            File.Delete(jsonFileLoc);
            Console.WriteLine("File deleted successfully.");
        }

        public void ViewPreviousCalculations()
        {
            Console.WriteLine("Viewing previous calculations...");
            string operationUsed = "";

            if (File.Exists(jsonFileLoc))
            {
                for (int i = 0; i < jsonList.Count; i++)
                {
                    switch (jsonList[i].Operation)
                    {
                        case "Addition":
                            operationUsed = "+";
                            break;
                        case "Subtraction":
                            operationUsed = "-"; 
                            break;
                        case "Multiplication":
                            operationUsed = "*";
                            break;
                        case "Division":
                            operationUsed = "/";
                            break;
                    }
                    Console.WriteLine($"{i + 1}. {jsonList[i].Num1} {operationUsed} {jsonList[i].Num2} = {jsonList[i].Result}");
                }
            }
        }

        public double UseResultAsOperand()
        {
            string? input = "";
            int chosenCalc = 0;

            Console.WriteLine("Enter the number at the left of the calculation of the result you wish to use.");
            input = Console.ReadLine();
            while (!int.TryParse(input, out chosenCalc) || (chosenCalc > jsonList.Count || chosenCalc < 1))
            {
                Console.WriteLine("Invald input. Please type the number at the left of the calculation you wish to choose.");
                input = Console.ReadLine();
            }
            return jsonList[chosenCalc - 1].Result;

        }
    }
}


