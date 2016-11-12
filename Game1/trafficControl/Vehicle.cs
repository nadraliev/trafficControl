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


        private float acceleration;
        private float speed;    //dots per millisecond
        private Vector3 position;
        private Model model;
        private float scale = 1;
        private Lane lane;
        private Lane.Direction direction;
  
        private int startTime;
        

        public float Acceleration { get { return acceleration; } }
        public float Speed { get { return speed; } }
        public Vector3 Position { get { return position; } set { position = value; } }
        public Model Model { get { return model; } set { model = value; } }
        public float Scale { get { return scale; } set { scale = value; } }
        public Lane Lane { get { return lane; } set { lane = value; } }

        public Vehicle(Model model, Lane.Direction initDirection, Vector3 initPosition, Lane initLane, float initAcceleration, int startTime)
        {
            this.model = model;
            direction = initDirection;
            lane = initLane;
            position = initPosition;
            speed = 0;
            acceleration = initAcceleration;
            this.startTime = startTime;
        }

        public Vehicle(Lane.Direction initDirection, Vector3 initPosition, Lane initLane, float initAcceleration, int startTime)
        {
            direction = initDirection;
            lane = initLane;
            position = initPosition;
            speed = 0;
            acceleration = initAcceleration;
            this.startTime = startTime;
        }

        public Vehicle(Lane.Direction initDirection,Lane initLane, float initAcceleration, int startTime)
        {
            direction = initDirection;
            lane = initLane;
            position = new Vector3(0,0,0);
            speed = 0;
            acceleration = initAcceleration;
            this.startTime = startTime;
        }

        public Vehicle(Lane initLane, float initAcceleration, int startTime, float scale)
        {
            direction = initLane.direction;
            lane = initLane;
            position = new Vector3(lane.StartPoint.X/scale, 5, lane.StartPoint.Y/scale);
            this.scale = scale;
            speed = 0;
            acceleration = initAcceleration;
            this.startTime = startTime;
        }

        public void Move()
        {
            if (lane.trafficLight.light.Equals(TrafficLight.Light.Green) || lane.trafficLight.light.Equals(TrafficLight.Light.Yellow))
            {
                position.X -= speed;
                
                speed += acceleration;
                if (speed < 0)
                {
                    speed = 0;
                    Stopped?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Turn(Lane.Direction newDirection)
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
