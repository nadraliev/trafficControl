using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trafficControl
{
    public class TrafficLight
    {
        public enum Light {Red=0, Yellow=1, Green=2 };

        public Light light;
        public Vector3 position;
        public Matrix rotation;
        public float scale;
        private Model model;
        public BoundingSphere boundingSphere;
        public double lastChangeSeconds;

        public Model Model {
            get {
                return model; }
            set
            {
                model = value;
                foreach (ModelMesh mesh in value.Meshes)
                    if (mesh.Name.Equals("planarTrimmedSurface1 group1"))
                        boundingSphere = mesh.BoundingSphere;
            } }


        public TrafficLight()
        {
            light = Light.Red;
            ChangeLight(light);
            lastChangeSeconds = 0;
        }

        private void ChangeLight(Light newLight)
        {
            light = newLight;   
        }

        public void ToggleLight()
        {
            if (light.Equals(Light.Green))
                ChangeLight(Light.Red);
            else ChangeLight(Light.Green);
        }

        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            int lightNumber = (int)light;
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (!mesh.Name.Equals(Enum.GetName(typeof(Light), (lightNumber + 1) % 3).ToString().ToLower())
                   &&
                   !mesh.Name.Equals(Enum.GetName(typeof(Light), (lightNumber + 2) % 3).ToString().ToLower()))
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.LightingEnabled = true;
                        effect.World = world * Matrix.CreateTranslation(position) * Matrix.CreateScale(scale) * rotation;
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    
                    mesh.Draw();
                }
                
            }
            
        }



    }
}
