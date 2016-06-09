# Pipeline-Bridge

An criteria extension for Andy Burland's [Personalisation Groups package](https://github.com/AndyButland/UmbracoPersonalisationGroups) for Umbraco, which matches a visitor's Organisation as stored in [Pipeline CRM](http://pipelinecrm.co.uk/). 

The important files are:

####App_Code/PipelineGroup/PipelinePersonalisationGroupCriteria.cs
A C# class that implements the IPersonalisationGroupCriteria interfac so install a personalisation group criteria based on whether the visitor is in a Pipeline CRM organisation.

####App_Plugins/UmbracoPersonalisationGroups
The manifest, template and JS Angular files for the Umbraco back-office. 
