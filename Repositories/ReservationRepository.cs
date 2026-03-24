namespace HotelReservation.Repositories;

using HotelReservation.Models;

// Exercice 1.1 — SRP : INFRASTRUCTURE LAYER
// Accès aux données des réservations.
public class ReservationRepository
{
    private readonly Dictionary<string, Reservation> _reservations = new();
    private int _counter = 0;

    public string NextId()
    {
        _counter++;
        return $"R-{_counter:D3}";
    }

    public void Save(Reservation reservation)
    {
        _reservations[reservation.Id] = reservation;
    }

    public Reservation? GetById(string id)
    {
        return _reservations.TryGetValue(id, out var r) ? r : null;
    }

    public List<Reservation> GetAll()
    {
        return _reservations.Values.ToList();
    }

    public void Clear()
    {
        _reservations.Clear();
        _counter = 0;
    }
}
