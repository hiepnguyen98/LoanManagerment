using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISendMailProvider
    {
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = false, string mailCc = null);
        Task SendTemplateEmailAsync(string emails, string templateId, string subject, object templateData, string mailCc = null);
    }
}
