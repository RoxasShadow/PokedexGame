using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Net;
using System.Collections;

namespace Pokédex {
    static class RandomExtensions {
        public static void Shuffle<T>(this Random rng, T[] array) {
            int n = array.Length;
            while(n > 1) {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }

    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldState, newState;
        Player player;
        List<Pokemon> pokemon;
        Ball ball;
        Texture2D background, texture;
        Microsoft.Xna.Framework.Color[] color;
        Screen screen;
        Vector2 playerSpeed;
        float pokemonSpeed;
        float ballSpeed;
        bool pause = false;
        Vector2 screenCenter;
        Pokemon target1, target2;
        List<SoundManager> sound;
        bool loading;
        SpriteFont font, smallFont;
        Texture2D[] soundIcon;
        bool soundActive;
        int spriteNum;
        int points;
        string text;
        int currentSound;
        Vector2 soundIconPosition;
        FpsMonitor fps;
        float shotDelay, shotIntervalCounter;
        Hashtable dictionary;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Utils.GetCurrentDirectory() + "\\Data\\";

            screen = new Screen(600, 470);
            graphics.PreferredBackBufferWidth = screen.Width;
            graphics.PreferredBackBufferHeight = screen.Height;

            fps = new FpsMonitor(false);
            screenCenter = new Vector2(screen.Width, screen.Height) / 2;
            loading = true;
            playerSpeed = new Vector2(0.75f, 0.75f);
            pokemonSpeed = 3.5f;
            ballSpeed = 8f;
            spriteNum = 656;
            points = 0;
            currentSound = 0;
            shotDelay = 2000.0f;
            soundActive = true;
            dictionary = Strings.GetString();
        }

        protected override void Initialize() {
            base.Initialize();
        }

        private Texture2D LoadTexture(string file) {
            return Utils.LoadTexture(GraphicsDevice, Content.RootDirectory + file + ".png");
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font\\SpriteFont1");
            font.DefaultCharacter = '*';
            smallFont = Content.Load<SpriteFont>("Font\\SpriteFont2");
            smallFont.DefaultCharacter = '*';

            background = LoadTexture("Background");
            soundIcon = new Texture2D[2] { LoadTexture("SoundOff"), LoadTexture("SoundOn") };
            soundIconPosition = new Vector2(0, screen.Height - soundIcon[0].Height);
            pokemon = new List<Pokemon>();
            player = new Player(LoadTexture("Trainer"), new Vector2(screen.Width / 2, screen.Height), playerSpeed, new Vector2(0.9f, 0.9f), this);
            ball = new Ball(LoadTexture("Ball"), new Vector2(player.Position.X + player.Texture.Width / 2, player.Position.Y - player.Texture.Height), ballSpeed, false, true, this);
            ball.SetClockwise(true);
            ball.SetRotationSpeed(45f);

            sound = new List<SoundManager>();
            sound.Add(new SoundManager(SoundManager.Sound.Main, true));
            sound.Add(new SoundManager(SoundManager.Sound.Shot, false));
            sound.Add(new SoundManager(SoundManager.Sound.Denied, false));
            sound.Add(new SoundManager(SoundManager.Sound.Main2, false));
            sound.Add(new SoundManager(SoundManager.Sound.Zelda, false));
            sound[0].soundInstance.Volume = 0.3f;
            sound[1].soundInstance.Volume = 0.1f;
            sound[2].soundInstance.Volume = 0.3f;
            sound[3].soundInstance.Volume = 1.0f;
            sound[4].soundInstance.Volume = 0.3f;

            LoadPokemon();
        }

        private void LoadPokemon() {
            float first = screen.Width, x;
            int[] rand = Utils.ListOfInt(spriteNum, 0, spriteNum);
            for(int i = 0; i < spriteNum; ++i) {
                texture = LoadTexture("Sprite\\" + rand[i]);
                x = first;
                first += texture.Width;
                pokemon.Add(new Pokemon(rand[i], texture, new Vector2(x, 55.0f), pokemonSpeed, true, this));
            }
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime) {
            newState = Keyboard.GetState();
            fps.Run = !pause && !loading; 

            if((newState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape)) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if((newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter)) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                loading = false;
            if(newState.IsKeyDown(Keys.P) && !oldState.IsKeyDown(Keys.P))
                loading = true;
            if(newState.IsKeyDown(Keys.I) && !oldState.IsKeyDown(Keys.I)) {
                Utils.Language = RNGReporter.Language.Italian;
                dictionary = Strings.GetString();
            }
            if(newState.IsKeyDown(Keys.E) && !oldState.IsKeyDown(Keys.E)) {
                Utils.Language = RNGReporter.Language.English;
                dictionary = Strings.GetString();
            }

            
            if(!loading) {
                if(!pause) {
                    if((newState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space)) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) {
                        if(!ball.Visible) {
                            ball.Visible = true;
                            ball.Position = new Vector2(player.Position.X + player.Texture.Width / 2, player.Position.Y - player.Texture.Height);
                            if(soundActive)
                                sound[1].soundInstance.Play();
                        }
                        else
                            if(soundActive)
                                sound[2].soundInstance.Play();
                    }
                    ball.Update(gameTime);

                    foreach(Pokemon p in pokemon)
                        p.Update(gameTime);
                    bool end = true;
                    foreach(Pokemon p in pokemon)
                        if(p.Position.X > 0)
                            end = false;
                    if(end)
                        LoadPokemon();
                    if(ball.Position.Y < 0)
                        ball.Visible = false;
                    foreach(Pokemon p in pokemon) {
                        if(ball.Visible && ball.Intersect(p)) {
                            ball.Visible = false;
                            pause = true;
                            p.Position.X = player.Position.X + player.Texture.Width;
                            p.Position.Y = player.Position.Y - (p.Texture.Height / 2);
                            p.Rotate = false;
                            if(target1 == null)
                                target1 = p;
                            else {
                                target2 = target1;
                                target1 = p;
                            }
                            if(target1 == null || target2 == null)
                                points += 50;
                            else {
                                bool similar = false;
                                if(target1.dex[target1.Num].Ability0 == target2.dex[target2.Num].Ability0) {
                                    points += 75;
                                    similar = true;
                                }
                                if(target1.dex[target1.Num].Ability1 == target2.dex[target2.Num].Ability1) {
                                    points += 75;
                                    similar = true;
                                }
                                if(target1.dex[target1.Num].Ability0 == target2.dex[target2.Num].Ability1) {
                                    points += 65;
                                    similar = true;
                                }
                                if(target1.dex[target1.Num].Ability1 == target2.dex[target2.Num].Ability0) {
                                    points += 65;
                                    similar = true;
                                }
                                if(!similar)
                                    points += 50;
                            }
                            if(points > 500) {
                                currentSound = 3;
                                sound[0].soundInstance.Stop();
                                sound[3].soundInstance.Play();
                            }
                        }
                    }

                    player.Update(pokemon, newState, screen, gameTime);
                }
                else
                    if((newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.E)) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed) {
                        pause = false;
                        pokemon.Remove(target1);
                        pokemon.Remove(target2);
                    }
            }

            if(newState.IsKeyDown(Keys.OemMinus) && !oldState.IsKeyDown(Keys.OemMinus))
                if(sound[currentSound].soundInstance.Volume > 0.1f)
                    sound[currentSound].soundInstance.Volume -= 0.1f;
            if(newState.IsKeyDown(Keys.OemPlus) && !oldState.IsKeyDown(Keys.OemPlus))
                if(sound[currentSound].soundInstance.Volume < 0.9f)
                    sound[currentSound].soundInstance.Volume += 0.1f;

            if(newState.IsKeyDown(Keys.LeftAlt) && !oldState.IsKeyDown(Keys.LeftAlt))
                soundActive = !soundActive;
            if(soundActive)
                sound[currentSound].soundInstance.Resume();
            else
                sound[currentSound].soundInstance.Pause();
            oldState = newState;

            fps.Update(gameTime);
            base.Update(gameTime);
        }

        // Draw a background repeating the tile
        private void DrawBackground(Vector2 scrollOffset) {
            int tileX = (int)scrollOffset.X % background.Width;
            int tileY = (int)scrollOffset.Y % background.Height;

            if(tileX > 0)
                tileX -= background.Width;
            if(tileY > 0)
                tileY -= background.Height;

            for(int x = tileX; x < screen.Width; x += background.Width)
                for(int y = tileY; y < screen.Height; y += background.Height)
                    spriteBatch.Draw(background, new Vector2(x, y), Color.White);
        }

        private void DrawBackground() {
            spriteBatch.Draw(background, new Vector2(-5, -8), Color.White);
        }

        private Texture2D CreateRectangle(int width, int height, Color c) {
            texture = new Texture2D(GraphicsDevice, width, height);
            color = new Color[width * height];

            for(int i = 0, size = color.Length; i < size; i++)
                color[i] = new Color(c.R, c.G, c.B, c.A);
            texture.SetData(color);
            return texture;
        }

        public static List<string> strings;

        protected override void Draw(GameTime gameTime) {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();


            //DrawBackground(screenCenter);
            DrawBackground();
            player.Draw(spriteBatch);
            foreach(Pokemon p in pokemon)
                p.Draw(spriteBatch);
            if(ball.Visible)
                ball.Draw(spriteBatch);

            if(pause) {
                texture = CreateRectangle((int)smallFont.MeasureString(text).X*2, (int)smallFont.MeasureString(text).Y * 5, Color.Red);
                spriteBatch.Draw(texture, screen.Vector / 2, Color.White);
                if(target1 != null)
                    text = target1.dex[target1.Num].Dex(dictionary);
                else if(target2 != null)
                    text = target2.dex[target2.Num].Dex(dictionary);
                spriteBatch.DrawString(smallFont, text, screen.Vector / 2, Color.White);
            }
            if(loading) {
                text = (string)dictionary["intro"];
                texture = CreateRectangle(screen.Width, screen.Height, Color.Black);
                spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
                Vector2 x = new Vector2(0, screen.Vector.Y / 2); 
                spriteBatch.DrawString(font, text, x, Color.White);
            }

            if(target1 != null)
                text = dictionary["last"] + ": " + target1.dex[target1.Num].ToString() + "\n" + dictionary["points"] + ": " + points;
            else if(target2 != null)
                text = dictionary["last"] + ": " + target2.dex[target2.Num].ToString() + "\n" + dictionary["points"] + ": " + points;
            else
                text = dictionary["points"] + ": " + points;
            spriteBatch.Draw(soundIcon[Convert.ToInt16(soundActive)], soundIconPosition, Color.White);
            spriteBatch.DrawString(font, text, soundIconPosition - font.MeasureString(text), Color.Yellow);
            fps.Draw(spriteBatch, smallFont, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, text, screen.Vector - font.MeasureString(text), Color.Yellow);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}