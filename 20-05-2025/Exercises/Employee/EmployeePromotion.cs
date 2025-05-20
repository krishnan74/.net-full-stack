using System;
using System.Collections.Generic;

namespace Day_12{
    class EmployeePromotion
    {
        private List<string> eligibleEmployees = new List<string>();

        public void CollectEmployeeNames()
        {
            Console.WriteLine("Please enter the employee names in the order of their eligibility for promotion (Enter blank to stop):");

            while (true)
            {
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;

                eligibleEmployees.Add(input);
            }

            Console.WriteLine($"\nThe current size of the collection is {eligibleEmployees.Capacity}");

            eligibleEmployees.TrimExcess(); 

            Console.WriteLine($"The size after removing the extra space is {eligibleEmployees.Capacity}");
        }

        public void DisplayPromotionList()
        {
            Console.WriteLine("\n--- Promotion Eligibility List ---");
            for (int i = 0; i < eligibleEmployees.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {eligibleEmployees[i]}");
            }
        }

        public void DisplayNamesInAscendingOrder()
        {
            if (eligibleEmployees.Count == 0)
            {
                Console.WriteLine("No employees in the promotion list.");
                return;
            }

            List<string> sortedList = new List<string>(eligibleEmployees);
            sortedList.Sort();

            Console.WriteLine("\n--- Employee Names in Ascending Order ---");
            foreach (var name in sortedList)
            {
                Console.WriteLine(name);
            }
        }

        public void FindPromotionPositionByName(string employeeName)
        {
            int index = eligibleEmployees.IndexOf(employeeName);

            if (index != -1)
                Console.WriteLine($"\n\"{employeeName}\" is at position {index + 1} for promotion.");
            else
                Console.WriteLine($"\n\"{employeeName}\" is not found in the promotion list.");
        }
    }
}