namespace DevPrime.State.Connections;
public class ConnectionMongo : MongoBaseState
{
    public ConnectionMongo(MongoBaseState stateContext) : base(stateContext)
    {
    }

    public IMongoCollection<DevPrime.State.Repositories.Order.Model.Order> Order
    {
        get
        {
            return Db.GetCollection<DevPrime.State.Repositories.Order.Model.Order>("Order");
        }
    }
}