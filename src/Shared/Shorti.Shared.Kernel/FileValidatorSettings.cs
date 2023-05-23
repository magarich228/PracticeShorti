namespace Shorti.Shared.Kernel
{
    public class FileValidatorSettings
    {
        public string[] ValidFileTypes { get; set; } = new string[0];
        public uint MaxFileSize { get; set; }
    }
}
