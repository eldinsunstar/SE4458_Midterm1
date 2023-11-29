public record House(
    int Id,
    string Name,
    string Description,
    List<string> Amenities,
    List<BookingDate> BookedDates)
{
    public House() : this(default, default, default, new List<string>(), new List<BookingDate>())
    {
    }
}