using System.Text.Json.Serialization;

namespace WasabiCli.Models.WalletWasabi;

public class SendInfo
{
    [JsonPropertyName("txid")]
    public string? TxId { get; set; }

    [JsonPropertyName("tx")]
    public string? Tx { get; set; }
}
