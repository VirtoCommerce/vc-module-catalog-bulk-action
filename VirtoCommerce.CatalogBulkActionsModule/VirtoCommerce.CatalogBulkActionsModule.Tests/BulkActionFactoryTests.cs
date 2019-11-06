namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;

    using Xunit;

    public class BulkActionFactoryTests
    {
        private readonly IBulkActionFactory _bulkActionFactory;

        public BulkActionFactoryTests()
        {
            var lazyServiceProvider = new Mock<ILazyServiceProvider>();
            _bulkActionFactory = new BulkActionFactory(lazyServiceProvider.Object);
        }

        [Fact]
        public void Create_Result_CategoryChangeBulkAction()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext();

            // act
            var result = _bulkActionFactory.Create(context);

            // assert
            result.Should().BeOfType<CategoryChangeBulkAction>();
        }

        [Fact]
        public void Create_Result_PropertiesUpdateBulkAction()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();

            // act
            var result = _bulkActionFactory.Create(context);

            // assert
            result.Should().BeOfType<PropertiesUpdateBulkAction>();
        }

        [Fact]
        public void Create_EmptyContext_ThrowArgumentException()
        {
            // arrange
            var context = new BaseBulkActionContext();

            // act
            var action = new Action(
                () =>
                {
                    _bulkActionFactory.Create(context);
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }
    }
}