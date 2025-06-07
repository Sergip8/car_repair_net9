

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailTemplate emailTemplate, string destination);

     Task<bool> SendEmailWithAttachmentAsync(EmailTemplate emailTemplate, string destination, string attachmentName, byte[] attachmentData);
}