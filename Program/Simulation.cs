using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Discrete_Simulation_Population_2.Program.Person;
using static DiscreteSimulation.Program;
using static Discrete_Simulation_Population_2.Program.Constants;

namespace Discrete_Simulation_Population_2.Program
{
    internal class Simulation
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
                    person.LifeTime = NumberGen.GetPoisson(LifeExpect);
                    // Ready to start having relations
                    person.RelationshipAge = NumberGen.GetPoisson(RelationAge);
                    // Pregnancy Age (only women)
                    if (person is Female)
                    {
                        (person as Female).PregnantAge = Convert.ToInt32(NumberGen.SampleGaussian(PregAge, PregVar));
                        (person as Female).ChildrenCount = Math.Abs(Convert.ToInt32(NumberGen.SampleGaussian(ChildCount, ChildVar)));
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
                        double divident = (double)PopSize / (double)100;
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
                    if (_CurrentTime == _illness.WhenEnd() + 1)
                    {
                        foreach (var person in Population)
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

