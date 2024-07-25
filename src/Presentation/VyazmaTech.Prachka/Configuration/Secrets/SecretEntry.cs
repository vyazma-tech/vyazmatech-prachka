using System.Text;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;

internal sealed record SecretEntry(string Key, string TextValue, string BinaryValue)
{
    public string Value => string.IsNullOrWhiteSpace(TextValue)
        ? Encoding.UTF8.GetString(Convert.FromBase64String(BinaryValue))
        : TextValue;

    public override string ToString()
    {
        return string.Join(" = ", Key, Value);
    }
}