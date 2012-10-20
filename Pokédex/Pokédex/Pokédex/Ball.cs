using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Pokédex {
    class Ball : Sprite {
        public float Speed;

        public Ball(Texture2D Texture, Vector2 Position, float Speed, bool Visible, bool Rotate, Game game)
            : base(Texture, Position, Rotate, game) {
            this.Speed = Speed;
            this.Visible = Visible;
        }

        public Ball(Texture2D Texture, Vector2 Position, float Speed, bool Visible, Game game)
            : base(Texture, Position, game) {
            this.Speed = Speed;
            this.Visible = Visible;
        }

        public override void Update(GameTime gameTime) {
            if(Visible)
                Position.Y -= Speed;
        }
    }
}
