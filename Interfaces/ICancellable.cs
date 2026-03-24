namespace HotelReservation.Interfaces;

// Exercice 3.1 — LSP : séparation de la hiérarchie d'interfaces.
// IReservation = base sans Cancel (toutes les réservations).
// ICancellableReservation = avec Cancel (seulement les réservations annulables).
// Le compilateur empêche l'appel invalide sur une NonRefundableReservation.

public interface IReservation
{
    string Id { get; }
    string GuestName { get; }
    string Status { get; }
    decimal TotalPrice { get; }
    decimal CalculateRefund();
}

public interface ICancellableReservation : IReservation
{
    void Cancel();
}
