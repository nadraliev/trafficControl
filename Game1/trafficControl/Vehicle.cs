using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trafficControl
{
    public class Vehicle
    {
        public enum Direction { Left, Top, Right, Bottom };

        public Vector2 position;
        public Direction direction;
        public Lane lane;
        public float speed;
        public float acceleration;
        public int startTime;

        public Vehicle(Direction initDirection, Vector2 initPosition, Lane initLane, float initAcceleration, int startTime)
        {
            direction = initDirection;
            lane = initLane;
            position = initPosition;
            speed = 0;
            acceleration = initAcceleration;
            this.startTime = startTime;
        }

        public void Move()
        {
            switch (direction)
            {
                case Direction.Left:
                    position.X -= speed;
                    break;
                case Direction.Top:
                    position.Y += speed;
                    break;
                case Direction.Right:
                    position.X += speed;
                    break;
                case Direction.Bottom:
                    position.Y -= speed;
                    break;
            }
            speed += acceleration;
        }

        public void Break()
        {
            while (acceleration > 0) {
                speed -= acceleration--;
                Move();
            }
            acceleration = 0;
        }
    }
}
