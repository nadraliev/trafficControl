using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trafficControl
{
    public class Vehicle
    {
        public event EventHandler Stopped;
        public event EventHandler Turned;

        public enum Direction { Left, Top, Right, Bottom };

        private float acceleration;
        private float speed;    //dots per millisecond
        private Vector3 position;
        private Model model;
        private Direction direction;
        private Lane lane;
        private int startTime;
        

        public float Acceleration { get { return acceleration; } }
        public float Speed { get { return speed; } }
        public Vector3 Position { get { return position; } }
        public Model Model { get { return model; } set { model = value; } }

        public Vehicle(Model model, Direction initDirection, Vector3 initPosition, Lane initLane, float initAcceleration, int startTime)
        {
            this.model = model;
            direction = initDirection;
            lane = initLane;
            position = initPosition;
            speed = 0;
            acceleration = initAcceleration;
            this.startTime = startTime;
        }

        public Vehicle(Direction initDirection, Vector3 initPosition, Lane initLane, float initAcceleration, int startTime)
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
                    position.Z -= speed;
                    break;
                case Direction.Top:
                    position.X -= speed;
                    break;
                case Direction.Right:
                    position.Z += speed;
                    break;
                case Direction.Bottom:
                    position.X += speed;
                    break;
            }
            speed += acceleration;
            if (speed < 0)
            {
                speed = 0;
                Stopped?.Invoke(this, new EventArgs());
            }
        }

        public void Turn(Direction newDirection)
        {
            direction = newDirection;
            Turned?.Invoke(this, new EventArgs());
            //change lane
        }

        /*
        public void Break()
        {
            while (acceleration > 0) {
                speed -= acceleration--;
                Move();
            }
            acceleration = 0;
            Stopped?.Invoke(this, new EventArgs());
        }
        */

        public void GiveAcceleration(float acceleration)
        {
            this.acceleration += acceleration;
        }

        private void ChangeLane(Lane newLane)
        {
            lane = newLane;
        }
    }
}
