using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Transactions;
using GenericsDelegatesEventsLinq;
using GenericsDelegatesEventsLinq.MyExtensions;
using static GenericsDelegatesEventsLinq.Program;
namespace GenericsDelegatesEventsLinq
{
    public class Program
    {
        public delegate decimal CalcDelegate(decimal a, decimal b);
        public delegate int IntDelegate(string ConsoleText);
        delegate void OutputTextDel(string text);

        public static void Main(string[] args)
        {
            //    IntDelegate intDelegate = new IntDelegate(GetNumber);
            //    int num1 = GetNumber("first");
            //    int num2 = GetNumber("second");
            // Console.WriteLine(num1 + num2);     
            //OutputTextDel outputTextDel = OutputTextToConsole;
            //outputTextDel("Hello World");
            //outputTextDel = OutputTextToFile;
            //outputTextDel("text to be saved in file");
            //outputTextDel += OutputTextToConsole;
            //outputTextDel("AdditionalText");
            //OutputTextDel secondOutputDel;
            //secondOutputDel = OutputTextToConsole;
            //outputTextDel = OutputTextToFile;
            //OutputTextDel multiDel = secondOutputDel + outputTextDel;
            //multiDel("NewestText");
            //Console.ReadLine();
        }
        static void OutputTextToFile(string text)
        {
            StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "textfile.txt"), true);
            sw.WriteLine(text);
            sw.Close();
        }
        static void OutputTextToConsole(string text)
        {
            Console.WriteLine($"{DateTime.Now} {text}");
        }
        static void LinqPractice()
        {
            List<Department> departments = Data.GetDepartmentsData();
            List<Employee> employees = Data.GetEmployeesData();
            //foreach(var item in employees)
            //{
            //    item.IntroduceEmployee();
            //}
            //if (employees.CheckAllElementsForPredicate(x => x.IsManager == true)) { Console.WriteLine("All employees are managers."); }
            //else { Console.WriteLine("Not all employees are managers."); }
            //if (employees.All(x => x.IsManager == true)) { Console.WriteLine("All employees are managers."); }
            //else { Console.WriteLine("Not all employees are managers."); }
            if (employees.CheckAllElementsForPredicate(item => item.Salary >= 10000))
            {
                Console.WriteLine("Everyone earns more than 10k");
            }
            else
            {
                Console.WriteLine("Not everyone earns 10k");
            }
            if (employees.All(item => item.Salary >= 10000))
            {
                Console.WriteLine("Everyone earns more than 10k");
            }
            else
            {
                Console.WriteLine("Not everyone earns 10k");
            }
            if (employees.CheckAnyElementForPredicate(item => item.Salary >= 10000))
            {
                Console.WriteLine("At least one earns more than 10k");
            }
            else
            {
                Console.WriteLine("Not everyone earns more than 10k");
            }
            if (employees.CheckAnyElementForPredicate(item => item.Salary >= 1000))
            {
                Console.WriteLine("At least one earns more than 1k");
            }
            else
            {
                Console.WriteLine("Not even one earns more than 1k");
            }
            string name = "kuba";
            if (name.CheckAnyElementForPredicate(q => char.IsWhiteSpace(q)))
            {
                Console.WriteLine("At least one element is a whitespace");
            }
            else
            {
                Console.WriteLine("not even one element is white space");
            }
            var orderedEmployees = employees.OrderBy(q => q.DepartmentId).ToList();
            Employee.OutputEmployeListToConsole(orderedEmployees);
            orderedEmployees = employees.OrderBy(q => q.Salary).ToList();
            Employee.OutputEmployeListToConsole(orderedEmployees);
            CalcDelegate calcDelegate = AddSalary;
            Console.WriteLine(calcDelegate(2000, 3000));
            calcDelegate = Employee.DecimalTestMethod;
            Console.WriteLine(calcDelegate(2000, 3000));
            var list1 = employees.Where(q => q.IsManager).ToList();
            Employee.OutputEmployeListToConsole(list1);
            var list2 = (from emp in employees
                         where emp.IsManager == true
                         select emp).ToList();
            Employee.OutputEmployeListToConsole(list2);
            Console.WriteLine((list1.SequenceEqual(list2), new EmployeeComparer()));
            var newList = employees.Where(q => q.DateOfBirth.DayOfWeek == DayOfWeek.Monday).ToList();
            var newList2 = (from emp in employees
                            where emp.DateOfBirth.DayOfWeek == DayOfWeek.Monday
                            select emp).ToList();
            Employee.OutputEmployeListToConsole(newList);
            Employee.OutputEmployeListToConsole(newList2);
            Console.WriteLine((newList.SequenceEqual(newList2), new EmployeeComparer()));
            Console.WriteLine("halo");
            Employee e = employees.First();
            Console.WriteLine(e.DateOfBirth);
            var underAgedEmployees = (from emp in employees
                                      let years = emp.DateOfBirth.Year
                                      where DateTime.Now.Year - years < 18
                                      select emp).ToList();
            var results = from emp in employees
                          join dept in departments on emp.DepartmentId equals dept.Id
                          select new
                          {
                              Fullname = emp.FirstName + " " + emp.LastName,
                              DepartmentName = dept.DepartmentName,

                          };
            foreach (var item in results)
            {
                Console.WriteLine(item.Fullname + " " + item.DepartmentName + "\n");
            }
            Employee.OutputEmployeListToConsole(underAgedEmployees);
            Console.Clear();
            var productionEmployees = (from emp in employees
                                       where emp.DepartmentId == 1
                                       orderby emp.Salary
                                       select emp).ToList();
            var averageProductionSalary = productionEmployees.Average(emp => emp.Salary);
            Console.WriteLine("Average Production Workers Salary is Equal to: " + averageProductionSalary.ToString());
            Employee.OutputEmployeListToConsole(productionEmployees);
            Console.WriteLine(productionEmployees.Count.ToString());
            var fajnySelect = from emp in employees
                              where emp.Gender == 'K'
                              && emp.Salary <= 3000
                              orderby emp.Id descending
                              select new
                              {
                                  Gender = emp.Gender,
                                  Salary = emp.Salary,
                                  Id = emp.Id,
                              };
            foreach (var item in fajnySelect)
            {
                Console.WriteLine(item.Id + " " + item.Salary + " " + item.Gender + "\n");
            }
            var fajnySelectWithMethods = employees.Join(departments, e => e.DepartmentId, f => f.Id,
                (emp, dept) => new
                {
                    Fullname = emp.FirstName + emp.LastName,
                    Date = emp.DateOfBirth,
                    DepaId = dept.Id,
                    Id = emp.Id,
                    Salary = emp.Salary
                }).OrderByDescending(emp => emp.Salary).Where(q => q.DepaId == 3);
            foreach (var item in fajnySelectWithMethods)
            {
                Console.WriteLine(item.Fullname + " " + item.DepaId + " " + item.Id + " " + item.Salary + "\n");
            }
            double averageYearOfBirth = employees.Average(q => q.DateOfBirth.Year);
            Console.WriteLine("Average employees year of birth: " + averageYearOfBirth);
            IEnumerable<Employee[]> employeeChunks = employees.Chunk(2);
            int counter = 1;
            foreach (Employee[] empChunk in employeeChunks)
            {
                Console.WriteLine($"Chunk nr: {counter}" + "\n");
                foreach (Employee emp in empChunk)
                {
                    emp.IntroduceEmployee();
                }
                counter++;
            }
            int x = employees.Count(q => q.Gender == 'K');
            Console.WriteLine(x);
            List<Employee> managerList = employees.FindAll(q => q.IsManager);
            foreach (Employee emp in managerList)
            {
                emp.IntroduceEmployee();
            }
            employees.ForEach(q => q.Salary += 5000);
            Employee.OutputEmployeListToConsole(employees);
            employees.ForEach(q => q.Salary -= 5000);
            var enumer = employees.GetEnumerator();
            Console.WriteLine(enumer);
            Console.WriteLine($"Youngest employee was born: " + employees.Max(q => q.DateOfBirth));
            List<Employee> selectQuery1 = (from emp in employees
                                           where emp.DateOfBirth.Year > 2000
                                           select emp).ToList();
            var select1 = employees.Where(q => q.DateOfBirth.Year > 2000);
            var containsLetterAInNameGroup = from emp in employees
                                             where emp.FirstName.Contains("A")
                                             select new
                                             {
                                                 Name = emp.FirstName,
                                             };
            foreach (var item in containsLetterAInNameGroup)
            {
                Console.WriteLine(item.Name);
            }
            var orderedList = employees.OrderBy(q => q.FirstName).ToList();
            Employee.OutputEmployeListToConsole(orderedList);
            var listOrderedByName = (from emp in employees
                                     orderby emp.FirstName
                                     select emp).ToList();
            Employee.OutputEmployeListToConsole(listOrderedByName);

            var groupByList = from emp in employees
                              orderby emp.DepartmentId
                              group emp by emp.DepartmentId;
            foreach (var item in groupByList)
            {
                Console.Write(item.Key + "\n");
                foreach (var employee in item)
                {
                    employee.IntroduceEmployee();
                }
            }

            var list4 = employees.Select(q => new
            {
                LastName = q.LastName,
                YearOfBirth = q.DateOfBirth.Year
            }).Where(q => DateTime.Now.Year - q.YearOfBirth >= 18);
            foreach (var item in list4)
            {
                Console.WriteLine(item.LastName + " " + item.YearOfBirth);
            }
            var list5 = from emp in employees
                        let years = DateTime.Now.Year - emp.DateOfBirth.Year
                        where years >= 18
                        select new
                        {
                            LastName = emp.LastName,
                            YearOfBirth = emp.DateOfBirth.Year
                        };
            foreach (var item in list5)
            {
                Console.WriteLine(item.LastName + " " + item.YearOfBirth);
            }

            var list6 = from emp in employees
                        where emp.IsManager == false
                        select new
                        {
                            emp.FirstName,
                            NewSalary = (double)emp.Salary * 1.1
                        };
            foreach (var item in list6)
            {
                Console.WriteLine(item.FirstName + " " + item.NewSalary);
            }
            var listGroupedByGender = (from emp in employees
                                       let today = emp.DateOfBirth.DayOfWeek
                                       where today == DateTime.Today.DayOfWeek
                                       group emp by emp.Gender).ToList();
            foreach (var itemGroup in listGroupedByGender)
            {
                Console.WriteLine(itemGroup.Key);
                foreach (var item in itemGroup)
                {
                    item.IntroduceEmployee();
                }
            }
            var listGBGusingMethods = employees.Where(e => e.DateOfBirth.DayOfWeek == DateTime.Today.DayOfWeek).GroupBy(q => q.Gender).ToList();
            foreach (var itemGroup in listGBGusingMethods)
            {
                Console.WriteLine(itemGroup.Key);
                foreach (var item in itemGroup)
                {
                    item.IntroduceEmployee();
                }
            }
            var groupByManagerAndGender = from emp in employees
                                          group emp by emp.IsManager into newGroup
                                          from emp in (
                                          from emp in newGroup
                                          group emp by emp.Gender)
                                          group emp by newGroup.Key;
            foreach (var outerGroup in groupByManagerAndGender)
            {
                Console.WriteLine(outerGroup.Key);
                foreach (var innerGroup in outerGroup)
                {
                    Console.WriteLine(innerGroup.Key);
                    foreach (var item in innerGroup)
                    {
                        item.IntroduceEmployee();
                    }
                }
            }
            var complexGroup = from emp in employees
                               let years = emp.DateOfBirth.Year
                               group emp by years > 1950 into newGroup
                               from emp in (
                               from emp in newGroup
                               group emp by emp.Gender)
                               group emp by newGroup.Key;
            foreach (var outerGroup in complexGroup)
            {
                Console.WriteLine(outerGroup.Key);
                foreach (var innerGroup in outerGroup)
                {
                    Console.WriteLine(innerGroup.Key);
                    foreach (var item in innerGroup)
                    {
                        item.IntroduceEmployee();
                    }
                }
            }
            Console.WriteLine("\n");
            var gby = from emp in employees
                      where emp.DateOfBirth.Year > 1950
                      group emp by emp.Gender;
            //Console.WriteLine((listGroupedByGender.SequenceEqual(listGBGusingMethods), new EmployeeArraysComparer())); ////compare <T>[]

            foreach (var groups in gby)
            {
                foreach (var item in groups)
                {
                    item.IntroduceEmployee();
                }
            }
            var listlist = employees.Join(departments, q => q.DepartmentId, z => z.Id,
                (emp, dept) => new
                {
                    FullName = emp.FirstName,
                    DepartmentName = dept.DepartmentName,
                    EmployeeId = emp.Id,
                    DepID = dept.Id
                }).OrderBy(q => q.EmployeeId).GroupBy(q => q.DepartmentName);
            foreach (var group in listlist)
            {
                Console.WriteLine(group.Key);
                foreach (var emp in group)
                {
                    Console.WriteLine(emp.EmployeeId + " " + emp.DepID + " " + emp.FullName + " " + emp.DepartmentName);
                }
            }
            Console.WriteLine("\n");
            var listlistQuery = (from emp in employees
                                 join dept in departments
                                 on emp.DepartmentId equals dept.Id
                                 orderby emp.Id
                                 select new
                                 {
                                     FullName = emp.FirstName + " " + emp.LastName,
                                     DepartmentName = dept.DepartmentName,
                                     EmployeeId = emp.Id,
                                     DepID = dept.Id
                                 }).GroupBy(q => q.DepartmentName);
            foreach (var group in listlistQuery)
            {
                Console.WriteLine(group.Key);
                foreach (var emp in group)
                {
                    Console.WriteLine(emp.EmployeeId + " " + emp.DepID + " " + emp.FullName + " " + emp.DepartmentName);
                }
            }
            var listGroupedByDayOfWeek = from emp in employees
                                         orderby emp.DateOfBirth.DayOfWeek
                                         group emp by emp.DateOfBirth.DayOfWeek;

            foreach (var group in listGroupedByDayOfWeek)
            {
                foreach (var emp in group)
                {
                    Console.Write($"Day of birth: {emp.DateOfBirth.DayOfWeek}"); emp.IntroduceEmployee();
                }
            }
            Console.WriteLine("\n");

            employees.Where(q => q.IsManager).ToList().ForEach(employee => employee.Salary += 0);
            Employee.OutputEmployeListToConsole((employees));
            Console.WriteLine(employees.Average(q => q.Salary));

            var elist = from emp in employees
                        where emp.DateOfBirth.Year < 2000 && emp.DateOfBirth.Year >= 1990
                        group emp by emp.Gender into gr
                        from emp in (
                        from emp in gr
                        group emp by emp.IsManager)
                        group emp by gr.Key;

            foreach (var group in elist)
            {
                Console.WriteLine(group.Key);
                {
                    foreach (var gr in group)
                    {
                        Console.WriteLine(gr.Key);
                        foreach (var emp in gr)
                        {
                            emp.IntroduceEmployee();
                        }
                    }
                }
            }
            var groupByDeptId = from emp in employees
                                group emp by emp.DepartmentId;
            foreach (var gr in groupByDeptId)
            {
                Console.WriteLine(gr.Key);
                foreach (var emp in gr)
                {
                    emp.IntroduceEmployee();
                }
            }
            var groupByManager = employees.GroupBy(e => e.IsManager);
            foreach (var gr in groupByManager)
            {
                Console.WriteLine(gr.Key);
                foreach (var emp in gr)
                {
                    emp.IntroduceEmployee();
                }
            }
            var someList = employees.Where(e => e.IsManager);
            var someList2 = employees.Where(e => !e.IsManager);
            var unionList = someList.Union(someList2).OrderBy(q => q.Id);
            Console.WriteLine("\n");
            foreach (var emp in unionList)
            {
                emp.IntroduceEmployee();
            }
            //IEnumerable<Employee> unionOf2 = groupByDeptId.Union(groupByManager);
            Console.WriteLine("\n");
            var listt = groupByDeptId.SelectMany(q => q);
            var listtt = groupByManager.SelectMany(q => q);
            var listjoin = listt.Union(listtt).OrderBy(q => q.Id).ToList();
            Console.WriteLine("\n");
            Employee.OutputEmployeListToConsole(listjoin);
            var somelist = groupByDeptId.SelectMany(q => q).Union(groupByManager.SelectMany(q => q)).OrderBy(q => q.Id).ToList();
            Console.WriteLine("\n");
            Employee.OutputEmployeListToConsole(somelist);
            Console.WriteLine("\n");
            var elist2 = employees.Where(q => q.DateOfBirth.Year >= 1990 && q.DateOfBirth.Year < 2000).GroupBy(q => q.Gender);
            foreach (var group in elist2)
            {
                Console.WriteLine(group.Key);
                {
                    foreach (var gr in group)
                    {
                        gr.IntroduceEmployee();

                    }
                }
            }
            var list45 = employees.Select(q => new
            {
                NewGender = q.Gender,
                q.IsManager,
                NewSalary = q.Salary + 5000
            }).OrderByDescending(q => q.NewSalary);
            foreach (var item in list45)
            {
                Console.WriteLine(item.NewGender + " " + item.IsManager + " " + item.NewSalary);
            }
            var joinedListsMethodSyntax = employees.Join(departments, q => q.DepartmentId, dept => dept.Id,
                (emp, dept) => new
                {
                    FullName = emp.FirstName + " " + emp.LastName,
                    NewSalary = Convert.ToDecimal(Math.Pow((double)emp.Salary, 2)),
                    DepName = dept.DepartmentName
                }).OrderBy(q => q.NewSalary).GroupBy(q => q.DepName);
            Console.WriteLine("\n");
            foreach (var group in joinedListsMethodSyntax)
            {
                Console.WriteLine(group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine(item.FullName + " " + item.DepName + " " + item.NewSalary);
                }
            }
            Console.WriteLine("\n");
            var joinedListsQuerySyntax = from emp in employees
                                         join dept in departments
                                         on emp.DepartmentId equals dept.Id
                                         select new
                                         {
                                             FullName = emp.FirstName + " " + emp.LastName,
                                             NewSalary = Convert.ToDecimal(Math.Pow((double)emp.Salary, 2)),
                                             DepName = dept.DepartmentName
                                         } into gr
                                         orderby gr.NewSalary
                                         group gr by gr.DepName;
            foreach (var group in joinedListsQuerySyntax)
            {
                Console.WriteLine(group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine(item.FullName + " " + item.DepName + " " + item.NewSalary);
                }
            }
            Console.WriteLine("\n");
            var listsx = employees.Where(q => q.Salary >= 2500 && q.DateOfBirth.DayOfWeek == DayOfWeek.Sunday).Select(q => q).ToList();
            Employee.OutputEmployeListToConsole(listsx);
            Console.WriteLine("\n");
            List<Employee> listsxQuerySyntax = (from emp in employees
                                                where emp.DateOfBirth.DayOfWeek == DayOfWeek.Sunday && emp.Salary >= 2500
                                                select emp).ToList();
            Employee.OutputEmployeListToConsole(listsxQuerySyntax);
            Console.WriteLine("\n");
            Console.WriteLine(employees.Sum(q => DateTime.Now.Year - q.DateOfBirth.Year));
            Console.WriteLine("\n");
            Console.WriteLine(employees.Average(q => DateTime.Now.Year - q.DateOfBirth.Year));
            Console.WriteLine("\n");
            List<Employee> emplList = (from ex in employees
                                       let Initials1 = ex.FirstName[0]
                                       let Initials2 = ex.LastName[0]
                                       where Initials1 == 'J' && Initials2 == 'M'
                                       select ex).ToList();
            Employee.OutputEmployeListToConsole(emplList);
            Console.WriteLine("\n");
            IEnumerable<Employee> empListMethodSyntax = employees.Where(q => q.FirstName[0] == 'J' && q.LastName[0] == 'M');
            Employee.OutputEmployeListToConsole(empListMethodSyntax);
            Console.WriteLine("\n");
            int number = employees.Where(q => q.FirstName[0] == 'J' && q.LastName[0] == 'M').Count();
            Console.WriteLine(number);
            Console.WriteLine("\n");
            decimal managerSalarySum = employees.Where(q => q.IsManager).Sum(q => q.Salary);
            decimal nonManagerSalarySum = employees.Where(q => !q.IsManager).Sum(q => q.Salary);
            Console.WriteLine(managerSalarySum);
            Console.WriteLine(nonManagerSalarySum);
            var collection = from z in employees
                             let fullNameLength = z.FirstName.Length + z.LastName.Length
                             where fullNameLength > 18
                             select z;
            Console.WriteLine("\n");
            Employee.OutputEmployeListToConsole(collection);
            Console.WriteLine("\n");
            var collectionMethodSyntax = employees.Where(q => q.FirstName.Length + q.LastName.Length > 18);
            Employee.OutputEmployeListToConsole(collection);
            Console.WriteLine("\n");
            var group1 = from emp in employees
                         group emp by emp.IsManager;
            var group2 = employees.GroupBy(q => q.IsManager);
            bool isEqual2Groups = group1.SelectMany(q => q).SequenceEqual(group2.SelectMany(q => q), new EmployeeComparer());
            Console.WriteLine(isEqual2Groups);
            Console.WriteLine("\n");
            var group3 = employees.GroupBy(q => q.Gender);
            var unionOf2Gropus = group1.SelectMany(q => q).Union(group3.SelectMany(q => q)).OrderBy(q => q.Id);
            Console.WriteLine("\n");
            Employee.OutputEmployeListToConsole(unionOf2Gropus);
            Console.WriteLine("\n");
            Employee youngestEmployee = employees.MaxBy(q => q.DateOfBirth);
            youngestEmployee.IntroduceEmployee();
            Console.WriteLine("\n");
            Employee oldestEmployee = employees.MinBy(q => q.DateOfBirth);
            oldestEmployee.IntroduceEmployee();
            employees.Clear();
            Console.WriteLine("\n");
            Employee.OutputEmployeListToConsole(employees);
            Console.WriteLine("\n");
            employees = Data.GetEmployeesData();
            Employee emp1 = new Employee { Id = 41, DateOfBirth = DateTime.Now, Gender = 'K', DepartmentId = 1, FirstName = "Kuba", IsManager = true, LastName = "Wojciechowski", Salary = 1000 };
            employees.Insert(5, emp1);
            Employee.OutputEmployeListToConsole(employees);

            Console.WriteLine("\n");
            var grouppedByMonth = employees.GroupBy(q => q.DateOfBirth.Month);
            foreach (var item in grouppedByMonth)
            {
                Console.WriteLine(item.Key);
                foreach (var employee in item)
                {
                    employee.IntroduceEmployee();
                }
            }
            Console.WriteLine("\n");
            var grouppedByMonthQuerySyntax = from employee in employees
                                             group employee by employee.DateOfBirth.Month;
            foreach (var item in grouppedByMonthQuerySyntax)
            {
                Console.WriteLine(item.Key);
                foreach (var employee in item)
                {
                    employee.IntroduceEmployee();
                }
            }
            Console.WriteLine("\n");
            var isEquals = grouppedByMonth.SelectMany(q => q).SequenceEqual(grouppedByMonthQuerySyntax.SelectMany(q => q), new EmployeeComparer());
            Console.WriteLine(isEquals);
        }
        static int GetNumber(string consoleText)
        {
            string a;
            do
            {
                Console.Clear();
                Console.Write($"Input {consoleText} number: ");
                a = Console.ReadLine();
            } while ((!a.All(q => char.IsDigit(q))) || (String.IsNullOrEmpty(a)));
            return Convert.ToInt32(a);
        }
        public class EmployeeComparer : IEqualityComparer<Employee>
        {
            public bool Equals(Employee? x, Employee? y)
            {
                if (x.FirstName == y.FirstName && x.LastName == y.LastName && x.Id == y.Id && x.IsManager == y.IsManager && x.Salary == y.Salary && x.DateOfBirth == y.DateOfBirth)
                    return true;
                return false;
            }

            public int GetHashCode([DisallowNull] Employee obj)
            {
                throw new NotImplementedException();
            }
        }
        private static decimal AddSalary(decimal salary1, decimal salary2)
        {
            return salary1 + salary2;
        }
        private static List<Employee> RaiseManagerSalary(List<Employee> employees)
        {
            foreach (var emp in employees)
            {
                if (emp.IsManager)
                {
                    emp.Salary = Convert.ToDecimal((double)emp.Salary * 1.2);
                }
                else
                    emp.Salary += 100;
            }
            return employees;
        }
    }
}