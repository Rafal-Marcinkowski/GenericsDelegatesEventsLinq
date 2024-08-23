using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GenericsDelegatesEventsLinq
{
    public static class Data
    {
        private static List<Employee> employees = new List<Employee>();

        private static List<Department> departments = new List<Department>();

        public static List<Employee> GetEmployeesData()
        {
            StreamReader streamReader = new StreamReader("EmployeeDataTextFile.txt");
            int counter = 1;
            while (!streamReader.EndOfStream)
            {
                Employee employee = new Employee();
                string[] lineWords = streamReader.ReadLine().Trim().Split("\t");
                employee.Id = counter;
                employee.FirstName = lineWords[0];
                employee.LastName = lineWords[1];
                employee.Gender = Convert.ToChar(lineWords[2]);
                DateTime date = DateTime.Parse(lineWords[3]);
                employee.DateOfBirth = date;
                employee.DepartmentId = Convert.ToInt32(lineWords[4]);
                employee.Salary = Convert.ToInt32(lineWords[5]);
                if (lineWords[6] == "PRAWDA")
                {
                    employee.IsManager = true;
                }
                else
                    employee.IsManager = false;
                employees.Add(employee);
                counter++;
            }
            return employees;
        }
        public static List<Department> GetDepartmentsData()
        {
            Department department = new Department
            {
                Id = 1,
                DepartmentName = "Production"
            };
            departments.Add(department);
            department = new Department { Id = 2, DepartmentName = "IT" };
            departments.Add(department);
            department = new Department { Id = 3, DepartmentName = "HR" };
            departments.Add(department);
            return departments;
        }
    }
}
