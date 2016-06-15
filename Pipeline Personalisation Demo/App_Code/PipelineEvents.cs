using System;
using Umbraco.Core;
using PipelineApp.Controllers;
using PipelineApp.Models;
using PipelineApp.DataServices;
using PipelineApp.Services;
using System.Linq;

namespace GrowCreate.PipelineIntegration
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Attach a method after a task is saved
            var contactService = ContactDbService.Instance;
            contactService.OnAfterSave += new EventHandler(CheckIfB2B);
        }

        void CheckIfB2B(object sender, EventArgs e)
        {
            var contact = sender as Contact;
            int b2BgroupId = 7;
            
            // Check if the contact belongs to a Group
            if (!string.IsNullOrEmpty(contact.OrganisationIds) && !contact.OrganisationIds.Contains(b2BgroupId.ToString()))
            {
                // Add to B2B Group
                contact.AddOrganisation(b2BgroupId);
            }
        }

    }
}