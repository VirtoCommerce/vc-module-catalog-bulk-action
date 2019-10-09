namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services.Abstractions;
    using VirtoCommerce.Platform.Core.Common;

    public class BulkActionRegistrar : IBulkActionRegistrar
    {
        private readonly ConcurrentDictionary<string, BulkActionDefinition> _knownActionTypes =
            new ConcurrentDictionary<string, BulkActionDefinition>();

        public IEnumerable<BulkActionDefinition> GetAll()
        {
            return _knownActionTypes.Values.ToArray();
        }

        public BulkActionDefinition GetByName(string name)
        {
            return _knownActionTypes.Values.FirstOrDefault(value => value.Name.EqualsInvariant(name));
        }

        public BulkActionDefinition Register(BulkActionDefinition definition)
        {
            var actionName = definition.Name;

            if (_knownActionTypes.ContainsKey(actionName))
            {
                // idle
            }
            else
            {
                _knownActionTypes.TryAdd(actionName, definition);
            }

            return _knownActionTypes[actionName];
        }
    }
}