namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using System.Collections.Generic;

    public class BulkUpdateProgressContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdateProgressContext"/> class.
        /// </summary>
        public BulkUpdateProgressContext()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets the processed count.
        /// </summary>
        public int? ProcessedCount { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        public int? TotalCount { get; set; }
    }
}