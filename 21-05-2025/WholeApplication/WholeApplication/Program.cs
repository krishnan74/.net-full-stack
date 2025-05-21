using WholeApplication.Models;

namespace WholeApplication
{
    internal class Program
    {
        static void Main(string[] args)
            {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n====== Employee Management Menu ======");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Print All Employees");
                Console.WriteLine("3. Find Employee by ID");
                Console.WriteLine("4. Modify Employee (by ID)");
                Console.WriteLine("5. Delete Employee (by ID)");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;
                    case "2":
                        PrintAllEmployees();
                        break;
                    case "3":
                        PrintEmployeeById();
                        break;
                    case "4":
                        ModifyEmployee();
                        break;
                    case "5":
                        DeleteEmployee();
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select between 1 and 6.");
                        break;
                }
            }
        }
        
            
    }
}
