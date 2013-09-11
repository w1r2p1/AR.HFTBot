﻿using System;
using System.Collections.Generic;

namespace Core
{
    public class Trader
    {
        private readonly IStockbroker _stockbroker;

        private readonly List<StockSignal> _stockSignals = new List<StockSignal>();

        public Trader(IStockbroker stockbroker)
        {
            _stockbroker = stockbroker;
        }

        public void RegisterStock(TickerSymbol ticker, ISignal signal)
        {
            _stockSignals.Add(new StockSignal
            {
                Ticker = ticker,
                Signal = signal
            });
        }

        public void Trade()
        {
            foreach (var stockSignal in _stockSignals)
            {
                ProcessSignal(stockSignal);
            }
        }

        private void ProcessSignal(StockSignal stockSignal)
        {
            if (stockSignal.Assess(DateTime.Now) == 1)
                _stockbroker.Buy(stockSignal.Ticker, 1);
            else if (stockSignal.Assess(DateTime.Now) == -1)
                _stockbroker.Sell(stockSignal.Ticker, 1);
        }
    }

    public class StockSignal
    {
        public TickerSymbol Ticker { get; set; }
        public ISignal Signal { get; set; }

        public int Assess(DateTime time)
        {
            return Signal.Assess(Ticker, time);
        }
    }
}