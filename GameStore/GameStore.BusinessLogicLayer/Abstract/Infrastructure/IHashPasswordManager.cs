namespace GameStore.BusinessLogicLayer.Abstract.Infrastructure
{
    public interface IHashPasswordManager
    {
        string EncryptPassword(string password);

        bool VerifyPassword(string passwordInput, string passwordHash);
    }
}