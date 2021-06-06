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
        Rectangle Road2 = new Rectangle(384, 60, 61, 160);
        Rectangle Road3 = new Rectangle(384, 216, 224, 60);
        Rectangle Road4 = new Rectangle(552, 60, 60, 216);
        Rectangle Road5 = new Rectangle(552, 60, 188, 60);
        Rectangle Road6 = new Rectangle(684, 60, 60, 360);
        Rectangle Road7 = new Rectangle(0, 360, 740, 60);


        int ME_x = 300;
        int ME_y = 200;

        int TurretCD = 0;       //hindrar mig skapa flera torn samtidigt

        int Time = 0;       //tid för att räkna hur ofta minions ska spawna 

        int shotspeed = 20;

        float player_max_hp = 4;
        float player_current_hp = 4;      
        float ofmaxhp = 1;
        int realofmaxhp;        //ofmaxhp fast den som används efter att gjort den till en int.

        private SpriteFont money;
        private int money1 = 3;

        private SpriteFont enemys;

        int turret_cost = 3;
        int turret_cost_increase = 2;       //ökning av kostnad varje gång ett torn görs.

        int Enemys = 0;

        int enemy_hp = 2;
        int enemy_speed = 2;

        int enemys_frequency = 100;         //hur ofta en fiende ska skapas


        //få distansen mellan två punkter
        private static double GetDistance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }



        //KOmentar

        float current_highest_traveled = -1;     //skapar variabel som sparar "highest traveled" bland de fiender inom räckvidd.


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }




        //metoder för att skapa fiender, torn och skott  +  lista till de respektiva klasserna
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
        public void SpawnShot(Vector2 position, Vector2 direction, float traveled, float to_target, List<Minion> target)
        {
            Shot shot1 = new Shot(position, direction, traveled, to_target, target);
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
            money = Content.Load<SpriteFont>("Money"); 
            enemys = Content.Load<SpriteFont>("Money");

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
        
        protected override void Update(GameTime gameTime)
        {
            Time += 1;      //ökar variabeln som skapar fiender

            TurretCD++;     //ökar variabeln som hindrar mig skapa flera torn


            //skapar en fiende var 100 enhet
            if (Time == enemys_frequency)
            {
                SpawnMinion(new Vector2(-60, 60), new Vector2(enemy_speed, 0), enemy_hp, 0);             //kallar på metoden som skapar en "Minion"(fiende)
                Enemys++;                               //ökar antalet enemies för att veta hur många "Time" ska delas med.
                Time = 0;
            }

            ofmaxhp = player_current_hp / player_max_hp;        //ofmaxhp är procent av max hp som spelaren just nu har. Kan vara mellan 0 och 1.

            realofmaxhp = Convert.ToInt32(ofmaxhp*800);         //gör om ofmaxhp till int. Multiplicerar även 800 för att täcka hela x-axeln.



            if (player_current_hp <= 0)         
            {
                Exit();
            }



            //gör spelet svårare genom: hastighet, hälsa och frekvens av fiender
            if (Enemys == 20)
            {
                enemy_hp = 3;
            }

            if (Enemys == 30)
            {
                turret_cost_increase = 3;
            }

            if (Enemys == 60)
            {
                enemy_speed = 3;
                turret_cost_increase = 4;
                enemys_frequency = 70;
            }

            if (Enemys == 70)
            {
                turret_cost_increase = 5;
            }

            if (Enemys == 100)
            {
                enemy_hp = 5;
                enemy_speed = 4;
                turret_cost_increase = 6;
            }

            if (Enemys == 150)
            {
                enemys_frequency = 50;
                turret_cost_increase = 10;
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
            if (kstate.IsKeyDown(Keys.Space) && TurretCD > 40 && money1 >= turret_cost)
            {
                TurretCD = 0;           //Kan bara skapa ett torn var 40'onde enhet. Här blir enheten 0 igen.
                SpawnTurret(ME_x + 40, ME_y, 100, 0, 200, new List<Minion>(), new List<Minion>());          //kallar på metoden som skapar ett torn.
                
                //ändrar pengar
                money1 = money1 - turret_cost;
                turret_cost = turret_cost + turret_cost_increase;
            }

            


            //går igenom varje torn så att de ska:   antingen skjuta rätt fiende   eller  göra sig redo för att skjuta
            foreach (Turret a in turrets)    //a = ally
            {
                current_highest_traveled = -1;     //minskar variabel som sparar "highest traveled" bland de fiender inom räckvidd till -1.

                a.attack_Wind_UP++;     //får tornet redo att attackera. Just nu behöver denna bli 100(attack speed).

                    
                if (a.attack_Wind_UP > a.attack_speed)     //får tornet att attackera ifall den får det  
                {

                    a.attack_Wind_UP = 0;

                    a.minions_in_range.Clear();

                    //kollar distansen till varje fiende
                    foreach (Minion e in minions)   //e = enemy
                    {
                        if (GetDistance(a.x_position, a.y_position, e.Position.X + 30, e.Position.Y + 25) <= a.attack_range)
                            a.minions_in_range.Add(e);      //lägger till dem till variabeln om dem är in-range
                    }

                    //kollar vilken fiende in-range som har färdats längst
                    if (a.minions_in_range != null)
                    foreach (Minion e in a.minions_in_range)    
                    {
                       if (e.units_traveled > current_highest_traveled)
                       {
                            a.list_target.Clear();
                            current_highest_traveled = e.units_traveled;    //Den sparar vilket värde som färdats längst inom räckhåll.
                            a.list_target.Add(e);
                       }
                    }

                    //skadar fienden som har samma position som den sparade variabeln "highest traveled"
                    for (int e = minions.Count - 1; e >= 0; e--)
                    {
                        if (minions[e].units_traveled == current_highest_traveled)
                        {
                            //skapar ett grafiskt skott som bara är för syns skull.
                            SpawnShot(new Vector2(a.x_position, a.y_position), minions[e].Position - new Vector2(a.x_position, a.y_position), 0, ((minions[e].Position.X + 30) - (a.x_position) + ((minions[e].Position.Y + 25) - a.y_position)), new List<Minion>(e));
                            
                            minions[e].health--;
                         
                        }
                    }
                }
            }



            //förflyttar varje skott beroende på dess riktning och hastighet.
            foreach (Shot s in shots)           //s = shots
            {
                s.Direction.Normalize();
                s.Position += (s.Direction * shotspeed);
            }

            //ökar varje skotts "units_traveled", och tar bort skottet ifall den nått sin destination.
            if (shots.Count > 0)
            for (int i = shots.Count - 1; i >= 0; i--)
            {
                    shots[i].units_traveled = (shots[i].units_traveled) + (shots[i].Direction.X * shotspeed) + (shots[i].Direction.Y * shotspeed);      //ökar "units traveled"
                if (shots[i].units_traveled >= shots[i].units_to_target)
                {
                    
                    shots.RemoveAt(i);        //tar bort "shot".
                }
            }



            //om en fiende får 0 eller mindre i hälso så försvinner den, och man får pengar.
            for (int i = minions.Count - 1; i >= 0; i--)
            {
                if (minions[i].health <= 0)
                {
                    minions.RemoveAt(i);            //tar bort fiende.
                    money1++;                   //man får pengar
                }
            }



            //flyttar varje fiende(minion), och ökar dess "units_traveled"
            foreach (Minion e in minions)
            {
                e.units_traveled = e.units_traveled + e.Speed.X + e.Speed.Y;        //units_traveled

                e.Position += e.Speed;      //position
            }
             

            //fiende ändrar riktning så de följer banan
            foreach (Minion e in minions)
            {
                if (e.Position.X == 384 && e.Position.Y == 60)
                {
                    e.Speed.X = 0;
                    e.Speed.Y = enemy_speed;
                }

                if (e.Position.X == 384 && e.Position.Y == 216)
                {
                    e.Speed.X = enemy_speed;
                    e.Speed.Y = 0;
                }

                if (e.Position.X == 552 && e.Position.Y == 216)
                {
                    e.Speed.X = 0;
                    e.Speed.Y = -enemy_speed;
                }

                if (e.Position.X == 552 && e.Position.Y == 60)
                {
                    e.Speed.X = enemy_speed;
                    e.Speed.Y = 0;
                }

                if (e.Position.X == 684 && e.Position.Y == 60)
                {
                    e.Speed.X = 0;
                    e.Speed.Y = enemy_speed;
                }

                if (e.Position.X == 684 && e.Position.Y == 360)
                {
                    e.Speed.X = -enemy_speed;
                    e.Speed.Y = 0;
                }
            }

            //om en fiende passerar dess mål förlorar spelaren liv och fienden försvinner.
            for (int e = minions.Count - 1; e >= 0; e--)
            {
                if (minions[e].Position.X <= -61)
                {
                    minions.RemoveAt(e);
                    player_current_hp--;        
                }
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

            //ritar varje skott
            foreach (Shot e in shots)
            {
                spriteBatch.Draw(shot, new Vector2(e.Position.X, e.Position.Y), Color.White);
            }

            
            //ritar "health bar", spelarens liv.
            spriteBatch.Draw(pixel, new Rectangle(0, 450, realofmaxhp, 30), Color.GreenYellow);

            //ritar texten som visar mängd pengar
            spriteBatch.DrawString(money, "Money: "+ money1 + "  (turret cost: " + turret_cost + ")", new Vector2(50, 20), Color.Black);

            //ritar texten som visar mängd Enemys som spawnats, även detta som är tänkt vara ditt "Score".
            spriteBatch.DrawString(enemys, "Enemys spawned: " + Enemys + "  (Score)", new Vector2(550   , 20), Color.Black);


            spriteBatch.End();


            // TODO: Add your drawing code here.

            base.Draw(gameTime);
        }
    }
}
