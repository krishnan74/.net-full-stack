using System;

namespace CSharpExercises
{
    // Take 2 numbers from user, check the operation user wants to perform (+,-,*,/). Do the operation and print the result

    internal class Task_03
{
        static double GetValidNumber()
        {
            double num;
            while (!double.TryParse(Console.ReadLine(), out num))
            {
                Console.Write("Invalid input. Enter a valid number: ");
            }
            return num;
        }  

        static bool IsValidOperator(string op)
        {
            return op == "+" || op == "-" || op == "*" || op == "/";
        }

        static double Calculate(double num1, double num2, string op)
        {
            double result = 0;
            switch (op)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Division by zero is not allowed.");
                    }
                    else
                    {
                        result = num1 / num2;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
            return result;
        }

        public static void Run()
        {
            Console.Write("Enter first number: ");

            double num1 = GetValidNumber();

            Console.Write("\nEnter second number: ");

            double num2 = GetValidNumber();

            Console.Write("\nEnter operation (+, -, *, /): ");
            string? operation = Console.ReadLine();

            if (!IsValidOperator(operation))
            {
                Console.WriteLine("Invalid operator.");
                return;
            }

            double result = Calculate(num1, num2, operation);

            Console.WriteLine($"\nResult: {num1} {operation} {num2} = {result}");

        }
    }
}