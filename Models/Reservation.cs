namespace HotelReservation.Models;

using HotelReservation.Services;

// Exercice 1.3 — SRP : Reservation ne garde que les données et le cycle de vie (réceptionniste).
// Les responsabilités du comptable et de la gouvernante sont extraites.

public class Reservation : IInvoiceData
{
    public string Id { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestCount { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public string Status { get; set; } = "Confirmed";
    public string CancellationPolicy { get; set; } = "Flexible";
    public string Email { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    // Acteur : RÉCEPTIONNISTE — cycle de vie de la réservation
    public void Cancel()
    {
        if (Status == "CheckedIn")
            throw new InvalidOperationException("Cannot cancel after check-in");
        Status = "Cancelled";
    }
}
