namespace HotelReservation.Infrastructure;

using HotelReservation.Models;
using HotelReservation.Services;

// Exercice 5.2 — DIP : Adapter dans Infrastructure.
// Implémente ICleaningNotifier (défini dans Domain) en déléguant à EmailSender.
//
// HousekeepingService -> ICleaningNotifier (dans Domain)
//                              ^
//               EmailCleaningNotifier (dans Infrastructure)
//                              |
//                         EmailSender (dans Infrastructure)

public class EmailCleaningNotifier : ICleaningNotifier
{
    private readonly EmailSender _emailSender;

    public EmailCleaningNotifier(EmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public void Notify(CleaningTask task)
    {
        _emailSender.Send(
            task.HousekeeperEmail,
            "New cleaning task",
            $"Room {task.RoomId} needs {task.Type} on {task.Date:dd/MM/yyyy}");
    }
}
