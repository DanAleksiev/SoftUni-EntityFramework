using SoftUni.Data;

namespace SoftUni
    {
    internal class Program
        {
        static void Main(string[] args)
            {
            SoftUniContext context = new SoftUniContext();
            Console.WriteLine(context.Employees.FirstOrDefault().FirstName.ToString());
            // Scaffold-DbContext "Server=MSI\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -d
            }
        }
    }