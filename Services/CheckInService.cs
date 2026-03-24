namespace HotelReservation.Services;

using HotelReservation.Models;

// Exercice 1.2 — SRP : séparation des niveaux d'abstraction
// La méthode principale ne contient que des appels de haut niveau.
// Les détails (cache, notifications) sont délégués à des méthodes dédiées.

public class CheckInService
{
    private readonly Dictionary<string, CacheEntry> _cache = new();

    private const decimal LateCheckInFee = 25m;
    private const int LateCheckInHour = 22;

    public void ProcessCheckIn(Reservation reservation)
    {
        ValidateCheckIn(reservation);
        ApplyLateCheckInFeeIfNeeded(reservation);
        reservation.Status = "CheckedIn";
        UpdateCache(reservation.Id, "CheckedIn");
        NotifyRoomOccupied(reservation.RoomId);
    }

    public void ProcessCheckOut(Reservation reservation)
    {
        ValidateCheckOut(reservation);
        reservation.Status = "CheckedOut";
        InvalidateCache(reservation.Id);
        NotifyRoomFree(reservation.RoomId);
    }

    // --- High level: business rules ---

    private void ValidateCheckIn(Reservation reservation)
    {
        if (reservation.Status != "Confirmed")
            throw new Exception($"Cannot check in: reservation is {reservation.Status}");
    }

    private void ValidateCheckOut(Reservation reservation)
    {
        if (reservation.Status != "CheckedIn")
            throw new Exception($"Cannot check out: reservation is {reservation.Status}");
    }

    private void ApplyLateCheckInFeeIfNeeded(Reservation reservation)
    {
        if (DateTime.Now.Hour >= LateCheckInHour)
            reservation.TotalPrice += LateCheckInFee;
    }

    // --- Low level: cache management ---

    private void UpdateCache(string reservationId, string status)
    {
        if (_cache.ContainsKey(reservationId))
            _cache.Remove(reservationId);
        _cache[reservationId] = new CacheEntry(DateTime.Now, status);
    }

    private void InvalidateCache(string reservationId)
    {
        if (_cache.ContainsKey(reservationId))
            _cache.Remove(reservationId);
    }

    // --- Low level: notifications ---

    private void NotifyRoomOccupied(string roomId)
    {
        Console.WriteLine($"[SMS] Room {roomId} is now occupied");
    }

    private void NotifyRoomFree(string roomId)
    {
        Console.WriteLine($"[SMS] Room {roomId} is now free");
    }
}
