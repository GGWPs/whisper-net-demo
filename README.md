# Whisper-Net-Demo: A .NET Implementation using Whisper.Net

## Overview
This project, `whisper-net-demo`, demonstrates a basic .NET implementation of OpenAI's Whisper model using the Whisper.Net package. Whisper is a state-of-the-art automatic speech recognition (ASR) system, capable of transcribing speech into text with high accuracy. This demo integrates Whisper into a .NET project to transcribe audio files into text.

## Features
- **Speech-to-Text Transcription**: Transcribes speech from audio files into text using Whisper's ASR capabilities.
- **Cross-Platform Compatibility**: Built using .NET, this project runs on Windows, Linux, and macOS.
- **Flexible Audio Format Support**: Can handle multiple audio formats (e.g., WAV, MP3, FLAC).

## Prerequisites
Before running this demo, make sure you have the following installed on your machine:
- [.NET SDK 6.0 or later](https://dotnet.microsoft.com/download)
- Whisper.Net package (see installation instructions below)

## Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/GGWPs/whisper-net-demo.git
   cd whisper-net-demo
   ```

2. **Install Whisper.Net NuGet package**:
   You can install the Whisper.Net package using the .NET CLI:
   ```bash
   dotnet add package Whisper.Net
   ```

   Alternatively, use the NuGet Package Manager in Visual Studio to search for and install the `Whisper.Net` package.

3. **Build the project**:
   Run the following command to restore dependencies and build the project:
   ```bash
   dotnet build
   ```

## Usage

To use this demo, simply provide an audio file that you want to transcribe. The default language is set to English, but you can customize this based on Whisper's language model support.

### Sample Command

You can run the demo with an example audio file using the command below:
```bash
dotnet run --audio-file "path_to_audio_file.wav"
```


### Parameters
- `--audio-file` (Optional): Path to the audio file you wish to transcribe.

## Supported Audio Formats
Whisper.Net supports a variety of audio formats:
- WAV
- MP3
- FLAC
- And more...

## Configuration
You can configure different aspects of the Whisper model such as language, model size, and processing options. For more advanced users, consult the Whisper.Net documentation for configuring model parameters to suit your needs.

## Contributing
We welcome contributions to improve this demo. If you'd like to contribute:
1. Fork the repository.
2. Create a new branch.
3. Make your changes and submit a pull request.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Acknowledgments
This project uses OpenAI's Whisper model and the Whisper.Net package. Special thanks to the open-source community for providing these tools.
