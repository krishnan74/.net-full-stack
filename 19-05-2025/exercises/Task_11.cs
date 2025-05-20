namespace CSharpExercises
{
    // 11)  In the question ten extend it to validate a sudoku game.
    // Validate all 9 rows(use int[,] board = new int[9, 9])

    internal class Task_11
    {
        static int GetValidNumber()
        {
            int num;
            while (!int.TryParse(Console.ReadLine(), out num) || num < 1 || num > 9)
            {
                Console.Write("Invalid input. Enter a number between 1 and 9: ");
            }
            return num;
        }

        

        static bool AreColumnsValid(int[,] board)
        {
            for (int j = 0; j < 9; j++)
            {
                int[] column = new int[9];
                for (int i = 0; i < 9; i++)
                {
                    column[i] = board[i, j];
                }

                if (!IsValidGroup(column))
                {
                    Console.WriteLine($"\nColumn {j + 1} is invalid.");
                    return false;
                }
            }
            return true;
        }

        static bool AreSubgridsValid(int[,] board)
        {
            for (int rowStart = 0; rowStart < 9; rowStart += 3)
            {
                for (int colStart = 0; colStart < 9; colStart += 3)
                {
                    int[] box = new int[9];
                    int index = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            box[index++] = board[rowStart + i, colStart + j];
                        }
                    }

                    if (!IsValidGroup(box))
                    {
                        Console.WriteLine($"\n3x3 box starting at ({rowStart + 1},{colStart + 1}) is invalid.");
                        return false;
                    }
                }
            }
            return true;
        }


        static bool IsValidGroup(int[] group)
        {
            HashSet<int> seen = new();
            foreach (int num in group)
            {
                if (num < 1 || num > 9 || !seen.Add(num))
                   return false;
            }
            return true;
        }


        static bool AreRowsValid(int[,] board)
        {
            for (int i = 0; i < 9; i++)
            {
                int[] row = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    row[j] = board[i, j];
                }

                if (!IsValidGroup(row))
                {
                    Console.WriteLine($"\nRow {i + 1} is invalid.");
                    return false;
                }
            }
            return true;
        }


        public static void Run()
        {

            int[,] board = new int[9, 9];

            Console.WriteLine("Enter the Sudoku board elements row by row");

            for (int i = 0; i < 9; i++)
            {
               Console.WriteLine($"\nRow {i + 1}:");
               for (int j = 0; j < 9; j++)
               {
                   Console.Write($"Element {j + 1}: ");
                   board[i, j] = GetValidNumber();
               }
            }

            for (int i = 0; i < board.GetLength(0); i++) 
            {
                for (int j = 0; j < board.GetLength(1); j++) 
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }

            if (AreRowsValid(board) && AreColumnsValid(board) && AreSubgridsValid(board))
            {
                Console.WriteLine("Valid Sudoku Board!");
            }
            else
            {
                Console.WriteLine("\nThe Sudoku board is invalid.");
            }
        }
    }
}