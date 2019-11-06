namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.CatalogModule.Web.Model;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Xunit;

    using Property = VirtoCommerce.Domain.Catalog.Model.Property;

    public class PropertiesUpdateBulkActionTests
    {
        [Fact]
        public void Context_Result_NotBeNull()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var bulkAction = BuildBulkAction(context);

            // act
            var result = bulkAction.Context;

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Execute_BulkPropertyUpdateManager_InvokeUpdateProperties()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider>();
            var manager = new Mock<IBulkPropertyUpdateManager>();
            serviceProvider.Setup(t => t.Resolve<IItemService>()).Returns(Mock.Of<IItemService>());
            context.Properties = new CatalogModule.Web.Model.Property[] { };
            var bulkAction = BuildBulkAction(context, manager, serviceProvider);

            // act
            bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            manager.Verify(t => t.UpdateProperties(It.IsAny<CatalogProduct[]>(), context.Properties));
        }

        [Fact]
        public void Execute_ItemService_InvokeGetByIds()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider>();
            var itemService = new Mock<IItemService> { DefaultValueProvider = DefaultValueProvider.Mock };
            serviceProvider.Setup(t => t.Resolve<IItemService>()).Returns(itemService.Object);
            context.Properties = new CatalogModule.Web.Model.Property[] { };
            var bulkAction = BuildBulkAction(context, serviceProvider);

            // act
            bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            itemService.Verify(
                t => t.GetByIds(
                    It.IsAny<string[]>(),
                    ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties,
                    null));
        }

        [Fact]
        public void Execute_ShouldResolve_IBulkPropertyUpdateManager()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider>();
            var bulkAction = BuildBulkAction(context, serviceProvider);

            // act
            try
            {
                bulkAction.Execute(Enumerable.Empty<IEntity>());
            }
            catch
            {
                // idle
            }

            // assert
            serviceProvider.Verify(t => t.Resolve<IBulkPropertyUpdateManager>());
        }

        [Fact]
        public void Execute_ShouldResolve_IItemService()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider>();
            var bulkAction = BuildBulkAction(context, serviceProvider);

            // act
            try
            {
                bulkAction.Execute(Enumerable.Empty<IEntity>());
            }
            catch
            {
                // idle
            }

            // assert
            serviceProvider.Verify(t => t.Resolve<IItemService>());
        }

        [Fact]
        public void Execute_UnknownTypeOfListEntry_ArgumentException()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var bulkAction = BuildBulkAction(context);

            // act
            var action = new Action(
                () =>
                {
                    bulkAction.Execute(new List<IEntity> { new ListEntry("someUnknownType", null) });
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetActionData_BulkPropertyUpdateManager_InvokeGetProperties()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var manager = new Mock<IBulkPropertyUpdateManager>();
            var bulkAction = BuildBulkAction(context, manager);

            // act
            bulkAction.GetActionData();

            // assert
            manager.Verify(t => t.GetProperties(context));
        }

        [Fact]
        public void GetActionData_Result_NotBeNull()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var bulkAction = BuildBulkAction(context);

            // act
            var result = bulkAction.GetActionData();

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetActionData_ShouldResolve_IBulkPropertyUpdateManager()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider>();
            var bulkAction = BuildBulkAction(context, serviceProvider);

            // act
            bulkAction.GetActionData();

            // assert
            serviceProvider.Verify(t => t.Resolve<IBulkPropertyUpdateManager>());
        }

        [Fact]
        public void Validate_Result_True()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var bulkAction = BuildBulkAction(context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(true);
        }

        private static IBulkAction BuildBulkAction(PropertiesUpdateBulkActionContext context)
        {
            var manager = new Mock<IBulkPropertyUpdateManager>();
            manager.Setup(t => t.GetProperties(It.IsAny<PropertiesUpdateBulkActionContext>()))
                .Returns(new List<Property>().ToArray());
            var serviceProvider = new Mock<ILazyServiceProvider>();
            serviceProvider.Setup(t => t.Resolve<IBulkPropertyUpdateManager>()).Returns(manager.Object);

            var bulkAction = new PropertiesUpdateBulkAction(serviceProvider.Object, context);

            return bulkAction;
        }

        private static IBulkAction BuildBulkAction(
            PropertiesUpdateBulkActionContext context,
            Mock<ILazyServiceProvider> serviceProvider)
        {
            var manager = new Mock<IBulkPropertyUpdateManager>();
            manager.Setup(t => t.GetProperties(It.IsAny<PropertiesUpdateBulkActionContext>()))
                .Returns(new List<Property>().ToArray());

            serviceProvider.Setup(t => t.Resolve<IBulkPropertyUpdateManager>()).Returns(manager.Object);
            return new PropertiesUpdateBulkAction(serviceProvider.Object, context);
        }

        private static IBulkAction BuildBulkAction(
            PropertiesUpdateBulkActionContext context,
            IMock<IBulkPropertyUpdateManager> manager)
        {
            var serviceProvider = new Mock<ILazyServiceProvider>();
            serviceProvider.Setup(t => t.Resolve<IBulkPropertyUpdateManager>()).Returns(manager.Object);

            return new PropertiesUpdateBulkAction(serviceProvider.Object, context);
        }

        private static IBulkAction BuildBulkAction(
            PropertiesUpdateBulkActionContext context,
            IMock<IBulkPropertyUpdateManager> manager,
            Mock<ILazyServiceProvider> serviceProvider)
        {
            serviceProvider.Setup(t => t.Resolve<IBulkPropertyUpdateManager>()).Returns(manager.Object);
            return new PropertiesUpdateBulkAction(serviceProvider.Object, context);
        }
    }
}