namespace DevPrime.State.Connections;
public class ConnectionMongo : MongoBaseState
{
    public ConnectionMongo(MongoBaseState stateContext) : base(stateContext)
    {
    }

    public IMongoCollection<DevPrime.State.Repositories.Payment.Model.Payment> Payment
    {
        get
        {
            return Db.GetCollection<DevPrime.State.Repositories.Payment.Model.Payment>("Payment");
        }
    }
}