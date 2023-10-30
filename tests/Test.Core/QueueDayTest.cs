using Domain.Core.ValueObjects;
using NUnit.Framework;

namespace Test.Core;

public class QueueDayTest
{
    [Test]
    public void TestCreatingDayWithDateBeforeThisDay()
    {
        var dateBeforeThisDay = new DateTime(2007, 01, 02, 12, 16, 00);
        Assert.Catch<ArgumentException>(() =>
        {
            var testDay = QueueDay.Create(dateBeforeThisDay);
        });
    }
    
    [Test]
    public void TestCreatingDayWithDateAfterThisDay()
    {
        var dateAfterThisDay = new DateTime(2024, 01, 02, 12, 16, 00);
        Assert.Catch<ArgumentException>(() =>
        {
            var testDay = QueueDay.Create(dateAfterThisDay);
        });
    }
}