namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using System.Collections.Generic;

    using VirtoCommerce.Platform.Core.Common;

    public interface IPagedDataSource
    {
        int PageSize { get; set; }
        bool Fetch();
        IEnumerable<IEntity> Items { get; }
        int GetTotalCount();
    }
}