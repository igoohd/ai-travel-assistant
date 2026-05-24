namespace AiTravelPlanner.Domain.Trips;

public sealed record CurrencyCode
{
    public string Value { get; }

    public CurrencyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Currency code is required.", nameof(value));
        }

        var normalizedValue = value.Trim().ToUpperInvariant();

        if (normalizedValue.Length != 3)
        {
            throw new ArgumentException("Currency code must use a 3-letter ISO format.", nameof(value));
        }

        if (!normalizedValue.All(char.IsLetter))
        {
            throw new ArgumentException("Currency code must contain only letters.", nameof(value));
        }

        Value = normalizedValue;
    }

    public override string ToString()
    {
        return Value;
    }
}