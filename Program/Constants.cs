using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discrete_Simulation_Population_2.Program
{
    internal class Constants
    {
        public const int PregAge = 28;
        public const int PregVar = 5;

        public const int LifeExpect = 70;

        public const int ChildCount = 2;
        public const int ChildVar = 2;

        public const int ChildrenTime = 2;
        public const int ChildVarTime = 8;

        public const int RelationAge = 20;

        public const int IlEnds = 3;
        public const int IlVar = 2;

        public const int MinRan = 0;
        public const int MaxRan = 100;

        public const int AgeMin = 1;
        public const int AgeMax = 10;

        public Random random = new Random();


    }
}
