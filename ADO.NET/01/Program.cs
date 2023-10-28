using Microsoft.Data.SqlClient;

namespace _01
    {
    internal class Program
        {
        //install nuget
        //Microsoft.Data.SqlClient

        //1. connection string 
        const string conectionString = "Server=MSI\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";
        static SqlConnection connection;


        static async Task Main(string[] args)
            {

            //2. Connection to the database    
            try
                {
                connection = new SqlConnection(conectionString);
                connection.Open();

                await GetOrderedMinionsByVillionId(1);
                }

            finally
                {
                connection?.Close();
                }

            }

        //1.
        static void GetValue()
            {
            //3. Create command
            using SqlCommand getVillains = new SqlCommand(SqlQueries.getValues, connection);

            //4. Read the data
            using SqlDataReader sqlReader = getVillains.ExecuteReader();

            while (sqlReader.Read())
                {
                Console.WriteLine($"{sqlReader["Name"]} - {sqlReader["TotalMinions"]}");
                }
            }

        //2.
        static async Task NameGoesHere(int id)
            {
          
            }

        //3.
        static async Task GetOrderedMinionsByVillionId(int id)
            {
            using SqlCommand command = new SqlCommand(SqlQueries.getVillianById, connection);
            command.Parameters.AddWithValue("@id", id);
            var result = await command.ExecuteScalarAsync();
            if (result is null)
                {
                await Console.Out.WriteLineAsync($"No Villians with Id {id} exist in the databse.");
                }
            else
                {
                await Console.Out.WriteLineAsync($"Villian: {result.ToString()}");

                using SqlCommand commandGetMinionData = new SqlCommand(SqlQueries.getOrderedMynionsByVillianID, connection);
                commandGetMinionData.Parameters.AddWithValue("@id", id);

                var minionsReader = await commandGetMinionData.ExecuteReaderAsync();

                while (minionsReader.Read()) 
                    {
                    await Console.Out.WriteLineAsync($"{minionsReader["RowNum"]}. " +
                        $"{minionsReader["Name"]} {minionsReader["Age"]}");
                }
                }
            }

        }
    }