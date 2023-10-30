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

            string output = GetAddressesByTown(context);
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
                    });

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees.ToList())
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
        }
    }