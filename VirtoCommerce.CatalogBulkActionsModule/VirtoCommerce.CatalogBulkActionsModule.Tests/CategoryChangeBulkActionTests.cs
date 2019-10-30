namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Xunit;

    public class CategoryChangeBulkActionTests
    {
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
        public void Execute_Result_ShouldBe_Of_BulkActionResult_Type()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider> { DefaultValueProvider = DefaultValueProvider.Mock };
            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Execute(Enumerable.Empty<IEntity>());

            // assert
            result.Should().BeOfType(typeof(BulkActionResult));
        }

        [Fact]
        public void Validate_Result_ShouldBe_Of_BulkActionResult_Type()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider> { DefaultValueProvider = DefaultValueProvider.Mock };
            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Should().BeOfType(typeof(BulkActionResult));
        }

        [Fact]
        public void GetActionData_Should_Return_Null()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext();
            var serviceProvider = new Mock<ILazyServiceProvider>();
            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.GetActionData();

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void Validate_Result_Should_Contain_Errors()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var fakeCatalog = Mock.Of<Catalog>(catalog => catalog.IsVirtual == true);
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog")).Returns(fakeCatalog);
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Errors.Should().HaveCount(1, "Because we can't move in virtual catalog'");
        }

        [Fact]
        public void Validate_ShouldReturn_False()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var fakeCatalog = Mock.Of<Catalog>(catalog => catalog.IsVirtual == true);
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog")).Returns(fakeCatalog);
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(false);
        }

        [Fact]
        public void Validate_ShouldReturn_True()
        {
            // arrange
            var context = new CategoryChangeBulkActionContext { CatalogId = "catalog" };
            var fakeCatalog = Mock.Of<Catalog>(catalog => catalog.IsVirtual == false);
            var catalogService = new Mock<ICatalogService>();
            var serviceProvider = new Mock<ILazyServiceProvider>();

            catalogService.Setup(t => t.GetById("catalog")).Returns(fakeCatalog);
            serviceProvider.Setup(t => t.Resolve<ICatalogService>()).Returns(catalogService.Object);

            var bulkAction = new CategoryChangeBulkAction(serviceProvider.Object, context);

            // act
            var result = bulkAction.Validate();

            // assert
            result.Succeeded.Should().Be(true);
        }
    }
}