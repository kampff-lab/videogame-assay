using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using Bonsai.Expressions;

namespace Bonsai.Videogame
{
    [TypeVisualizer("Bonsai.Design.BooleanTimeSeriesVisualizer, Bonsai.Design")]
    public class SquareWave : Combinator<int>
    {
        public SquareWave()
        {
            Count = 1;
        }

        public int Value { get; set; }

        [XmlIgnore]
        public TimeSpan Duration { get; set; }

        [Browsable(false)]
        [XmlElement("Duration")]
        public string DurationXml
        {
            get { return XmlConvert.ToString(Duration); }
            set { Duration = XmlConvert.ToTimeSpan(value); }
        }

        [XmlIgnore]
        public TimeSpan Period { get; set; }

        [Browsable(false)]
        [XmlElement("Period")]
        public string PeriodXml
        {
            get { return XmlConvert.ToString(Period); }
            set { Period = XmlConvert.ToTimeSpan(value); }
        }

        public int Count { get; set; }

        public override IObservable<int> Process<TSource>(IObservable<TSource> source)
        {
            return from evt in source
                   from value in Observable.Timer(TimeSpan.Zero, Period, HighResolutionScheduler.Default).Take(Count)
                   from wave in Observable.Repeat(Value, 1).Merge(Observable.Repeat(0, 1).Delay(Duration, HighResolutionScheduler.Default))
                   select wave;
        }
    }
}