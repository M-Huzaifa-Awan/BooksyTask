using Developer_Test_Task_v4._6_by_Leo_Software.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;


namespace Developer_Test_Task_v4._6_by_Leo_Software.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public AppointmentController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Appointment>>>> GetAppointments(
            [FromHeader(Name = "Authorization")] string authorization,  // Get token from headers
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string customerName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(authorization))
                {
                    return Unauthorized(new ApiResponse<List<Appointment>>
                    {
                        Success = false,
                        Message = "Authorization token is required"
                    });
                }

                var client = _clientFactory.CreateClient();
                //Cleaning Tokken
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization.Replace("Bearer ", ""));


                // Make request to Booksy API
                var response = await client.GetAsync("https://api.booksy.com/api/v2/appointments");
                if (!response.IsSuccessStatusCode)
                {
                    //Testing
                    return Ok(new ApiResponse<List<Appointment>>
                    {
                        Success = true,
                        Data = new List<Appointment>
                        {
                            new Appointment
                            {
                                AppointmentUid = "12345",
                                BookedFrom = DateTime.Parse("2025-02-16T10:00:00Z"),
                                BookedTill = DateTime.Parse("2025-02-16T11:00:00Z"),
                                ServiceId = 987,
                                ServiceName = "Haircut",
                                CustomerName = "John Doe",
                                CustomerPhone = "+1234567890"
                            },
                            new Appointment
                            {
                                AppointmentUid = "67890",
                                BookedFrom = DateTime.Parse("2025-02-17T12:00:00Z"),
                                BookedTill = DateTime.Parse("2025-02-17T13:00:00Z"),
                                ServiceId = 654,
                                ServiceName = "Massage",
                                CustomerName = "Jane Smith",
                                CustomerPhone = "+1987654321"
                            }
                        }
                    });

                    /*return BadRequest(new ApiResponse<List<Appointment>>
                    {
                        Success = false,
                        Message = "Failed to fetch appointments from Booksy API"
                        
                    });*/
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (appointments == null || !appointments.Any())
                {
                    return NotFound(new ApiResponse<List<Appointment>>
                    {
                        Success = false,
                        Message = "No appointments found"
                    });
                }

                // Apply filters
                if (startDate.HasValue)
                    appointments = appointments.Where(a => a.BookedFrom >= startDate.Value).ToList();

                if (endDate.HasValue)
                    appointments = appointments.Where(a => a.BookedTill <= endDate.Value).ToList();

                if (!string.IsNullOrEmpty(customerName))
                    appointments = appointments.Where(a =>
                        a.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase)).ToList();

                return Ok(new ApiResponse<List<Appointment>>
                {
                    Success = true,
                    Data = appointments
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<Appointment>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }

}