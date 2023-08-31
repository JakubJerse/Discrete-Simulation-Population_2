using static Discrete_Simulation_Population_2.Program;
using static Discrete_Simulation_Population_2.Program.Person;



namespace Discrete_Simulation_Population_2
{
    internal class Program
    {


        static void Main(string[] args)
        {
            var population = new List<Person>
            {
                new Male(2),
                new Female(2),
                new Male(5),
                new Female(4),
                new Male(6),
                new Female(7),
                new Male(8),
                new Female(9),    
                new Male(10),
                                new Male(2),
                new Female(2),
                new Male(5),
                new Female(4),
                new Male(6),
                new Female(7),
                new Male(8),
                new Female(9),
                new Male(10),
                                new Male(2),
                new Female(2),
                new Male(5),
                new Female(4),
                new Male(6),
                new Female(7),
                new Male(8),
                new Female(9),
                new Male(10),                new Male(2),
                new Female(2),
                new Male(5),
                new Female(4),
                new Male(6),
                new Female(7),
                new Male(8),
                new Female(9),
                new Male(10)
            };
            var sim = new Simulation(population, 1000);
            sim.Execute();

            foreach (var person in sim.Population)
            {
                Console.WriteLine("Person {0}", person);
                
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

            public void FindPartner(IEnumerable<Person> population, int CurrentTime)

            {
                Random x = new Random();
                foreach (var candidate in population)
                {
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


        public class Simulation
        {
            //the simulation includes a list of the population, time - the upper limit
            //and _CurrentTime, the simulation runs till _CurrentTime != Time
            public List<Person> Population { get; set; }
            public int Time { get; set; }
            private int _CurrentTime;

            public Simulation(IEnumerable<Person> population, int time)
            {
                
                Population = new List<Person>(population);
                Time = time;
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
                    // Go through every individual and what happens to him this year
                    for (var i = 0; i < Population.Count; i++)
                    {
                        var person = Population[i];
                        // Giving birth
                        if (person is Female && (person as Female).IsPregnant)
                            Population.Add((person as Female).GiveBirth(_CurrentTime));
                        // Check for a new relationship this year
                        if (person.SuitableRelationship())
                            person.FindPartner(Population, _CurrentTime);
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
                        // Check for people dying this year
                        if (person.Age.Equals(person.LifeTime))
                        {
                            // if he died, the relationship is gonna end and he is removed
                            if (person.Engaged)
                                person.Disengage();
                            Population.RemoveAt(i);
                        }
                        //Console.WriteLine(_CurrentTime.ToString());
                        person.Age++;
                        _CurrentTime++;
                    }
                }
            }
        }
    }
}