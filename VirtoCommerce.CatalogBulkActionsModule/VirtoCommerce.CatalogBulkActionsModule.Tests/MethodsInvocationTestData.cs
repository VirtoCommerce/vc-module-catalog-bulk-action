namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Moq;

    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogModule.Web.Model;
    using VirtoCommerce.CatalogModule.Web.Services;
    using VirtoCommerce.Domain.Catalog.Model;

    using Category = VirtoCommerce.Domain.Catalog.Model.Category;

    public class MethodsInvocationTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var categoryMock = new Mock<ListEntryMover<Category>>();
            var productMock = new Mock<ListEntryMover<CatalogProduct>>();

            var serviceProvider = new Mock<ILazyServiceProvider>();
            serviceProvider.Setup(t => t.Resolve<ListEntryMover<Category>>()).Returns(categoryMock.Object);
            serviceProvider.Setup(t => t.Resolve<ListEntryMover<CatalogProduct>>()).Returns(productMock.Object);

            var action1 = new Action(
                () =>
                {
                    categoryMock.Verify(t => t.PrepareMove(It.IsAny<MoveInfo>()));
                });
            yield return new object[] { serviceProvider, action1 };

            var action2 = new Action(
                () =>
                {
                    productMock.Verify(t => t.PrepareMove(It.IsAny<MoveInfo>()));
                });
            yield return new object[] { serviceProvider, action2 };

            var action3 = new Action(
                () =>
                {
                    categoryMock.Verify(t => t.ConfirmMove(It.IsAny<IEnumerable<Category>>()));
                });
            yield return new object[] { serviceProvider, action3 };

            var action4 = new Action(
                () =>
                {
                    productMock.Verify(t => t.ConfirmMove(It.IsAny<IEnumerable<CatalogProduct>>()));
                });
            yield return new object[] { serviceProvider, action4 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}