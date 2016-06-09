using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using PipelineApp.Helpers;
using System.Web;

namespace Overflow.Controllers
{
    public class UmbContactController : UmbracoApiController
    {
        // POST umbraco/api/umbcontact/post
        public HttpResponseMessage Post([FromBody]UmbContactMail message)
        {
            // Return errors if the model validation fails
            // The model defines validations for empty or invalid email addresses
            // See the UmbContactMail class below 
            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.First().Value.Errors.First().ErrorMessage);
            
            // Create a new Pipeline Contact and store in cookie
            var contact = PipelineHelper.CreateContact(message.Name, message.Email, message.Telephone, message.Organisation);
            HttpCookie cookie = new HttpCookie("PipelineContactId");
            cookie.Value = contact.Id.ToString();
            cookie.Expires = DateTime.MaxValue;
            HttpContext.Current.Response.SetCookie(cookie);
            
            // Create new opportunity in Pipeline CRM
            var pipeline = PipelineHelper.CreatePipeline(
                "Website query",
                message.Name,
                message.Email,
                message.Telephone,
                message.Organisation,
                message.Message
            );

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        private static bool TrySendMail(UmbContactMail message, string mailTo)
        {
            try
            {
                var content = string.Empty;
                content += string.Format("You have a new contact mail from {0}", string.IsNullOrWhiteSpace(message.Name) ? "[no name given]" : message.Name);
                content += "\r\n";
                content += "They said:";
                content += "\r\n";
                content += string.Format("{0}", string.IsNullOrWhiteSpace(message.Message) ? "[no message entered]" : message.Message);

                var mailFrom = new System.Net.Mail.MailAddress(message.Email, message.Name);

                var mailMsg = new System.Net.Mail.MailMessage
                {
                    From = mailFrom,
                    Subject = "Contact mail",
                    Body = content,
                    IsBodyHtml = false
                };

                mailMsg.To.Add(new System.Net.Mail.MailAddress(mailTo));
                mailMsg.ReplyToList.Add(mailFrom);

                var smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.Send(mailMsg);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error<UmbContactController>("Error sending contact mail", ex);
            }

            return false;
        }

        public class UmbContactMail
        {
            public int SettingsNodeId { get; set; }

            public string Name { get; set; }

            [Required(ErrorMessage = "Please provide a valid e-mail address")]
            [RegularExpression(@"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?",
                ErrorMessage = "Please provide a valid e-mail address")]
            public string Email { get; set; }

            public string Organisation { get; set; }
            public string Telephone { get; set; }
            public string Message { get; set; }            
        }
    }
}
