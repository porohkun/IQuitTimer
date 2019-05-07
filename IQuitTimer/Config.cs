using MimiJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQuitTimer
{
    public class Config
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        private DateTime[] _excludeDates;
        public IEnumerable<DateTime> ExcludeDates => _excludeDates;
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public TimeSpan Step { get; private set; }

        public Config(JsonValue json)
        {
            StartDate = DateTime.Parse(json["start"]).Date;
            EndDate = DateTime.Parse(json["end"]).Date;
            _excludeDates = json["exclude"].Array.Select(e => DateTime.Parse(e)).ToArray();
            StartTime = TimeSpan.Parse(json["work_hours"]["start"]);
            EndTime = TimeSpan.Parse(json["work_hours"]["end"]);
            Step = TimeSpan.Parse(json["step"]);
        }
    }
}
