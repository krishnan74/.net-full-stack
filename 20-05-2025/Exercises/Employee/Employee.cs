using System;
using System.Collections.Generic;

namespace Day_12{ 
    
    class Employee : IComparable<Employee>
    {
        int id, age; 
        string name; 
        double salary; 

        public Employee(){

        }

        public Employee( int id, int age, string name, double salary){
            this.id = id;
            this.age = age;
            this.name = name;
            this.salary = salary;
        }

        public void TakeEmployeeDetailsFromUser(bool includeId = true)
        {
            if (includeId)
            {
                Console.WriteLine("Please enter the employee ID"); 
                id = Convert.ToInt32(Console.ReadLine()); 
            }

            Console.WriteLine("Please enter the employee name"); 
            name = Console.ReadLine(); 

            Console.WriteLine("Please enter the employee age"); 
            age = Convert.ToInt32(Console.ReadLine()); 

            Console.WriteLine("Please enter the employee salary"); 
            salary = Convert.ToDouble(Console.ReadLine()); 
        }

        public int CompareTo(Employee other)
        {
            return this.Salary.CompareTo(other.Salary);
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Age: {Age}, Salary: {Salary}";
        }
        
        public int Id { get => id; set => id = value; } 

        public int Age { get => age; set => age = value; } 

        public string Name { get => name; set => name = value; } 

        public double Salary { get => salary; set => salary = value; } 
    }
}