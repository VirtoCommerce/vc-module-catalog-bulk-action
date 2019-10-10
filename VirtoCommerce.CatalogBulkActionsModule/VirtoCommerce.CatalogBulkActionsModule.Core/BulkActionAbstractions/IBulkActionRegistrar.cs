namespace VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionAbstractions
{
    using System.Collections.Generic;

    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;

    public interface IBulkActionRegistrar
    {
        IEnumerable<BulkActionDefinition> GetAll();

        BulkActionDefinition GetByName(string name);

        BulkActionDefinition Register(BulkActionDefinition definition);
    }
}