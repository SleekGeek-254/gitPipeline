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

        private List<MaintenanceRequest> maintenanceRequests = new List<MaintenanceRequest>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            CheckMaintenance();
        }

        private void CheckMaintenance()
        {
            try
            {
                maintenanceRequests.Clear();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = "SELECT * FROM MaintenanceSchedule WHERE Date >= @StartDate AND Date <= @EndDate";
                    command.Parameters.AddWithValue("@StartDate", DateTime.Now);
                    command.Parameters.AddWithValue("@EndDate", DateTime.Now.AddDays(1));

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        MaintenanceRequest maintenanceRequest = new MaintenanceRequest
                        {
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = Convert.ToDateTime(reader["Time"]),
                            Duration = Convert.ToInt32(reader["Duration"]),
                            Reason = reader["Reason"].ToString()
                        };

                        maintenanceRequests.Add(maintenanceRequest);
                    }
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
        }

        private void ScheduleMaintenance(MaintenanceRequest maintenanceRequest)
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

        private void CancelMaintenance(int maintenanceId)
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
        public DateTime Time { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }
    }

}
