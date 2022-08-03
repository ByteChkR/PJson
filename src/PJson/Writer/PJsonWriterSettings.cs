namespace PJson.Writer;

public class PJsonWriterSettings
{
    public static readonly PJsonWriterSettings Default = new PJsonWriterSettings();
    public bool PrettyPrint { get; set; } = true;
}