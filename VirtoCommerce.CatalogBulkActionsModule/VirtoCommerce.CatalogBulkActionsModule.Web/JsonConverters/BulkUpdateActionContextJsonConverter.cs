﻿namespace VirtoCommerce.CatalogBulkActionsModule.Web.JsonConverters
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.Platform.Core.Common;

    public class BulkUpdateActionContextJsonConverter : JsonConverter
    {
        private static readonly Type[] _knownTypes = { typeof(BulkUpdateActionContext) };

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return _knownTypes.Any(x => x.IsAssignableFrom(objectType));
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var typeName = objectType.Name;
            var contextTypeName = obj["contextTypeName"];
            if (contextTypeName != null)
            {
                typeName = contextTypeName.Value<string>();
            }

            var result = AbstractTypeFactory<BulkUpdateActionContext>.TryCreateInstance(typeName);
            if (result == null)
            {
                throw new NotSupportedException("Unknown BulkUpdateActionContext type: " + typeName);
            }

            serializer.Populate(obj.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}