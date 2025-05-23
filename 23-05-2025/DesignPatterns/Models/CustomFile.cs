using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Interfaces;

namespace DesignPatterns.Models
{
    public class CustomFile : IFile
    {
        private string _fileName;

        public CustomFile(string fileName)
        {
            _fileName = fileName;
        }

        public void Read()
        {
            Console.WriteLine("[Access Granted] Reading sensitive file content...");
            Console.WriteLine($"[File Content] Secret content of {_fileName}.");
        }

        public void ReadMetadata()
        {
            Console.WriteLine("[Limited Access] Reading file metadata only...");
            Console.WriteLine($"[Metadata] File: {_fileName}, Created: {DateTime.Now.ToShortDateString()}");
        }
    }

}
