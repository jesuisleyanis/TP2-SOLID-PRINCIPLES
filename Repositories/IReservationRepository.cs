namespace HotelReservation.Repositories;

using HotelReservation.Models;

// Exercice 4 — ISP : l'interface monolithique (9 méthodes) est segmentée
// en interfaces fines que chaque consommateur peut utiliser indépendamment.

public interface IReservationReader
{
    Reservation? GetById(string id);
    List<Reservation> GetAll();
}

public interface IReservationQueryService
{
    List<Reservation> GetByDateRange(DateTime from, DateTime to);
    List<Reservation> GetByGuest(string guestName);
}

public interface IReservationWriter
{
    void Add(Reservation reservation);
    void Update(Reservation reservation);
    void Delete(string id);
}

public interface IReservationStats
{
    decimal GetTotalRevenue(DateTime from, DateTime to);
    Dictionary<string, int> GetOccupancyStats(DateTime from, DateTime to);
}

// Interface composite pour les consommateurs qui ont besoin de tout
public interface IReservationRepository : IReservationReader, IReservationQueryService, IReservationWriter, IReservationStats
{
}
