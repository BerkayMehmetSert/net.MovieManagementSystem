using Core.Utilities.Date;
using Xunit;

namespace Core.Test.Utilities.Date;

public class DateHelperTest
{
    [Fact]
    public void GetCurrentDateShouldReturnCurrentDate()
    {
        var currentDate = DateTime.Now;
        var result = DateHelper.GetCurrentDate();
        Assert.Equal(currentDate.Date, result.Date);
    }
}