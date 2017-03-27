using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Saper
{
    public class ScoreItem : IComparable<ScoreItem>
    {
        [XmlIgnore]
        public TimeSpan Score { get; set; }

        public DateTime Date { get; set; }


        #region operators and icomparable

        static public bool operator < (ScoreItem lhs, ScoreItem rhs)
        {
            return lhs.Score < rhs.Score;
        }

        static public bool operator > (ScoreItem lhs, ScoreItem rhs)
        {
            return lhs.Score > rhs.Score;
        }

        int IComparable<ScoreItem>.CompareTo(ScoreItem rhs)
        {
            if (this < rhs) return -1;
            if (this > rhs) return 1;
            return 0;
        }

        #endregion

        #region serialization

        [XmlElement("Score")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long ScoreTicks
        {
            get => Score.Ticks;
            set => Score = TimeSpan.FromTicks(value);
        }

        #endregion
    }
}