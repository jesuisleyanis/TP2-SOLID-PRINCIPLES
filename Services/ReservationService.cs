namespace HotelReservation.Services;

using HotelReservation.Models;
using HotelReservation.Repositories;

// Exercice 1.1 — SRP : APPLICATION LAYER
// Orchestration du workflow de réservation.
public class ReservationService
{
    private readonly ReservationRepository _reservationRepo;
    private readonly ReservationDomainService _domainService;

    public ReservationService()
    {
        _reservationRepo = new ReservationRepository();
        var roomRepo = new RoomRepository();
        _domainService = new ReservationDomainService(_reservationRepo, roomRepo);
    }

    public string CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email)
    {
        Console.WriteLine($"[LOG] Creating reservation for {guestName}...");

        var room = _domainService.ValidateRoom(roomId, guestCount);
        _domainService.CheckAvailability(roomId, checkIn, checkOut);
        var total = _domainService.CalculatePrice(room, checkIn, checkOut);

        var reservation = new Reservation
        {
            Id = _reservationRepo.NextId(),
            GuestName = guestName,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            RoomType = roomType,
            Status = "Confirmed",
            Email = email,
            TotalPrice = total
        };
        _reservationRepo.Save(reservation);

        Console.WriteLine($"[LOG] Reservation {reservation.Id} created.");
        return reservation.Id;
    }

    public Reservation? GetReservation(string id) => _reservationRepo.GetById(id);

    public List<Reservation> GetAllReservations() => _reservationRepo.GetAll();
}
