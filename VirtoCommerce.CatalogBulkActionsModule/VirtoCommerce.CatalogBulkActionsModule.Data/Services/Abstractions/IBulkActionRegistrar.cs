namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services.Abstractions
{
    using System.Collections.Generic;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public interface IBulkActionRegistrar
    {
        IEnumerable<BulkActionDefinition> GetAll();

        BulkActionDefinition GetByName(string name);

        BulkActionDefinition Register(BulkActionDefinition definition);
    }
}