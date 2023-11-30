using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SendingEmail.Models;

public class EmailEntity
{
    [ValidateNever]
    public string FromEmailAddress { get; set; } = string.Empty;
    public string ToEmailAddress { get; set; } = string.Empty;
    public string EmailSubject { get; set; } = string.Empty;
    public string EmailBodyMessage { get; set; } = string.Empty;

    [ValidateNever]
    public string AttachmentURL {  get; set; } = string.Empty;

}
