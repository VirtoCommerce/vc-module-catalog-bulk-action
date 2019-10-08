namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.Platform.Core.Common;

    public class BulkUpdateActionRegistrar : IBulkUpdateActionRegistrar
    {
        private readonly ConcurrentDictionary<string, BulkUpdateActionDefinition> _knownActionTypes =
            new ConcurrentDictionary<string, BulkUpdateActionDefinition>();

        public IEnumerable<BulkUpdateActionDefinition> GetAll()
        {
            return _knownActionTypes.Values.ToArray();
        }

        public BulkUpdateActionDefinition GetByName(string name)
        {
            return _knownActionTypes.Values.FirstOrDefault(value => value.Name.EqualsInvariant(name));
        }

        public BulkUpdateActionDefinition Register(BulkUpdateActionDefinition definition)
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