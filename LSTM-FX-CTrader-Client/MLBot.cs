// MIT License
// Copyright (c) 2020 Adam Tibi (https://linkedin.com/in/adamtibi/ , https://adamtibi.net)

// CTrader Automate Bot

using System;
using System.Linq;
using System.Net;
using System.Text;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class MLBot : Robot
    {
        private const string Name = "MLBot";

        [Parameter("Base Url", DefaultValue = "http://localhost:5000/predict")]
        public string BaseUrl { get; set; }

        [Parameter("Batch Size", MinValue = 1, DefaultValue = 128)]
        public int BatchSize { get; set; }

        [Parameter("Window Size", MinValue = 1, DefaultValue = 256)]
        public int WindowSize { get; set; }

        [Parameter("MA Periods", MinValue = 1, DefaultValue = 14)]
        public int MAPeriods { get; set; }

        [Parameter("Pips", MinValue = 1, DefaultValue = 8)]
        public int Pips { get; set; }

        [Parameter("Prediction Size", MinValue = 1, DefaultValue = 4)]
        public int PredSize { get; set; }

        [Parameter("Volume", Group = "Standard", DefaultValue = 20000)]
        public double Volume { get; set; }

        [Parameter("Max Spread Limit (Pips)", Group = "Standard", DefaultValue = 1.5)]
        public double MaxSpreadLimitInPips { get; set; }

        private WebClient _webClient;
        private double _maxSpreadLimitAbsolute = 0;
        private string _baseUrlParam;

        private string GetUrl()
        {
            StringBuilder sb = new StringBuilder(_baseUrlParam);
            sb.Append("/");
            sb.Append(Time.ToString("yyyyMMddHHmmss"));
            sb.Append("/");

            for (int i = WindowSize + MAPeriods - 1; i >= 0; i--)
            {
                sb.Append(((Bars.Last(i).High + Bars.Last(i).Low)/2).ToString());
                sb.Append(",");
            }
            sb.Length = sb.Length - 1; // removing the last comma
            return sb.ToString();
        }

        private TradeType? GetMLPrediction()
        {
            string url = GetUrl();
            string tradeType = _webClient.DownloadString(url);
            switch (tradeType)
            {
                case "1":
                    return TradeType.Buy;
                case "-1":
                    return TradeType.Sell;
                case "0":
                    return null;
                default:
                    throw new InvalidOperationException("Not an expected return from the ML: " + (tradeType ?? "(null)"));
            }
        }

        protected override void OnStart()
        {
            _webClient = new WebClient();
            _maxSpreadLimitAbsolute = MaxSpreadLimitInPips * Symbol.PipSize;
            _baseUrlParam = BaseUrl + "/" + Symbol.Name.ToLower() + "/" + BatchSize + "/" + WindowSize + "/" + MAPeriods + "/" + (Symbol.PipSize * Pips) + "/" + PredSize;
        }

        protected override void OnBar()
        {
            // Prevents running this in production
            if (!IsBacktesting) 
            {
                return;
            }
            if ((Ask - Bid) > _maxSpreadLimitAbsolute)
            {
                return;
            }

            if (Positions.FindAll(Name, SymbolName).Any())
            {
                return;
            }

            TradeType? tradeType = GetMLPrediction();

            if (tradeType == null)
            {
                return;
            }

            ExecuteMarketOrder(tradeType.Value, Symbol.Name, Volume, Name, Pips, Pips);
        }

    }
}