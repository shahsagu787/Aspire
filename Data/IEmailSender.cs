namespace Aspire.Data
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
