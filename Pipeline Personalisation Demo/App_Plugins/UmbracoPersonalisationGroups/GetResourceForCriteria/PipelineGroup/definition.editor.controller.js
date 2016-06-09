angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.PipelineGroupPersonalisationGroupCriteriaController",
        function ($scope, $http) {

            $scope.renderModel = {};
            $scope.renderModel.orgs = [];

            $http.get("/umbraco/backoffice/PipelineApp/OrganisationApi/GetAll").then(function (response) {
                $scope.orgs = response.data;

                if ($scope.dialogOptions.definition) {
                    $scope.renderModel.orgs = JSON.parse($scope.dialogOptions.definition);
                    console.log($scope.renderModel.orgs);
                }
            });

            $scope.isSelected = function (id) {
                return $scope.renderModel.orgs.indexOf(id) > -1;
            };

            $scope.toggle = function (id) {
                if ($scope.isSelected(id)) {
                    var i = $scope.renderModel.orgs.indexOf(id);
                    $scope.renderModel.orgs.splice(i, 1);
                } else {
                    $scope.renderModel.orgs.push(id);
                }
            };

            $scope.saveAndClose = function () {
                var serializedResult = "[" + $scope.renderModel.orgs.join(",") + "]";
                console.log(serializedResult);
                $scope.submit(serializedResult);
            };

        });