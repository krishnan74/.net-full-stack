using Day_12;

namespace Day_12{

    internal class Easy_Program
    {
        public static void Run()
        {
            EmployeePromotion promotion = new EmployeePromotion();
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n====== Employee Promotion Management ======");
                Console.WriteLine("1. Create Employee Promotion List");
                Console.WriteLine("2. Display Employee Promotion List");
                Console.WriteLine("3. Display Sorted Employee List");
                Console.WriteLine("4. Find Employee Promotion Position (by Name)");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        promotion.CollectEmployeeNames();
                        break;
                    case "2":
                        promotion.DisplayPromotionList();
                        break;
                    case "3":
                        promotion.DisplayNamesInAscendingOrder();
                        break;
                    case "4":
                        Console.WriteLine("Please enter the employee name:");
                        string input = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(input)) break;

                        promotion.FindPromotionPositionByName(input);
                        break;
                    case "5":
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