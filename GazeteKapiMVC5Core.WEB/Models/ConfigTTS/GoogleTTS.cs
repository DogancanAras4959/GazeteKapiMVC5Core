using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Models.ConfigTTS
{
    public static class GoogleTTS
    {

        public static void Speak(string plainText)
        {
            var client = TextToSpeechClient.Create();

            var input = new SynthesisInput
            {
                Text = plainText
            };

            var voiceSelection = new VoiceSelectionParams 
            {
                LanguageCode = "tr-TR",
                SsmlGender = SsmlVoiceGender.Female
            };

            var audioConfig = new AudioConfig 
            { 

            AudioEncoding = AudioEncoding.Mp3

            };

            var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            using (var outpot = File.Create("outpıt.mp3"))
            {
                response.AudioContent.WriteTo(outpot);
            }
        }
    }
}
