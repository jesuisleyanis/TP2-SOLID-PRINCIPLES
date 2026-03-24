namespace HotelReservation.Services;

using HotelReservation.Models;

// Exercice 1.3 — SRP : Acteur GOUVERNANTE
// Responsable du planning de changement de linge.

public class HousekeepingScheduler
{
    public List<DateTime> GetLinenChangeDays(Reservation reservation)
    {
        var days = new List<DateTime>();
        var current = reservation.CheckIn.AddDays(3);
        while (current < reservation.CheckOut)
        {
            days.Add(current);
            current = current.AddDays(3);
        }
        return days;
    }
}
