using System.Data;
using System.Data.SqlClient;

namespace TaskMangementSystem.Models
{
    public class DatabaseHelper(IConfiguration configuration)
    {
        private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task<List<TaskModel>> GetTasks(string? userId)
        {
            var tasks = new List<TaskModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetUserTasks", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tasks.Add(new TaskModel
                    {
                        Id = reader["Id"] != System.DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                        Title = reader["Title"] != System.DBNull.Value ? Convert.ToString(reader["Title"]) : "",
                        Description = reader["Description"] != System.DBNull.Value ? Convert.ToString(reader["Description"]) : "",
                        DueDate = Convert.ToDateTime(reader["DueDate"]),
                        IsComplete = reader["IsComplete"] != System.DBNull.Value ? Convert.ToBoolean(reader["IsComplete"]) : false,
                        UserId = reader["UserId"] != System.DBNull.Value ? Convert.ToString(reader["UserId"]) : ""
                    });
                }
            }
            return tasks;
        }

        public async Task CreateTask(TaskModel task)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("CreateTask", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@DueDate", task.DueDate);
            command.Parameters.AddWithValue("@UserId", task.UserId);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<TaskModel?> GetTaskById(int id, string? userId)
        {
            TaskModel? task = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetTaskById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    task = new TaskModel
                    {
                        Id = reader["Id"] != System.DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                        Title = reader["Title"] != System.DBNull.Value ? Convert.ToString(reader["Title"]) : "",
                        Description = reader["Description"] != System.DBNull.Value ? Convert.ToString(reader["Description"]) : "",
                        DueDate = Convert.ToDateTime(reader["DueDate"]),
                        IsComplete = reader["IsComplete"] != System.DBNull.Value ? Convert.ToBoolean(reader["IsComplete"]) : false,
                        UserId = reader["UserId"] != System.DBNull.Value ? Convert.ToString(reader["UserId"]) : ""
                    };
                }
            }

            return task;
        }

        public async Task UpdateTask(TaskModel task)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UpdateTask", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@DueDate", task.DueDate);
            command.Parameters.AddWithValue("@UserId", task.UserId);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteTask(int id, string? userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DeleteTask", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task MarkTaskComplete(int id, string? userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("MarkTaskComplete", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

}
