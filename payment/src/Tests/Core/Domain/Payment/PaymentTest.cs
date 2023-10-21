namespace Core.Tests;
public class PaymentTest
{
    public static Guid FixedID = new Guid("494601a0-7d69-4a09-9f59-f9019ed6b578");
    public static Guid OrderIDFixedID = new Guid("03d57425-295f-49f2-b482-f858925c5d0a");

#region fixtures
    public static Domain.Aggregates.Payment.Payment Create_Payment_Required_Properties_OK(DpTest dpTest)
    {
        var payment = new Domain.Aggregates.Payment.Payment();
        dpTest.MockDpDomain(payment);
        dpTest.Set<Guid>(payment, "ID", FixedID);
        dpTest.Set<string>(payment, "CustomerName", Faker.Lorem.Sentence(1));
        dpTest.Set<Guid>(payment, "OrderID", OrderIDFixedID);
        return payment;
    }
    public static Domain.Aggregates.Payment.Payment Create_Payment_With_CustomerName_Required_Property_Missing(DpTest dpTest)
    {
        var payment = new Domain.Aggregates.Payment.Payment();
        dpTest.MockDpDomain(payment);
        dpTest.Set<Guid>(payment, "ID", FixedID);
        dpTest.Set<Guid>(payment, "OrderID", OrderIDFixedID);
        return payment;
    }

#endregion fixtures

#region add
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Success")]
    public void Add_Required_properties_filled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var payment = Create_Payment_Required_Properties_OK(dpTest);
        dpTest.MockDpProcessEvent<bool>(payment, "CreatePayment", true);
        dpTest.MockDpProcessEvent(payment, "PaymentCreated");
        //Act
        payment.Add();
        //Assert
        var domainevents = dpTest.GetDomainEvents(payment);
        Assert.True(domainevents[0] is CreatePayment);
        Assert.True(domainevents[1] is PaymentCreated);
        Assert.NotEqual(payment.ID, Guid.Empty);
        Assert.True(payment.IsNew);
        Assert.True(payment.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Fail")]
    public void Add_CustomerName_Missing_Fail()
    {
        //Arrange
        var dpTest = new DpTest();
        var payment = Create_Payment_With_CustomerName_Required_Property_Missing(dpTest);
        //Act and Assert
        var ex = Assert.Throws<PublicException>(payment.Add);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerName is required", i));
        Assert.False(payment.Dp.Notifications.IsValid);
    }

#endregion add

#region update
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Success")]
    public void Update_Required_properties_filled_Success()
    {
        //Arrange        
        var dpTest = new DpTest();
        var payment = Create_Payment_Required_Properties_OK(dpTest);
        dpTest.MockDpProcessEvent<bool>(payment, "UpdatePayment", true);
        dpTest.MockDpProcessEvent(payment, "PaymentUpdated");
        //Act
        payment.Update();
        //Assert
        var domainevents = dpTest.GetDomainEvents(payment);
        Assert.True(domainevents[0] is UpdatePayment);
        Assert.True(domainevents[1] is PaymentUpdated);
        Assert.NotEqual(payment.ID, Guid.Empty);
        Assert.True(payment.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Fail")]
    public void Update_CustomerName_Missing_Fail()
    {
        //Arrange
        var dpTest = new DpTest();
        var payment = Create_Payment_With_CustomerName_Required_Property_Missing(dpTest);
        //Act and Assert
        var ex = Assert.Throws<PublicException>(payment.Update);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerName is required", i));
        Assert.False(payment.Dp.Notifications.IsValid);
    }

#endregion update

#region delete
    [Fact]
    [Trait("Aggregate", "Delete")]
    [Trait("Aggregate", "Success")]
    public void Delete_IDFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var payment = Create_Payment_Required_Properties_OK(dpTest);
        dpTest.MockDpProcessEvent<bool>(payment, "DeletePayment", true);
        dpTest.MockDpProcessEvent(payment, "PaymentDeleted");
        //Act
        payment.Delete();
        //Assert
        var domainevents = dpTest.GetDomainEvents(payment);
        Assert.True(domainevents[0] is DeletePayment);
        Assert.True(domainevents[1] is PaymentDeleted);
    }

#endregion delete
}