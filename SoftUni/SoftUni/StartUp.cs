using Microsoft.EntityFrameworkCore;
using SoftUni.Data;

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
            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            }

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


            string result = string.Join(Environment.NewLine, employees.Select( e=> $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));
            return result;
            }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
            {
            var employees = context.Employees
                .Select(e => new
                    {
                    e.FirstName,
                    e.Salary
                    })
                .Where(e => e.Salary > 50000 )
                .OrderBy(e => e.FirstName)
                .ToList();


            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} - {e.Salary:F2}"));
            return result;
            }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
            {
            var employees = context.Employees
                .Where(d => d.Department.Name == "Research and Development")
                .Select(e => new
                    {
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                    e.Department.Name
                    })
                .OrderBy(e => e.Salary)
                .ThenBy(e => e.Name)
                .ToList();


            return string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} from {e.Name} - {e.Salary:F2}"));
            }
        }
    }