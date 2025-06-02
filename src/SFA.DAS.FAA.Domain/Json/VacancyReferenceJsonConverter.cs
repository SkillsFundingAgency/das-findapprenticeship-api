using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Models;
using System;


namespace SFA.DAS.FAA.Domain.Json
{
    public class VacancyReferenceJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(VacancyReference);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return VacancyReference.None;

            if (reader.TokenType == JsonToken.String)
                return new VacancyReference((string)reader.Value);

            if (reader.TokenType == JsonToken.Integer)
                return new VacancyReference((long)(reader.Value ?? 0));

            return VacancyReference.None;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var reference = (VacancyReference)value;
            if (VacancyReference.None.Equals(reference))
                writer.WriteNull();
            else
                writer.WriteValue(reference.ToString());
        }
    }
}