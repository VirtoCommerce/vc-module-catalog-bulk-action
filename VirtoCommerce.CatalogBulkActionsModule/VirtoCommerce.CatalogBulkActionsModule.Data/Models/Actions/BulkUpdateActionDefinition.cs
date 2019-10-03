namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using Newtonsoft.Json;

    public class BulkUpdateActionDefinition
    {
        public string Name { get; set; }

        /// <summary>
        /// Entity types to which action could be applied: Category, Product, … 
        /// </summary>
        public string[] AppliableTypes { get; set; }

        [JsonIgnore]
        public IBulkUpdateActionFactory Factory { get; set; }
        [JsonIgnore]
        public IPagedDataSourceFactory DataSourceFactory { get; set; }
        public string ContextTypeName { get; set; }
    }
}