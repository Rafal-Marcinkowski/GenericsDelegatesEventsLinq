using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GenericsDelegatesEventsLinq
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary {  get; set; }
        public bool IsManager { get; set; }
        public void IntroduceEmployee()
        {
            Console.WriteLine($"{FirstName}" + " " + LastName + " is working in department: " + DepartmentId+" for the salary of: "+Salary+ ". Manager: "+IsManager+
                ". Was born: "+DateOfBirth+". Gender: "+Gender+"\n");
        }
        public static void OutputEmployeListToConsole(IEnumerable<Employee>senderList)
        {
            foreach (Employee emp in senderList)
            {
                emp.IntroduceEmployee();
            }
        }
        public static  decimal DecimalTestMethod(decimal value1, decimal value2)
        { return value1; }
    }
}
