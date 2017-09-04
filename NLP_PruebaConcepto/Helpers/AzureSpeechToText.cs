using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bing.Speech;

namespace NLP_PruebaConcepto.Helpers
{
    public class AzureSpeechToText
    {
        private static readonly Task CompletedTask = Task.FromResult(true);
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public async Task Run(string audioFile, string locale, Uri serviceUrl, string subscriptionKey)
        {
            // create the preferences object
            var preferences = new Preferences(locale, serviceUrl, new CognitiveServicesAuthorizationProvider(subscriptionKey));

            // Create a a speech client
            using (var speechClient = new SpeechClient(preferences))
            {
                speechClient.SubscribeToPartialResult(this.OnPartialResult);
                speechClient.SubscribeToRecognitionResult(this.OnRecognitionResult);

                // create an audio content and pass it a stream.
                using (var audio = new FileStream(audioFile, FileMode.Open, FileAccess.Read))
                {
                    var deviceMetadata = new DeviceMetadata(DeviceType.Near, DeviceFamily.Desktop, NetworkType.Ethernet, OsName.Windows, "1607", "Dell", "T3600");
                    var applicationMetadata = new ApplicationMetadata("SampleApp", "1.0.0");
                    var requestMetadata = new RequestMetadata(Guid.NewGuid(), deviceMetadata, applicationMetadata, "SampleAppService");

                    await speechClient.RecognizeAsync(new SpeechInput(audio, requestMetadata), this.cts.Token).ConfigureAwait(false);
                }
            }
        }

        public Task OnRecognitionResult(RecognitionResult args)
        {
            var response = args;

            // Print the recognition status.
            Console.WriteLine("***** Phrase Recognition Status = [{0}] ***", response.RecognitionStatus);
            if (response.Phrases != null)
            {
                foreach (var result in response.Phrases)
                {
                    // Print the recognition phrase display text.
                    Console.WriteLine("{0} (Confidence:{1})", result.DisplayText, result.Confidence);
                }
            }

            return CompletedTask;
        }

        public Task OnPartialResult(RecognitionPartialResult args)
        {
            // Print the partial response recognition hypothesis.
            Console.WriteLine(args.DisplayText);

            Console.WriteLine();

            return CompletedTask;
        }
    }
}