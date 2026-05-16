namespace CodeBase.Infrastructure.DataProvider
{
    public interface IDataProvider
    {
        public void Save();
        public bool TryLoad();
    }
}