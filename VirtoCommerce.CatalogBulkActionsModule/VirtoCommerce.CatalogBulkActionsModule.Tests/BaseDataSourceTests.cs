namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Data.DataSources;
    using VirtoCommerce.CatalogModule.Web.Model;
    using VirtoCommerce.CatalogModule.Web.Services;

    using Xunit;

    using SearchCriteria = VirtoCommerce.Domain.Catalog.Model.SearchCriteria;

    public class BaseDataSourceTests
    {
        [Fact]
        public void Fetch_Should_Contain_1_Item()
        {
            // arrange
            var entries = new List<ListEntry> { new ListEntry() };
            var dataQuery = Mock.Of<DataQuery>(t => t.ListEntries == entries.ToArray());
            var searchService = Mock.Of<IListEntrySearchService>();
            var dataSource = new BaseDataSource(searchService, dataQuery);

            // act
            dataSource.Fetch();

            // assert
            dataSource.Items.Should().HaveCount(1);
        }

        [Fact]
        public void Fetch_Should_Invoke_Search()
        {
            // arrange
            var entries = new List<ListEntry> { new ListEntry() };
            var listEntrySearchResult = Mock.Of<ListEntrySearchResult>(t => t.ListEntries == entries);
            var dataQuery = Mock.Of<DataQuery>(t => t.SearchCriteria == Mock.Of<SearchCriteria>());
            var searchService = new Mock<IListEntrySearchService>();
            searchService.Setup(t => t.Search(It.IsAny<SearchCriteria>())).Returns(listEntrySearchResult);
            var dataSource = new BaseDataSource(searchService.Object, dataQuery);

            // act
            dataSource.Fetch();

            // assert
            searchService.Verify(t => t.Search(It.IsAny<SearchCriteria>()));
        }

        [Fact]
        public void Fetch_Should_Return_True()
        {
            // arrange
            var entries = new List<ListEntry> { new ListEntry() };
            var dataQuery = Mock.Of<DataQuery>(t => t.ListEntries == entries.ToArray());
            var searchService = Mock.Of<IListEntrySearchService>();
            var dataSource = new BaseDataSource(searchService, dataQuery);

            // act
            var result = dataSource.Fetch();

            // assert
            result.Should().Be(true);
        }

        [Fact]
        public void Fetch_Should_Set_Items()
        {
            // arrange
            var dataQuery = Mock.Of<DataQuery>();
            var searchService = Mock.Of<IListEntrySearchService>();
            var dataSource = new BaseDataSource(searchService, dataQuery);

            // act
            dataSource.Fetch();

            // assert
            dataSource.Items.Should().NotBeNull();
        }

        [Fact]
        public void GetTotalCount_Should_Be_Equal_0()
        {
            // arrange
            var dataQuery = Mock.Of<DataQuery>();
            var searchService = Mock.Of<IListEntrySearchService>();
            var dataSource = new BaseDataSource(searchService, dataQuery);

            // act
            var result = dataSource.GetTotalCount();

            // assert
            result.Should().Be(0);
        }

        [Fact]
        public void GetTotalCount_Should_Be_Equal_1()
        {
            // arrange
            var dataQuery = Mock.Of<DataQuery>(t => t.SearchCriteria == new SearchCriteria());
            var searchService = new Mock<IListEntrySearchService>();
            var listEntrySearchResult = new ListEntrySearchResult { TotalCount = 1 };
            searchService.Setup(t => t.Search(It.IsAny<SearchCriteria>())).Returns(listEntrySearchResult);

            var dataSource = new BaseDataSource(searchService.Object, dataQuery);

            // act
            var result = dataSource.GetTotalCount();

            // assert
            result.Should().Be(1);
        }

        [Fact]
        public void GetTotalCount_Should_Be_Equal_2()
        {
            // arrange
            var dataQuery = Mock.Of<DataQuery>(t => t.ListEntries == new[] { new ListEntry(),  });
            var searchService = new Mock<IListEntrySearchService>();

            var dataSource = new BaseDataSource(searchService.Object, dataQuery);

            // act
            var result = dataSource.GetTotalCount();

            // assert
            result.Should().Be(1);
        }
    }
}