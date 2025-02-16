using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Developer_Test_Task_v4._6_by_Leo_Software.Models;
using System.Text.Json;
using System.Text;

namespace Developer_Test_Task_v4._6_by_Leo_Software.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IHttpClientFactory httpClientFactory, ILogger<AuthController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            // Prepare the request to Booksy
            var booksyRequest = new
            {
                username = request.Email,
                password = request.Password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(booksyRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Make the request to Booksy's login endpoint
            var response = await client.PostAsync(
                "https://booksy.com/pro/en-us/onboarding/Login",
                content
            );

            // Read the response
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var booksyResponse = JsonSerializer.Deserialize<BooksyLoginResponse>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Securing the access_tokken
                if (booksyResponse?.AccessToken != null)
                {
                    // Set token in a secure HTTP-only cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true, // Prevents JavaScript access
                        Secure = true,   // Ensures it's sent only over HTTPS
                        SameSite = SameSiteMode.Strict, // Prevents CSRF attacks
                        Expires = DateTime.UtcNow.AddHours(1) // Expiry time
                    };

                    Response.Cookies.Append("booksy_token", booksyResponse.AccessToken, cookieOptions);

                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Login successful",
                        Data = booksyResponse
                    });
                }
            }
            string errorMessage = "Invalid credentials";
            if (!string.IsNullOrEmpty(responseContent))
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
                    if (errorResponse != null && errorResponse.ContainsKey("message"))
                    {
                        errorMessage = errorResponse["message"];
                    }
                }
                catch (JsonException)
                {
                    errorMessage = "Unexpected error from Booksy.";
                }
            }

            _logger.LogWarning($"Failed login attempt for {request.Email}. Status: {response.StatusCode}, Message: {errorMessage}");

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = errorMessage,
                Data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login process");

            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred during login"
            });
        }
    }
}