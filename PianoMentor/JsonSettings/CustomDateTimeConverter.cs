using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PianoMentor.JsonSettings
{
	public class CustomDateTimeConverter : DateTimeConverterBase
	{
		private const string Format = "yyyy-MM-dd'T'HH:mm:ss.fffffff";

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteValue(((DateTime)value).ToString(Format));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
			{
				return null;
			}

			if (reader.TokenType == JsonToken.Date)
			{
				return reader.Value;
			}

			if (reader.TokenType == JsonToken.String)
			{
				if (DateTime.TryParseExact(reader.Value.ToString(), Format, null, System.Globalization.DateTimeStyles.None, out DateTime result))
				{
					return result;
				}

				throw new JsonSerializationException($"Unable to parse '{reader.Value}' as DateTime.");
			}

			throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
		}
	}
}
