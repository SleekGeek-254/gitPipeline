using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Components;

namespace gitPipeline.Data
{
    public partial class AdminManagement : ComponentBase
    {
        private readonly string connectionString = "Data Source=PHANTOM\\SQLEXPRESSPIPE;Initial Catalog=adminDb;Integrated Security=True;";

        public List<MaintenanceRequest> MaintenanceRequests { get; private set; } = new List<MaintenanceRequest>();

        public string ErrorMessage { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            CheckMaintenance();
        }

        public async Task<string> GetMaintenanceSchedule()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = "SELECT TOP 1 Date, Time FROM MaintenanceSchedule ORDER BY Date DESC";
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        DateTime date = reader.GetDateTime(0); // Assuming Date column is the first column (index 0)
                        TimeSpan time = reader.GetTimeSpan(1); // Assuming Time column is the second column (index 1)
                        DateTime maintenanceDateTime = date.Date + time;

                        string maintenanceSchedule = maintenanceDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        return maintenanceSchedule;
                    }
                }

                return string.Empty; // Return an empty string if no maintenance schedule is found
            }
            catch (Exception ex)
            {
                // Handle the exception
                // You can return an error message or throw an exception if needed
                return string.Empty;
            }
        }


        public async Task CheckMaintenance()
        {
            try
            {
                MaintenanceRequests.Clear();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = "SELECT * FROM MaintenanceSchedule";
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        MaintenanceRequest maintenanceRequest = new MaintenanceRequest
                        {
                            MaintenanceScheduleId = Convert.ToInt32(reader["MaintenanceScheduleId"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = TimeOnly.Parse(reader["Time"].ToString()), // Convert to TimeOnly
                            Duration = Convert.ToInt32(reader["Duration"]),
                            Reason = reader["Reason"].ToString()
                        };

                        MaintenanceRequests.Add(maintenanceRequest);
                    }
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                // Handle the exception
                //ErrorMessage = "An error occurred while retrieving maintenance requests: " + ex.Message;
                //Console.WriteLine("An error occurred while retrieving maintenance requests: " + ex.Message);
                // You can display an error message to the user using a component property or by setting a flag
            }
        }

        public void ScheduleMaintenance(MaintenanceRequest maintenanceRequest)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO MaintenanceSchedule (Date, Time, Duration, Reason) VALUES (@Date, @Time, @Duration, @Reason)";
                    command.Parameters.AddWithValue("@Date", maintenanceRequest.Date);
                    command.Parameters.AddWithValue("@Time", maintenanceRequest.Time.ToString("HH:mm:ss")); // Convert TimeOnly to string
                    command.Parameters.AddWithValue("@Duration", maintenanceRequest.Duration);
                    command.Parameters.AddWithValue("@Reason", maintenanceRequest.Reason);
                    command.ExecuteNonQuery();
                }

                CheckMaintenance();
            }
            catch (Exception ex)
            {
                // Handle the exception
                ErrorMessage = "An error occurred while adding ScheduleMaintenance requests: " + ex.Message;
                Console.WriteLine("An error occurred while adding ScheduleMaintenance requests: " + ex.Message);
            }
        }

        public void CancelMaintenance(int maintenanceId)
        {
           
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = "DELETE FROM MaintenanceSchedule WHERE MaintenanceScheduleId = @Id";
                    command.Parameters.AddWithValue("@Id", maintenanceId);
                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        CheckMaintenance();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                ErrorMessage = "An error occurred while CancelMaintenance requests: " + ex.Message;
                Console.WriteLine("An error occurred while  CancelMaintenance requests: " + ex.Message);
            }
        }
    }

    public class MaintenanceRequest
    {
        public int MaintenanceScheduleId { get; set; } 

        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }
    }




}
