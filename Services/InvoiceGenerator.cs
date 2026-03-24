namespace HotelReservation.Services;

using HotelReservation.Models;

// Exercice 4 — ISP : InvoiceGenerator dépend de IInvoiceData (6 champs)
// au lieu du Reservation complet (10+ champs).

public interface IInvoiceData
{
    string Id { get; }
    string GuestName { get; }
    string RoomId { get; }
    string RoomType { get; }
    DateTime CheckIn { get; }
    DateTime CheckOut { get; }
    int GuestCount { get; }
}

public class InvoiceGenerator
{
    public Invoice Generate(IInvoiceData data)
    {
        var nights = (data.CheckOut - data.CheckIn).Days;
        var pricePerNight = data.RoomType switch
        {
            "Standard" => 80m,
            "Suite" => 200m,
            "Family" => 120m,
            _ => 0m
        };
        var subtotal = nights * pricePerNight;
        var tva = subtotal * 0.10m;
        var touristTax = data.GuestCount * nights * 1.50m;
        var total = subtotal + tva + touristTax;

        return new Invoice
        {
            ReservationId = data.Id,
            GuestName = data.GuestName,
            RoomDescription = $"{data.RoomType} {data.RoomId}",
            Nights = nights,
            Subtotal = subtotal,
            Tva = tva,
            TouristTax = touristTax,
            Total = total
        };
    }

    public void PrintInvoice(Invoice invoice, IInvoiceData data)
    {
        Console.WriteLine($"Invoice for {invoice.GuestName}:");
        Console.WriteLine($"  Room: {invoice.RoomDescription}, " +
            $"{data.CheckIn:dd/MM} -> {data.CheckOut:dd/MM} " +
            $"({invoice.Nights} nights)");
        Console.WriteLine($"  Subtotal: {invoice.Subtotal:F2} EUR");
        Console.WriteLine($"  TVA (10%): {invoice.Tva:F2} EUR");
        Console.WriteLine($"  Tourist Tax ({data.GuestCount} guests x " +
            $"{invoice.Nights} nights x 1.50 EUR): {invoice.TouristTax:F2} EUR");
        Console.WriteLine($"  Total: {invoice.Total:F2} EUR");
    }
}
