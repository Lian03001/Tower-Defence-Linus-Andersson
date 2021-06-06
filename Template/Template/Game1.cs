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
using Template.Shots;

namespace Template
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Spelet är gjort med upplösningen 800x480 

        

        Texture2D pixel;
        Texture2D meeleminion;
        Texture2D poro;
        Texture2D turret;
        Texture2D shot;

        //ritar upp banan
        Rectangle Road1 = new Rectangle(0, 60, 445, 60);        
        Rectangle Road2 = new Rectangle(385, 60, 61, 160);
        Rectangle Road3 = new Rectangle(385, 220, 225, 60);
        Rectangle Road4 = new Rectangle(550, 60, 60, 220);
        Rectangle Road5 = new Rectangle(550, 60, 190, 60);
        Rectangle Road6 = new Rectangle(680, 60, 60, 360);
        Rectangle Road7 = new Rectangle(0, 360, 740, 60);


        int ME_x = 300;
        int ME_y = 200;

        int TurretCD = 0;       //hindrar mig skapa flera torn samtidigt

        int Time = 0;       //tid för att räkna hur ofta minions ska spawna 



        //få distansen mellan två punkter
        private static double GetDistance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }


        private static double GetXComparedToY(float x1, float y1, float x2, float y2)
        {
            return (x1 - x2)/(y1 - y2);
        }



        //KOmentar




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }




        //metoder för att skapa fiender och torn  +  lista till de respektiva klasserna
        List<Minion> minions = new List<Minion>();
        public void SpawnMinion(Vector2 position, Vector2 speed, int hp, float traveled)
        {
            Minion Minion1 = new Minion(position, speed, hp, traveled);
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

        List<Shot> shots = new List<Shot>();
        public void SpawnShot(Vector2 position, Vector2 speed, float traveled, float to_target, List<Minion> target)
        {
            Shot shot1 = new Shot(position, speed, traveled, to_target, target);
            shots.Add(shot1);
            shots.Count();
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
            shot = Content.Load<Texture2D>("Shot");

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
            Time += 1;      //ökar variabeln som skapar fiender

            TurretCD++;     //ökar variabeln som hindrar mig skapa flera torn


            //skapar en fiende var 100 enhet
            if ((Time / (Enemys + 1)) == 100)
            {
                SpawnMinion(new Vector2(0, 60), new Vector2(2, 0), 1, 0);             //kallar på metoden som skapar en "Minion"(fiende)
                Enemys++;                               //ökar antalet enemies för att veta hur många "Time" ska delas med.
            }



            //förflytta spelarens karaktär
            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.W))
                ME_y = ME_y - 3;
            if (kstate.IsKeyDown(Keys.S))
                ME_y = ME_y + 3;
            if (kstate.IsKeyDown(Keys.D))
                ME_x = ME_x + 3;
            if (kstate.IsKeyDown(Keys.A))
                ME_x = ME_x - 3;

            //skapar ett torn där spelaren står
            if (kstate.IsKeyDown(Keys.Space) && TurretCD > 40)
            {
                TurretCD = 0;           //Kan bara skapa ett torn var 40'onde enhet. Här blir enheten 0 igen.
                SpawnTurret(ME_x + 40, ME_y, 100, 0, 200, new List<Minion>(), new List<Minion>());          //kallar på metoden som skapar ett torn.
            }


            //går igenom varje torn så att de ska:   antingen skjuta rätt fiende   eller  göra sig redo för att skjuta
            foreach (Turret a in turrets)    //a = ally
            {
                float current_highest_traveled = 0;     //skapar variabel som sparar "highest traveled" bland de fiender inom räckvidd

                a.attack_Wind_UP++;     //får tornet redo att attackera. Just nu behöver denna bli 100.

                //får tornet att attackera ifall den får det
                if (a.attack_Wind_UP > a.attack_speed)     //e = enemy
                {

                    a.attack_Wind_UP = 0;
                    //a.minions_in_range.Clear();  ?? detta förstörde allt
                    //kollar distansen till varje fiende
                    foreach (Minion e in minions)
                    {
                        if (GetDistance(e.Position.X, e.Position.Y, a.x_position, a.y_position) <= a.attack_range)
                            a.minions_in_range.Add(e);
                    }

                    //kollar vilken fiende som är inom räckvidd
                    if (a.minions_in_range != null)
                    foreach (Minion e in a.minions_in_range)
                    {
                       if (e.units_traveled > current_highest_traveled)
                       {
                            a.list_target.Clear();
                            current_highest_traveled = e.units_traveled;    //Den sparar vilket värde dem som färdats längst inom räckhåll har färdats.
                            a.list_target.Add(e);
                       }
                    }

                    //skadar fienden som har samma position som den sparade variabeln "highest traveled"
                    for (int e = minions.Count - 1; e >= 0; e--)
                    {
                        if (minions[e].units_traveled == current_highest_traveled)
                        {
                            minions[e].health--;

                            SpawnShot(new Vector2(a.x_position, a.y_position), new Vector2(a.x_position, a.y_position) - minions[e].Position, 0, (a.x_position - (minions[e].Position.X) + (a.y_position - minions[e].Position.Y)), new List<Minion>(e));
                            
                        
                        }
                    }
                    /*foreach (Minion e in minions)
                    {
                        if (e.units_traveled == current_highest_traveled)
                        {
                            e.health--;

                            SpawnShot(new Vector2(a.x_position, a.y_position), (new Vector2(a.x_position, a.y_position) - e.Position).Normalize, (a.x_position - (e.Position.X) + (a.y_position - e.Position.Y)), new List<Minion>(e));


                        }
                    }*/
                }
            }

            //Vector2 dir = player.Position - enemy.Position;
            //dir.Normalize();
            //enemy.Position += dir * speed;




            foreach (Shot s in shots)           //s = shots
            {
                s.Speed.Normalize();
                s.Position += s.Speed * 10;
            }

            if (shots.Count > 0)
            for (int i = shots.Count - 1; i >= 0; i--)
            {
                    shots[i].units_traveled = shots[i].units_traveled + shots[i].Speed.X + shots[i].Speed.Y;
                if (shots[i].units_traveled >= shots[i].units_to_target)
                {
                    shots.RemoveAt(i);        //tar bort "shot" när den är vid sin destination.
                }
            }



            //om en fiende får 0 eller mindre i hälso så försvinner den
            for (int i = minions.Count - 1; i >= 0; i--)
            {
                if (minions[i].health <= 0)
                {
                    minions.RemoveAt(i);        //tar bort fiende när den har 0 i liv.
                }
            }



            //flyttar varje fiende
            foreach (Minion e in minions)
            {
                //e.Position.X = e.x_position + e.x_speed;
                //e.y_position = e.y_position + e.y_speed;
                //e.units_traveled = e.units_traveled + e.x_speed + e.y_speed;

                e.Position += e.Speed;
            }
             
            //fiende ändrar riktning så de följer banan
            foreach (Minion e in minions)
            {
                if (e.Position.X == 386 && e.Position.Y == 60)
                {
                    e.Speed.X = 0;
                    e.Speed.Y = 2;
                }

                if (e.Position.X == 386 && e.Position.Y == 220)
                {
                    e.Speed.X = 2;
                    e.Speed.Y = 0;
                }

                if (e.Position.X == 550 && e.Position.Y == 220)
                {
                    e.Speed.X = 0;
                    e.Speed.Y = -2;
                }

                if (e.Position.X == 550 && e.Position.Y == 60)
                {
                    e.Speed.X = 2;
                    e.Speed.Y = 0;
                }

                if (e.Position.X == 680 && e.Position.Y == 60)
                {
                    e.Speed.X = 0;
                    e.Speed.Y = 2;
                }

                if (e.Position.X == 680 && e.Position.Y == 360)
                {
                    e.Speed.X = -2;
                    e.Speed.Y = 0;
                }
            }

            /*foreach (Minion e in minions)
            {
                if (e.Health == 0 || e.x_position == 300)               tänkt att senare lägga till så att dem försvinner när fiende går utanför banan
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

            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);    //ändrat så att färger på en sprite kan vara "transparent"

            //skapar grafiskt banan
            spriteBatch.Draw(pixel, Road1, Color.Black);
            spriteBatch.Draw(pixel, Road2, Color.Black);
            spriteBatch.Draw(pixel, Road3, Color.Black);
            spriteBatch.Draw(pixel, Road4, Color.Black);
            spriteBatch.Draw(pixel, Road5, Color.Black);
            spriteBatch.Draw(pixel, Road6, Color.Black);
            spriteBatch.Draw(pixel, Road7, Color.Black);

            //ritar varje fiende
            foreach (Minion e in minions)
            {
                spriteBatch.Draw(meeleminion, new Vector2(e.Position.X, e.Position.Y), Color.White);
            }

            //ritar varje torn
            foreach (Turret e in turrets)
            {
                spriteBatch.Draw(turret, new Vector2(e.x_position, e.y_position), Color.White);
            }

            //ritar spelaren karaktär
            spriteBatch.Draw(poro, new Vector2(ME_x, ME_y), Color.White);           // "/processorParam:ColorKeyColor = 119,197,213,255"  denna raden finns i content. Detta gör så att fyrkanten 
                                                                                    // runt inte syns. 
            foreach (Shot e in shots)
            {
                spriteBatch.Draw(shot, new Vector2(e.Position.X, e.Position.Y), Color.White);
            }

            spriteBatch.End();


            // TODO: Add your drawing code here.

            base.Draw(gameTime);
        }
    }
}
