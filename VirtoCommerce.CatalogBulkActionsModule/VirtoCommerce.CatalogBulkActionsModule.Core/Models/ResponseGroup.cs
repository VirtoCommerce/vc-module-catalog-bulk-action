﻿namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using System;

    [Flags]
    public enum ResponseGroup
    {
        WithProducts = 1,
        WithCategories = 2,
        WithProperties = 4,
        WithCatalogs = 8,
        WithVariations = 16,
        Full = WithCatalogs | WithCategories | WithProperties | WithProducts | WithVariations
    }
}