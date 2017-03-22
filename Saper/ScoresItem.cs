using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Saper
{
    public class ScoreItem
    {
        [XmlIgnore]
        public TimeSpan Score { get; set; }

        public DateTime Date { get; set; }

        #region operators

        static public bool operator < (ScoreItem lhs, ScoreItem rhs)
        {
            return lhs.Score < rhs.Score;
        }

        static public bool operator > (ScoreItem lhs, ScoreItem rhs)
        {
            return lhs.Score > rhs.Score;
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