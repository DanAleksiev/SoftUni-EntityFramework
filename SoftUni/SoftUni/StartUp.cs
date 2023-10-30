using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace SoftUni
    {
    public class StartUp
        {
        static void Main(string[] args)
            {

            // Scaffold - DbContext "Server=MSI\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer - o Models - d
            // cls to lean the manager

            //Scaffold-DbContext -Connection "Server=MSI\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data/Models


            SoftUniContext context = new SoftUniContext();

            string output = RemoveTown(context);
            Console.WriteLine(output);
            }
        //3
        public static string GetEmployeesFullInformation(SoftUniContext context)
            {
            var employees = context.Employees
                .Select(e => new
                    {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                    }).ToList();


            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));
            return result;
            }
        //4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
            {
            var employees = context.Employees
                .Select(e => new
                    {
                    e.FirstName,
                    e.Salary
                    })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();


            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} - {e.Salary:F2}"));
            return result;
            }
        //5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
            {
            var employees = context.Employees
                .Where(d => d.Department.Name == "Research and Development")
                .Select(e => new
                    {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                    })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
                {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:F2}");
                }

            return sb.ToString().TrimEnd();
            }

        //6
        public static string AddNewAddressToEmployee(SoftUniContext context)
            {
            Address address = new Address()
                {
                AddressText = "Vitoshka 15",
                TownId = 4
                };

            var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Select(e => new
                    {
                    e.AddressId,
                    e.Address.AddressText
                    })
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
                {
                sb.AppendLine(e.AddressText);
                }

            string result = sb.ToString().Trim();
            return result;
            }

        //7
        public static string GetEmployeesInPeriod(SoftUniContext context)
            {
            var employees = context.Employees
                .Take(10)
                .Include(e => e.EmployeesProjects)
                .Include(e => e)
                .Select(e => new
                    {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                            {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = ep.Project.EndDate.ToString != null
                                ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                : "not finished"
                            })
                    }).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
                {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                if (e.Projects.Any())
                    {
                    sb.AppendLine(string.Join(Environment.NewLine, e.Projects
                        .Select(p => $"--{p.ProjectName} - {p.StartDate} - {p.EndDate}")));
                    }
                }

            string result = sb.ToString().Trim();
            return result;
            }

        //8
        public static string GetAddressesByTown(SoftUniContext context)
            {
            string[] employees = context.Addresses
                .Take(10)
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => $"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees")
                .ToArray();
            return string.Join(Environment.NewLine, employees);
            }

        //9

        public static string GetEmployee147(SoftUniContext context)
            {
            const int empIndex = 147;

            var employees = context.Employees
              .Where(e => e.EmployeeId == empIndex)
              .Select(e => new
                  {
                  e.FirstName,
                  e.LastName,
                  e.JobTitle,
                  Projects = e.EmployeesProjects
                        .OrderBy(p => p.Project.Name)
                        .Select(ep => new
                            {
                            ep.Project.Name,
                            })
                  }).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees.ToList())
                {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

                if (e.Projects.Any())
                    {
                    sb.AppendLine(string.Join(Environment.NewLine, e.Projects
                        .Select(p => $"{p.Name}")));
                    }
                }

            string result = sb.ToString().Trim();
            return result;
            }

        //10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
            {
            var departments = context.Departments
                .Where(a => a.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .OrderBy(d => d.Name)
                .Select(d => new
                    {
                    d.Name,
                    d.Manager,
                    })
                .ToList();

            var employees = context.Employees
                .Select(e => new
                    {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Department
                    });

            StringBuilder sb = new StringBuilder();
            foreach (var d in departments)
                {
                sb.AppendLine($"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}");
                foreach(var e in employees.Where(e => e.Department.Name == d.Name).OrderBy(e =>e.FirstName).ThenBy(e => e.LastName))
                    {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                    }
                
                }

            return sb.ToString().Trim();
            }

        //11
        public static string GetLatestProjects(SoftUniContext context)
            {
            var projects = context.Projects
                .OrderByDescending(p=>p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                    {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach(var p in projects)
                {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate);
                }

            return sb.ToString().Trim();
            }

        //12
        public static string IncreaseSalaries(SoftUniContext context)
            {
            decimal modifier = 1.12m;

            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
                {
                e.Salary *= modifier; 
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
                }
            //context.SaveChanges();

            return sb.ToString().Trim();
            }

        //13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
            {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                    {
                    e.FirstName, 
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                    })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
                {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
                }

            return sb.ToString().Trim();
            }

        //14
        public static string DeleteProjectById(SoftUniContext context)
            {

            var project = context.Projects
                .Where(e => e.ProjectId == 2);

            context.Projects.RemoveRange(project);
            //context.SaveChanges();

            var topTenProjects = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();

            return string.Join(Environment.NewLine, topTenProjects); 
            }

        //15
        public static string RemoveTown(SoftUniContext context)
            {
            const string target = "Seattle";


            var town = context.Towns
                .FirstOrDefault(t => t.Name == target);


            var addreses = context.Addresses
                .Where(a => a.Town.Name == target)
                .ToArray();
            
            var employeesToRemoveAddress = context.Employees
                .Where(e => addreses.Contains(e.Address))
                .ToArray();
            

            foreach (var a in employeesToRemoveAddress)
                {
                a.AddressId = null;
                }
            context.Addresses.RemoveRange(addreses);
            context.Towns.RemoveRange(town);
            //context.SaveChanges();


            return $"{addreses.Count()} addresses in Seattle were deleted";
            }
        }
    }