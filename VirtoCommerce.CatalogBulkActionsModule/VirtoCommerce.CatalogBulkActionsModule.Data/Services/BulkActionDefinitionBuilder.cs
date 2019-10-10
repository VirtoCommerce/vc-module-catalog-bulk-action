﻿namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public class BulkActionDefinitionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkActionDefinitionBuilder"/> class.
        /// </summary>
        /// <param name="definition">
        /// The definition.
        /// </param>
        public BulkActionDefinitionBuilder(BulkActionDefinition definition)
        {
            BulkActionDefinition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        /// <summary>
        /// Gets the bulk update action definition.
        /// </summary>
        public BulkActionDefinition BulkActionDefinition { get; }

        public static implicit operator BulkActionDefinition(BulkActionDefinitionBuilder builder)
        {
            return builder.BulkActionDefinition;
        }
    }
}