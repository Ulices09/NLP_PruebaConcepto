using System;

namespace NLP_PruebaConcepto.Constants {
    public static class APIUris
    {
        public static class AzureSTT {
            public static readonly Uri ShortPhraseUrl = new Uri(@"wss://speech.platform.bing.com/api/service/recognition");
            public static readonly Uri LongDictationUrl = new Uri(@"wss://speech.platform.bing.com/api/service/recognition/continuous");
        } 
    }
}