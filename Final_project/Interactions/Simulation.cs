using Final_project.Bodies;

namespace Final_project.Interactions
{
    public class Simulation
    {
        private int nrBodies = 100;
        private Body[] bodies;

        public Simulation()
        {
            // TODO - check how to simulate gravitational accelelration
            this.bodies = new Body[this.nrBodies];
            for (int i = 0; i < nrBodies; i++)
            {
                bodies[i] = new Body
                {
                    Mass = i % 20,
                    Acceleration = (i % 20) * Phyisics
                }
    
        }
        }


    }
}
