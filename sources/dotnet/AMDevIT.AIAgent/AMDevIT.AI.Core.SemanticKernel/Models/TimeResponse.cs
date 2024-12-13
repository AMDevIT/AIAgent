using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMDevIT.AI.Core.Models;

public class TimeResponse
{
    #region Properties

    [JsonPropertyName("currentTime")]
    [Description("The current time.")]
    public DateTimeOffset CurrentTime
    {
        get;
        set;
    }

    #endregion
}
