using System;
using System.Collections.Generic;
using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using Raylib_cs;

namespace FlappyBird
{
    class PipeHandler
    {
        private Color COLOR = DARKGREEN;
        public const int WIDTH = 100;
        public const int HEIGHT = (Game.HEIGHT / 4) * 5;
        public const int SPACING = 135;
        public const int SPEED = 125;
        public const int SPAWN_POS = Game.WIDTH - (Game.WIDTH / 3);

        public bool IsCounted { get; private set; }

        private readonly List<Vector2> _pipes;
        private readonly Random _rand;

        public PipeHandler()
        {
            _pipes = new List<Vector2>();
            _rand = new Random();
            SpawnPipe();
        }

        public void ClearPipes()
        {
            _pipes.Clear();
            SpawnPipe();
        }

        public List<Vector2> GetPipes()
        {
            return _pipes;
        }

        public bool IsOutOfBounds()
        {
            if (_pipes[0].X < 0 && !IsCounted)
            {
                IsCounted = true;
                return true;
            }

            return false;
        }

        public void Update(float dt)
        {
            SpawnPipe();
            MovePipes(dt);
            RemoveFirstPipe();
        }

        public void Render()
        {
            foreach (var p in _pipes)
            {
                int x = (int)p.X;
                int botY = (int)p.Y;
                int topY = botY - SPACING - HEIGHT;
                DrawRectangle(x, botY, WIDTH, HEIGHT, COLOR);
                DrawRectangle(x, topY, WIDTH, HEIGHT, COLOR);
            }
        }

        private void SpawnPipe()
        {
            int count = _pipes.Count;
            // Spawn pipes if there is no pipe
            // Or if the last pipe on the list is at the spawn position
            if ((count == 0) || (_pipes[count - 1].X <= SPAWN_POS))
            {
                _pipes.Add(new Vector2(Game.WIDTH, SetPipeHeight()));
            }
        }

        private void MovePipes(float dt)
        {
            for (int i = 0; i < _pipes.Count; i++)
            {
                var temp = _pipes[i];
                temp.X -= SPEED * dt;
                _pipes[i] = temp;
            }
        }

        private void RemoveFirstPipe()
        {
            if (_pipes[0].X + WIDTH < 0)
            {
                _pipes.RemoveAt(0);
                IsCounted = false;
            }
        }

        private float SetPipeHeight()
        {
            return _rand.Next(Game.HEIGHT / 3, Game.HEIGHT - 100);
        }
    }
}