using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CalculatorLibrary
{

    public class CalculationLog
    {
        public double Num1 { get; set; }
        public double Num2 { get; set; }
        public double Result { get; set; }
        public string? Operation { get; set; }

    }

    public class Calculator
    {  
        int counter = 0;
        string jsonFileLoc = "calculation.json";
        List<CalculationLog> calcLogList = new List<CalculationLog>();
        List<CalculationLog> jsonList = new List<CalculationLog>();
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
                    operationUsed = "Multiplcation";
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
            counter++;
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
            if (File.Exists(jsonFileLoc))
            {
                File.Delete(jsonFileLoc);
                Console.WriteLine("File deleted successfully.");
            }
        }

    }
}


