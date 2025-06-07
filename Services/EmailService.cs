
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly SendGridSettings _settings;

    public EmailService(ISendGridClient sendGridClient, IOptions<SendGridSettings> settings)
    {
        _sendGridClient = sendGridClient;
        _settings = settings.Value;
    }

    public async Task<bool> SendEmailAsync(EmailTemplate emailTemplate, string destination)
    {
        try
        {
            var from = new EmailAddress(_settings.Email, _settings.Name);
            var to = new EmailAddress(destination);
            
            var msg = MailHelper.CreateSingleEmail(
                from, 
                to, 
                emailTemplate.Subject, 
                emailTemplate.PlainTextContent ?? ConvertHtmlToPlainText(emailTemplate.HtmlContent), 
                emailTemplate.HtmlContent
            );

            var response = await _sendGridClient.SendEmailAsync(msg);
            
            Console.WriteLine(response.StatusCode);
            return response.StatusCode >= System.Net.HttpStatusCode.OK && 
                   response.StatusCode < System.Net.HttpStatusCode.MultipleChoices;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Error enviando email: {ex.Message}");
           
        }
    }

    public async Task<bool> SendEmailWithAttachmentAsync(EmailTemplate emailTemplate, string destination, string attachmentName, byte[] attachmentData)
    {
        try
        {
            var from = new EmailAddress(_settings.Email, _settings.Name);
            var to = new EmailAddress(destination);
            
            var msg = MailHelper.CreateSingleEmail(from, to, emailTemplate.Subject, null, emailTemplate.HtmlContent);
            
            // Agregar adjunto
            var attachment = new Attachment
            {
                Content = Convert.ToBase64String(attachmentData),
                Filename = attachmentName,
                Type = GetMimeType(attachmentName),
                Disposition = "attachment"
            };
            
            msg.AddAttachment(attachment);

            var response = await _sendGridClient.SendEmailAsync(msg);
            
            return response.StatusCode >= System.Net.HttpStatusCode.OK && 
                   response.StatusCode < System.Net.HttpStatusCode.MultipleChoices;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enviando email con adjunto: {ex.Message}");
            return false;
        }
    }

    private string ConvertHtmlToPlainText(string html)
    {
        // Conversión básica de HTML a texto plano
        return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", "");
    }

    private string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".txt" => "text/plain",
            _ => "application/octet-stream"
        };
    }
}