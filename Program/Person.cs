using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Discrete_Simulation_Population_2.Program.Constants;

namespace Discrete_Simulation_Population_2.Program
{
    internal abstract class Person
    {
        
            //setting up the class person to be further inheritance
            public int Age { get; set; }
            public bool Ilness { get; set; }

            public int HowLongIll { get; set; }

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
                    WhenIllEnd = Math.Abs(Convert.ToInt32(NumberGen.SampleGaussian(IlEnds, IlVar)));
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

                var Probability = random.Next(MinRan, MaxRan);

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
                    //Random x = new Random();
                    Random x = new Random();
                    var sample = x.NextDouble();

                    //basically a coinflip between a male or female 50% for each
                    var child = sample > 0.5 ? (Person)new Male(0) : new Female(0);
                    ChildrenCount--;
                    //when the child is born, his parameters need to be set which is done by 
                    //poisson number generator, for females it is further done by normal/gaussian number
                    //generator
                    child.LifeTime = NumberGen.GetPoisson(LifeExpect);
                    child.RelationshipAge = NumberGen.GetPoisson(RelationAge);
                    if (child is Female)
                    {
                        (child as Female).PregnantAge =
                            Convert.ToInt32(NumberGen.SampleGaussian(PregAge, PregVar));
                        (child as Female).ChildrenCount =
                            Math.Abs(Convert.ToInt32(NumberGen.SampleGaussian(ChildCount, ChildVar)));
                    }
                    //setting time for new child to be born
                    if (Engaged && ChildrenCount > 0)
                    {
                        Random x2 = new Random();
                        TimeChildren = x2.Next(ChildrenTime, ChildVarTime);

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
                        if (infection.Next(MinRan, MaxRan) < infectioness)
                        {
                            candidate.GetInfected();
                        }
                    }
                    if (candidate.IsIll())
                    {
                        if (infection.Next(MinRan, MaxRan) < infectioness)
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


                        var ChildTime = x2.Next(ChildrenTime, ChildVarTime);

                        candidate.TimeChildren = CurrentTime + ChildTime;
                        TimeChildren = CurrentTime + ChildTime;
                        break;
                    }
                }
            }


        
    }
}
