angular.module('virtoCommerce.catalogBulkActionsModule')
    .factory('virtoCommerce.catalogBulkActionsModule.webApi', ['$resource', function ($resource) {
        return $resource('api/VirtoCommerceCatalogBulkActionsModule');
}]);
