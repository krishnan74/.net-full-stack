using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    // Singleton FileManager
    public sealed class FileManager : IDisposable
    {
        private static readonly Lazy<FileManager> _instance = new Lazy<FileManager>(() => new FileManager());

        private StreamWriter _writer;
        private StreamReader _reader;
        private string _filePath = "log.txt";
        private bool _disposed = false;

        // Private constructor
        private FileManager()
        {
            // Open the file once for reading and writing
            FileStream fileStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _writer = new StreamWriter(fileStream);
            _reader = new StreamReader(fileStream);
            _writer.AutoFlush = true;
        }

        public static FileManager Instance => _instance.Value;

        public void Write(string message)
        {
            _writer.WriteLine(message);
        }

        public void ReadAll()
        {
            _reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _reader?.Dispose();
                _writer?.Dispose();
                _disposed = true;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Writing to file");
            var fileManager = FileManager.Instance;

            fileManager.Write("Hello World");
            fileManager.Write("How was the world");
            fileManager.Write("This file was only opened once.");

            Console.WriteLine("\nReading from file...");
            fileManager.ReadAll();

            // Dispose to close file resources once
            fileManager.Dispose();

            Console.WriteLine("\nDone. File was opened and closed once.");
        }
    }
}

