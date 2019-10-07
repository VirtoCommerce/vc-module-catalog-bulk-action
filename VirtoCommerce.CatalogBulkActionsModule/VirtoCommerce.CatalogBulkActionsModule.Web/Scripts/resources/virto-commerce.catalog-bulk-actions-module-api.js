angular.module('virtoCommerce.catalogBulkActionsModule')
    .factory('virtoCommerce.catalogBulkActionsModule.webApi', ['$resource', function ($resource) {
        return $resource('', null, {
            getActions: { method: 'GET', isArray: true, url: 'api/bulkUpdate/actions' },
            getActionData: { method: 'POST', url: 'api/bulkUpdate/action/data' },
            runBulkAction: { method: 'POST', url: 'api/bulkUpdate/run' },
            cancel: { method: 'POST', url: 'api/bulkUpdate/task/cancel' }
        });
}]);
