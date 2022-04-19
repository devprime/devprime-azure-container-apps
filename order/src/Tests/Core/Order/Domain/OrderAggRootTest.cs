namespace Tests_Domain.Order;
public partial class OrderAggRootTest
{
#region ValidateFields
    [Fact]
    [Trait("Category", "Validate")]
    [Trait("Category", "Success")]

    public void ValidateFields_FieldsAreValid_Success()
    {
        //Arrange
        var agg = MockOrder();
        //Act
        agg.DevPrimeCallMethod("ValidFields");
        //Assert
        Assert.True(agg.Dp.Notifications.IsValid);
    }

#endregion ValidateFields

#region Add
    [Fact]
    [Trait("Category", "Add")]
    [Trait("Category", "Success")]

    public void Add_AllFieldsFilled_TriggerEventOrderCreated()
    {
        //Arrange
        var agg = MockOrder();
        //Act
        agg.Add();
        //Assert
        Assert.True(agg.Dp.GetDomainEvent() is OrderCreated);
    }

#endregion Add

#region Update
    [Fact]
    [Trait("Category", "Update")]
    [Trait("Category", "Success")]

    public void Update_FieldsFilled_TriggerEventOrderUpdated()
    {
        //Arrange
        var agg = MockOrder();
        //Act
        agg.Update();
        //Assert
        Assert.True(agg.Dp.GetDomainEvent() is OrderUpdated);
    }

#endregion Update

#region Delete
    [Fact]
    [Trait("Category", "Delete")]
    [Trait("Category", "Success")]

    public void Delete_FieldsFilled_TriggerEventOrderDeleted()
    {
        //Arrange
        var agg = MockOrder();
        agg.ID = Guid.NewGuid();
        //Act
        agg.Delete();
        //Assert
        Assert.True(agg.Dp.GetDomainEvent() is OrderDeleted);
    }
    [Fact]
    [Trait("Category", "Delete")]
    [Trait("Category", "Failure")]

    public void Delete_FieldsNotFilled_DontTriggerEvent()
    {
        //Arrange
        var agg = MockOrder();
        agg.ID = Guid.Empty;
        //Act
        agg.Delete();
        //Assert
        Assert.False(agg.Dp.GetDomainEvent() is OrderDeleted);
    }

#endregion Delete
}