using EF_Fundamnatals.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_Fundamnatals
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


           //using SoftUniContext context = new SoftUniContext();
           //
           //var employee = await context.Employees
           //    .Include(e => e.Department)
           //    .Include(e => e.Manager)
           //    .Include(e => e.Projects)
           //    .FirstOrDefaultAsync(e => e.EmployeeId == 143);
           //
           //var manager = await context.e
           //
           //Console.WriteLine($"{employee.FirstName} {employee.LastName} and his manager is {employee.ManagerId}");




        }
        }
    }