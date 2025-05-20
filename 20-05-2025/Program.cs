using Day_12;
using System;

namespace Day_12
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n====== Employee Management Tasks ======");
                Console.WriteLine("1. Easy Task");
                Console.WriteLine("2. Medium Task");
                Console.WriteLine("3. Hard Task");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option (1-3): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Easy_Program.Run();
                        break;
                    case "2":
                        Medium_Program.Run();
                        break;
                    case "3":
                        Hard_Program.Run();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select between 1 and 4.");
                        break;
                }
            }
        }
    }
}