using System.Data;

namespace smart_doorbell_api.Factories.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
