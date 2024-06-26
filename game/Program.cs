﻿using Raylib_cs;
using System.Numerics;

namespace game
{
    internal class BallGame
    {
        // Variable set up
        static string title = "Ball Game";
        static Vector2 position, velocity, gravity;
        static Color color;
        static int radius, jumpCount = 5, squareSize = 10, score = 0, highScore = 0;
        static bool isGameOver = false;
        static Vector2[] squares = new Vector2[50];

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
            // draw player ball and adds world gravity
            position = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;
            color = Color.Blue;
            radius = 25;
            gravity = new Vector2(0, +800);
            // used array to randomely place squares on the screen as collectables, with randomized locations.
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i] = new Vector2(Raylib.GetRandomValue(0, Raylib.GetScreenWidth() - squareSize), Raylib.GetRandomValue(0, Raylib.GetScreenHeight() - squareSize));
            }
        }

        static void ResetGame()
        {
            // function to reset game to original state 
            position = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;
            velocity = Vector2.Zero;
            jumpCount = 5;
            isGameOver = false;
            score = 0;
            // randomly places square again after reset
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i] = new Vector2(Raylib.GetRandomValue(0, Raylib.GetScreenWidth() - squareSize), Raylib.GetRandomValue(0, Raylib.GetScreenHeight() - squareSize));
            }
        }

        static void Update()
        {
            // calls functions. did this to organize the code.
            if (isGameOver)
            {
                DisplayGameOver();
                if (Raylib.IsKeyPressed(KeyboardKey.Enter)) ResetGame();
                return;
            }

            UpdateVelocity();
            UpdatePosition();
            CheckCollisions();
            
            // this draws the game to the window. jumps left, score, the squares, and the player circle.
            Raylib.DrawText($"Jumps left: {jumpCount}", 10, 10, 20, Color.Black);
            Raylib.DrawText($"Score: {score}", 10, 30, 20, Color.Black); 

            foreach (Vector2 square in squares)
            {
                if (square.X >= 0 && square.Y >= 0) 
                {
                    Raylib.DrawRectangleV(square, new Vector2(squareSize, squareSize), Color.Blue);
                }
            }

            Raylib.DrawCircleV(position, radius, color);
        }
        // this function draws the gamer over screen displaying all the score and restart function.
        static void DisplayGameOver()
        {
            Raylib.DrawText("Game Over", 300, 200, 45, Color.Black);
            Raylib.DrawText($"Final Score: {score}", 300, 250, 30, Color.Black); 
            Raylib.DrawText($"High Score: {highScore}", 300, 300, 30, Color.Black); 
            Raylib.DrawText("Press Enter to Restart", 300, 350, 20, Color.Black); 
        }
        // this function enables player movement and velocity.
        static void UpdateVelocity()
        {
            if (Raylib.IsKeyDown(KeyboardKey.Right)) velocity.X += 30;
            if (Raylib.IsKeyDown(KeyboardKey.Left)) velocity.X -= 30;
            velocity += gravity * Raylib.GetFrameTime();
        }
        // this function checks if the player circle is on the ground to grant the ability to jump.
        // aswell checks if jump count is 0 to display game over.
        static void UpdatePosition()
        {
            position += velocity * Raylib.GetFrameTime();
            bool isOnGround = position.Y + radius >= Raylib.GetScreenHeight();

            if (isOnGround)
            {
                position.Y = Raylib.GetScreenHeight() - radius;
                velocity.Y = 0;

                if (jumpCount <= 0)
                {
                    isGameOver = true;
                    if (score > highScore) highScore = score;
                }
            }
            // this checks if the player cirlce is on the ground and up arrow key is pressed to add negative y velocity
            // aswell reduces jump count.
            if (Raylib.IsKeyPressed(KeyboardKey.Up) && isOnGround && jumpCount > 0)
            {
                velocity.Y = -900;
                jumpCount--;
            }
            // bounds of the window by adjusting postion and velocity to 0 
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
        }
        // this function checks collision between the player circle and each square in the array, on collision moves squares off screen and increments score.
        static void CheckCollisions()
        {
            for (int i = 0; i < squares.Length; i++)
            {
                if (Raylib.CheckCollisionCircles(position, radius, squares[i] + new Vector2(squareSize / 2, squareSize / 2), squareSize / 2))
                {
                    squares[i] = new Vector2(-1, -1); 
                    score++;
                }
            }
        }
    }
}
