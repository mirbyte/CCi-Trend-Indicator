using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Indicators
{
    [Cloud("CCI", "CCI 2", FirstColor = "Blue", Opacity = 0.15, SecondColor = "Red")]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class CCiTrend : Indicator
    {
        [Parameter("Public v1.1", DefaultValue = "https://ctrader.com/users/profile/64575", Group = "m0")]
        public string M_ { get; set; }
        
        [Output("CCI", LineColor = "Black")]
        public IndicatorDataSeries CCIa { get; set; }

        [Output("CCI 2", LineColor = "Black")]
        public IndicatorDataSeries CCIb { get; set; }

        [Output("Zero Line", Color = Colors.Black, LineStyle = LineStyle.Dots)]
        public IndicatorDataSeries ZeroLine { get; set; }

        [Output("Level +100", Color = Colors.Black, LineStyle = LineStyle.Dots)]
        public IndicatorDataSeries LevelPlus200 { get; set; }

        [Output("Level -100", Color = Colors.Black, LineStyle = LineStyle.Dots)]
        public IndicatorDataSeries LevelMinus200 { get; set; }

        [Parameter("CCi Period 1", DefaultValue = 12, Group = "Settings")]
        public int Period { get; set; }

        [Parameter("CCi Period 2", DefaultValue = 48, Group = "Settings")]
        public int Period2 { get; set; }
        
        [Parameter("Display Arrows?", DefaultValue = true, Group = "Settings")]
        public bool Showarrow { get; set; }
        
        [Parameter("Arrow Distance From Candle", DefaultValue = 2, Group = "Settings")]
        public int Arrowdist { get; set; }
        
        [Parameter("Arrow Color", DefaultValue = "B60071C1", Group = "Settings")]
        public Color Arrowcol { get; set; }
        
        
        private CommodityChannelIndex cci;
        private CommodityChannelIndex cci2;
        

        protected override void Initialize()
        {
            cci = Indicators.CommodityChannelIndex(Period);
            cci2 = Indicators.CommodityChannelIndex(Period2);
        }

        public override void Calculate(int index)
        {
            CCIa[index] = cci.Result[index];
            CCIb[index] = cci2.Result[index];

            ZeroLine[index] = 0;
            LevelPlus200[index] = 200;
            LevelMinus200[index] = -200;

            if (CCIa[index] > CCIb[index] && CCIa[index - 1] <= CCIb[index - 1] && Showarrow)
            {
                Chart.DrawIcon(Bars.OpenTimes.LastValue.ToString(), ChartIconType.UpArrow, Bars.OpenTimes.LastValue, Bars.LowPrices.LastValue - (Arrowdist * Symbol.PipSize), Arrowcol);
            }
            else if (CCIa[index] < CCIb[index] && CCIa[index - 1] >= CCIb[index - 1] && Showarrow)
            {
                Chart.DrawIcon(Bars.OpenTimes.LastValue.ToString(), ChartIconType.DownArrow, Bars.OpenTimes.LastValue, Bars.HighPrices.LastValue + (Arrowdist * Symbol.PipSize), Arrowcol);
            }
        }
    }
}
