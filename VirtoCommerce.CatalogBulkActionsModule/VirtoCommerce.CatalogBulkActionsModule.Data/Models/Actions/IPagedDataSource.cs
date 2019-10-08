namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using System.Collections.Generic;

    using VirtoCommerce.Platform.Core.Common;

    public interface IPagedDataSource
    {
        IEnumerable<IEntity> Items { get; }

        int PageSize { get; set; }

        bool Fetch();

        int GetTotalCount();
    }
}