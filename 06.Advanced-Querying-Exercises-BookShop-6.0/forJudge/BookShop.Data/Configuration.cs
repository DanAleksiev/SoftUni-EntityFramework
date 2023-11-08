namespace BookShop.Data
{
    internal class Configuration
    {
        internal static string ConnectionString
            => @"Server=MSI\SQLEXPRESS;Database=BookShop;Integrated Security=True;TrustServerCertificate=True";
        }
}
