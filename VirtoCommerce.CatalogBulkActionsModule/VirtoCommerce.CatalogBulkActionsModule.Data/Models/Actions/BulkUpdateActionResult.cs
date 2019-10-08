namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using System.Collections.Generic;

    public class BulkUpdateActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdateActionResult"/> class.
        /// </summary>
        public BulkUpdateActionResult()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// The success.
        /// </summary>
        public static BulkUpdateActionResult Success => new BulkUpdateActionResult { Succeeded = true };

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether succeeded.
        /// </summary>
        public bool Succeeded { get; set; }
    }
}