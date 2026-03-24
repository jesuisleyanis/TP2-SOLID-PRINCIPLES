namespace HotelReservation.Services;

using HotelReservation.Models;
using HotelReservation.Interfaces;

// Exercice 2.2 — OCP : le switch/case est remplacé par un dictionnaire de politiques.
// Ajouter une nouvelle politique ne nécessite plus de modifier cette classe.
public class CancellationService
{
    private readonly Dictionary<string, ICancellationPolicy> _policies;

    public CancellationService()
    {
        var policyList = new ICancellationPolicy[]
        {
            new FlexiblePolicy(),
            new ModeratePolicy(),
            new StrictPolicy(),
            new NonRefundablePolicy()
        };
        _policies = policyList.ToDictionary(p => p.Name);
    }

    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        var daysBeforeCheckIn = (reservation.CheckIn - now).Days;

        if (!_policies.TryGetValue(reservation.CancellationPolicy, out var policy))
            throw new ArgumentException(
                $"Unknown cancellation policy: {reservation.CancellationPolicy}");

        return policy.CalculateRefund(reservation, daysBeforeCheckIn);
    }

    public void CancelReservation(Reservation reservation, DateTime now)
    {
        var refund = CalculateRefund(reservation, now);
        reservation.Cancel();
        Console.WriteLine(
            $"[OK] Reservation {reservation.Id} cancelled " +
            $"({reservation.CancellationPolicy} policy: " +
            $"{(refund == reservation.TotalPrice ? "full" : "partial")} refund of {refund:F2} EUR)");
    }
}
