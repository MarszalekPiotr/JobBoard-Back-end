using JobBoard.Domain.FormDefinitionSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using JobBoard.Domain.Enums;
using JobBoard.Application.Exceptions;

namespace JobBoard.Infrastructure.Persistance
{  /// <summary>
///  zmienic base fild na form definition
/// </summary>
    public class FormDefinitionConverter : JsonConverter<FormDefinition>
    {
        public override FormDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                /// tu logika wyciagania z jsona pojedynczysz field definition
                /// foreach i lista ktora zwracamy na koncu ?
                /// 

                var fields = new List<FieldDefinition>();



                foreach (var element in doc.RootElement.GetProperty("FieldDefinitions").EnumerateArray())
                {   // dziala tylko d bazy nie do controlera xd
                   // var typeString = element.GetProperty("EnumFieldType").GetString();
                    var typeString = (EnumFieldType)(element.GetProperty("EnumFieldType").GetInt64());
                    Enum.TryParse(typeString.ToString(), out EnumFieldType fieldType);
                    FieldDefinition result;

                    switch (fieldType)
                    {
                        case EnumFieldType.List:
                            try
                            {
                                result = JsonSerializer.Deserialize<ListFieldDefinition>(element.GetRawText(), options);
                            }
                            catch
                            {
                                throw new ErrorException("deserialization not possible");
                            }
                            
                            break;
                        case EnumFieldType.Int:
                            result = JsonSerializer.Deserialize<IntFieldDefinition>(element.GetRawText(), options) ?? throw new ErrorException("deserialize exception");
                            break;
                        case EnumFieldType.Date:
                            result = JsonSerializer.Deserialize<DateFieldDefinition>(element.GetRawText(), options) ?? throw new ErrorException("deserialize exception");
                            break;
                        case EnumFieldType.Double:
                            result = JsonSerializer.Deserialize<DoubleFieldDefnition>(element.GetRawText(), options) ?? throw new ErrorException("deserialize exception");
                            break;
                        case EnumFieldType.Bool:
                            result = JsonSerializer.Deserialize<BoolFieldDefinition>(element.GetRawText(), options) ?? throw new ErrorException("deserialize exception");
                            break;
                        case EnumFieldType.Percentage:
                            result = JsonSerializer.Deserialize<PercentageFieldDefinition>(element.GetRawText(), options) ?? throw new ErrorException("deserialize exception");
                            break;
                        case EnumFieldType.String:
                            result = JsonSerializer.Deserialize<StringFieldDefinition>(element.GetRawText(), options) ?? throw new ErrorException("deserialize exception");
                            break;
                        default:
                            throw new JsonException($"Unknown type: {fieldType}");
                    }

                    fields.Add(result);
                }
                return new FormDefinition() { FieldDefinitions = fields };
                
            }
        }

        public override void Write(Utf8JsonWriter writer, FormDefinition value, JsonSerializerOptions options)
        {


            //writer.WriteStartObject();
            //writer.WritePropertyName("FormDefinition");
            writer.WriteStartObject(); // Start writing the outer JSON object

            if (value.FieldDefinitions != null)
            {
                writer.WritePropertyName("FieldDefinitions");
                writer.WriteStartArray(); // Start writing the array of field definitions

                foreach (var fieldDefinition in value.FieldDefinitions)
                {
                    writer.WriteStartObject(); // Start writing the JSON object for a field definition

                    var properties = fieldDefinition.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyName = property.Name;
                        var propertyValue = property.GetValue(fieldDefinition);

                        Console.WriteLine($"Serializing property: {propertyName}, Value: {propertyValue}");

                        if (propertyValue != null)
                        {
                            writer.WritePropertyName(propertyName);
                            JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
                        }
                    }

                    writer.WriteEndObject(); // End writing the JSON object for a field definition
                }

                writer.WriteEndArray(); // End writing the array of field definitions
            }

            writer.WriteEndObject(); // End writing the outer JSON object
            //writer.WriteEndObject();
        }


        //foreach (var fieldDefinition in value.FieldDefinitions)
        //{
        //    JsonSerializer.Serialize(writer, fieldDefinition, fieldDefinition.GetType(), options);
        //}


        //writer.WriteStartObject();
        //writer.WritePropertyName("FieldDefinitions");
        //JsonSerializer.Serialize(writer, value.FieldDefinitions, options);
        //writer.WriteEndObject();
        //JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);

    }
}
