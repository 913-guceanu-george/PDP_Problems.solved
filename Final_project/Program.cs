using Final_project.BodyObj;
using Final_project.NBodySim;
using Final_project.Renderer;
using OpenTK.Windowing.Desktop;

internal class Program
{
    static void usingTerminal()
    {
        // Create a list of Body objects
        List<Body> bodies = NBodySimulation.GenerateRandomBodies(20);
        NBodySimulation simulation = new NBodySimulation(bodies);

        // Specify simulation parameters
        int timeSteps = 10;
        float timeStepSize = 1.0f;

        // Run the simulation and write in the corresponding folders
        Body.SaveBodiesToFile("bodies_before_simulation.txt", "Before simulation:", bodies);
        simulation.Simulate(timeSteps, timeStepSize);
        Body.SaveBodiesToFile("bodies_after_simulation.txt", "After simulation:", simulation.bodies);
    }

    [STAThread]
    private static void Main(String[] args)
    {
        using (var window = new MainWindow(GameWindowSettings.Default, NativeWindowSettings.Default))
        {
            window.Run();
        }
        // usingTerminal();
    }
}
