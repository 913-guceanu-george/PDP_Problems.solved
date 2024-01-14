namespace Final_project.Bodies
{
    public class Body
    {
        public int Mass;
        public double Acceleration;
        public Vector2 Position;

        public Body(int mass, int acceleration, Vector2 pos)
        {
            this.Mass = mass;
            this.Acceleration = acceleration;
            this.Position = pos;
        }
    }
}
