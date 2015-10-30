using System;
using System.Collections;
using Microsoft.AspNet.Http;
using SciVacancies.Captcha.CaptchaStores;
using SciVacancies.Captcha.TextGenerators;

namespace SciVacancies.Captcha
{
    /// <summary>
    /// Amount of random font warping to apply to rendered text
    /// </summary>
    public enum FontWarpFactor
    {
        None,
        Low,
        Medium,
        High,
        Extreme
    }

    /// <summary>
    /// Amount of background noise to add to rendered image
    /// </summary>
    public enum BackgroundNoiseLevel
    {
        None,
        Low,
        Medium,
        High,
        Extreme
    }

    /// <summary>
    /// Amount of curved line noise to add to rendered image
    /// </summary>
    public enum LineNoiseLevel
    {
        None,
        Low,
        Medium,
        High,
        Extreme
    }


    public class CaptchaConfiguration
    {
        private static readonly IDictionary _config = null;//(IDictionary)ConfigurationManager.GetSection("CaptchaConfiguration");

        public static FontWarpFactor FontFactor = FontWarpFactor.Medium;
        public static BackgroundNoiseLevel BackgroundNoise = BackgroundNoiseLevel.Medium;
        public static LineNoiseLevel LineNoise = LineNoiseLevel.Medium;
        public static string Fonts = "Verdana";
        public static string Colors = "165.0.68";
        public static string Letters = "0123456789";
        public static int TextLength = 4;
        public static int MaxShift;
        public static int MinShift;
        public static int MaxAngle = 30;
        public static int MinAngle = -15;
        public static int FontSizeMin = 90;
        public static int FontSizeMax = 90;
        public static string Referer = "";

        public static Func<HttpContext, HttpSessionCaptchaStore> DefaultStoreFactory { get; set; }
        public static Func<ICaptchaTextGenerator> DefaultTextGeneratorFactory { get; set; }

        static CaptchaConfiguration()
        {
            DefaultStoreFactory = (context) => new HttpSessionCaptchaStore(context);
            DefaultTextGeneratorFactory = () => new DefaultCaptchaTextGenerator();

            if (_config != null)
            {
                if (_config.Contains("FontWarpFactor"))
                {
                    string s = _config["FontWarpFactor"].ToString();
                    switch (s)
                    {
                        case "None":
                            FontFactor = FontWarpFactor.None;
                            break;
                        case "Low":
                            FontFactor = FontWarpFactor.Low;
                            break;
                        case "Medium":
                            FontFactor = FontWarpFactor.Medium;
                            break;
                        case "High":
                            FontFactor = FontWarpFactor.High;
                            break;
                        case "Extreme":
                            FontFactor = FontWarpFactor.Extreme;
                            break;
                    }
                }
                if (_config.Contains("BackgroundNoiseLevel"))
                {
                    string s = _config["BackgroundNoiseLevel"].ToString();
                    switch (s)
                    {
                        case "None":
                            BackgroundNoise = BackgroundNoiseLevel.None;
                            break;
                        case "Low":
                            BackgroundNoise = BackgroundNoiseLevel.Low;
                            break;
                        case "Medium":
                            BackgroundNoise = BackgroundNoiseLevel.Medium;
                            break;
                        case "High":
                            BackgroundNoise = BackgroundNoiseLevel.High;
                            break;
                        case "Extreme":
                            BackgroundNoise = BackgroundNoiseLevel.Extreme;
                            break;
                    }
                }
                if (_config.Contains("LineNoiseLevel"))
                {
                    string s = _config["LineNoiseLevel"].ToString();
                    switch (s)
                    {
                        case "None":
                            LineNoise = LineNoiseLevel.None;
                            break;
                        case "Low":
                            LineNoise = LineNoiseLevel.Low;
                            break;
                        case "Medium":
                            LineNoise = LineNoiseLevel.Medium;
                            break;
                        case "High":
                            LineNoise = LineNoiseLevel.High;
                            break;
                        case "Extreme":
                            LineNoise = LineNoiseLevel.Extreme;
                            break;
                    }
                }
                if (_config.Contains("Fonts"))
                {
                    Fonts = _config["Fonts"].ToString();
                }
                if (_config.Contains("Colors"))
                {
                    Colors = _config["Colors"].ToString();
                }
                if (_config.Contains("Letters"))
                {
                    Letters = _config["Letters"].ToString();
                }
                if (_config.Contains("TextLength"))
                {
                    TextLength = Int32.Parse(_config["TextLength"].ToString());
                }
                if (_config.Contains("MaxShift"))
                {
                    MaxShift = Int32.Parse(_config["MaxShift"].ToString());
                }
                if (_config.Contains("MinShift"))
                {
                    MinShift = Int32.Parse(_config["MinShift"].ToString());
                }
                if (_config.Contains("MaxAngle"))
                {
                    MaxAngle = Int32.Parse(_config["MaxAngle"].ToString());
                }
                if (_config.Contains("MinAngle"))
                {
                    MinAngle = Int32.Parse(_config["MinAngle"].ToString());
                }
                if (_config.Contains("FontSizeMax"))
                {
                    FontSizeMax = Int32.Parse(_config["FontSizeMax"].ToString());
                }
                if (_config.Contains("FontSizeMin"))
                {
                    FontSizeMin = Int32.Parse(_config["FontSizeMin"].ToString());
                }
                if (_config.Contains("Referer"))
                {
                    Referer = _config["Referer"].ToString();
                }
            }
        }


    }

}
