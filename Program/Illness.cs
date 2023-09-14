namespace Discrete_Simulation_Population_2.Program
{
    public class Illness
    {
        public int StartingTime { get; set; }
        public int EndingTime { get; set; }

        public int infectioness { get; set; }

        public int deadliness { get; set; }

        public int StartProportion { get; set; }
         public Illness(int startingTime, int endingTime, int infectioness,
            int deadliness, int StartProportion)
        {
            StartingTime = startingTime;
            EndingTime = endingTime;
            this.infectioness = infectioness;
            this.deadliness = deadliness;
            this.StartProportion = StartProportion;
        }
        public int WhenStart()
        {
            return StartingTime;
        }

        public int WhenEnd()
        {
            return EndingTime;
        }

        public int HowInfection()
        {
            return infectioness;
        }
        public int HowDeadly()
        {
            return deadliness;
        }


    }
}