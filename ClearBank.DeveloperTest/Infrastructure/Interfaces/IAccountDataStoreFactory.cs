namespace ClearBank.DeveloperTest.Infrastructure.Interfaces
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore GetInstance();
    }
}
