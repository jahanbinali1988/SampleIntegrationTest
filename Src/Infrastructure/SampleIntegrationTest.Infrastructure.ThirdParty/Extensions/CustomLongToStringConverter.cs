using System.Buffers.Text;
using System.Buffers;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleIntegrationTest.Infrastructure.ThirdParty.Extensions
{
    public class CustomLongToStringConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> readOnlySpan;
                if (!reader.HasValueSequence)
                {
                    readOnlySpan = reader.ValueSpan;
                }
                else
                {
                    ReadOnlySequence<byte> sequence = reader.ValueSequence;
                    readOnlySpan = BuffersExtensions.ToArray(in sequence);
                }

                ReadOnlySpan<byte> source = readOnlySpan;
                if (Utf8Parser.TryParse(source, out long value, out int bytesConsumed, '\0') && source.Length == bytesConsumed)
                {
                    return value;
                }

                if (long.TryParse(reader.GetString(), out value))
                {
                    return value;
                }
            }

            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }
}
