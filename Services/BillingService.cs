namespace HotelReservation.Services;

using HotelReservation.Repositories;

// Exercice 4 — ISP : BillingService ne dépend plus que de IReservationStats
// au lieu de l'interface complète IReservationRepository (9 méthodes).
public class BillingService
{
    private readonly IReservationStats _stats;

    public BillingService(IReservationStats stats)
    {
        _stats = stats;
    }

    public decimal GetRevenueForPeriod(DateTime from, DateTime to)
    {
        return _stats.GetTotalRevenue(from, to);
    }
}
