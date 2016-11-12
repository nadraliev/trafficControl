using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace trafficControl
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector3 cameraPosition;
        Vector3 cameraTarget;
        Matrix world;
        Matrix projection;
        Matrix view;

        Model junction;
        Model mers;
        Vehicle car;
        List<TrafficLight> trafficLights;

        Dictionary<Lanes, Lane> lanes;
        enum Lanes { BottomRight, BottomLeft, TopRight, TopLeft, LeftTop, LeftBottom, RightTop, RightBottom};
       

        float roadNodeLength = 200;
        float roadNodeLengthScaled = 2000;
        int roadNodesCount = 6;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            cameraPosition = new Vector3(4000f, 6000f, 4000f);
            cameraTarget = new Vector3(0f, 0f, 0f);
            world = Matrix.CreateWorld(new Vector3(0, 0, 0), Vector3.Forward, Vector3.UnitY);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000000f);
            view = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.UnitY);

            lanes = new Dictionary<Lanes, Lane>();
            lanes.Add(Lanes.BottomRight, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), -roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, -roadNodeLengthScaled / 4, Lane.Direction.Top));
            lanes.Add(Lanes.BottomLeft, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, roadNodeLengthScaled / 4, Lane.Direction.Top));
            lanes.Add(Lanes.LeftBottom, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), -roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, -roadNodeLengthScaled / 4, Lane.Direction.Right));
            lanes.Add(Lanes.LeftTop, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, roadNodeLengthScaled / 4, Lane.Direction.Right));
            lanes.Add(Lanes.TopLeft, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), -roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, -roadNodeLengthScaled / 4, Lane.Direction.Bottom));
            lanes.Add(Lanes.TopRight, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, roadNodeLengthScaled / 4, Lane.Direction.Bottom));
            lanes.Add(Lanes.RightTop, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), -roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, -roadNodeLengthScaled / 4, Lane.Direction.Left));
            lanes.Add(Lanes.RightBottom, new Lane(roadNodeLengthScaled * (roadNodesCount + 1), roadNodeLengthScaled / 4, roadNodeLengthScaled / 2, roadNodeLengthScaled / 4, Lane.Direction.Left));

            trafficLights = new List<TrafficLight>();
            trafficLights.Add(new TrafficLight());
            lanes[Lanes.BottomRight].trafficLight = trafficLights[0];
            lanes[Lanes.BottomRight].trafficLight.position = new Vector3(40, 0, -30);
            lanes[Lanes.BottomRight].trafficLight.rotation = world;

            lanes[Lanes.LeftTop].trafficLight = trafficLights[0];
            lanes[Lanes.BottomLeft].trafficLight = trafficLights[0];
            lanes[Lanes.TopLeft].trafficLight = trafficLights[0];
            lanes[Lanes.RightBottom].trafficLight = trafficLights[0];
            car = new Vehicle(lanes[Lanes.RightBottom], 2, 1, 1);

            base.Initialize();

            
        }

        /// <summary>t
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
            junction = Content.Load<Model>("cylinder");
            mers = Content.Load<Model>("bv");
            Model light = Content.Load<Model>("trafficLight");
            trafficLights[0].Model = light;
            car.Model = mers;
           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.A) &&
                Math.Abs(trafficLights[0].lastChangeSeconds - gameTime.TotalGameTime.TotalSeconds) > 0.2)
            {
                trafficLights[0].ToggleLight();
                trafficLights[0].lastChangeSeconds = gameTime.TotalGameTime.TotalSeconds;
            }
            

            if (gameTime.ElapsedGameTime.TotalMilliseconds > 1)
                car.Move();

            car.Stopped += Car_Stopped;

            base.Update(gameTime);


        }

        private void Car_Stopped(object sender, EventArgs e)
        {
            int i = 1;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //center node
            DrawModel(junction, world, view, projection, new Vector3(0, 0, 0),10);

            //down
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(0, 0, roadNodeLength*i), 10);

            //up
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(0, 0, -roadNodeLength*i),10);

            //right
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(roadNodeLength*i, 0, 0),10);
            
            //left
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(-roadNodeLength*i, 0, 0),10);

            DrawModel(car.Model, world, view, projection, car.Position, car.Scale, Matrix.CreateRotationY(MathHelper.ToRadians((int)car.Lane.direction)));
            trafficLights[0].Draw(world, view, projection);


            

            base.Draw(gameTime);


        }


        public void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, Vector3 position, float scale, Matrix rotation)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.LightingEnabled = true;
                    effect.World = world;

                    
                    effect.World *= Matrix.CreateTranslation(position);
                    effect.World *= Matrix.CreateScale(scale);
                    effect.World *= rotation;
                   
                    
                    
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }

        public void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, Vector3 position, float scale)
        {
            DrawModel(model, world, view, projection, position, scale, world);
        }

        public void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, Vector3 position)
        {
            DrawModel(model, world, view, projection, position, 1f);
        }


        public Ray CalculateRay(Vector2 mouseLocation, Matrix view,
    Matrix projection, Viewport viewport)
        {
            Vector3 nearPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 0.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 farPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 1.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public float? IntersectDistance(BoundingSphere sphere, Vector2 mouseLocation,
            Matrix view, Matrix projection, Viewport viewport)
        {
            Ray mouseRay = CalculateRay(mouseLocation, view, projection, viewport);
            return mouseRay.Intersects(sphere);
        }
    }
}
