using System;

namespace Day_12
{
    
    internal class Hard_Program
    {
        static Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

        public static void Run()
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

        static void AddEmployee()
        {
            Employee emp = new Employee();
            emp.TakeEmployeeDetailsFromUser(includeId: true);

            if (employees.ContainsKey(emp.Id))
            {
                Console.WriteLine("An employee with this ID already exists. Use a unique ID.");
                return;
            }

            employees.Add(emp.Id, emp);
            Console.WriteLine("Employee added successfully.");
        }

        static void PrintAllEmployees()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employee records available.");
                return;
            }

            Console.WriteLine("\n--- Employee List ---");
            foreach (var emp in employees.Values)
                Console.WriteLine(emp);
        }

        static void PrintEmployeeById()
        {
            Console.Write("Enter Employee ID to search: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID input.");
                return;
            }

            if (employees.TryGetValue(id, out Employee emp))
            {
                Console.WriteLine(emp);
            }
            else
            {
                Console.WriteLine("Employee with this ID not found.");
            }
        }

        static void ModifyEmployee()
        {
            Console.Write("Enter Employee ID to modify: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID input.");
                return;
            }

            if (employees.TryGetValue(id, out Employee emp))
            {
                Console.WriteLine("Enter new details (ID cannot be changed):");
                emp.TakeEmployeeDetailsFromUser(includeId: false);
                Console.WriteLine("Employee details updated successfully.");
            }
            else
            {
                Console.WriteLine("Employee with this ID not found.");
            }
        }

        static void DeleteEmployee()
        {
            Console.Write("Enter Employee ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID input.");
                return;
            }

            if (employees.Remove(id))
            {
                Console.WriteLine("Employee deleted successfully.");
            }
            else
            {
                Console.WriteLine("Employee with this ID not found.");
            }
        }
    }

}