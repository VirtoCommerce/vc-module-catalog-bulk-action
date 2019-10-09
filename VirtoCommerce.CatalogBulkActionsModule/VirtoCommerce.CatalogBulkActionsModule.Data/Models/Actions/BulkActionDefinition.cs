namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using Newtonsoft.Json;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;

    public class BulkActionDefinition
    {
        /// <summary>
        /// Gets or sets the entity types to which action could be applied: Category, Product, … 
        /// </summary>
        public string[] ApplicableTypes { get; set; }

        /// <summary>
        /// Gets or sets the context type name.
        /// </summary>
        public string ContextTypeName { get; set; }

        /// <summary>
        /// Gets or sets the data source factory.
        /// </summary>
        [JsonIgnore]
        public IPagedDataSourceFactory DataSourceFactory { get; set; }

        /// <summary>
        /// Gets or sets the factory.
        /// </summary>
        [JsonIgnore]
        public IBulkActionFactory Factory { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}