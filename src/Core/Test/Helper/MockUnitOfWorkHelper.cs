using AutoMapper;
using Core.Persistence;
using Moq;

namespace Core.Test.Helper;

public class MockUnitOfWorkHelper
{
    public static Mock<IUnitOfWork> GetUnitOfWork()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChanges());
        return mockUnitOfWork;
    }
}

public class MockMapperHelper
{
    public static Mock<Mapper> GetMapper()
    {
        var mockMapper = new Mock<Mapper>();
        return mockMapper;
    }
}