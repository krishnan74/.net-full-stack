using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointment.Interfaces;
using CardiologistAppointment.Models;


namespace CardiologistAppointment
{
    public class ManageAppointment
    {
        private readonly IAppointmentService _appointmentService;

        public ManageAppointment(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public void Start()
        {
            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                int option = 0;
                while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 && option > 2))
                {
                    Console.WriteLine("Invalid entry. Please enter a valid option");
                }
                switch (option)
                {
                    case 1:
                        AddAppointment();
                        break;
                    case 2:
                        SearchAppointment();
                        break;
                    default:
                        exit = true;
                        break;
                }
            }
        }
        public void PrintMenu()
        {
            Console.WriteLine("Choose what you wanted");
            Console.WriteLine("1. Add Appointment");
            Console.WriteLine("2. Search Appointment");
        }
        public void AddAppointment()
        {
            Appointment appointment = new Appointment();
            appointment.TakeAppointmentDetailsFromUser();
            int id = _appointmentService.AddAppointment(appointment);
            Console.WriteLine($"Appointment created with ID: {id} \n");
        }
        public void SearchAppointment()
        {
            var searchMenu = PrintSearchMenu();
            var appointments = _appointmentService.SearchAppointment(searchMenu);
            Console.WriteLine("The search options you have selected");
            Console.WriteLine(searchMenu);
            if ((appointments == null))
            {
                Console.WriteLine("No Appointments for the search");
            }
            PrintAppointments(appointments);
            Console.WriteLine("\n");

        }

        private void PrintAppointments(List<Appointment>? appointments)
        {
            foreach (var appointment in appointments)
            {
                Console.WriteLine(appointment);
            }
        }

        private AppointmentSearchModel PrintSearchMenu()
        {
            Console.WriteLine("Please select the search option");
            AppointmentSearchModel searchModel = new AppointmentSearchModel();
            int selectOption = 0;
            
            Console.WriteLine("1. Search by Patient Name. ? Type 1 for yes Type 2 no");
            while (!int.TryParse(Console.ReadLine(), out selectOption) || (selectOption != 1 && selectOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if (selectOption == 1)
            {
                Console.WriteLine("Please enter the Patient Name");
                string name = Console.ReadLine() ?? "";
                searchModel.PatientName = name;
                selectOption = 0;
            }
            Console.WriteLine("2. Search by Patient Age. Please enter 1 for yes and 2 for no");
            while (!int.TryParse(Console.ReadLine(), out selectOption) || (selectOption != 1 && selectOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if (selectOption == 1)
            {
                searchModel.AgeRange = new Range<int>();
                int age;
                Console.WriteLine("Please enter the min Patient Age");
                while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
                {
                    Console.WriteLine("Invalid entry for min age. Please enter a valid patient age");
                }
                searchModel.AgeRange.MinAge = age;
                Console.WriteLine("Please enter the max Patient Age");
                while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
                {
                    Console.WriteLine("Invalid entry for max age. Please enter a valid patient age");
                }
                searchModel.AgeRange.MaxAge = age;
                selectOption = 0;
            }

            Console.WriteLine("3. Search by Appointment Date. Please enter 1 for yes and 2 for no");
            
            while (!int.TryParse(Console.ReadLine(), out selectOption) || (selectOption != 1 && selectOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }


            DateTime date;
            if (selectOption == 1)

            {
                Console.WriteLine("Please enter the Appointment Date in the format of MM/DD/YYYY");
                while (!DateTime.TryParse(Console.ReadLine(), out date))
                {
                    Console.WriteLine("Invalid entry for date. Please enter a valid Appointment date");
                }

                searchModel.AppointmentDate = date;
                selectOption = 0;
            }

            return searchModel;
        }
    }
}