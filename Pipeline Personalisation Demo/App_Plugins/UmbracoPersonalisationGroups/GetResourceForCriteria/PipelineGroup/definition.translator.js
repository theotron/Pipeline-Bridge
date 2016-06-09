angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.PipelineGroupTranslatorService", function ($http) {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedOrgs = JSON.parse(definition);
	                translation = selectedOrgs.length + " group" + (selectedOrgs.length != 1 ? "s" : "");
            	}
	            return translation;
	        }
	    };

	    return service;
	});