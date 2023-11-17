using Trainwreck.Enums;

namespace Trainwreck.Entities;

public class Train
{
    public bool IsMoving { get; set; }
    public int PassengerCount { get; set; }
    public ICollection<Passenger> Customers { get; set; }
    public bool DoorsClosed { get; internal set; }
    public Guid Id { get; internal set; }

    public TrainType TrainType { get; set; }
}
