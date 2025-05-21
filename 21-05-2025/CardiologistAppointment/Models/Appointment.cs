using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardiologistAppointment.Models
{
    public class Appointment : IComparable<Appointment>, IEquatable<Appointment>
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        public Appointment()
        {
            PatientName = string.Empty;
        }
        public Appointment(int id, int age, string name, DateTime date, string reason)
        {
            Id = id;
            PatientName = name;
            PatientAge = age;
            AppointmentDate = date;
            Reason = reason;
        }

        public void TakeAppointmentDetailsFromUser()
        {
            
            Console.WriteLine("Please enter the Patient name");
            PatientName = Console.ReadLine() ?? "";
            Console.WriteLine("Please enter the Patient age");
            int age;
            while (!int.TryParse(Console.ReadLine(), out age))
            {
                Console.WriteLine("Invalid entry for age. Please enter a valid Appointment age");
            }
            PatientAge = age;
            Console.WriteLine("Please enter the Appointment Date ( Format: dd/mm/yyyy )");
            
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date) || date < DateTime.Now )
            {
                Console.WriteLine("Invalid entry for date. Please enter a valid future Appointment date");
            }
            AppointmentDate = date;
            Console.WriteLine("Please enter the reason for the appointment");
            Reason = Console.ReadLine() ?? "";
            Console.WriteLine(

                "Appointment details are as follows:\n" +
                "Name: " + PatientName + "\n" +
                "Age: " + PatientAge + "\n" +
                "Date: " + AppointmentDate.ToString("dd/MM/yyyy") + "\n" +
                "Reason: " + Reason + "\n");
            Console.WriteLine(
                "Please confirm the appointment details (Y/N)");
            string? confirmation = Console.ReadLine();
            if (confirmation?.ToUpper() == "Y")
            {
                Console.WriteLine("Appointment confirmed \n");
            }
            else
            {
                Console.WriteLine("Appointment cancelled \n");
            }
                
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {PatientName}, Age: {PatientAge}, Date: {AppointmentDate.ToString("dd/MM/yyyy")}, Reason: {Reason}";
        }

        public int CompareTo(Appointment? other)
        {
            return this.Id.CompareTo(other?.Id);
        }

        public bool Equals(Appointment? other)
        {
            return this.Id == other?.Id;
        }
    }


}
