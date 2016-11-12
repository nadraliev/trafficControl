using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trafficControl
{
    public class Lane
    {

        private Vector2 startPoint;
        private Vector2 endPoint;

        public Vector2 StartPoint { get { return startPoint; } set { startPoint = value; } }
        public Vector2 EndPoint { get { return endPoint; } set { endPoint = value; } }

        public enum Direction { Left=0, Top=-90, Right=-180, Bottom=-270 };
        public Direction direction;

        public TrafficLight trafficLight;

        public Lane(Vector2 startPoint, Vector2 endPoint, Direction direction)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.direction = direction;
        }

        public Lane(float xStart, float yStart, float xEnd, float yEnd, Direction direction)
        {
            startPoint = new Vector2(xStart, yStart);
            endPoint = new Vector2(xEnd, yEnd);
            this.direction = direction;
        }
    }
}
