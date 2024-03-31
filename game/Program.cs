using Raylib_cs;
using System.Numerics;
namespace game
{
    internal class BallGame
    {
        static string title = "Ball Game";
        static Vector2 position;
        static Vector2 velocity;
        static Vector2 gravity;
        static Color color;
        static int radius;

        static void Main(string[] args)
        {
            Raylib.InitWindow(800, 600, title);

            Raylib.SetTargetFPS(60);

            Setup();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.RayWhite);

                Update();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        static void Setup()
        {
            position = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;
            color = Color.Blue;
            radius = 25;

            gravity = new Vector2(0, +800);
        }

        static void Update()
        {
            
            if (Raylib.IsKeyDown(KeyboardKey.Right))
                velocity.X += 30;
            if (Raylib.IsKeyDown(KeyboardKey.Left))
                velocity.X -= 30;

            velocity += gravity * Raylib.GetFrameTime();

            position += velocity * Raylib.GetFrameTime();

            bool isOnGround = position.Y + radius >= Raylib.GetScreenHeight();
            if (isOnGround)
            {
                position.Y = Raylib.GetScreenHeight() - radius;
                velocity.Y = 0;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Up) && isOnGround)
                velocity.Y = -900;

            if (position.X < 0 + radius)
            {
                position.X = 0 + radius;
                velocity.X = 0;
            }
            else if (position.X > Raylib.GetScreenWidth() - radius)
            {
                position.X = Raylib.GetScreenWidth() - radius;
                velocity.X = 0;
            }

            // Draw the ball
            Raylib.DrawCircleV(position, radius, color);
        }
    }
}
