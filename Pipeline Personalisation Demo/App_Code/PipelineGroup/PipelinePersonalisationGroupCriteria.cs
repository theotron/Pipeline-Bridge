namespace Zone.UmbracoPersonalisationGroups.Criteria.PipelineGroup
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;
    using System.Web;
    using PipelineApp.Services;

    /// <summary>
    /// Implements a personalisation group criteria based on whether the visitor is in a CRM group
    /// </summary>
    public class PipelineGroupPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        public string Name
        {
            get { return "Pipeline Group"; }
        }

        public string Alias
        {
            get { return "pipelineGroup"; }
        }

        public string Description
        {
            get { return "Matches visitor based on Pipeline CRM group"; }
        }

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            try
            {
                var definedGroup = JsonConvert.DeserializeObject<int[]>(definition);

                // Check if there is a Pipeline cookie
                if (definedGroup.Any() && HttpContext.Current.Request.Cookies["PipelineContactId"] != null)
                {
                    // Get the Pipeline contact
                    var contactId = int.Parse(HttpContext.Current.Request.Cookies["PipelineContactId"].Value);
                    var contact = new ContactService().GetById(contactId);

                    // Check if any of their Organisation is of a type in our criteria settings
                    if (contact != null && contact.Organisations != null && contact.Organisations.Any())
                    {
                        return definedGroup.Intersect(contact.Organisations.Select(x => x.Id)).Any();
                    }
                }

                // No cookie, or Contact doesn't exist, or it doesn't have an Org - criteria fails 
                return false; 
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }
        }
    }
}
