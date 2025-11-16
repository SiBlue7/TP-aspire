namespace projetMicrosoftTech.WebApp.Clients;

public interface IUserClient
{
    Task<string> GetCurrentUserIdAsync();
}