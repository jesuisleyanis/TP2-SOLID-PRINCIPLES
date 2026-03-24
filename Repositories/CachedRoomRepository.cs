namespace HotelReservation.Repositories;

using HotelReservation.Models;

// Exercice 3.2 — LSP : CachedRoomRepository respecte désormais le contrat de IRoomRepository.
// - GetAvailableRooms délègue au repository interne pour des données fraîches.
// - Save invalide le cache pour éviter les données périmées.
public class CachedRoomRepository : IRoomRepository
{
    private readonly IRoomRepository _inner;
    private readonly Dictionary<string, Room> _cache = new();

    public CachedRoomRepository(IRoomRepository inner)
    {
        _inner = inner;
    }

    public Room? GetById(string roomId)
    {
        if (!_cache.ContainsKey(roomId))
        {
            var room = _inner.GetById(roomId);
            if (room != null)
                _cache[roomId] = room;
            return room;
        }
        return _cache[roomId];
    }

    public List<Room> GetAvailableRooms(DateTime from, DateTime to)
    {
        // Délègue au repository interne pour respecter le contrat (données fraîches + paramètres de date)
        return _inner.GetAvailableRooms(from, to);
    }

    public void Save(Room room)
    {
        _inner.Save(room);
        // Invalide le cache pour cette chambre
        _cache.Remove(room.Id);
    }
}
