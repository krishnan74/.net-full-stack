using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.SOLID
{
    class Program
    {
        static void Main(string[] args)
        {
            // SRP
            var invoice = new Invoice();
            invoice.AddItem("Book");
            invoice.AddItem("Pen");
            new InvoicePrinter().Print(invoice);
            new InvoiceSaver().SaveToDatabase(invoice);

            // OCP
            var shapes = new List<Shape> { new Circle { Radius = 2 }, new Square { Side = 3 } };
            var areaCalculator = new PerimeterCalculator();
            Console.WriteLine("Total area: " + areaCalculator.TotalPerimeter(shapes));

            // LSP + ISP
            IFlyable eagle = new Eagle();
            IWalkable eagleWalkable = new Eagle();
            
            eagle.Fly();
            eagleWalkable.Walk();

            IWalkable hen = new Hen();
            hen.Walk();

            // DIP
            IMessageSender sender = new EmailSender();
            IMessageSender sender2 = new SmsSender();
            var notification = new Notification(sender);
            var smsNotification = new Notification(sender2);
            notification.Send("SOLID principles followed!");
            smsNotification.Send("SMS Notification sent!");
        }
    }
}
