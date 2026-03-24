namespace HotelReservation.Interfaces;

// Exercice 4 — ISP : l'interface monolithique (4 méthodes) est segmentée.
// Chaque consommateur ne dépend que du canal qu'il utilise.

public interface IEmailSender
{
    void SendEmail(string to, string subject, string body);
}

public interface ISmsSender
{
    void SendSms(string phoneNumber, string message);
}

public interface IPushNotifier
{
    void SendPushNotification(string deviceId, string message);
}

public interface ISlackNotifier
{
    void SendSlackMessage(string channel, string message);
}

// Interface composite pour les consommateurs qui ont besoin de tout
public interface INotificationService : IEmailSender, ISmsSender, IPushNotifier, ISlackNotifier
{
}
