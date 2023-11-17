using Trainwreck.Entities;

namespace Trainwreck.Repositories;

public class Repository
{
    public Repository(string connectionString)
    {
        
    }

    public void RunSql(string sql, CancellationToken cancellationToken)
    {
        
    }

    internal Task<Train> GetTrainAsync(Guid trainId)
    {
        return Task.FromResult(new Train());
    }

    internal void Update<T>(T train)
    {
        
    }
}
