namespace EmailApi.Services
{
    public interface IEmailMessageSender
    {
        Task<bool> SendMessageAsync(string to, string subject, string body);
    }
}
