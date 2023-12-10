using System.Windows.Forms.DataVisualization.Charting;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using CaExch;

namespace bot4;

public partial class Charty : ChartyBase
{
    public event Action? NeedToRepopulateChart;
    public event Action<Kline>? OnKlineUpdated;

    public async void SetInterval(string value)
    {
        {
            if (_interval != value)
            {
                _interval = value;

                if(_klineSubscriptionId > 0)
                    Exchange.UnsubKlineSocket(_klineSubscriptionId);

                if (_symbol.Length > 0 && _interval.Length > 0)
                {
                    _klineSubscriptionId = await Exchange.SubscribeToSocket(_symbol, _interval);
                }
            }
        }
    }

    public Charty(Chart chart, AnExchange ech, string symbo) : base(chart, ech, symbo)
    {
        SetCursor();

        Exchange.OnKlineUpdate += OnPriceUpdate;
    }
    void OnPriceUpdate(string s, Kline k)
    {
        if (_symbol == s)
        {
            OnKlineUpdated?.Invoke(k);
            UpdatePrice(k);
        }
    }

    string DL(DateTime d)
    {
        List<Kline> ks = new(_klines.Skip(_klines.Count - _zoom));
        DateTime xMin = ks.Min(k => k.OpenTime);
        DateTime xMax = ks.Max(k => k.OpenTime);
        if(_interval == "1s")
            if(xMax.Hour == xMin.Hour)
                return d.ToString("mm:ss");
            else
                return d.ToString("hh:mm:ss");
        else if (xMax.Day == xMin.Day)
            return d.ToString("hh:mm");
        else if(xMax.Year == xMin.Year)
            return d.ToString("dd.MM hh:mm");
        else
            return d.ToString("dd.MM.yy hh:mm");
    }
    public async Task populate() 
    {
        try
        {
            Series sKlines = _ch.Series["Klines"];
            Series sVolume = _ch.Series["Volume"];
            sKlines.Points.Clear();
            sVolume.Points.Clear();

            List<Kline> ks = new(_klines.Skip(_klines.Count - _zoom));

            _yMax = Convert.ToDouble(ks.Max(k => k.HighPrice));
            _yMin = Convert.ToDouble(ks.Min(k => k.LowPrice));
            _yMin = _yMin - 0.1 * (_yMax - _yMin);

            _cha.AxisY2.ScaleView.Zoom(_yMin, _yMax);
            
            double maxVolume = Convert.ToDouble(ks.Max(k => k.Volume));
            _volumeRate = 0.3 * (_yMax - _yMin) / maxVolume;

            foreach (var k in ks)
            {
                sKlines.Points.AddXY(DL(k.OpenTime), k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);

                decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
                int n = sVolume.Points.AddXY(DL(k.OpenTime), vol);
                if (n > 0)
                {
                    if ((double)vol! < sVolume.Points[n - 1].YValues[0])
                    {
                        sVolume.Points[n].Color = Color.Red;
                    }
                }
            }

            await DrawIndicators();
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "Charty.populate", "Error: " + ex.Message);
        }
    }

    async void UpdatePrice(Kline k)
    {
        Series sKlines, sVolume;
        try 
        { 
            if(_klines.Count == 0) return;  if(_ch.Series.Count < 2) return;
            sKlines = _ch.Series["Klines"]; if (sKlines.Points.Count == 0) return;
            sVolume = _ch.Series["Volume"]; if (sVolume.Points.Count == 0) return;

            var lk = _klines.Last();
            if (lk.OpenTime < k.OpenTime)
            {
                // Если пришла новая свеча, то полностью перерисовываем график
                // поновой получаем весь массив данных
                await GetKlines();

                // а далее посути надо просто нарисовать график,
                // но если тут вызвать populate(), возникает ошибка,
                // поэтому отправляем сообщение на форму графика,
                // и уже от туда делаем Charty.populate()
                NeedToRepopulateChart?.Invoke();

                // Уходим, больше ничего не надо
                return;
            }

            // Когда просто поменяется цена, но свеча еще не закрыта,
            // изменяем только последнюю свечу

            // Удаляем всю последнюю свечу целиком и 
            sKlines.Points.Remove(sKlines.Points.Last());
            sVolume.Points.Remove(sVolume.Points.Last());

            // ... добавляем ее же обновленную, с новой ценой и объемом
            sKlines.Points.AddXY(DL(k.OpenTime), k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);
            decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
            sVolume.Points.AddXY(DL(k.OpenTime), vol);

            // Индикаторы тоже надо подогнать под новую цену
            // Но, как выяснилось, индикатор не меняется под изменение текущей свечи,
            // так как он рассчитывается по предыдущим, а текущая не влияет
            //NeedToReDrawIndicator?.Invoke();
        }
        catch(Exception ex) 
        {
            Log.Error(Exchange.ID, "Charty.UpdateKline", "Error: " + ex.Message);
        }
    }

    public async Task GetKlines()
    {
        _klines = await Exchange.GetKlines(_symbol, _interval, 1000);
    }

    public void UnsubKlineSocket()
    {
        if (_klineSubscriptionId > 0)
            Exchange.UnsubKlineSocket(_klineSubscriptionId);
    }
}
