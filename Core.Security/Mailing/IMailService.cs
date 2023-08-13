namespace Core.Security.Mailing;

public interface IMailService
{
    public Task SendEmail(Mail mail);
}