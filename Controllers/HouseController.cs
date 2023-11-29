using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class HouseController : ControllerBase
{
    private readonly MyDbContext _context;

    public HouseController(MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<House>> GetAllHouses()
    {
        return await _context.Houses.ToListAsync();
    }
    private static List<House> houses = new List<House>
    {
        new House { Id = 1, Name = "We Bare Bears", Description = "A lovely cottage in the woods, with 3 bears(A Panda, Grizzly and a Polar)", Amenities = new List<string> { "Wi-Fi", "Kitchen", "1 bed" }, BookedDates = new List<BookingDate>() },
        new House { Id = 2, Name = "Cozy Cottage", Description = "A mediocre at best cottage in the outskirts of town", Amenities = new List<string> { "Wi-Fi", "Kitchen", "Parking" }, BookedDates = new List<BookingDate>() }
    };
    private static Dictionary<string, string> users = new Dictionary<string, string>
    {
        { "username", "password" }
    };

    [HttpGet("query-houses")]
    public IActionResult QueryHouses([FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] int numPeople, [FromQuery] int page = 1, [FromQuery] int perPage = 5)
    {
        try
        {
            var availableHouses = houses.Where(house => HouseAvailable(house, from, to, numPeople)).ToList();

            var paginatedHouses = availableHouses.Skip((page - 1) * perPage).Take(perPage);

            return Ok(paginatedHouses);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("book-stay")]
    public IActionResult BookStay([FromBody] BookingRequest request)
    {
        try
        {
            if (!CheckAuth(request.Username, request.Password))
            {
                return Unauthorized(new { error = "Authentication failed" });
            }

            var house = houses.FirstOrDefault(h => h.Id == request.HouseId);

            if (house == null || !HouseAvailable(house, request.From, request.To, request.Names?.Count ?? 0))
            {
                return BadRequest(new { status = "House not available for the specified dates" });
            }

            house.BookedDates.Add(new BookingDate { From = request.From, To = request.To });

            return Ok(new { status = "Booking successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private bool HouseAvailable(House house, DateTime from, DateTime to, int numPeople)
    {
        return house.BookedDates != null && house.BookedDates.All(date => from >= date.To || to <= date.From) && numPeople <= (house.Amenities?.Count ?? 0);
    }

    private bool CheckAuth(string? username, string? password)
    {
        return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && users.ContainsKey(username) && users[username] == password;
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
