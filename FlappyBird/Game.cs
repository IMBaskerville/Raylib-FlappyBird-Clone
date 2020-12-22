using System;
using Raylib_cs;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Raylib;

namespace FlappyBird
{
    class Game
    {
        // Window fields
        public const int WIDTH = 800;
        public const int HEIGHT = (WIDTH / 16) * 9;
        private const string TITLE = "Flappy Bird";
        private const int FPS = 60;

        // Text fields
        private Color TEXT_COLOR = WHITE; 
        private const int TEXT_X = 10;
        private const int FONT_SIZE = 18;

        // Time fields
        private float _dt;
        private bool _isPaused;

        // Game objects
        private Player _player;
        private PipeHandler _pipe;

        public Game()
        {
            CreateWindow();
            InitGame();
        }

        public void Run()
        {
            while (!WindowShouldClose())
            {
                VerifyEvents();
                UpdateDeltaTime();
                if (!_isPaused && _player.CanFall)
                {
                    Update();
                }
                Render();
            }
        }

        private void VerifyEvents()
        {
            if (IsKeyPressed(KEY_P))
            {
                _isPaused = !_isPaused;
            }
            if (IsKeyPressed(KEY_SPACE) && !_isPaused)
            {
                _player.Jump();
            }
        }

        private void UpdateDeltaTime()
        {
            _dt = GetFrameTime();
        }

        private void Update()
        {
            _player.Update(_dt);
            _pipe.Update(_dt);
            UpdateScore();
            CheckCollision();
        }

        private void Render()
        {
            BeginDrawing();
            ClearBackground(SKYBLUE);
            // Draw here
            _pipe.Render();
            _player.Render();
            
            DrawGameText();

            EndDrawing();
        }

        private void DrawGameText()
        {
            if (!_isPaused)
            {
                DrawRunningGameText();
            }
            else
            {
                DrawPausedGameText();
            }

            DisplayScore();
        }

        private void DrawRunningGameText()
        {
            DrawGameStateText("Running");
        }

        private void DrawPausedGameText()
        {
            DrawGameStateText("Paused");
        }

        private void DrawGameStateText(string state)
        {
            DrawText($"Game is {state}", TEXT_X, 10, FONT_SIZE, TEXT_COLOR);
        }

        private void DisplayScore()
        {
            DrawText($"Max Score: {_player.MaxScore}", TEXT_X, 30, FONT_SIZE, TEXT_COLOR);
            DrawText($"Score: {_player.Score}", TEXT_X, 50, FONT_SIZE, TEXT_COLOR);
        }

        private void CreateWindow()
        {
            InitWindow(WIDTH, HEIGHT, TITLE);
            SetTargetFPS(FPS);
        }

        private void InitGame()
        {
            _isPaused = false;
            _dt = 0;
            _player = new Player();
            _pipe = new PipeHandler();
            RestartGame();
        }

        private void RestartGame()
        {
            _player.InitPlayer();
            _pipe.ClearPipes();
        }

        private void UpdateScore()
        {
            if (_pipe.IsOutOfBounds())
            {
                _player.IncreaseScore();
            }
        }

        private void CheckCollision()
        {
            if (Crashed())
            {
                RestartGame();
            }
        }

        private bool Crashed()
        {
            if (_player.IsOutOfBounds()) 
            {
                return true;
            }

            var pipes = _pipe.GetPipes();

            int playerX = Player.POS_X;
            int playerY = _player.PosY;
            int playerSize = Player.SIZE;

            bool hasCrashed = false;

            foreach (var p in pipes)
            {
                if ((playerX + playerSize >= p.X) 
                        && (playerX <= p.X + PipeHandler.WIDTH))
                {
                    if ((playerY + playerSize >= p.Y) 
                            || (playerY <= p.Y - PipeHandler.SPACING))
                    {
                        hasCrashed = true;
                        break;
                    }
                }
            }

            return hasCrashed;
        }
    }
}
