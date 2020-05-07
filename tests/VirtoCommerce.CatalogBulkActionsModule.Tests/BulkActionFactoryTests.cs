using VirtoCommerce.CatalogBulkActionsModule.Core.Services;

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
        [Fact]
        public void Create_Result_CategoryChangeBulkAction()
        {
            // arrange
            var factory = BuildFactory();
            var context = new CategoryChangeBulkActionContext();

            // act
            var result = factory.Create(context);

            // assert
            result.Should().BeOfType<CategoryChangeBulkAction>();
        }

        [Fact]
        public void Create_Result_PropertiesUpdateBulkAction()
        {
            // arrange
            var factory = BuildFactory();
            var context = new PropertiesUpdateBulkActionContext();

            // act
            var result = factory.Create(context);

            // assert
            result.Should().BeOfType<PropertiesUpdateBulkAction>();
        }

        [Fact]
        public void Create_EmptyContext_ThrowArgumentException()
        {
            // arrange
            var factory = BuildFactory();
            var context = new BaseBulkActionContext();

            // act
            var action = new Action(
                () =>
                {
                    factory.Create(context);
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }

        private IBulkActionFactory BuildFactory()
        {
            var lazyServiceProvider = new Mock<ILazyServiceProvider>();
            return new BulkActionFactory(lazyServiceProvider.Object);
        }
    }
}
