using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using Template.Enemys;

namespace Template
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Spelet är 800x480 

        

        Texture2D pixel;
        Texture2D meeleminion;
        Rectangle Road1 = new Rectangle(0, 60, 445, 60);
        Rectangle Road2 = new Rectangle(385, 60, 60, 160);
        Rectangle Road3 = new Rectangle(385, 220, 225, 60);
        Rectangle Road4 = new Rectangle(550, 60, 60, 220);
        Rectangle Road5 = new Rectangle(550, 60, 190, 60);
        Rectangle Road6 = new Rectangle(680, 60, 60, 360);
        Rectangle Road7 = new Rectangle(0, 360, 740, 60);

        int Time = 0;


        

        //KOmentar




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        List<Minion> minions = new List<Minion>();
        public void SpawnMinion(float x_p, float y_p, float x_s, float y_s)
        {
            Minion Minion1 = new Minion(x_p, y_p, x_s, y_s);
            minions.Add(Minion1);
            minions.Count();
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // TODO: Add your initialization logic here

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

            pixel = Content.Load<Texture2D>("pixel");
            meeleminion = Content.Load<Texture2D>("Meele Minion");

            // TODO: use this.Content to load your game content here 
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
        /// 
        int Enemys = 0;
        protected override void Update(GameTime gameTime)
        {
            Time += 1;
            if ((Time / (Enemys + 1)) == 100)
            {
                SpawnMinion(0, 60, 2, 0);
                Enemys++;
            }




            foreach (Minion e in minions)
            {
                e.x_position = e.x_position + e.x_speed;
                e.y_position = e.y_position + e.y_speed;
            } 


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(pixel, Road1, Color.Black);
            spriteBatch.Draw(pixel, Road2, Color.Black);
            spriteBatch.Draw(pixel, Road3, Color.Black);
            spriteBatch.Draw(pixel, Road4, Color.Black);
            spriteBatch.Draw(pixel, Road5, Color.Black);
            spriteBatch.Draw(pixel, Road6, Color.Black);
            spriteBatch.Draw(pixel, Road7, Color.Black);
            foreach (Minion e in minions)
            {
                spriteBatch.Draw(meeleminion, new Vector2(e.x_position, e.y_position), Color.White);
            }


            spriteBatch.End();


            // TODO: Add your drawing code here.

            base.Draw(gameTime);
        }
    }
}
