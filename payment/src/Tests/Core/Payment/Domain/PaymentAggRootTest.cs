namespace Tests_Domain.Payment;
public partial class PaymentAggRootTest
{
#region ValidateFields
    [Fact]
    [Trait("Category", "Validate")]
    [Trait("Category", "Success")]

    public void ValidateFields_FieldsAreValid_Success()
    {
        //Arrange
        var agg = MockPayment();
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

    public void Add_AllFieldsFilled_TriggerEventPaymentCreated()
    {
        //Arrange
        var agg = MockPayment();
        //Act
        agg.Add();
        //Assert
        Assert.True(agg.Dp.GetDomainEvent() is PaymentCreated);
    }

#endregion Add

#region Update
    [Fact]
    [Trait("Category", "Update")]
    [Trait("Category", "Success")]

    public void Update_FieldsFilled_TriggerEventPaymentUpdated()
    {
        //Arrange
        var agg = MockPayment();
        //Act
        agg.Update();
        //Assert
        Assert.True(agg.Dp.GetDomainEvent() is PaymentUpdated);
    }

#endregion Update

#region Delete
    [Fact]
    [Trait("Category", "Delete")]
    [Trait("Category", "Success")]

    public void Delete_FieldsFilled_TriggerEventPaymentDeleted()
    {
        //Arrange
        var agg = MockPayment();
        agg.ID = Guid.NewGuid();
        //Act
        agg.Delete();
        //Assert
        Assert.True(agg.Dp.GetDomainEvent() is PaymentDeleted);
    }
    [Fact]
    [Trait("Category", "Delete")]
    [Trait("Category", "Failure")]

    public void Delete_FieldsNotFilled_DontTriggerEvent()
    {
        //Arrange
        var agg = MockPayment();
        agg.ID = Guid.Empty;
        //Act
        agg.Delete();
        //Assert
        Assert.False(agg.Dp.GetDomainEvent() is PaymentDeleted);
    }

#endregion Delete
}