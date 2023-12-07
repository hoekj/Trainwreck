using Microsoft.AspNetCore.Mvc;
using Trainwreck.Entities;
using Microsoft.EntityFrameworkCore;
using Trainwreck.Repositories;
using Trainwreck.Enums;
using Trainwreck.Models.Interfaces;
using Trainwreck.Models;

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
    public async Task<IActionResult> AddPassenger([FromBody] Passenger passenger)
    {
        var repository = GetRepository();
        await repository.RunSqlAsync($"INSERT INTO Passenger (id, name) VALUES (1, '{passenger.Name}')");
        return Ok();
    }

    [HttpGet("{id}")]
    public Task<Passenger> GetPassenger(Guid passengerId, CancellationToken cancellationToken)
    {
        return c.Passengers.Where(c => c.Id == passengerId).FirstAsync(cancellationToken);
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

        return Ok(names);
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

        return NoContent();
    }

    [HttpPatch("train/{trainId}/start")]
    public async Task<IActionResult> StartTrain(Guid trainId)
    {
        var train = await c.Trains.SingleOrDefaultAsync(t => t.Id == trainId);
        if (train == null)
        {
            return BadRequest();
        }

        ILocomotive locomotive = null;

        switch (train.TrainType)
        {
            case TrainType.Electric:
                locomotive = new ElectricLocomotive();
                break;
            case TrainType.Diesel:
                locomotive = new DieselLocomotive();
                break;
            case TrainType.Steam:
                locomotive = new SteamLocomotive();
                break;
        }

        locomotive.ConnectToTrain(train);

        return NoContent();
    }

    [HttpPost("/train/{trainId}/leaveStation")]
    public async Task<IActionResult> LeaveStation(Guid trainId)
    {
        var repository = GetRepository();
        var train = await repository.GetTrainAsync(trainId);
        train.IsMoving = true;
        train.DoorsClosed = true;

        await repository.UpdateAsync(train);

        return NoContent();
    }

    private static Repository GetRepository()
    {
        return new Repository("Server=(localdb)\\mssqllocaldb;Database=trains;User=sa_sql;Password=P@ssw0rd");
    }
}
