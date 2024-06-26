﻿using JobBoard.Domain.FormDefinitionSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using JobBoard.Domain.Enums;

namespace JobBoard.Infrastructure.Persistance
{  /// <summary>
///  zmienic base fild na form definition
/// </summary>
    public class BaseFieldDefinitionConverter : JsonConverter<BaseFieldDefinition>
    {
        public override BaseFieldDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                /// tu logika wyciagania z jsona pojedynczysz field definition
                /// foreach i lista ktora zwracamy na koncu ?
                var typeString = doc.RootElement.GetProperty("EnumFieldType").GetString();
                Enum.TryParse(typeString, out EnumFieldType fieldType);
                BaseFieldDefinition result;

                switch (fieldType)
                {
                    case EnumFieldType.List:
                        result = JsonSerializer.Deserialize<ListFieldDefinition>(doc.RootElement.GetRawText(), options) ?? throw new Exception("deserialize exception")     ;
                        break;
                    case EnumFieldType.Int:
                        result = JsonSerializer.Deserialize<IntFieldDefinition>(doc.RootElement.GetRawText(), options) ?? throw new Exception("deserialize exception");
                        break;
                    case EnumFieldType.Date:
                        result = JsonSerializer.Deserialize<DateFieldDefinition>(doc.RootElement.GetRawText(), options) ?? throw new Exception("deserialize exception");
                        break;
                    case EnumFieldType.Double:
                        result = JsonSerializer.Deserialize<DoubleFieldDefnition>(doc.RootElement.GetRawText(), options) ?? throw new Exception("deserialize exception");
                        break;
                    case EnumFieldType.Bool:
                        result = JsonSerializer.Deserialize<BoolFieldDefinition>(doc.RootElement.GetRawText(), options) ?? throw new Exception("deserialize exception");
                        break;
                    case EnumFieldType.Percentage:
                        result = JsonSerializer.Deserialize<PercentageFieldDefinition>(doc.RootElement.GetRawText(), options) ?? throw new Exception("deserialize exception");
                        break;
                    case EnumFieldType.String:
                        result = JsonSerializer.Deserialize<StringFieldDefinition>(doc.RootElement.GetRawText(), options) ?? throw new Exception("deserialize exception") ;
                        break;
                    default:
                        throw new JsonException($"Unknown type: {fieldType}");
                }
                // zwroc kolekcje po foreachu
                return result;
            }
        }

        public override void Write(Utf8JsonWriter writer, BaseFieldDefinition value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }
}
