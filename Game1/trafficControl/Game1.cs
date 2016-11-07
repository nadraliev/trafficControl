using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
       

        float roadStart = 400;

        
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
            
            cameraPosition = new Vector3(500f, 900f, 500f);
            cameraTarget = new Vector3(0f, 0f, 0f);
            world = Matrix.CreateWorld(new Vector3(0, 0, 0), Vector3.Forward, Vector3.UnitY);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GraphicsDevice.DisplayMode.AspectRatio, 1f, 10000f);
            view = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.UnitY);

            base.Initialize();

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
            junction = Content.Load<Model>("cylinder");
            mers = Content.Load<Model>("bv");
           
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



            base.Update(gameTime);


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawModel(junction, world, view, projection, new Vector3(0, 0, 0));
            DrawModel(junction, world, view, projection, new Vector3(0, 0, roadStart/2));
            DrawModel(junction, world, view, projection, new Vector3(0, 0, roadStart));
            DrawModel(junction, world, view, projection, new Vector3(0, 0, -roadStart/2));
            DrawModel(junction, world, view, projection, new Vector3(0, 0, -roadStart));
            DrawModel(junction, world, view, projection, new Vector3(roadStart/2, 0, 0));
            DrawModel(junction, world, view, projection, new Vector3(roadStart, 0, 0));
            DrawModel(junction, world, view, projection, new Vector3(-roadStart/2, 0, 0));
            DrawModel(junction, world, view, projection, new Vector3(-roadStart, 0, 0));
            DrawModel(mers, world, view, projection, new Vector3(1600, 5, -600), 0.1f, Matrix.CreateRotationY(MathHelper.ToRadians(-90)));
            

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
