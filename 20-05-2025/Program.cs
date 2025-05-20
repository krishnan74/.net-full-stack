using System;

namespace CsharpFundamentals
{
    class Program
    {
        public class Post
        {
            public string Caption { get; set; }
            public int Likes { get; set; }
        }

        static void Main(string[] args)
        {
            Console.Write("Enter number of users: ");
            int userCount = int.Parse(Console.ReadLine());

            Post[][] userPosts = new Post[userCount][];

            for (int i = 0; i < userCount; i++)
            {
                Console.Write($"User {i + 1}: How many posts? ");
                int postCount = int.Parse(Console.ReadLine());
                userPosts[i] = new Post[postCount];

                for (int j = 0; j < postCount; j++)
                {
                    Console.Write($"Enter caption for post {j + 1}: ");
                    string caption = Console.ReadLine();

                    Console.Write("Enter likes: ");
                    int likes = int.Parse(Console.ReadLine());

                    userPosts[i][j] = new Post { Caption = caption, Likes = likes };
                }
            }

            Console.WriteLine("\n--- Displaying Instagram Posts ---");
            for (int i = 0; i < userPosts.Length; i++)
            {
                Console.WriteLine($"User {i + 1}:");
                for (int j = 0; j < userPosts[i].Length; j++)
                {
                    var post = userPosts[i][j];
                    Console.WriteLine($"Post {j + 1} - Caption: {post.Caption} | Likes: {post.Likes}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}