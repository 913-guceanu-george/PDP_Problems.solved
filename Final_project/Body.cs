using System.Numerics;

namespace Final_project.BodyObj
{

    public class Body
    {
        public int Id { get; set; }
        public float Mass { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        public void Save(StreamWriter writer)
        {
            // Save body information to the file
            writer.WriteLine($"{Id + 1}\n" +
                $"Position({Position.X},{Position.Y},{Position.Z})\n" +
                $"Velocity({Velocity.X},{Velocity.Y},{Velocity.Z},{Mass})");
        }

        public static void SaveBodiesToFile(string filePath, string message, List<Body> Bodies)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"{message}");
                foreach (var body in Bodies)
                {
                    body.Save(writer);
                }
            }
        }
    }
}
