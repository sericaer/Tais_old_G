using System;
using System.Collections.Generic;

namespace RunData
{
    public class Process
    {
        public int costDays;

        public int startDays;

        public SubjectValue<double> percent;

        private bool isFinished;

        internal Process(int costDays)
        {
            this.costDays = costDays;
            this.startDays = Date.inst.total_days.Value;

            this.isFinished = false;
            this.percent = new SubjectValue<double>(0.0);
        }

        internal static List<Process> Init()
        {
            return new List<Process>();
        }

        internal static void DaysInc()
        {
            Root.inst.processes.RemoveAll(x => x.isFinished);
            Root.inst.processes.ForEach(x => x.ProcessDaysInc());
        }

        internal void ProcessDaysInc()
        {
            percent.Value = (Date.inst.total_days.Value - startDays) * 100.0 / costDays;

            if (isFinishedDay())
            {
                DoFinished();
                isFinished = true;
            }
        }

        internal bool isFinishedDay()
        {
            return Date.inst.total_days.Value >= startDays + costDays;
        }

        internal virtual void DoFinished()
        {

        }
    }
}
