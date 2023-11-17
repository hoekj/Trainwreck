using Microsoft.AspNetCore.Mvc;
using Trainwreck.Entities;
using Microsoft.EntityFrameworkCore;
using Trainwreck.Repositories;
using Trainwreck.Enums;

namespace Trainwreck.Controllers;
[ApiController]
[Route("[controller]")]
public class PassengerController : ControllerBase
{
    private readonly AppDbContext c;

    public PassengerController(AppDbContext context)
    {
        c = context;
    }

    [HttpPut]
    public IActionResult AddPassenger([FromBody] Passenger passenger, CancellationToken cancellationToken)
    {
        var repository = GetRepository();
        repository.RunSql($"INSERT INTO Passenger (id, name) VALUES (1, '{passenger.Name}') ", cancellationToken);
        return this.Ok();
    }

    [HttpGet("{id}")]
    public Task<Passenger> GetPassenger(Guid passengerId, CancellationToken cancellationToken)
    {
        return c.Passengers.Where(c => c.Id == passengerId).FirstAsync(cancellationToken);
    }

    [HttpPost("{passengerName}/ticket")]
    public IActionResult SavePassengerTicket(string passengerName)
    {
        try
        {
            using var file = System.IO.File.Create($"C:/data/traintickets/{passengerName}");
            Request.Body.CopyTo(file);
        }
        catch
        {
        }

        return this.NoContent();
    }

    [HttpGet("Train/{trainId}/Passengers/List")]
    public async Task<ActionResult<string>> GetPassengerList(Guid trainId, int pageSize, int page)
    {
        var passengerNames = await c.Passengers
                                .Where(p => p.TrainId == trainId)
                                .Select(p => p.Name)
                                .Take(pageSize)
                                .ToListAsync();

        var names = string.Empty;
        for (var i = 1; i < 100; i++)
        {
            names += $", {passengerNames[i]}";
        }

        return this.Ok(names);
    }

    [HttpPost("/train/{trainId}/leaveStation")]
    public async Task<IActionResult> LeaveStation(Guid trainId)
    {
        var repository = GetRepository();
        var train = await repository.GetTrainAsync(trainId);
        train.IsMoving = true;
        train.DoorsClosed = true;

        repository.Update(train);

        return this.NoContent();
    }

    [HttpPatch("train/{trainId}/start")]
    public async Task<IActionResult> StartTrain(Guid trainId)
    {
        var train = await c.Trains.SingleOrDefaultAsync(t => t.Id == trainId);
        if (train == null)
        {
            return this.BadRequest();
        }

        switch (train.TrainType)
        {
            case TrainType.ElectricHeavyLocomotive:
                this.StartElectricEngines(train);
                break;
            case TrainType.ElectricLightWeightLocomotive:
                StartElectricEngine(train);
                break;
            case TrainType.DieselHeavyLocomotive:
                StartDieselEngines(train);
                break;
            case TrainType.DieselLightWeightLocomotive:
                StartDieselEngine(train);
                break;
        }

        return NoContent();
    }

    private static Repository GetRepository()
    {
        return new Repository("Server=(localdb)\\mssqllocaldb;Database=trains;User=sa_sql;Password=P@ssw0rd");
    }

    private void StartDieselEngine(Train train)
    {
        // custom logic to start this specific train type
    }

    private void StartDieselEngines(Train train)
    {
        // custom logic to start this specific train type
    }

    private void StartElectricEngine(Train train)
    {
        // custom logic to start this specific train type
    }

    private void StartElectricEngines(Train train)
    {
        // custom logic to start this specific train type
    }
}
