using System;
using static DiscreteSimulation.Program;
using static DiscreteSimulation.Program.Person;



namespace DiscreteSimulation
{
    internal class Program
    {


        static void Main(string[] args)
        {
            //just a wall of text that is supposed to gather starting 
            //parameters from the user - how long shoud the simulation run
            //and what parameters should the disease have
            int DisStart = 0;
            int DisEnd = 0;
            int SpreadRate = 0;
            int DeadlyRate = 0;
            int StartingSpread = 0;
            Console.WriteLine("Hello, welcome to the application for" +
                " Discrete Simulation of Population, press enter to continue");
            Console.ReadLine();
            Console.WriteLine("Firstly, it is important to know for how many years " +
                "should the simulation run? (It is important to keep in mind the " +
                "lenght of the simulation with the number of people added to it)");
            Console.WriteLine();
            int time = Convert.ToInt32(Console.ReadLine());

            if(time < 20)
            {
                Console.WriteLine();
                Console.WriteLine("Are you sure something is going to happen " +
                    "in this time period? Try again");
                Console.WriteLine();
                time = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine();
            Console.WriteLine("Now tell me how many males should be at the begining");
            Console.WriteLine();
            int males = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Now females..");
            Console.WriteLine();
            int females = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("There is also an option to create a disease with " +
                "your choice of parameters, please type 'yes' in case you would " +
                "like to have a disease in this simulation");
            Console.WriteLine();
            string input = Console.ReadLine();
            if (input.ToLower() == "yes")
            {
                Console.WriteLine();
                Console.WriteLine("Great choice!, please tell me in which year " +
                    " the disease should be starting");
                Console.WriteLine();
                DisStart = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Which year should it end?");
                Console.WriteLine();
                DisEnd = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("How easily should it spread though population? " +
                    "please write it as a whole number 0-100");
                Console.WriteLine();
                SpreadRate = Convert.ToInt32(Console.ReadLine());
                if (SpreadRate < 0 || SpreadRate > 100)
                {
                    Console.WriteLine();
                    Console.WriteLine("Let's try again...");
                    Console.WriteLine();
                    SpreadRate = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine();
                Console.WriteLine("How deadly should it be?, " +
                    "please write it as a whole number 0-100");
                Console.WriteLine();
                DeadlyRate = Convert.ToInt32(Console.ReadLine());

                if (DeadlyRate < 0 || DeadlyRate > 100)
                {
                    Console.WriteLine();
                    Console.WriteLine("Let's try again...");
                    Console.WriteLine();
                    DeadlyRate = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine();
                Console.WriteLine("Finally, what percentage of population " +
                    "should be hit by the disease when it starts?, " +
                    "please write it as a whole number 0-100");
                Console.WriteLine();
                StartingSpread = Convert.ToInt32(Console.ReadLine());

                if (StartingSpread < 0 || StartingSpread > 100)
                {
                    Console.WriteLine();
                    Console.WriteLine("Let's try again...");
                    Console.WriteLine();
                    StartingSpread = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine();


            }
            //declaring the ilness with parameters gathered as well as starting
            //a population list where Males and Females are added with random
            //ages in range from 1 to 10
            Illness ilness = new Illness(DisStart, DisEnd, SpreadRate,
                DeadlyRate, StartingSpread);

            var population = new List<Person> { };

            for(int i = 0; i < males; i++)
            {
                Random random = new Random();

                population.Add(new Male(random.Next(1, 10)));
            }
            for (int i = 0; i < females; i++)
            {
                Random random = new Random();

                population.Add(new Female(random.Next(1, 10)));
            }
            //var ilness = new Ilness(50, 100, 20, 1, 20); //start, end, infec, deadly, startinf
            var sim = new Simulation(population, time, ilness);
            sim.Execute();
            //3 forms of output where first one writes on a new line
            //how long should the person live, his gender and for how long
            //he has lived

            //the second goes through each year and writes how many people were
            //alive at the end of the year

            //the third one gives how many people were infected in the year
            //skips the years where nobody was infected
            //specifies the year and how many people were infected
            foreach (var person in sim.Population)
            {
                Console.WriteLine("Person {0}", person);
                
            }

            Console.WriteLine();

            for(int i = 0; i < time; i++)
            {
                Console.WriteLine("The year " + (i+1) + " had " +
                    sim.SizePopulation[i] + " living people");
            }
           
           Console.WriteLine();

            for(int i = 0; i < time; i++)
            {
                if (sim.SizeInfected[i] != 0)
                {
                    Console.WriteLine("The year " + (i+1) + " had " +
                        sim.SizeInfected[i] + " infected people");
                }
            }


        }



        public static double SampleGaussian(double mean, double stddev)
        {
            Random random = new Random();
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }
        public static int GetPoisson(double lambda)
        {
            {
                Random x = new Random();
                // Algorithm due to Donald Knuth, 1969.
                double p = 1.0,
                L = Math.Exp(-lambda);
                int k = 0;
                while (p > L)
                {
                    k++;
                    p *= x.NextDouble();
                }

                return k - 1;
            }
        }

        public abstract class Person
        {
            //setting up the class person to be further inheritance
            public int Age { get; set; }
            public bool Ilness { get; set; }

            public int HowLongIll {  get; set; }

            public bool immunity { get; set; }

            public int WhenIllEnd { get; set; }
            public int RelationshipAge { get; set; }
            public int LifeTime { get; set; }
            public double TimeChildren { get; set; }
            public Person Couple { get; set; }
            protected Person(int age) { Age = age; }


            public override string ToString()
                // a simple function usable later for output
            {
                return string.Format("Age: {0} LifeTime: {1}", Age, LifeTime);
            }

            public void GetInfected()
            {
                //just a simple function that asks if person is immune to the
                //disease, if hes not, it sets parameter for ilness to be true
                //sets the parameter for how long he has been ill to 0
                //and randomly sets the lenght of the disease for the person
                if (!immunity)
                {
                    Ilness = true;
                    HowLongIll = 0;
                    WhenIllEnd = Math.Abs(Convert.ToInt32(SampleGaussian(3, 2)));
                }
            }

            public void InfectedEnd()
            {
                //a simple function comparing the parameters that
                //either sets the person to be cured and immune
                //or does nothing
                if (HowLongIll == WhenIllEnd)
                {
                    immunity = true;
                    Ilness = false;
                }
            }

            public bool IsIll()
            {
                return Ilness;
            }

            public void IlnessDie(int deadliness)
            {
                //according to the deadliness, it sets a random variable
                //if it is lower than the deadliness, then he dies at the end
                //of the round as his age is set to his lifespan
                Random random = new Random();

                var Probability = random.Next(0, 100);

                if (Probability <= deadliness)
                {
                    Age = LifeTime;
                }
            }

            public class Male : Person
            {
                //male is a very simple class as he just lives and finds relationships
                public Male(int age) : base(age)
                {
                }



                public override string ToString()
                    //overriding the tostring function to additionally state that hes a male
                {
                    return base.ToString() + " Male";
                }
            }

            public class Female : Person
            {
                //female is a more complex class as she needs to be pregnant and 
                //deliver new children - therefore she has new parameters
                //which are bool for pregnancy, age when she is legitimate for pregnancy
                //and the total number of children she can give birth to
                public Female(int age) : base(age)
                {
                }
                public override string ToString()
                {
                    return base.ToString() + " Female";
                }
                public bool IsPregnant { get; set; }
                public double PregnantAge { get; set; }
                public double ChildrenCount { get; set; }

                //now treating the pregnancy

                public bool SuitablePregnancy(int CurrentTime)
                {
                    //this function serves as a check for female if she is ready to have 
                    //children - her age is larger than the age when she could firstly have 
                    //children, the time set for the couple to have children is also passed (TimeChildren)
                    //and she can still have more children (chidlrenCount)
                    return Age >= PregnantAge && CurrentTime <= TimeChildren && ChildrenCount > 0;
                }

                public Person GiveBirth(int CurrentTime)
                {
                    //here comes the first bit of probability x.NextDouble() gives a value between 0 and 1
                    Random x = new Random();
                    var sample = x.NextDouble();

                    //basically a coinflip between a male or female 50% for each
                    var child = sample > 0.5 ? (Person)new Male(0) : new Female(0);
                    ChildrenCount--;
                    //when the child is born, his parameters need to be set which is done by 
                    //poisson number generator, for females it is further done by normal/gaussian number
                    //generator
                    child.LifeTime = GetPoisson(70);
                    child.RelationshipAge = GetPoisson(20);
                    if (child is Female)
                    {
                        (child as Female).PregnantAge =
                            Convert.ToInt32(SampleGaussian(28, 5));
                        (child as Female).ChildrenCount =
                            Math.Abs(Convert.ToInt32(SampleGaussian(2, 2)));
                    }
                    //setting time for new child to be born
                    if (Engaged && ChildrenCount > 0)
                    {
                        Random x2 = new Random();
                        TimeChildren = x2.Next(2, 8);

                        Couple.TimeChildren = TimeChildren + CurrentTime;
                    }
                    else
                        // case for no more children
                    {
                        TimeChildren = 0;
                        IsPregnant = false;
                    }
                    return child;

                }
            }


            //Now the block that treats relationships

            public bool SuitableRelationship()
            {
                //check for a person if hes ready to have relationship
                return Age > RelationshipAge && Couple == null;
            }

            public bool SuitablePartner(Person person)
            {
                //check if the two people being compared are suitable for each another
                //done by checking sex and difference in age
                return ((person is Male && this is Female) ||
                    (person is Female && this is Male)) && Math.Abs(person.Age - Age) <= 5;
            }
            public bool Engaged
            {
                //if they get engaged, their status is no longer null for Couple
                get { return Couple != null; }
            }

            public void Disengage()
            {
                //if the two disangage, their status is again null and they will not have 
                //children in this couple
                Couple.Couple = null;
                Couple = null;
                TimeChildren = 0;
            }

            public bool EndRelationship()
            {
                //again a random chance for relationship ending,
                //the older the people are, the less likely they are
                //to end their relationship
                Random x = new Random();

                var sample = x.NextDouble();

                if (Age >= 14 && Age <= 20 && sample <= 0.8)
                    return true;
                if (Age >= 21 && Age <= 28 && sample <= 0.4)
                    return true;
                if (Age >= 29 && sample <= 0.1)
                    return true;
                return false;

            }

            public void FindPartner(IEnumerable<Person> population, int CurrentTime,
                int infectioness)
                //as this is the only part where people are meeting each other
                //it is the part where getting infected is being calculated
                //so regadless of being a suitable partner, meeting a person
                //is being calculated as potentionally infecting others

                //therefore, random variable is constructed and if the person
                //meeting others is ill, there is a chance that the other person
                //is infected and likewise
            {
                Random x = new Random();
                Random infection = new Random();
                foreach (var candidate in population)
                {
                    if (this.IsIll())
                    {
                        if (infection.Next(0,100) < infectioness)
                        {
                            candidate.GetInfected();
                        }
                    }
                    if (candidate.IsIll())
                    {
                        if (infection.Next(0, 100) < infectioness)
                        {
                            this.GetInfected();
                        }
                    }
                    //going through people and checking if they are sutiable
                    //for relationship together + a bit of luck included
                    if (SuitablePartner(candidate) && candidate.SuitableRelationship()
                    && x.NextDouble() <= 0.5)
                    {
                        //if they are suitable, they get engaged and the time for their first
                        //child is set
                        candidate.Couple = this;
                        Couple = candidate;

                        Random x2 = new Random();


                        var ChildTime = x2.Next(2,8);

                        candidate.TimeChildren = CurrentTime + ChildTime;
                        TimeChildren = CurrentTime + ChildTime;
                        break;
                    }
                }
            }


        }

        public class Illness
        {
            public int StartingTime { get; set; }
            public int EndingTime { get; set; }

            public int infectioness { get; set; }

            public int deadliness { get; set; }

            public int StartProportion {  get; set; }   

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

        public class Simulation
        {
            //the simulation includes a list of the population, time - the upper limit
            //and _CurrentTime, the simulation runs till _CurrentTime != Time
            public List<Person> Population { get; set; }
            public int Time { get; set; }
            private int _CurrentTime;
            public Illness _illness;
            public int[] SizePopulation;
            public int[] SizeInfected;

            public Simulation(IEnumerable<Person> population, int time, Illness illness)
            {
                _illness = illness;
                Population = new List<Person>(population);
                Time = time;
                SizePopulation = new int[time];
                SizeInfected = new int[time];
                //iterating through each person in our list and setting up their values
                foreach (var person in Population)
                {
                    // LifeTime
                    person.LifeTime = GetPoisson(70);
                    // Ready to start having relations
                    person.RelationshipAge = GetPoisson(20);
                    // Pregnancy Age (only women)
                    if (person is Female)
                    {
                        (person as Female).PregnantAge = Convert.ToInt32(SampleGaussian(28, 5));
                        (person as Female).ChildrenCount = Math.Abs(Convert.ToInt32(SampleGaussian(2, 2)));
                    }
                }
            }
            public void Execute()
            {
                
                while (_CurrentTime < Time)
                {
                    int InfectedPeople = 0;
                    //this part treats the start of epidemics where a set proportion
                    //of the population gets to be infected
                    //have to treat part of the calculation as double as normal division
                    //could often result in 0
                    //then it just keeps randomly choosing people upon a specific
                    //number of people are infected
                    if (_CurrentTime == _illness.WhenStart())
                    {
                        
                        int PopSize = Population.Count;
                        double divident = (double)PopSize/(double)100;
                        int proportion = Convert.ToInt32(divident * _illness.StartProportion);
                        Random random = new Random();
                        for (int i = 0; i < proportion; i++)
                        {
                            int RanPerson = random.Next(0, PopSize);
                            var person = Population[RanPerson];
                            if (person.IsIll())
                            {
                                i--;
                            }
                            else
                            { person.GetInfected(); }
                        }
                    }
                    if (_CurrentTime == _illness.WhenEnd()+1)
                    {
                        foreach(var person in Population)
                        {
                            person.Ilness = false;
                        }
                    }
                    // Go through every individual and what happens to him this year
                    for (var i = 0; i < Population.Count; i++)
                    {
                        var person = Population[i];
                        // Giving birth
                        if (person is Female && (person as Female).IsPregnant)
                        {
                            Population.Add((person as Female).GiveBirth(_CurrentTime));
                        }
                        // Check for a new relationship this year
                        if (person.SuitableRelationship())
                        {
                            person.FindPartner(Population, _CurrentTime,
                                 _illness.infectioness);
                        }
                        // if a person is engaged, we should check for end of relationship
                        // or geting pregnant
                        if (person.Engaged)
                        {
                            // Check wheter a relationship has ended this year
                            if (person.EndRelationship())
                                person.Disengage();
                            // Check for child legitimacy
                            if (person is Female &&
                            (person as Female).SuitablePregnancy(_CurrentTime))
                                (person as Female).IsPregnant = true;
                        }

                        if (person.IsIll())
                        {
                            InfectedPeople++;
                            person.IlnessDie(_illness.deadliness);
                            person.HowLongIll++;
                            person.InfectedEnd();
                        }
                        

                        // Check for people dying this year
                        if (person.Age.Equals(person.LifeTime))
                        {
                            // if he died, the relationship is gonna end and he is removed
                            if (person.Engaged)
                                person.Disengage();
                            Population.RemoveAt(i);
                            i--;
                        }
                        //Console.WriteLine(_CurrentTime.ToString());
                        person.Age++;
                        
                    }
                    SizePopulation[_CurrentTime] = Population.Count;
                    SizeInfected[_CurrentTime] = InfectedPeople;
                    _CurrentTime++;
                }
            }
        }
    }
}