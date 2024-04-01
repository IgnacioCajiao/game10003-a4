﻿using Raylib_cs;
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
        static int jumpCount = 5;
        static bool isGameOver = false;
        static Vector2[] squares = new Vector2[50];
        static int squareSize = 10;

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

            for (int i = 0; i < squares.Length; i++)
            {
                squares[i] = new Vector2(Raylib.GetRandomValue(0, Raylib.GetScreenWidth() - squareSize), Raylib.GetRandomValue(0, Raylib.GetScreenHeight() - squareSize));
            }
        }

        static void Update()
        {
            if (isGameOver)
            {
                Raylib.DrawText("Game Over", 300, 200, 45, Color.Black);
                return;
            }

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

                if (jumpCount <= 0)
                {
                    isGameOver = true;
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Up) && isOnGround && jumpCount > 0)
            {
                velocity.Y = -900;
                jumpCount--;
            }

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

            for (int i = 0; i < squares.Length; i++)
            {
                if (Raylib.CheckCollisionCircles(position, radius, squares[i] + new Vector2(squareSize / 2, squareSize / 2), squareSize / 2))
                {
                    squares[i] = new Vector2(-1, -1); 
                }
            }

            Raylib.DrawText($"Jumps left: {jumpCount}", 10, 10, 20, Color.Black);

            foreach (Vector2 square in squares)
            {
                if (square.X >= 0 && square.Y >= 0) 
                {
                    Raylib.DrawRectangleV(square, new Vector2(squareSize, squareSize), Color.Blue);
                }
            }

            Raylib.DrawCircleV(position, radius, color);
        }
    }
}
