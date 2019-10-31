namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

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
        private readonly CategoryChangeBulkActionContext _context;

        private CategoryChangeBulkAction _bulkAction;

        public CategoryChangeBulkActionTests()
        {
            _context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var serviceProvider = new Mock<ILazyServiceProvider> { DefaultValueProvider = DefaultValueProvider.Mock };
            _bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, _context);
        }

        [Fact]
        public void Context_ShouldBe_NotNull()
        {
            // arrange

            // act
            var result = _bulkAction.Context;

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Execute_Result_ShouldBe_Of_BulkActionResult_Type()
        {
            // arrange

            // act
            var result = _bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            result.Should().BeOfType(typeof(BulkActionResult));
        }

        [Theory]
        [ClassData(typeof(MethodsInvocationTestData))]
        public void Execute_Should_Invoke_Mover_Method(Mock<ILazyServiceProvider> serviceProvider, Action assertAction)
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
        public void GetActionData_Should_Return_Null()
        {
            // arrange

            // act
            var result = _bulkAction.GetActionData();

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void Validate_Result_Should_Contain_Errors()
        {
            // arrange
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog"))
                .Returns(Mock.Of<Catalog>(catalog => catalog.IsVirtual == true));
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            _bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, _context);

            // act
            var result = _bulkAction.Validate();

            // assert
            result.Errors.Should().HaveCount(1, "Because we can't move in virtual catalog'");
        }

        [Fact]
        public void Validate_Result_ShouldBe_Of_BulkActionResult_Type()
        {
            // arrange

            // act
            var result = _bulkAction.Validate();

            // assert
            result.Should().BeOfType(typeof(BulkActionResult));
        }

        [Fact]
        public void Validate_ShouldReturn_False()
        {
            // arrange
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog"))
                .Returns(Mock.Of<Catalog>(catalog => catalog.IsVirtual == true));
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            _bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, _context);

            // act
            var result = _bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(false);
        }

        [Fact]
        public void Validate_ShouldReturn_True()
        {
            // arrange
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog"))
                .Returns(Mock.Of<Catalog>(catalog => catalog.IsVirtual == false));
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            _bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, _context);

            // act
            var result = _bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(true);
        }

        [Fact]
        public void Execute_Should_Throw_Exception()
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
    }
}