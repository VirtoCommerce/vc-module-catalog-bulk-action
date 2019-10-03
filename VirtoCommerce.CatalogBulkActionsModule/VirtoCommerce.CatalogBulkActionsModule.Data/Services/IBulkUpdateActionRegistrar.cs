namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System.Collections.Generic;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public interface IBulkUpdateActionRegistrar
    {
        BulkUpdateActionDefinition Register(BulkUpdateActionDefinition definition);

        BulkUpdateActionDefinition GetByName(string name);

        IEnumerable<BulkUpdateActionDefinition> GetAll();
    }
}