namespace HotelReservation.Services;

using HotelReservation.Models;

// Exercice 1.1 — SRP : séparation en 3 couches
// INFRASTRUCTURE (Repository) : accès aux données
// BUSINESS (Domain Service) : règles métier (disponibilité, capacité, prix)
// APPLICATION (Application Service) : orchestration du workflow

// --- INFRASTRUCTURE LAYER : Repository ---
public class ReservationRepository
{
    private readonly Dictionary<string, Reservation> _reservations = new();
    private int _counter = 0;

    public string NextId()
    {
        _counter++;
        return $"R-{_counter:D3}";
    }

    public void Save(Reservation reservation)
    {
        _reservations[reservation.Id] = reservation;
    }

    public Reservation? GetById(string id)
    {
        return _reservations.TryGetValue(id, out var r) ? r : null;
    }

    public List<Reservation> GetAll()
    {
        return _reservations.Values.ToList();
    }

    public void Clear()
    {
        _reservations.Clear();
        _counter = 0;
    }
}

public class RoomRepository
{
    private readonly List<Room> _rooms = new()
    {
        new Room { Id = "101", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "102", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "201", Type = "Suite", MaxGuests = 2, PricePerNight = 200m },
        new Room { Id = "301", Type = "Family", MaxGuests = 4, PricePerNight = 120m }
    };

    public Room? GetById(string roomId) => _rooms.FirstOrDefault(r => r.Id == roomId);

    public List<Room> GetAll() => _rooms;
}

// --- BUSINESS LAYER : Domain Service ---
public class ReservationDomainService
{
    private readonly ReservationRepository _reservationRepo;
    private readonly RoomRepository _roomRepo;

    public ReservationDomainService(ReservationRepository reservationRepo, RoomRepository roomRepo)
    {
        _reservationRepo = reservationRepo;
        _roomRepo = roomRepo;
    }

    public Room ValidateRoom(string roomId, int guestCount)
    {
        var room = _roomRepo.GetById(roomId)
            ?? throw new Exception($"Room {roomId} not found");

        if (guestCount > room.MaxGuests)
            throw new Exception($"Room {roomId} max capacity is {room.MaxGuests}");

        return room;
    }

    public void CheckAvailability(string roomId, DateTime checkIn, DateTime checkOut)
    {
        var isAvailable = !_reservationRepo.GetAll().Any(r =>
            r.RoomId == roomId &&
            r.Status != "Cancelled" &&
            r.CheckIn < checkOut &&
            r.CheckOut > checkIn);

        if (!isAvailable)
            throw new Exception($"Room {roomId} is not available for {checkIn:dd/MM} -> {checkOut:dd/MM}");
    }

    public decimal CalculatePrice(Room room, DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        return nights * room.PricePerNight;
    }
}

// --- APPLICATION LAYER : Application Service (orchestration) ---
public class ReservationService
{
    private readonly ReservationRepository _reservationRepo;
    private readonly RoomRepository _roomRepo;
    private readonly ReservationDomainService _domainService;

    public ReservationService()
    {
        _reservationRepo = new ReservationRepository();
        _roomRepo = new RoomRepository();
        _domainService = new ReservationDomainService(_reservationRepo, _roomRepo);
    }

    public string CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email)
    {
        Console.WriteLine($"[LOG] Creating reservation for {guestName}...");

        var room = _domainService.ValidateRoom(roomId, guestCount);
        _domainService.CheckAvailability(roomId, checkIn, checkOut);
        var total = _domainService.CalculatePrice(room, checkIn, checkOut);

        var reservation = new Reservation
        {
            Id = _reservationRepo.NextId(),
            GuestName = guestName,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            RoomType = roomType,
            Status = "Confirmed",
            Email = email,
            TotalPrice = total
        };
        _reservationRepo.Save(reservation);

        Console.WriteLine($"[LOG] Reservation {reservation.Id} created.");
        return reservation.Id;
    }

    public Reservation? GetReservation(string id) => _reservationRepo.GetById(id);

    public List<Reservation> GetAllReservations() => _reservationRepo.GetAll();

    public static List<Room> GetRooms() => new RoomRepository().GetAll();

    public static void Reset()
    {
        // Kept for backward compatibility with Program.cs
    }
}
