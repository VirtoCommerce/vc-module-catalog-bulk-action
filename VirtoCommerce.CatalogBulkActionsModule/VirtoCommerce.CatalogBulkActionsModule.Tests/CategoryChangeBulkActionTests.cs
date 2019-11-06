namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogModule.Web.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Xunit;

    using Catalog = VirtoCommerce.Domain.Catalog.Model.Catalog;

    public class CategoryChangeBulkActionTests
    {
        [Fact]
        public void Context_Result_NotNull()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext();
            var bulkAction = BuildBulkAction(context);

            // act
            var result = bulkAction.Context;

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Execute_Result_BulkActionResult()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext();
            var bulkAction = BuildBulkAction(context);

            // act
            var result = bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            result.Should().BeOfType(typeof(BulkActionResult));
        }

        [Theory]
        [ClassData(typeof(MethodsInvocationTestData))]
        public void Execute_Should_InvokeMethods(Mock<ILazyServiceProvider> serviceProvider, Action assertAction)
        {
            // arrange
            var context = new CategoryChangeBulkActionContext();
            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            serviceProvider.VerifyAll();
            assertAction();
        }

        [Fact]
        public void Execute_Should_ThrowException()
        {
            // arrange
            var categoryId = "fakeId";
            var entries = new List<ListEntry> { new ListEntry { Id = categoryId } };
            var context = new CategoryChangeBulkActionContext { CategoryId = categoryId };
            var serviceProvider = new Mock<ILazyServiceProvider>();
            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var action = new Action(
                () =>
                {
                    bulkAction.Execute(entries);
                });

            // assert
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void GetActionData_Result_Null()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var bulkAction = BuildBulkAction(context);

            // act
            var result = bulkAction.GetActionData();

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void Validate_Result_BulkActionResult()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var bulkAction = BuildBulkAction(context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Should().BeOfType(typeof(BulkActionResult));
        }

        [Fact]
        public void Validate_Result_False()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog"))
                .Returns(Mock.Of<Catalog>(catalog => catalog.IsVirtual == true));
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(false);
        }

        [Theory]
        [InlineData(1)]
        public void Validate_Result_HaveErrorCount(int errorCount)
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog"))
                .Returns(Mock.Of<Catalog>(catalog => catalog.IsVirtual == true));
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Errors.Should().HaveCount(errorCount, "Because we can't move in virtual catalog'");
        }

        [Fact]
        public void Validate_Result_True()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog"))
                .Returns(Mock.Of<Catalog>(catalog => catalog.IsVirtual == false));
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(true);
        }

        private IBulkAction BuildBulkAction(CategoryChangeBulkActionContext context)
        {
            var serviceProvider = new Mock<ILazyServiceProvider> { DefaultValueProvider = DefaultValueProvider.Mock };
            return new CategoryChangeBulkAction(serviceProvider.Object, context);
        }
    }
}