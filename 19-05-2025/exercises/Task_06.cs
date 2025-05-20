namespace CSharpExercises

{
    
    //6) Count the Frequency of Each Element
    //Given an array, count the frequency of each element and print the result.
    //Input: { 1, 2, 2, 3, 4, 4, 4}

    //output
    //1 occurs 1 times  
    //2 occurs 2 times  
    //3 occurs 1 times  
    //4 occurs 3 times
    internal class Task_06
    {
        static int GetValidNumber()
        {
            int num;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.Write("Invalid input. Please try again: ");
            }
            return num;
        }

        static Dictionary<int, int> CountFrequencies(int[] arr)
        {
            Dictionary<int, int> frequency = new Dictionary<int, int>();

            foreach (int num in arr)
            {
                if (frequency.ContainsKey(num))
                {
                    frequency[num]++;
                }
                else
                {
                    frequency[num] = 1;
                }
            }
            return frequency;

        }

        public static void Run()
        {
            Console.Write("Enter the size of the array: ");
            int n;
            while (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
            {
                Console.Write("Invalid input. Enter a positive integer: ");
            }

            int[] arr = new int[n];

            Console.WriteLine($"\nEnter {n} integers.\n");

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Enter number {i + 1}: ");
                arr[i] = GetValidNumber();
            }

            Dictionary<int, int> frequencyDictionary = CountFrequencies(arr);

            Console.WriteLine("\nFrequencies of numbers: ");
            foreach (var freqDict in frequencyDictionary)
            {
                Console.WriteLine($"{freqDict.Key} occurs {freqDict.Value} times.");
            }
        }
    }
}