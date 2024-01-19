using System.Data.SqlClient;

namespace Lesson3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=Office;Trusted_Connection=True;TrustServerCertificate=True;";
            Transaction(connectionString);
        }

        static void InsertRecord(SqlTransaction transaction, int id, int departmentID, string name, string position, decimal salary)
        {
            string insertQuery = "INSERT INTO Worker (id, Name, Position, Salary) VALUES (@Id, @Name, @Position, @Salary)";

            using (SqlCommand command = new SqlCommand(insertQuery, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@DepartmentID", departmentID);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Position", position);
                command.Parameters.AddWithValue("@Salary", salary);

                command.ExecuteNonQuery();
            }
        }
        static void Transaction(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("pt_NewWorker1");

                try
                {
                    InsertRecord(transaction, 5, 2, "NewWorker1", "TestPosition", 10000);
                    transaction.Save("pt_NewWorker1");
                    InsertRecord(transaction, 6, 3, "NewWorker2", "TestPosition", 20000);
                }
                catch (Exception ex)
                {
                    transaction.Rollback("pt_NewWorker1");
                }
                finally
                {
                    transaction.Commit();
                }
            }
        }
    }
}
