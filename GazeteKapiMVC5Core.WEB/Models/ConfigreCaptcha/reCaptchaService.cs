using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Models.ConfigreCaptcha
{
    public class reCaptchaService
    {
        private reCaptchaSettings _settings;

        public reCaptchaService(IOptions<reCaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

        public virtual async Task<resPo> RecVer(string _token)
        {
            reCaptchaData data = new reCaptchaData();
            data.response = _token;
            data.secret = _settings.RecaptchaV3SecretKey;

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?=secret{data.secret}&response={data.response}");

            var _capResp = JsonConvert.DeserializeObject<resPo>(response);
            return _capResp;
        }
    }

    public class reCaptchaData
    {
        public string response { get; set; }
        public string secret { get; set; }
    }

    public class resPo
    {
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challange_ts { get; set; }
        public string hostname { get; set; }

    }
}
