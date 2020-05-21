/*  CTRADER GURU --> Indicator Template 1.0.8

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{

    // --> Estensioni che rendono il codice più leggibile
    #region Extensions

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per il simbolo
    /// </summary>
    public static class SymbolExtensions
    {

        /// <summary>
        /// Converte il numero di pips corrente da digits a double
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Digits</param>
        /// <returns></returns>
        public static double DigitsToPips(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips / MySymbol.PipSize, 2);

        }

        /// <summary>
        /// Converte il numero di pips corrente da double a digits
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Double (2)</param>
        /// <returns></returns>
        public static double PipsToDigits(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips * MySymbol.PipSize, MySymbol.Digits);

        }

    }

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per le Bars
    /// </summary>
    public static class BarsExtensions
    {

        /// <summary>
        /// Converte l'indice di una bar partendo dalla data di apertura
        /// </summary>
        /// <param name="MyTime">La data e l'ora di apertura della candela</param>
        /// <returns></returns>
        public static int GetIndexByDate(this Bars MyBars, DateTime MyTime)
        {

            for (int i = MyBars.ClosePrices.Count - 1; i >= 0; i--)
            {

                if (MyTime == MyBars.OpenTimes[i]) return i;

            }

            return -1;

        }

    }

    #endregion

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FiboPivotPoint : Indicator
    {

        #region Enums

        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Fibo Pivot Point";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.2";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/fibo-pivot-point/")]
        public string ProductInfo { get; set; }

        [Parameter("Fibo 1°", Group = "Params", DefaultValue = 0.382)]
        public double Fibo1 { get; set; }

        [Parameter("Fibo 2°", Group = "Params", DefaultValue = 0.618)]
        public double Fibo2 { get; set; }

        [Parameter("Fibo 3°", Group = "Params", DefaultValue = 1.0)]
        public double Fibo3 { get; set; }

        [Parameter("Only Last Day ?", Group = "Options", DefaultValue = true)]
        public bool OnlyLastDay { get; set; }

        [Parameter("Show Labels", Group = "Options", DefaultValue = true)]
        public bool ShowLabels { get; set; }

        [Parameter("Pivot Color", Group = "Styles", DefaultValue = MyColors.Black)]
        public MyColors PivotColor { get; set; }

        [Parameter("Support Color", Group = "Styles", DefaultValue = MyColors.Red)]
        public MyColors SupportColor { get; set; }

        [Parameter("Resistance Color", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors ResistanceColor { get; set; }

        #endregion

        #region Property

        private DateTime _previousPeriodStartTime;
        private int _previousPeriodStartIndex;
        private TimeFrame PivotTimeFrame;

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            if (TimeFrame <= TimeFrame.Hour)
            {

                PivotTimeFrame = TimeFrame.Daily;

            }
            else if (TimeFrame < TimeFrame.Daily)
            {

                PivotTimeFrame = TimeFrame.Weekly;

            }
            else
            {

                PivotTimeFrame = TimeFrame.Monthly;

            }

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            var currentPeriodStartTime = GetStartOfPeriod(Bars.OpenTimes[index]);

            if (currentPeriodStartTime == _previousPeriodStartTime)
                return;

            if (index > 0)
                CalculatePivots(_previousPeriodStartTime, _previousPeriodStartIndex, currentPeriodStartTime, index);

            _previousPeriodStartTime = currentPeriodStartTime;
            _previousPeriodStartIndex = index;

        }

        #endregion

        #region Private Methods


        private DateTime GetStartOfPeriod(DateTime dateTime)
        {

            return CutToOpenByNewYork(dateTime, PivotTimeFrame);

        }

        private DateTime GetEndOfPeriod(DateTime dateTime)
        {

            if (PivotTimeFrame == TimeFrame.Monthly)
            {

                return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1);

            }

            return AddPeriod(CutToOpenByNewYork(dateTime, PivotTimeFrame), PivotTimeFrame);

        }
        
        private static DateTime CutToOpenByNewYork(DateTime date, TimeFrame timeFrame)
        {

            if (timeFrame == TimeFrame.Daily)
            {

                var hourShift = (date.Hour + 24 - 17) % 24;
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, DateTimeKind.Unspecified).AddHours(-hourShift);

            }

            if (timeFrame == TimeFrame.Weekly)
                return GetStartOfTheWeek(date);

            if (timeFrame == TimeFrame.Monthly)
            {

                return new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Unspecified);

            }

            throw new ArgumentException(string.Format("Unknown timeframe: {0}", timeFrame), "timeFrame");

        }

        private static DateTime GetStartOfTheWeek(DateTime dateTime)
        {

            return dateTime.Date.AddDays((double)DayOfWeek.Sunday - (double)dateTime.Date.DayOfWeek).AddHours(-7);

        }
        
        private void CalculatePivots(DateTime startTime, int startIndex, DateTime startTimeOfNextPeriod, int index)
        {

            DateTime currentOpenTime = Bars.OpenTimes[index];
            DateTime today = DateTime.Now.AddDays(-1);

            // Only show output in todays timeframe
            if (OnlyLastDay && currentOpenTime.Date.Day != today.Date.Day)
                return;

            var high = Bars.HighPrices[startIndex];
            var low = Bars.LowPrices[startIndex];
            var close = Bars.ClosePrices[startIndex];
            var i = startIndex + 1;

            while (GetStartOfPeriod(Bars.OpenTimes[i]) == startTime && i < Bars.ClosePrices.Count)
            {
                high = Math.Max(high, Bars.HighPrices[i]);
                low = Math.Min(low, Bars.LowPrices[i]);
                close = Bars.LowPrices[i];

                i++;
            }

            var pivotStartTime = startTimeOfNextPeriod;
            var pivotEndTime = GetEndOfPeriod(startTimeOfNextPeriod);

            var pivot = (high + low + close) / 3;

            var r1 = pivot + (Fibo1 * (high - low));
            var s1 = pivot - (Fibo1 * (high - low));

            var r2 = pivot + (Fibo2 * (high - low));
            var s2 = pivot - (Fibo2 * (high - low));

            var r3 = pivot + (Fibo3 * (high - low));
            var s3 = pivot - (Fibo3 * (high - low));

            Chart.DrawTrendLine("pivot " + startIndex, pivotStartTime, pivot, pivotEndTime, pivot, Color.FromName( PivotColor.ToString( "G" ) ), 1, LineStyle.DotsVeryRare);
            Chart.DrawTrendLine("r1 " + startIndex, pivotStartTime, r1, pivotEndTime, r1, Color.FromName(ResistanceColor.ToString("G") ), 1, LineStyle.DotsRare);
            Chart.DrawTrendLine("r2 " + startIndex, pivotStartTime, r2, pivotEndTime, r2, Color.FromName(ResistanceColor.ToString("G")), 1, LineStyle.Lines);
            Chart.DrawTrendLine("r3 " + startIndex, pivotStartTime, r3, pivotEndTime, r3, Color.FromName(ResistanceColor.ToString("G")), 1, LineStyle.Solid);
            Chart.DrawTrendLine("s1 " + startIndex, pivotStartTime, s1, pivotEndTime, s1, Color.FromName(SupportColor.ToString("G")), 1, LineStyle.DotsRare);
            Chart.DrawTrendLine("s2 " + startIndex, pivotStartTime, s2, pivotEndTime, s2, Color.FromName(SupportColor.ToString("G")), 1, LineStyle.Lines);
            Chart.DrawTrendLine("s3 " + startIndex, pivotStartTime, s3, pivotEndTime, s3, Color.FromName(SupportColor.ToString("G")), 1, LineStyle.Solid);

            if (!ShowLabels)
                return;

            Chart.DrawText("Lpivot " + startIndex, "P", index, pivot, Color.FromName(PivotColor.ToString("G")));
            Chart.DrawText("Lr1 " + startIndex, "R1", index, r1, Color.FromName(ResistanceColor.ToString("G")));
            Chart.DrawText("Lr2 " + startIndex, "R2", index, r2, Color.FromName(ResistanceColor.ToString("G")));
            Chart.DrawText("Lr3 " + startIndex, "R3", index, r3, Color.FromName(ResistanceColor.ToString("G")));
            Chart.DrawText("Ls1 " + startIndex, "S1", index, s1, Color.FromName(SupportColor.ToString("G")));
            Chart.DrawText("Ls2 " + startIndex, "S2", index, s2, Color.FromName(SupportColor.ToString("G")));
            Chart.DrawText("Ls3 " + startIndex, "S3", index, s3, Color.FromName(SupportColor.ToString("G")));


        }

        public DateTime AddPeriod(DateTime dateTime, TimeFrame timeFrame)
        {

            if (timeFrame == TimeFrame.Daily)
            {

                return dateTime.AddDays(1);

            }

            if (timeFrame == TimeFrame.Weekly)
            {

                return dateTime.AddDays(7);

            }

            if (timeFrame == TimeFrame.Monthly)
                return dateTime.AddMonths(1);

            throw new ArgumentException(string.Format("Unknown timeframe: {0}", timeFrame), "timeFrame");

        }

        #endregion

    }

}