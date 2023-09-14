using Discrete_Simulation_Population_2.Program;
using System;
using static Discrete_Simulation_Population_2.Program.Person;
using static Discrete_Simulation_Population_2.Program.Constants;





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
            int time;
            int males;
            int females;

            Console.WriteLine("Hello, welcome to the application for" +
                " Discrete Simulation of Population, press enter to continue");
            Console.ReadLine();
            Console.WriteLine("Firstly, it is important to know for how many years " +
                "should the simulation run? (It is important to keep in mind the " +
                "lenght of the simulation with the number of people added to it)");
            Console.WriteLine();
            while (!int.TryParse(Console.ReadLine(), out time))
            {
                Console.WriteLine("That was invalid. Enter number.");
            }

            if (time < 20)
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
            while (!int.TryParse(Console.ReadLine(), out males))
            {
                Console.WriteLine("That was invalid. Enter a number.");
            }
            Console.WriteLine();
            Console.WriteLine("Now females..");
            Console.WriteLine();
            while (!int.TryParse(Console.ReadLine(), out females))
            {
                Console.WriteLine("That was invalid. Enter a number.");
            }
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
                while (!int.TryParse(Console.ReadLine(), out DisStart))
                {
                    Console.WriteLine("That was invalid. Enter a number.");
                }
                Console.WriteLine();
                Console.WriteLine("Which year should it end?");
                Console.WriteLine();
                while (!int.TryParse(Console.ReadLine(), out DisEnd))
                {
                    Console.WriteLine("That was invalid. Enter a number.");
                }
                Console.WriteLine();
                Console.WriteLine("How easily should it spread though population? " +
                    "please write it as a whole number 0-100");
                Console.WriteLine();
                while (!int.TryParse(Console.ReadLine(), out SpreadRate))
                {
                    Console.WriteLine("That was invalid. Enter a number.");
                }
                Console.WriteLine();
                Console.WriteLine("How deadly should it be?, " +
                    "please write it as a whole number 0-100");
                Console.WriteLine();
                while (!int.TryParse(Console.ReadLine(), out DeadlyRate))
                {
                    Console.WriteLine("That was invalid. Enter a number.");
                }

                Console.WriteLine();
                Console.WriteLine("Finally, what percentage of population " +
                    "should be hit by the disease when it starts?, " +
                    "please write it as a whole number 0-100");
                Console.WriteLine();
                while (!int.TryParse(Console.ReadLine(), out StartingSpread))
                {
                    Console.WriteLine("That was invalid. Enter a number.");
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

                population.Add(new Male(random.Next(AgeMin, AgeMax)));
            }
            for (int i = 0; i < females; i++)
            {
                Random random = new Random();

                population.Add(new Female(random.Next(AgeMin, AgeMax)));
            }
            
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



    }
}