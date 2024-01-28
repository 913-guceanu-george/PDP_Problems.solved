using Final_project.NBodySim;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;


namespace Final_project.Renderer
{
    public class MainWindow : GameWindow
    {
        private readonly NBodySimulation simulation;
        private readonly int numBodies = 500;
        private readonly float timeStepSize = 0.1f;
        private readonly float pointSize = 4f;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private const float G = 6.67430e-11f; // gravitational constant

        public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            simulation = new NBodySimulation(NBodySimulation.GenerateRandomBodies(numBodies));
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.Black);
            GL.PointSize(pointSize);

            float[] vertices = new float[numBodies * 3];
            for (int i = 0; i < numBodies; i++)
            {
                vertices[i * 3] = simulation.bodies[i].Position.X;
                vertices[i * 3 + 1] = simulation.bodies[i].Position.Y;
                vertices[i * 3 + 2] = simulation.bodies[i].Position.Z;
            }

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                BufferUsageHint.DynamicDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.GetError();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            for (float step = 0; step < timeStepSize; step += 0.01f)
            {
                Parallel.ForEach(simulation.bodies, (body, state) =>
                {
                    System.Numerics.Vector3 acceleration = simulation.ComputeAcceleration(body);
                    simulation.UpdateBody(body, acceleration, timeStepSize);
                });
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            float[] vertices = new float[numBodies * 3];
            for (int i = 0; i < numBodies; i++)
            {
                vertices[i * 3] = simulation.bodies[i].Position.X;
                vertices[i * 3 + 1] = simulation.bodies[i].Position.Y;
                vertices[i * 3 + 2] = simulation.bodies[i].Position.Z;
            }

            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                BufferUsageHint.DynamicDraw);

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.Points, 0, numBodies);

            SwapBuffers();
            GL.GetError();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
        protected override void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);


            base.OnUnload();
        }

    }
}
