using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.SOLID
{
    // Single Responsibility - Each class has one responsibility
    public class Invoice
    {
        public List<string> Items = new List<string>();
        public void AddItem(string item) => Items.Add(item);
        public decimal CalculateTotal() => Items.Count * 10; 
    }

    public class InvoicePrinter
    {
        public void Print(Invoice invoice)
        {
            Console.WriteLine("Invoice Items:");
            invoice.Items.ForEach(Console.WriteLine);
            Console.WriteLine("Total: " + invoice.CalculateTotal());
        }
    }

    public class InvoiceSaver
    {
        public void SaveToDatabase(Invoice invoice)
        {
            Console.WriteLine("Invoice saved to DB.");
        }
    }

    // Open Closed - Open for extension, closed for modification
    public abstract class Shape
    {
        public abstract double Perimeter();
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }
        public override double Perimeter() => Math.PI * Radius * 2;
    }

    public class Square : Shape
    {
        public double Side { get; set; }
        public override double Perimeter() => Side * 4;
    }

    public class PerimeterCalculator
    {
        public double TotalPerimeter(List<Shape> shapes)
        {
            return shapes.Sum(s => s.Perimeter());
        }
    }

    // Liskov Substitution + Interface Segregation - Replace Bird with separate interfaces
    public interface IFlyable
    {
        void Fly();
    }

    public interface IWalkable
    {
        void Walk();
    }

    public class Eagle : IFlyable, IWalkable
    {
        public void Fly() => Console.WriteLine("Eagle flying.");
        public void Walk() => Console.WriteLine("Eagle walking.");
    }

    public class Hen : IWalkable
    {
        public void Walk() => Console.WriteLine("Hen walking.");
    }

    // Depencdency Inversion - High-level depends on abstraction
    public interface IMessageSender
    {
        void SendMessage(string message);
    }

    public class EmailSender : IMessageSender
    {
        public void SendMessage(string message) => Console.WriteLine($"Email: {message}");
    }

    public class SmsSender : IMessageSender
    {
        public void SendMessage(string message) => Console.WriteLine($"SMS: {message}");
    }

    public class Notification
    {
        private readonly IMessageSender _sender;

        public Notification(IMessageSender sender)
        {
            _sender = sender;
        }

        public void Send(string message)
        {
            _sender.SendMessage(message);
        }
    }

}