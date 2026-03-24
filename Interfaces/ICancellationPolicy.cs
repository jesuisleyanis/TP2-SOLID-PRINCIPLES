namespace HotelReservation.Interfaces;

using HotelReservation.Models;

// Exercice 2.2 — OCP : interface pour les politiques d'annulation.
// Ajouter une nouvelle politique (ex: "SuperFlexible") ne nécessite
// qu'une nouvelle implémentation, sans modifier le CancellationService.
public interface ICancellationPolicy
{
    string Name { get; }
    decimal CalculateRefund(Reservation reservation, int daysBeforeCheckIn);
}

public class FlexiblePolicy : ICancellationPolicy
{
    public string Name => "Flexible";

    public decimal CalculateRefund(Reservation reservation, int daysBeforeCheckIn)
    {
        return daysBeforeCheckIn >= 1 ? reservation.TotalPrice : 0m;
    }
}

public class ModeratePolicy : ICancellationPolicy
{
    public string Name => "Moderate";

    public decimal CalculateRefund(Reservation reservation, int daysBeforeCheckIn)
    {
        if (daysBeforeCheckIn >= 5) return reservation.TotalPrice;
        if (daysBeforeCheckIn >= 2) return reservation.TotalPrice * 0.5m;
        return 0m;
    }
}

public class StrictPolicy : ICancellationPolicy
{
    public string Name => "Strict";

    public decimal CalculateRefund(Reservation reservation, int daysBeforeCheckIn)
    {
        if (daysBeforeCheckIn >= 14) return reservation.TotalPrice;
        if (daysBeforeCheckIn >= 7) return reservation.TotalPrice * 0.5m;
        return 0m;
    }
}

public class NonRefundablePolicy : ICancellationPolicy
{
    public string Name => "NonRefundable";

    public decimal CalculateRefund(Reservation reservation, int daysBeforeCheckIn)
    {
        return 0m;
    }
}
