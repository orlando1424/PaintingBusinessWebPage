using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaintingBusinessWebPage.Models;

namespace PaintingBusinessWebPage.Controllers
{
    public class ContactController : Controller
    {
        private EmailAddress FromAndToEmailAddress;
        private IEmailService EmailService;

        public ContactController(EmailAddress _fromAddress, IEmailService _emailService)
        {
            FromAndToEmailAddress = _fromAddress;
            EmailService = _emailService;
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                EmailMessage msgToSend = new EmailMessage
                {
                    FromAddresses = new List<EmailAddress> { FromAndToEmailAddress },
                    ToAddresses = new List<EmailAddress> { FromAndToEmailAddress },
                    Content = $"Here is your message: Name: {model.Name}, " +
                    $"Email: {model.Email}, Message: {model.Message}",
                    Subject = "Contact Form Message "
                };

                EmailService.Send(msgToSend);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}
