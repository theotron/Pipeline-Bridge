using System;
using Umbraco.Core;
using PipelineApp.Controllers;
using PipelineApp.Models;
using PipelineApp.DataServices;
using PipelineApp.Services;
using System.Linq;

namespace GrowCreate.PipelineIntegration
{
    public static class ContactExtensions
    {
        public static void AddOrganisation(this Contact contact, int OrganisationId)
        {
            if (!contact.OrganisationIds.Contains(OrganisationId.ToString()))
            {
                contact.OrganisationIds =
                    (!string.IsNullOrEmpty(contact.OrganisationIds) ? contact.OrganisationIds + "," : "")
                    + OrganisationId.ToString();
                contact.Save();
            }            
        }
    }
}