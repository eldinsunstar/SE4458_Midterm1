public class BookingDate
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int HouseId { get; set; }
    public House? House { get; set; }
}

public class BookingRequest
{
    private readonly MyDbContext _context;

    public BookingRequest(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<BookingRequest> GetBookingRequests()
    {
        return _context.BookingRequests.ToList();
    }
    public int Id { get; set; }
    public int HouseId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<string>? Names { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public House? House { get; set; }
}