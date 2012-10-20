using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pokédex {
    public class FpsMonitor {
        public float Value { get; private set; }
        public TimeSpan Sample { get; set; }
        private Stopwatch sw;
        private int Frames;

        private long Seconds;
        private long Milliseconds = 0;
        private float Interval = 1000.0f;

        public bool Run;

        public FpsMonitor(bool Run) {
            this.Run = Run;
            Sample = TimeSpan.FromSeconds(1);
            Value = 0;
            Frames = 0;
            sw = Stopwatch.StartNew();
        }

        public void Update(GameTime gameTime) {
            if(sw.Elapsed > Sample) {
                Value = (float)(Frames / sw.Elapsed.TotalSeconds);
                sw.Reset();
                sw.Start();
                Frames = 0;
            }
            if(Run) {
                Milliseconds += (long)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(Milliseconds >= Interval) {
                    ++Seconds;
                    Milliseconds = 0;
                }
            }
        }

        public void Draw(SpriteBatch SpriteBatch, SpriteFont Font, Vector2 Location, Color Color) {
            ++Frames;
            SpriteBatch.DrawString(Font, ((int)Value).ToString() + " | " + (Seconds.ToString() + "." + Milliseconds.ToString()), Location, Color);
        }
    }
}