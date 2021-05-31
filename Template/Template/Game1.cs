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
using Template.Turrets;

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
        Texture2D poro;
        Texture2D turret;
        Rectangle Road1 = new Rectangle(0, 60, 445, 60);
        Rectangle Road2 = new Rectangle(385, 60, 61, 160);
        Rectangle Road3 = new Rectangle(385, 220, 225, 60);
        Rectangle Road4 = new Rectangle(550, 60, 60, 220);
        Rectangle Road5 = new Rectangle(550, 60, 190, 60);
        Rectangle Road6 = new Rectangle(680, 60, 60, 360);
        Rectangle Road7 = new Rectangle(0, 360, 740, 60);


        int ME_x = 300;
        int ME_y = 200;

        int TurretCD = 0;

        int Time = 0;



        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }




        //KOmentar




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        List<Minion> minions = new List<Minion>();
        public void SpawnMinion(float x_p, float y_p, float x_s, float y_s, int hp, float traveled)
        {
            Minion Minion1 = new Minion(x_p, y_p, x_s, y_s, hp, traveled);
            minions.Add(Minion1);
            minions.Count();
        }


        List<Turret> turrets = new List<Turret>();
        public void SpawnTurret(float x_p, float y_p, float AS, float Atk_WUP, float range, List<Minion> in_range, List<Minion> target)
        {
            Turret turret1 = new Turret(x_p, y_p, AS, Atk_WUP, range, in_range, target);
            turrets.Add(turret1);
            turrets.Count();
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
            poro = Content.Load<Texture2D>("poro");
            turret = Content.Load<Texture2D>("Turret");

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
            TurretCD++;
            if ((Time / (Enemys + 1)) == 100)
            {
                SpawnMinion(0, 60, 2, 0, 1, 0);
                Enemys++;
            }


            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.W))
                ME_y = ME_y - 3;
            if (kstate.IsKeyDown(Keys.S))
                ME_y = ME_y + 3;
            if (kstate.IsKeyDown(Keys.D))
                ME_x = ME_x + 3;
            if (kstate.IsKeyDown(Keys.A))
                ME_x = ME_x - 3;

            if (kstate.IsKeyDown(Keys.Space) && TurretCD > 40)
            {
                TurretCD = 0;
                SpawnTurret(ME_x + 40, ME_y, 100, 0, 200, new List<Minion>(), new List<Minion>());
            }



            foreach (Turret a in turrets)    //a = allie
            {
                float current_highest_traveled = 0;
                a.attack_Wind_UP++;
                if (a.attack_Wind_UP > a.attack_speed)     //e = enemy
                {
                    //a.minions_in_range.Clear();  ?? detta förstörde allt
                    foreach (Minion e in minions)
                    {
                        if (GetDistance(e.x_position, e.y_position, a.x_position, a.y_position) <= a.attack_range)
                            a.minions_in_range.Add(e);
                    }

                    if (a.minions_in_range != null)
                    foreach (Minion e in a.minions_in_range)
                    {
                       if (e.units_traveled > current_highest_traveled)
                       {
                            a.list_target.Clear();
                            current_highest_traveled = e.units_traveled;
                            a.list_target.Add(e);
                       }
                    }
                    foreach (Minion e in minions)
                    {
                        if (e.units_traveled == current_highest_traveled)
                            e.health--;
                    }
                }
            }


            for (int i = minions.Count - 1; i >= 0; i--)
            {
                if (minions[i].health <= 0)
                {
                    minions.RemoveAt(i);
                }
            }



            foreach (Minion e in minions)
            {
                e.x_position = e.x_position + e.x_speed;
                e.y_position = e.y_position + e.y_speed;
                e.units_traveled = e.units_traveled + e.x_speed + e.y_speed;
            }
             



            foreach (Minion e in minions)
            {
                if (e.x_position == 386 && e.y_position == 60)
                {
                    e.x_speed = 0;
                    e.y_speed = 2;
                }

                if (e.x_position == 386 && e.y_position == 220)
                {
                    e.x_speed = 2;
                    e.y_speed = 0;
                }

                if (e.x_position == 550 && e.y_position == 220)
                {
                    e.x_speed = 0;
                    e.y_speed = -2;
                }

                if (e.x_position == 550 && e.y_position == 60)
                {
                    e.x_speed = 2;
                    e.y_speed = 0;
                }

                if (e.x_position == 680 && e.y_position == 60)
                {
                    e.x_speed = 0;
                    e.y_speed = 2;
                }

                if (e.x_position == 680 && e.y_position == 360)
                {
                    e.x_speed = -2;
                    e.y_speed = 0;
                }
            }

            /*foreach (Minion e in minions)
            {
                if (e.Health == 0 || e.x_position == 300)
                {
                    minions.Remove(e);
                }
            }*/



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

            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
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

            foreach (Turret e in turrets)
            {
                spriteBatch.Draw(turret, new Vector2(e.x_position, e.y_position), Color.White);
            }

            spriteBatch.Draw(poro, new Vector2(ME_x, ME_y), Color.White);

            spriteBatch.End();


            // TODO: Add your drawing code here.

            base.Draw(gameTime);
        }
    }
}
