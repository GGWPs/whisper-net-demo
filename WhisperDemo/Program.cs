// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using Whisper.net;
using Whisper.net.Ggml;

class Program
{
    static async Task Main(string[] args)
    {

        var audioFilePath = "";

        // Check if the provided file exists
        if (!File.Exists(audioFilePath))
        {
            Console.WriteLine("Audio file not found.");
            return;
        }

        // Download the Whisper model if it doesn't exist
        var modelName = "ggml-large-v3.bin";
        if (!File.Exists(modelName))
        {
            using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(GgmlType.Base);
            using var fileWriter = File.OpenWrite(modelName);
            await modelStream.CopyToAsync(fileWriter);
        }

        // Convert any audio format to WAV using FFmpeg
        var wavFilePath = ConvertToWav(audioFilePath);

        if (string.IsNullOrEmpty(wavFilePath))
        {
            Console.WriteLine("Failed to convert audio file to WAV.");
            return;
        }

        // Initialize Whisper with the downloaded model
        using var whisperFactory = WhisperFactory.FromPath(modelName);
        using var processor = whisperFactory.CreateBuilder()
            .WithLanguage("auto")
            .Build();

        // Open the WAV file and process it
        using var fileStream = File.OpenRead(wavFilePath);

        await foreach (var result in processor.ProcessAsync(fileStream))
        {
            Console.WriteLine($"{result.Start}->{result.End}: {result.Text}");
        }

        // Clean up the temporary WAV file if necessary
        if (wavFilePath != audioFilePath)
        {
            File.Delete(wavFilePath);
        }
    }

    static string ConvertToWav(string inputFilePath)
    {
        try
        {
            // Output WAV file path
            var outputFilePath = Path.ChangeExtension(inputFilePath, ".wav");

            // Construct the FFmpeg conversion command
            string ffmpegArgs = $"-i \"{inputFilePath}\" -ar 16000 -ac 1 -sample_fmt s16 \"{outputFilePath}\"";

            // Start the FFmpeg process
            var ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = ffmpegArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            ffmpegProcess.Start();
            ffmpegProcess.WaitForExit();

            // Check if the conversion was successful
            if (ffmpegProcess.ExitCode != 0)
            {
                throw new Exception("FFmpeg failed to convert the file.");
            }

            return outputFilePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting file to WAV: {ex.Message}");
            return null;
        }
    }
}