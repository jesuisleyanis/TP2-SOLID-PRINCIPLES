namespace HotelReservation.Services;

using HotelReservation.Models;
using HotelReservation.Repositories;

// Exercice 1.1 — SRP : BUSINESS LAYER
// Règles métier : validation chambre, disponibilité, calcul de prix.
public class ReservationDomainService
{
    private readonly ReservationRepository _reservationRepo;
    private readonly RoomRepository _roomRepo;

    public ReservationDomainService(ReservationRepository reservationRepo, RoomRepository roomRepo)
    {
        _reservationRepo = reservationRepo;
        _roomRepo = roomRepo;
    }

    public Room ValidateRoom(string roomId, int guestCount)
    {
        var room = _roomRepo.GetById(roomId)
            ?? throw new Exception($"Room {roomId} not found");

        if (guestCount > room.MaxGuests)
            throw new Exception($"Room {roomId} max capacity is {room.MaxGuests}");

        return room;
    }

    public void CheckAvailability(string roomId, DateTime checkIn, DateTime checkOut)
    {
        var isAvailable = !_reservationRepo.GetAll().Any(r =>
            r.RoomId == roomId &&
            r.Status != "Cancelled" &&
            r.CheckIn < checkOut &&
            r.CheckOut > checkIn);

        if (!isAvailable)
            throw new Exception($"Room {roomId} is not available for {checkIn:dd/MM} -> {checkOut:dd/MM}");
    }

    public decimal CalculatePrice(Room room, DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        return nights * room.PricePerNight;
    }
}
