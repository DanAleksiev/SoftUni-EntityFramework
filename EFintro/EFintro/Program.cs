using EFintro.Models;
using Microsoft.EntityFrameworkCore;

namespace EFintro
    {
    internal class Program
        {
        static async Task Main(string[] args)
            {
            // make sure the names are currenct and not modyfied by .net 
            // consol use
            // dotnet ef dbcontext scaffold "Server=MSI\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer
            // vs us
            // Scaffold-DbContext "Server=MSI\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -d


            using SoftUniContext context = new SoftUniContext();

            //const int employeeNum = 147;
            //var employee = await context.Employees
            //    .Include(e => e.Department)
            //    .Include(e => e.Manager)
            //    .Include(e => e.Projects)
            //    .FirstOrDefaultAsync(e => e.EmployeeId == employeeNum);

            //string query = context.Employees
            //    .Include(e => e.Department)
            //    .Include(e => e.Manager)
            //    .Include(e => e.Projects)
            //    .Where(e => e.EmployeeId == employeeNum)
            //    .ToQueryString();


            //Console.WriteLine($"Name: {employee?.FirstName} {employee?.LastName}." +
            //    $"\nDepartment: {employee?.Department.Name}." +
            //    $"\nManager: {employee?.Manager?.FirstName} {employee?.Manager?.LastName}" +
            //    $"\nProject: {employee?.Projects?.OrderBy(P => P.StartDate)?.FirstOrDefault()?.Name}");
            //await Console.Out.WriteLineAsync("-------------------------------------------------");
            //await Console.Out.WriteLineAsync(query);

            //await context.Projects.Remove(new Project
            //    {
            //    Name = "Gosho OtPochivka",
            //    StartDate = DateTime.Now,
            //    Description = "Rapper ta Drunka"
            //    }).GetDatabaseValuesAsync();

            await context.Projects.AddAsync(new Project
                {
                Name = "Judge System",
                StartDate = new DateTime(2023, 1, 26),
                Description = "Rapper ta Drunka"
                });

            await context.SaveChangesAsync();

            await Console.Out.WriteLineAsync("done");

        }
        }
    }