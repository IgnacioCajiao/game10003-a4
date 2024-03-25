using Raylib_cs;
using System.Numerics;

namespace game;

internal class BallGame
{
    static string title = "Game Title";
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

        gravity = new Vector2(0, +10);
    }

    static void Update()
    {
        velocity += gravity * Raylib.GetFrameTime();
        position += velocity;

        bool isBelowScreen = position.Y + radius > +Raylib.GetScreenHeight();
        bool isTravellingDown = position.Y > 0;

        if (isBelowScreen && isTravellingDown)
        {
            position.Y = Raylib.GetScreenHeight() - radius;
            velocity.Y = -velocity.Y * 0.8f;
        }

        Raylib.DrawCircleV(position, radius, color);
    }
}
