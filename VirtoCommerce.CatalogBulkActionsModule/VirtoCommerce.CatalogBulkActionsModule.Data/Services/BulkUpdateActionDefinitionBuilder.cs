namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public class BulkUpdateActionDefinitionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdateActionDefinitionBuilder"/> class.
        /// </summary>
        /// <param name="definition">
        /// The definition.
        /// </param>
        public BulkUpdateActionDefinitionBuilder(BulkUpdateActionDefinition definition)
        {
            BulkUpdateActionDefinition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        /// <summary>
        /// Gets the bulk update action definition.
        /// </summary>
        public BulkUpdateActionDefinition BulkUpdateActionDefinition { get; }

        public static implicit operator BulkUpdateActionDefinition(BulkUpdateActionDefinitionBuilder builder)
        {
            return builder.BulkUpdateActionDefinition;
        }
    }
}