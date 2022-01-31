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

                if (MyTime == MyBars.OpenTimes[i])
                    return i;

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

        public enum TimeFrameOptions
        {
            Auto,
            H4,
            Daily,
            Weekly,
            Monthly

        };

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Fibo Pivot Point";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.4";

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

        [Parameter("TimeFrame", Group = "Options", DefaultValue = TimeFrameOptions.Auto)]
        public TimeFrameOptions TFoption { get; set; }

        [Parameter("Show Labels", Group = "Options", DefaultValue = true)]
        public bool ShowLabels { get; set; }

        [Parameter("Pivot Color", Group = "Styles", DefaultValue = MyColors.White)]
        public MyColors PivotColor { get; set; }

        [Parameter("Support Color", Group = "Styles", DefaultValue = MyColors.Red)]
        public MyColors SupportColor { get; set; }

        [Parameter("Resistance Color", Group = "Styles", DefaultValue = MyColors.LimeGreen)]
        public MyColors ResistanceColor { get; set; }

        #endregion

        #region Property

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

            switch (TFoption)
            {

                case TimeFrameOptions.H4:

                    PivotTimeFrame = TimeFrame.Hour4;
                    break;
                
                case TimeFrameOptions.Daily:

                    PivotTimeFrame = TimeFrame.Daily;
                    break;
                
                case TimeFrameOptions.Weekly:

                    PivotTimeFrame = TimeFrame.Weekly;
                    break;
                
                case TimeFrameOptions.Monthly:

                    PivotTimeFrame = TimeFrame.Monthly;
                    break;
                
                default:

                    if (TimeFrame <= TimeFrame.Minute15)
                    {

                        PivotTimeFrame = TimeFrame.Hour4;

                    }
                    else if (TimeFrame < TimeFrame.Hour)
                    {

                        PivotTimeFrame = TimeFrame.Daily;

                    }
                    else if (TimeFrame < TimeFrame.Day3)
                    {

                        PivotTimeFrame = TimeFrame.Weekly;

                    }
                    else
                    {

                        PivotTimeFrame = TimeFrame.Monthly;

                    }

                    break;
            
            }            

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            _drawLevelFromCustomBar();

        }

        #endregion

        #region Private Methods
        private void _drawLevelFromCustomBar()
        {

            // --> Prelevo le candele scelte
            Bars BarsCustom = MarketData.GetBars(PivotTimeFrame);

            int index = BarsCustom.Count - 1;

            // --> Potrei non avere un numero sufficiente di candele
            if (index < 1 || (BarsCustom[index].Close == BarsCustom[index].Open))
                return;


            
            try
            {

                // --> TimeSpan DiffTime = BarsCustom[index - i].OpenTime.Subtract(BarsCustom[(index - i) - 1].OpenTime); // <-- Strategia da valutare

                DateTime thisCandle = BarsCustom[index].OpenTime;
                DateTime nextCandle = thisCandle.AddMinutes(_getTimeFrameCandleInMinutes(PivotTimeFrame));

                double high = BarsCustom[index - 1].High;
                double low = BarsCustom[index - 1].Low;
                double close = BarsCustom[index - 1].Close;

                double pivot = (high + low + close) / 3;

                double r1 = pivot + (Fibo1 * (high - low));
                double s1 = pivot - (Fibo1 * (high - low));

                double r2 = pivot + (Fibo2 * (high - low));
                double s2 = pivot - (Fibo2 * (high - low));

                double r3 = pivot + (Fibo3 * (high - low));
                double s3 = pivot - (Fibo3 * (high - low));

                Chart.DrawTrendLine("pivot ", thisCandle, pivot, nextCandle, pivot, Color.FromName(PivotColor.ToString("G")), 1, LineStyle.DotsVeryRare);
                Chart.DrawTrendLine("r1 ", thisCandle, r1, nextCandle, r1, Color.FromName(ResistanceColor.ToString("G")), 1, LineStyle.DotsRare);
                Chart.DrawTrendLine("r2 ", thisCandle, r2, nextCandle, r2, Color.FromName(ResistanceColor.ToString("G")), 1, LineStyle.Lines);
                Chart.DrawTrendLine("r3 ", thisCandle, r3, nextCandle, r3, Color.FromName(ResistanceColor.ToString("G")), 1, LineStyle.Solid);
                Chart.DrawTrendLine("s1 ", thisCandle, s1, nextCandle, s1, Color.FromName(SupportColor.ToString("G")), 1, LineStyle.DotsRare);
                Chart.DrawTrendLine("s2 ", thisCandle, s2, nextCandle, s2, Color.FromName(SupportColor.ToString("G")), 1, LineStyle.Lines);
                Chart.DrawTrendLine("s3 ", thisCandle, s3, nextCandle, s3, Color.FromName(SupportColor.ToString("G")), 1, LineStyle.Solid);

                if (!ShowLabels)
                    return;

                Chart.DrawText("Lpivot ", "P", nextCandle, pivot, Color.FromName(PivotColor.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                Chart.DrawText("Lr1 ", "R1", nextCandle, r1, Color.FromName(ResistanceColor.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                Chart.DrawText("Lr2 ", "R2", nextCandle, r2, Color.FromName(ResistanceColor.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                Chart.DrawText("Lr3 ", "R3", nextCandle, r3, Color.FromName(ResistanceColor.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                Chart.DrawText("Ls1 ", "S1", nextCandle, s1, Color.FromName(SupportColor.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                Chart.DrawText("Ls2 ", "S2", nextCandle, s2, Color.FromName(SupportColor.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                Chart.DrawText("Ls3 ", "S3", nextCandle, s3, Color.FromName(SupportColor.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;

            }
            catch
            {


            }

        }

        private int _getTimeFrameCandleInMinutes(TimeFrame MyCandle)
        {

            if (MyCandle == TimeFrame.Daily)
                return 60 * 24;
            if (MyCandle == TimeFrame.Day2)
                return 60 * 24 * 2;
            if (MyCandle == TimeFrame.Day3)
                return 60 * 24 * 3;
            if (MyCandle == TimeFrame.Hour)
                return 60;
            if (MyCandle == TimeFrame.Hour12)
                return 60 * 12;
            if (MyCandle == TimeFrame.Hour2)
                return 60 * 2;
            if (MyCandle == TimeFrame.Hour3)
                return 60 * 3;
            if (MyCandle == TimeFrame.Hour4)
                return 60 * 4;
            if (MyCandle == TimeFrame.Hour6)
                return 60 * 6;
            if (MyCandle == TimeFrame.Hour8)
                return 60 * 8;
            if (MyCandle == TimeFrame.Minute)
                return 1;
            if (MyCandle == TimeFrame.Minute10)
                return 10;
            if (MyCandle == TimeFrame.Minute15)
                return 15;
            if (MyCandle == TimeFrame.Minute2)
                return 2;
            if (MyCandle == TimeFrame.Minute20)
                return 20;
            if (MyCandle == TimeFrame.Minute3)
                return 3;
            if (MyCandle == TimeFrame.Minute30)
                return 30;
            if (MyCandle == TimeFrame.Minute4)
                return 4;
            if (MyCandle == TimeFrame.Minute45)
                return 45;
            if (MyCandle == TimeFrame.Minute5)
                return 5;
            if (MyCandle == TimeFrame.Minute6)
                return 6;
            if (MyCandle == TimeFrame.Minute7)
                return 7;
            if (MyCandle == TimeFrame.Minute8)
                return 8;
            if (MyCandle == TimeFrame.Minute9)
                return 9;
            if (MyCandle == TimeFrame.Monthly)
                return 60 * 24 * 30;
            if (MyCandle == TimeFrame.Weekly)
                return 60 * 24 * 7;

            return 0;

        }

        #endregion

    }

}
