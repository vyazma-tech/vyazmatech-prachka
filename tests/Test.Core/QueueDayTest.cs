using Domain.Core.ValueObjects;
using Xunit;

namespace Test.Core;

public class QueueDayTest
{
    [Fact]
    public void TestCreatingDayWithDateBeforeThisDay()
    {
        var dateBeforeThisDay = new DateTime(2007, 01, 02, 12, 16, 00);
        Assert.Catch<ArgumentException>(() =>
        {
            var testDay = JSType.Date.Create(dateBeforeThisDay);
        });
    }
    
    [Fact]
    public void TestCreatingDayWithDateAfterThisDay()
    {
        var dateAfterThisDay = new DateTime(2024, 01, 02, 12, 16, 00);
        Assert.Catch<ArgumentException>(() =>
        {
            var testDay = JSType.Date.Create(dateAfterThisDay);
        });
    }
}