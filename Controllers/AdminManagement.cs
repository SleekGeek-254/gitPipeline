using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace gitPipeline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminManagement : ControllerBase
    {

        /* ## TL:Dr
        >> Create a scheduled time for updates
        >> Create a unscheduled/impromptu time for updates
        >> Ability to cancel both scheduled unscheduled

        >> The endpoint should handle the Db logic for this data
        >> The endpoint should communicate with the user 

        */

        private readonly string connectionString = "Data Source=PHANTOM;Initial Catalog=adminDb;Integrated Security=True;";
        
        [HttpPost("scheduleMaintenance")]
        public IActionResult ScheduleMaintenance(MaintenanceRequest maintenanceRequest)
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
                
                return Ok("Maintenance scheduled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to schedule maintenance: {ex.Message}");
            }
        }
        
        [HttpGet("checkMaintenance")]
        public IActionResult CheckMaintenance()
        {
            try
            {
                List<MaintenanceRequest> maintenanceRequests = new List<MaintenanceRequest>();
                
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
                
                if (maintenanceRequests.Count > 0)
                {
                    // Display pop-up notification or alert to inform users about scheduled maintenance
                    // You can return maintenanceRequests to the front-end and handle the notification there
                    return Ok(maintenanceRequests);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to check maintenance: {ex.Message}");
            }
        }
        
        [HttpPost("cancelMaintenance")]
        public IActionResult CancelMaintenance(int maintenanceId)
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
                        return Ok("Maintenance canceled successfully.");
                    }
                    else
                    {
                        return NotFound("Maintenance not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to cancel maintenance: {ex.Message}");
            }
        }
    }
    
    public class MaintenanceRequest
    {
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }
    }
}



//Readme
/*
This code represents a C# ASP.NET Core API controller called "AdminManagement" that handles maintenance 
scheduling and cancellation. Let's go through it step by step:

The code begins with the necessary using statements, which import the required namespaces and classes for 
the controller.

The AdminManagement class is defined as a derived class from ControllerBase and decorated with the [ApiController]
 and [Route("[controller]")] attributes. This indicates that it is an API controller and sets the route for the
  controller's actions to be based on the controller's name.

The connectionString variable holds the connection string to connect to a SQL Server database. It specifies the
 server name (Data Source), initial catalog (database name), and integrated security (Windows authentication).

The controller defines three actions: ScheduleMaintenance, CheckMaintenance, and CancelMaintenance.

a. ScheduleMaintenance is an HTTP POST action that takes a MaintenanceRequest object as a parameter. It attempts
 to insert the maintenance request data into the MaintenanceSchedule table in the database. It opens a connection
  to the database, creates a SQL command with parameters, executes the command, and returns an appropriate 
  response.

b. CheckMaintenance is an HTTP GET action that retrieves the scheduled maintenance requests for the current day. 
It retrieves the data from the MaintenanceSchedule table by executing a SQL command with parameters to filter by
 the current date. The retrieved maintenance requests are stored in a list and returned as the API response. If 
 no maintenance requests are found, a NoContent response is returned.

c. CancelMaintenance is an HTTP POST action that cancels a scheduled maintenance request. It takes the 
maintenanceId as a parameter, which identifies the specific maintenance request to be canceled. It executes a 
SQL command to delete the corresponding record from the MaintenanceSchedule table based on the provided ID. The 
response indicates whether the cancellation was successful or not.

The MaintenanceRequest class represents the data structure for a maintenance request. It has properties for Date, 
Time, Duration, and Reason.

Overall, this code defines an API controller that provides functionality for scheduling maintenance requests, 
checking scheduled maintenance, and canceling scheduled maintenance in a SQL Server database.


*/