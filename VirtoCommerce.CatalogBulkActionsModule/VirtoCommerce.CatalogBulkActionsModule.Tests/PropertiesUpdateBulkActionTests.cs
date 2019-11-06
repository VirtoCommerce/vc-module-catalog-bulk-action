namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

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
        private readonly PropertiesUpdateBulkAction _bulkAction;

        private readonly PropertiesUpdateBulkActionContext _context;

        private readonly Mock<IBulkPropertyUpdateManager> _manager;

        private readonly Mock<ILazyServiceProvider> _serviceProvider;

        public PropertiesUpdateBulkActionTests()
        {
            _manager = new Mock<IBulkPropertyUpdateManager>();
            _manager.Setup(t => t.GetProperties(It.IsAny<PropertiesUpdateBulkActionContext>()))
                .Returns(new List<Property>().ToArray());
            _serviceProvider = new Mock<ILazyServiceProvider>();
            _serviceProvider.Setup(t => t.Resolve<IBulkPropertyUpdateManager>()).Returns(_manager.Object);
            _context = new PropertiesUpdateBulkActionContext();

            _bulkAction = new PropertiesUpdateBulkAction(_serviceProvider.Object, _context);
        }

        [Fact]
        public void Context_Result_NotBeNull()
        {
            // arrange

            // act
            var result = _bulkAction.Context;

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Execute_ItemService_InvokeGetByIds()
        {
            // arrange
            var itemService = new Mock<IItemService> { DefaultValueProvider = DefaultValueProvider.Mock };
            _serviceProvider.Setup(t => t.Resolve<IItemService>()).Returns(itemService.Object);
            _context.Properties = new CatalogModule.Web.Model.Property[] { };

            // act
            _bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            itemService.Verify(
                t => t.GetByIds(
                    It.IsAny<string[]>(),
                    ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties,
                    null));
        }

        [Fact]
        public void Execute_UnknownTypeOfListEntry_ArgumentException()
        {
            // arrange

            // act
            var action = new Action(
                () =>
                {
                    _bulkAction.Execute(new List<IEntity> { new ListEntry("someUnknownType", null) });
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Execute_BulkPropertyUpdateManager_InvokeUpdateProperties()
        {
            // arrange
            _serviceProvider.Setup(t => t.Resolve<IItemService>()).Returns(Mock.Of<IItemService>());
            _context.Properties = new CatalogModule.Web.Model.Property[] { };

            // act
            _bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            _manager.Verify(t => t.UpdateProperties(It.IsAny<CatalogProduct[]>(), _context.Properties));
        }

        [Fact]
        public void Execute_ShouldResolve_IBulkPropertyUpdateManager()
        {
            // arrange

            // act
            try
            {
                _bulkAction.Execute(Enumerable.Empty<IEntity>());
            }
            catch
            {
                // idle
            }

            // assert
            _serviceProvider.Verify(t => t.Resolve<IBulkPropertyUpdateManager>());
        }

        [Fact]
        public void Execute_ShouldResolve_IItemService()
        {
            // arrange

            // act
            try
            {
                _bulkAction.Execute(Enumerable.Empty<IEntity>());
            }
            catch
            {
                // idle
            }

            // assert
            _serviceProvider.Verify(t => t.Resolve<IItemService>());
        }

        [Fact]
        public void GetActionData_Result_NotBeNull()
        {
            // arrange

            // act
            var result = _bulkAction.GetActionData();

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetActionData_BulkPropertyUpdateManager_InvokeGetProperties()
        {
            // arrange

            // act
            _bulkAction.GetActionData();

            // assert
            _manager.Verify(t => t.GetProperties(_context));
        }

        [Fact]
        public void GetActionData_ShouldResolve_IBulkPropertyUpdateManager()
        {
            // arrange

            // act
            _bulkAction.GetActionData();

            // assert
            _serviceProvider.Verify(t => t.Resolve<IBulkPropertyUpdateManager>());
        }

        [Fact]
        public void Validate_Result_True()
        {
            // arrange

            // act
            var result = _bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(true);
        }
    }
}