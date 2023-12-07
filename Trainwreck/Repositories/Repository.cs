using Trainwreck.Entities;

namespace Trainwreck.Repositories;

public class Repository
{
    public Repository(string connectionString)
    {
        
    }

    public Task RunSqlAsync(string sql)
    {
        return Task.CompletedTask;
    }

    internal Task<Train> GetTrainAsync(Guid trainId)
    {
        return Task.FromResult(new Train());
    }

    internal Task UpdateAsync<T>(T train)
    {
        return Task.CompletedTask;
    }
}
