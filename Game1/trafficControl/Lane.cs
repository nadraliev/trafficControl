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

        public Vector2 StartPoint { get; }
        public Vector2 EndPoint { get; }

        public Lane(Vector2 startPoint, Vector2 endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public Lane(float xStart, float yStart, float xEnd, float yEnd)
        {
            startPoint = new Vector2(xStart, yStart);
            endPoint = new Vector2(xEnd, yEnd);
        }
    }
}
