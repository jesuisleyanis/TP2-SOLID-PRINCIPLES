namespace HotelReservation.Services;

using HotelReservation.Models;

// Exercice 5.1 — DIP : les abstractions sont définies dans le namespace du service métier.
// BookingService dépend des abstractions, pas des implémentations concrètes.

public interface IReservationStore
{
    void Add(Reservation reservation);
    Reservation? GetById(string id);
}

public interface ILogger
{
    void Log(string message);
}

public class BookingService
{
    private readonly IReservationStore _store;
    private readonly ILogger _logger;
    private int _counter = 0;

    public BookingService(IReservationStore store, ILogger logger)
    {
        _store = store;
        _logger = logger;
    }

    public string CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email)
    {
        _logger.Log($"Creating reservation for {guestName}...");

        var nights = (checkOut - checkIn).Days;
        var pricePerNight = roomType switch
        {
            "Standard" => 80m,
            "Suite" => 200m,
            "Family" => 120m,
            _ => throw new Exception($"Unknown room type: {roomType}")
        };

        _counter++;
        var reservation = new Reservation
        {
            Id = $"R-{_counter:D3}",
            GuestName = guestName,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            RoomType = roomType,
            Status = "Confirmed",
            Email = email,
            TotalPrice = nights * pricePerNight
        };

        _store.Add(reservation);
        _logger.Log($"Reservation {reservation.Id} created.");

        return reservation.Id;
    }
}
