

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

static class Helpers
{

    
    // public static IQueryable<T> ApplySorting(IQueryable<T> query, string sortField, string direction)
    // {
    //     if (string.IsNullOrWhiteSpace(sortField))
    //         sortField = "id";

    //     if (string.IsNullOrWhiteSpace(direction))
    //         direction = "asc";

    //     var isAscending = direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

    //     return sortField.ToLowerInvariant() switch
    //     {

    //         "make" or "brand" or "model" or "plate" => isAscending
    //             ? query.OrderBy(v => v.Make ?? string.Empty)
    //             : query.OrderByDescending(v => v.Make ?? string.Empty),
    //         "year or price" => isAscending
    //             ? query.OrderBy(v => v.Year)
    //             : query.OrderByDescending(v => v.Year),
    //         _ => isAscending
    //             ? query.OrderBy(v => v.Id)
    //             : query.OrderByDescending(v => v.Id)
    //     };
    // }

    public static string UploadImage(IFormFile image, Cloudinary cloudinary)
    {
        var uploadResults = new ImageUploadResult();
        if (image.Length > 0)
        {
            using (var stream = image.OpenReadStream())
            {
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(image.Name, stream),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                uploadResults = cloudinary.Upload(imageUploadParams);

            }
        }
        string url = uploadResults.Url.ToString();
        if (url != "")
        {
            return url;
        }
        else
        {
            throw new BusinessLogicException("Error Al subir la imagen");

        }
    }
    public static async void SendMail(SendGridSettings sendGridSettings, EmailTemplate emailTemplate, string destination)
    {
        var apiKey = sendGridSettings.ApiKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(sendGridSettings.Email, sendGridSettings.Name);
        var to = new EmailAddress(destination);
        var msg = MailHelper.CreateSingleEmail(from, to, emailTemplate.Subject, emailTemplate.PlainTextContent, emailTemplate.PlainTextContent);
        await client.SendEmailAsync(msg);
    }
}