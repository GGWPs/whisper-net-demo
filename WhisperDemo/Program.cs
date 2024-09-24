// See https://aka.ms/new-console-template for more information
using NAudio.Wave;
using Whisper.net;
using Whisper.net.Ggml;

class Program
{
    static async Task Main(string[] args)
    {

        var audioFilePath = "c:/sounds/test.mp3";


        var modelName = "ggml-large-v3.bin";
        if (!File.Exists(modelName))
        {
            using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(GgmlType.Base);
            using var fileWriter = File.OpenWrite(modelName);
            await modelStream.CopyToAsync(fileWriter);
        }

        // Convert any audio format to WAV
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
        /*if (wavFilePath != audioFilePath)
        {
            File.Delete(wavFilePath);
        }*/
    }

    static string ConvertToWav(string inputFilePath)
    {
        try
        {
            // Output WAV file path
            var outputFilePath = Path.ChangeExtension(inputFilePath, ".wav");

            using (var reader = CreateReaderForFile(inputFilePath))
            {
                // Ensure the WAV format is PCM (Pulse Code Modulation), 16kHz, 16-bit, mono
                var pcmFormat = new WaveFormat(16000, 16, 1); // 16kHz, 16-bit, mono
                using (var resampler = new MediaFoundationResampler(reader, pcmFormat))
                {
                    resampler.ResamplerQuality = 60;  // Set quality level (0-60), 60 is highest
                    WaveFileWriter.CreateWaveFile(outputFilePath, resampler);
                }
            }

            return outputFilePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting file to WAV: {ex.Message}");
            return null;
        }
    }

    // Factory method to create the correct type of reader depending on the file format
    static WaveStream CreateReaderForFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        if (extension == ".mp3")
        {
            return new Mp3FileReader(filePath);
        }
        else if (extension == ".wav")
        {
            return new WaveFileReader(filePath);
        }
        else
        {
            // Use MediaFoundationReader for most other formats (e.g., AAC, WMA, etc.)
            return new MediaFoundationReader(filePath);
        }
    }
}