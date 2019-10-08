namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System.Collections.Generic;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public interface IBulkUpdateActionRegistrar
    {
        IEnumerable<BulkUpdateActionDefinition> GetAll();

        BulkUpdateActionDefinition GetByName(string name);

        BulkUpdateActionDefinition Register(BulkUpdateActionDefinition definition);
    }
}