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
        TrafficLight trafficLight;

        Dictionary<string, Lane> lanes;
       

        float roadNodeLength = 200;
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
            
            cameraPosition = new Vector3(400f, 600f, 400f);
            cameraTarget = new Vector3(0f, 0f, 0f);
            world = Matrix.CreateWorld(new Vector3(0, 0, 0), Vector3.Forward, Vector3.UnitY);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GraphicsDevice.DisplayMode.AspectRatio, 1f, 100000f);
            view = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.UnitY);

            lanes = new Dictionary<string, Lane>();
            Lane lane = new Lane(roadNodeLength * (roadNodesCount+1), roadNodeLength / 2, roadNodeLength, roadNodeLength / 2);
            trafficLight = new TrafficLight();
            lane.trafficLight = trafficLight;
            car = new Vehicle(Vehicle.Direction.Top, lane, 2, 1);
            car.Scale = 0.1f;
            car.Position = new Vector3(400/car.Scale, 0.5f/car.Scale, -60/car.Scale);

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
            trafficLight.model = light;
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

            if (gameTime.ElapsedGameTime.TotalMilliseconds > 1)
                car.Move();
            if (Math.Abs(gameTime.TotalGameTime.TotalSeconds - 5) < 0.001)
                car.GiveAcceleration(-4f);

            if (Math.Abs(gameTime.TotalGameTime.TotalSeconds - 2) < 0.01)
                trafficLight.light = TrafficLight.Light.Green;

            car.Stopped += Car_Stopped;

            base.Update(gameTime);


        }

        private void Car_Stopped(object sender, System.EventArgs e)
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
            DrawModel(junction, world, view, projection, new Vector3(0, 0, 0));

            //down
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(0, 0, roadNodeLength*i));

            //up
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(0, 0, -roadNodeLength*i));

            //right
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(roadNodeLength*i, 0, 0));
            
            //left
            for (int i = 1; i <= roadNodesCount; i++)
                DrawModel(junction, world, view, projection, new Vector3(-roadNodeLength*i, 0, 0));

            DrawModel(car.Model, world, view, projection, car.Position, car.Scale, Matrix.CreateRotationY(MathHelper.ToRadians(-90)));
            

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
                    effect.World = world * Matrix.CreateTranslation(position) * Matrix.CreateScale(scale) * rotation;
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
    }
}
