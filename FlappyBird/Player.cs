using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using Raylib_cs;

namespace FlappyBird
{
    class Player
    {
        public const int POS_X = 100;
        public const int SIZE = 40;
        public const int SPEED = 450;

        private const float G_SPEED = 20f;

        private readonly Color _color;

        private float _ySpeed;

        public Player()
        {
            MaxScore = 0;
            _color = RED;        
        }

        public int Score { get; private set; }
        public int MaxScore { get; private set; }
        public int PosY { get; private set; }
        public bool CanFall { get; set; }

        public void InitPlayer()
        {
            PosY = (Game.HEIGHT / 2) - (SIZE / 2);
            Score = 0;
            _ySpeed = 0f;
            CanFall = false;
        }

        public void Jump()
        {
            if (!CanFall)
            {
                CanFall = true;
            }
            _ySpeed = -SPEED;
        }

        public void IncreaseScore()
        {
            Score += 1;
            IncreaseMaxScore();
        }

        public void Update(float dt)
        {
            if (CanFall)
            {
                PosY += (int)(_ySpeed * dt);
                _ySpeed += G_SPEED;
            }
        }

        public void Render()
        {
            DrawRectangle(POS_X, PosY, SIZE, SIZE, _color);
        }

        public bool IsOutOfBounds()
        {
            return (PosY >= Game.HEIGHT - SIZE) || (PosY < 0);
        }        

        private void IncreaseMaxScore()
        {
            if (Score > MaxScore)
            {
                MaxScore = Score;
            }
        }
    }
}
