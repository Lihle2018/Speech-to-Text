using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Speech_to_Text
{
        class Program
        {
            static async Task Main(string[] args)
            {
                string speechKey = "";
                string region = "eastus"; // e.g., "eastus"

                var config = SpeechConfig.FromSubscription(speechKey, region);
                config.SpeechRecognitionLanguage = "en-US";

                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                using var recognizer = new SpeechRecognizer(config, audioConfig);

                Console.WriteLine("Say something... (Press Ctrl+C to stop)");

                // Subscribe to events
                recognizer.Recognizing += (s, e) =>
                {
                    Console.WriteLine($"[Partial] {e.Result.Text}");
                };

                recognizer.Recognized += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        Console.WriteLine($"[Final] {e.Result.Text}");
                    }
                    else if (e.Result.Reason == ResultReason.NoMatch)
                    {
                        Console.WriteLine("[NoMatch] Speech could not be recognized.");
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    Console.WriteLine($"[Canceled] Reason: {e.Reason}");
                    if (e.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"[Error] Code: {e.ErrorCode}, Details: {e.ErrorDetails}");
                    }
                };

                recognizer.SessionStarted += (s, e) =>
                {
                    Console.WriteLine("[SessionStarted]");
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    Console.WriteLine("[SessionStopped]");
                    Console.WriteLine("Press any key to exit...");
                };

                // Start recognition
                await recognizer.StartContinuousRecognitionAsync();

                // Wait for user input to stop
                Console.ReadKey();

                await recognizer.StopContinuousRecognitionAsync();
            }
        }
}
