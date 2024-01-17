using System.Numerics;
using Final_project.BodyObj;


namespace Final_project.NBodySim
{
    public class NBodySimulation
    {
        private const float G = 6.67430e-11f; // gravitational constant
        public readonly List<Body> bodies;
        public IReadOnlyList<Body> Bodies => bodies;

        public NBodySimulation(List<Body> bodies)
        {
            this.bodies = bodies;
        }


        public void Simulate(int timeSteps, float timeStepSize)
        {
            for (int step = 0; step < timeSteps; step++)
            {
                Parallel.ForEach(bodies, (body, state) =>
                {
                    Vector3 acceleration = ComputeAcceleration(body);
                    UpdateBody(body, acceleration, timeStepSize);
                });
            }
        }

        public Vector3 ComputeAcceleration(Body body)
        {
            Vector3 acceleration = Vector3.Zero;

            foreach (Body otherBody in bodies)
            {
                if (otherBody != body)
                {
                    Vector3 direction = otherBody.Position - body.Position;
                    float distanceSquared = direction.LengthSquared();

                    if (distanceSquared > 0)
                    {
                        float forceMagnitude = G * body.Mass * otherBody.Mass / distanceSquared;
                        Vector3 forceDirection = Vector3.Normalize(direction);
                        acceleration += forceDirection * (forceMagnitude / body.Mass);
                    }
                }
            }

            return acceleration;
        }

        public void UpdateBody(Body body, Vector3 acceleration, float timeStepSize)
        {
            // Update velocity and position using Euler's method
            body.Velocity += acceleration * timeStepSize;
            body.Position += body.Velocity * timeStepSize;
        }

        public static List<Body> GenerateRandomBodies(int numBodies)
        {
            Random random = new Random();
            List<Body> bodies = new List<Body>();

            for (int i = 0; i < numBodies; i++)
            {
                Body body = new Body
                {
                    Id = i,
                    Mass = (float)random.NextDouble() * 10f, // Random mass between 0 and 1e24
                    Position = new Vector3(
                        (float)random.NextDouble() * -0.5f, // Random x position between 0 and 1e6
                        (float)random.NextDouble() * -0.5f, // Random y position between 0 and 1e6
                        (float)random.NextDouble()  // Random z position between 0 and 1e6
                    ),
                    Velocity = new Vector3(
                        (float)random.NextDouble() / 1e8f, // Random x velocity between 0 and 1000
                        (float)random.NextDouble() / 1e8f, // Random y velocity between 0 and 1000
                        (float)random.NextDouble() / 1e8f // Random z velocity between 0 and 1000
                    )
                };

                bodies.Add(body);
            }

            return bodies;
        }
    }

}
