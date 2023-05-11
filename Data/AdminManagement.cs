using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Components;

namespace gitPipeline.Data
{
    public partial class AdminManagement : ComponentBase
    {
        private readonly string connectionString = "Data Source=PHANTOM;Initial Catalog=adminDb;Integrated Security=True;";

        public List<MaintenanceRequest> MaintenanceRequests { get; private set; } = new List<MaintenanceRequest>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            CheckMaintenance();
        }
        public void CheckMaintenance()
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
                Console.WriteLine("An error occurred while retrieving maintenance requests: " + ex.Message);
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
                    command.Parameters.AddWithValue("@Time", maintenanceRequest.Time);
                    command.Parameters.AddWithValue("@Duration", maintenanceRequest.Duration);
                    command.Parameters.AddWithValue("@Reason", maintenanceRequest.Reason);
                    command.ExecuteNonQuery();
                }

                CheckMaintenance();
            }
            catch (Exception ex)
            {
                // Handle the exception
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
            }
        }
    }

    public class MaintenanceRequest
    {
        public int Id { get; set; } // Add the Id property

        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; } // Change the type to TimeOnly
        public int Duration { get; set; }
        public string Reason { get; set; }
    }




}
