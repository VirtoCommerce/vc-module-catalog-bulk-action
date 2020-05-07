using VirtoCommerce.CatalogBulkActionsModule.Core.Services;

namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogBulkActionsModule.Data.DataSources;
    using VirtoCommerce.CatalogModule.Web.Services;

    using Xunit;

    public class ClassCtorTests
    {
        [Fact]
        public void PropertiesUpdateBulkAction_NullArgs_ThrowArgumentException()
        {
            // arrange

            // act
            var action = new Action(
                () =>
                {
                    new PropertiesUpdateBulkAction(Mock.Of<ILazyServiceProvider>(), null);
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void BaseDataSource_NullArgs_ThrowArgumentException()
        {
            // arrange

            // act
            var action = new Action(
                () =>
                {
                    new BaseDataSource(Mock.Of<IListEntrySearchService>(), null);
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CategoryChangeBulkAction_NullArgs_ThrowArgumentException()
        {
            // arrange

            // act
            var action = new Action(
                () =>
                {
                    new CategoryChangeBulkAction(Mock.Of<ILazyServiceProvider>(), null);
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }
    }
}
