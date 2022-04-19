namespace DevPrime.Web.Models.Order;
public class Item
{
    public string Description { get; set; }
    public int Amount { get; set; }
    public string SKU { get; set; }
    public double Price { get; set; }
    public static Application.Services.Order.Model.Item ToApplication(DevPrime.Web.Models.Order.Item item)
    {
        if (item is null)
            return new Application.Services.Order.Model.Item();
        Application.Services.Order.Model.Item _item = new Application.Services.Order.Model.Item();
        _item.Description = item.Description;
        _item.Amount = item.Amount;
        _item.SKU = item.SKU;
        _item.Price = item.Price;
        return _item;
    }

    public static List<Application.Services.Order.Model.Item> ToApplication(IList<DevPrime.Web.Models.Order.Item> itemList)
    {
        List<Application.Services.Order.Model.Item> _itemList = new List<Application.Services.Order.Model.Item>();
        if (itemList != null)
        {
            foreach (var item in itemList)
            {
                Application.Services.Order.Model.Item _item = new Application.Services.Order.Model.Item();
                _item.Description = item.Description;
                _item.Amount = item.Amount;
                _item.SKU = item.SKU;
                _item.Price = item.Price;
                _itemList.Add(_item);
            }
        }
        return _itemList;
    }

    public virtual Application.Services.Order.Model.Item ToApplication()
    {
        var model = ToApplication(this);
        return model;
    }
}