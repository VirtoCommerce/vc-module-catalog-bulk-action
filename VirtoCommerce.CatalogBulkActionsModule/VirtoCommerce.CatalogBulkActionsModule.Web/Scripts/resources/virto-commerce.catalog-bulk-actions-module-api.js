angular.module('virtoCommerce.catalogBulkActionsModule')
    .factory('virtoCommerce.catalogBulkActionsModule.webApi', ['$resource', function ($resource) {
        return $resource('', null, {
            getActions: { method: 'GET', isArray: true, url: 'api/bulk' },
            getActionData: { method: 'POST', url: 'api/bulk/data' },
            runBulkAction: { method: 'POST', url: 'api/bulk' },
            cancel: { method: 'DELETE', url: 'api/bulk' }
        });
}]);
