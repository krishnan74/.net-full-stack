namespace Day_12{
    internal class Medium_Program
    {
        public static void Run()
        {
            Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

            while (true)
            {
                Employee emp = new Employee();
                emp.TakeEmployeeDetailsFromUser();

                if (!employees.ContainsKey(emp.Id))
                    employees.Add(emp.Id, emp);
                else
                    Console.WriteLine("Duplicate ID, try again.");

                Console.Write("Add another employee? (y/n): ");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }

            Console.WriteLine("\nSorted Employees by Salary:");
            var sortedList = employees.Values.ToList();
            sortedList.Sort();
            foreach (var emp in sortedList)
                Console.WriteLine(emp);


            Console.Write("\nEnter ID to find employee: ");
            int searchId = Convert.ToInt32(Console.ReadLine());
            var found = employees.Values.FirstOrDefault(e => e.Id == searchId);
            Console.WriteLine(found != null ? found.ToString() : "Not Found");


            Console.Write("\nEnter name to find all matching employees: ");
            string name = Console.ReadLine();
            var matches = employees.Values.Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            foreach (var emp in matches)
                Console.WriteLine(emp);


            Console.Write("\nEnter ID of employee to compare age: ");
            int baseId = Convert.ToInt32(Console.ReadLine());
            if (employees.TryGetValue(baseId, out Employee baseEmp))
            {
                var older = employees.Values.Where(e => e.Age > baseEmp.Age);
                Console.WriteLine($"\nEmployees older than {baseEmp.Name}:");
                foreach (var emp in older)
                    Console.WriteLine(emp);
            }
            else
            {
                Console.WriteLine("Employee ID not found.");
            }
        }
    }
}