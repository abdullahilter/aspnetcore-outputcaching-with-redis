using System.Text.Json.Serialization;

namespace aspnetcore_outputcaching_with_redis;

public class Customer : CustomerDto
{
    [JsonPropertyOrder(98)]
    public DateTime CreatedOn { get; set; }

    [JsonPropertyOrder(99)]
    public DateTime? ModifiedOn { get; set; }
}