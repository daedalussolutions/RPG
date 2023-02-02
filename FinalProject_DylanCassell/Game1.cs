using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;
using FinalProject_DylanCassell.Enemies;
using FinalProject_DylanCassell.Buttons;
using FinalProject_DylanCassell.Screens;

namespace FinalProject_DylanCassell
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spritefont;
        private Player player;
        private Slime slime;
        private Dragon dragon;
        private Skeleton skeleton;
        private PlayerStats playerStats;
        private EnemyStats enemyStats;
        private int enemyMaxHealth;
        private SwordButton swordButton;
        private MagicButton magicButton;
        private BlockButton blockButton;
        private WaitButton waitButton;
        private Vector2 stage;
        private Level level = Level.Level1;
        private VictoryScreen victoryScreen;
        private GameOverScreen gameOverScreen;
        private StartScreen startScreen;
        Random attackModifier = new Random();
        Rectangle swordRectangle;
        Rectangle magicRectangle;
        Rectangle blockRectangle;
        Rectangle waitRectangle;
        MouseState ms;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 920;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spritefont = Content.Load<SpriteFont>("fonts/GameFont");
            // TODO: use this.Content to load your game content here

            stage = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Vector2 playerPosition = new Vector2(stage.X / 2 - 400, stage.Y / 2 - 100);
            Vector2 enemyPosition = new Vector2(stage.X / 2 + 150, stage.Y / 2 - 100);

            Texture2D victoryTexture = this.Content.Load<Texture2D>("sprites/background");
            victoryScreen = new VictoryScreen(this, _spriteBatch, victoryTexture, new Vector2(stage.X - 1340, stage.Y - 1150));

            Texture2D startTexture = this.Content.Load<Texture2D>("sprites/Start");
            startScreen = new StartScreen(this, _spriteBatch, startTexture, new Vector2(stage.X - 1350, stage.Y - 1150));

            Texture2D gameOverTexture = this.Content.Load<Texture2D>("sprites/GameOver");
            gameOverScreen = new GameOverScreen(this, _spriteBatch, gameOverTexture, new Vector2(stage.X - 1340, stage.Y - 1150));

            Texture2D rainTexture = this.Content.Load<Texture2D>("sprites/rain");
            Rectangle srcRect = new Rectangle(0, 0, rainTexture.Width, rainTexture.Height);
            Rain rain = new Rain(this, _spriteBatch, rainTexture, new Vector2(0, stage.Y - srcRect.Width), srcRect, new Vector2(0, -2));

            Texture2D playerTexture = this.Content.Load<Texture2D>("sprites/Player");
            player = new Player(this, _spriteBatch, playerTexture, playerPosition);

            Texture2D slimeTexture = this.Content.Load<Texture2D>("sprites/Slime");
            slime = new Slime(this, _spriteBatch, slimeTexture, enemyPosition);

            Texture2D skeletonTexture = this.Content.Load<Texture2D>("sprites/Skeleton");
            skeleton = new Skeleton(this, _spriteBatch, skeletonTexture, new Vector2(enemyPosition.X, enemyPosition.Y - 200));
            skeleton.Visible = false;

            Texture2D dragonTexture = this.Content.Load<Texture2D>("sprites/Dragon");
            dragon = new Dragon(this, _spriteBatch, dragonTexture, new Vector2(enemyPosition.X - 200, enemyPosition.Y - 300));
            dragon.Visible = false;

            Texture2D swordTexture = this.Content.Load<Texture2D>("sprites/Sword");
            swordButton = new SwordButton(this, _spriteBatch, swordTexture, new Vector2(stage.X / 2 - 450, stage.Y / 2 + 300));
            Texture2D magicTexture = this.Content.Load<Texture2D>("sprites/Magic");
            magicButton = new MagicButton(this, _spriteBatch, magicTexture, new Vector2(stage.X / 2 - 230, stage.Y / 2 + 300));
            Texture2D blockTexture = this.Content.Load<Texture2D>("sprites/Block");
            blockButton = new BlockButton(this, _spriteBatch, blockTexture, new Vector2(stage.X / 2, stage.Y / 2 + 300));
            Texture2D waitTexture = this.Content.Load<Texture2D>("sprites/Wait");
            waitButton = new WaitButton(this, _spriteBatch, waitTexture, new Vector2(stage.X / 2 + 230, stage.Y / 2 + 300));
            
            
            this.Components.Add(player);
            this.Components.Add(slime);
            this.Components.Add(skeleton);
            this.Components.Add(dragon);
            this.Components.Add(rain);
            this.Components.Add(swordButton);
            this.Components.Add(magicButton);
            this.Components.Add(blockButton);
            this.Components.Add(waitButton);
            this.Components.Add(victoryScreen);
            victoryScreen.Visible = false;
            this.Components.Add(gameOverScreen);
            gameOverScreen.Visible = false;
            this.Components.Add(startScreen);
            
            playerStats = new PlayerStats(this,"Warrior", 780, 90);
            this.Components.Add(playerStats);

            enemyStats = new EnemyStats(this, 400);
            this.Components.Add(enemyStats);
            enemyMaxHealth = 400;

            swordRectangle = swordButton.getBounds();
            magicRectangle = magicButton.getBounds();
            blockRectangle = blockButton.getBounds();
            waitRectangle = waitButton.getBounds();

        }

        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            SoundEffect slash = Content.Load<SoundEffect>("sounds/slash");
            SoundEffect magic = Content.Load<SoundEffect>("sounds/magic");
            SoundEffect hit = Content.Load<SoundEffect>("sounds/hit");
            MouseState previousMs = ms;
            ms = Mouse.GetState();

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Enter))
            {
                startScreen.Visible = false;
            }
            if (swordRectangle.Contains(ms.Position) && ms.LeftButton == ButtonState.Released && previousMs.LeftButton == ButtonState.Pressed)
            { 
                enemyStats.HealthPoints -= attackModifier.Next(40, 70);
                playerStats.HealthPoints -= attackModifier.Next(3, 50);
                slash.Play();
                hit.Play();
                

            }
            if (magicRectangle.Contains(ms.Position) && ms.LeftButton == ButtonState.Released && previousMs.LeftButton == ButtonState.Pressed)
            {
                if(playerStats.MagicPoints >= 5)
                {
                    enemyStats.HealthPoints -= attackModifier.Next(70, 120);
                    playerStats.MagicPoints -= 5;
                    playerStats.HealthPoints -= attackModifier.Next(3, 50);
                    magic.Play();
                    hit.Play();
                }
                
            }
            if (blockRectangle.Contains(ms.Position) && ms.LeftButton == ButtonState.Released && previousMs.LeftButton == ButtonState.Pressed)
            {
                enemyStats.HealthPoints -= attackModifier.Next(3, 10);
                playerStats.HealthPoints -= attackModifier.Next(3, 25);
                hit.Play();
            }
            if (waitRectangle.Contains(ms.Position) && ms.LeftButton == ButtonState.Released && previousMs.LeftButton == ButtonState.Pressed)
            {
                if (playerStats.MagicPoints <= 83)
                {
                    playerStats.MagicPoints += 7;
                    playerStats.HealthPoints -= attackModifier.Next(3, 50);
                    hit.Play();
                }
                else
                {
                    playerStats.HealthPoints -= attackModifier.Next(3, 50);
                    hit.Play();
                }
            }
            if(enemyStats.HealthPoints <= 0)
            {
                if(level == Level.Level1)
                {
                    level = Level.Level2;
                    slime.Visible = false;
                    skeleton.Visible = true;
                    enemyStats.HealthPoints = 680;
                    playerStats.HealthPoints += 80;
                    enemyMaxHealth = 680;
                }
                else if(level == Level.Level2)
                {
                    level = Level.Level3;
                    skeleton.Visible = false;
                    dragon.Visible = true;
                    dragon.Visible = true;
                    enemyStats.HealthPoints = 999;
                    playerStats.HealthPoints += 80;
                    enemyMaxHealth = 999;
                }else if(level == Level.Level3)
                {
                    victoryScreen.Visible = true;
                }
                
            }
            if(playerStats.HealthPoints <= 0)
            {
                gameOverScreen.Visible = true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here


            _spriteBatch.Begin();
            _spriteBatch.DrawString(_spritefont, playerStats.PlayerName, new Vector2(1, 1), Color.White);
            _spriteBatch.DrawString(_spritefont, "HP: " + playerStats.HealthPoints + "/780", new Vector2(190, 310), Color.Red);
            _spriteBatch.DrawString(_spritefont, "MP: " + playerStats.MagicPoints + "/90", new Vector2(190, 350), Color.Blue);
            _spriteBatch.DrawString(_spritefont, "HP: " + enemyStats.HealthPoints + "/" + enemyMaxHealth, new Vector2(700, 600), Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}