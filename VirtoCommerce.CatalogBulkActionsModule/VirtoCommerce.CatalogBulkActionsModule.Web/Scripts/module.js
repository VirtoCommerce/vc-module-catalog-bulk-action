// Call this to register your module to main application
var moduleName = "virtoCommerce.catalogBulkActionsModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('workspace.virtoCommerceCatalogBulkActionsModuleState', {
                    url: '/virtoCommerce.catalogBulkActionsModule',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'virtoCommerce.catalogBulkActionsModule.helloWorldController',
                                template: 'Modules/$(virtoCommerce.catalogBulkActionsModule)/Scripts/blades/hello-world.html',
                                isClosingDisabled: true
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.toolbarService',
        function ($rootScope, mainMenuService, widgetService, $state, toolbarService) {
            function isItemsChecked(blade) {
                var gridApi = blade.$scope.gridApi;
                return gridApi && _.any(gridApi.selection.getSelectedRows());
            }

            //Register module in main menu
            var menuItem = {
                path: 'browse/virtoCommerce.catalogBulkActionsModule',
                icon: 'fa fa-cube',
                title: 'VirtoCommerce.CatalogBulkActionsModule',
                priority: 100,
                action: function () { $state.go('workspace.virtoCommerceCatalogBulkActionsModuleState'); },
                permission: 'virtoCommerce.catalogBulkActionsModule.WebPermission'
            };
            mainMenuService.addMenuItem(menuItem);

            toolbarService.register({
                name: "Bulk Actions", icon: 'fa fa-cubes',
                executeMethod: function (blade) {
                    //console.log('test: ' + this.name + this.icon + blade);
                },
                canExecuteMethod: isItemsChecked,
                index: 20,
            }, 'virtoCommerce.catalogModule.categoriesItemsListController');
        }
]);
