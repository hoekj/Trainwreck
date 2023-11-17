namespace Trainwreck.Entities;

public class Passenger
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid TrainId { get; internal set; }
}
