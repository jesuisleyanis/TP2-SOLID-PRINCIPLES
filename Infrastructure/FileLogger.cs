namespace HotelReservation.Infrastructure;

using HotelReservation.Services;

// Exercice 5.1 — DIP : implémente ILogger défini dans le module métier.
public class FileLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}
