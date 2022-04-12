using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Core.Models.ConfigTTS
{
    public static class GoogleTTS
    {

        public static string Speak(string title, string plainText)
        {
            var client = TextToSpeechClient.Create();
            string titleCrop = title.Substring(0, 10).ToLower().Replace("-", "").Trim();
            titleCrop = titleCrop + "sound" + DateTime.Now.ToShortDateString().Replace(".","") + ".mp3";
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

            using (var outpot = File.Create(@"wwwroot\sound\"+titleCrop.Trim().ToLower()))
            {
                response.AudioContent.WriteTo(outpot);
            }

            return titleCrop;
        }

        public static string HtmlToPlainTextTTS(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public static string GenerateSlug(string title, int Id)
        {
            string phrase = string.Format("{0}-{1}", Id, title);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

    }
}   
