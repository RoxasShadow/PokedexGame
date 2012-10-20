using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pokédex {
    public class Sprite : DrawableGameComponent {
        public Texture2D Texture;
        public Vector2 Position;
        public float RotationAngle;
        public float RotationSpeed;
        public bool Rotate;
        public bool Stop;
        private Vector2 origin;
        public bool Clockwise;

        public Sprite(Texture2D Texture, Vector2 Position, bool Rotate, Game game)
            : base(game) {
            this.Texture = Texture;
            this.Position = Position;
            this.Rotate = Rotate;
            Visible = true;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public Sprite(Texture2D Texture, Vector2 Position, Game game)
            : base(game) {
            this.Texture = Texture;
            this.Position = Position;
            Visible = true;
        }

        public void SetRotationSpeed(float RotationSpeed) {
            this.RotationSpeed = RotationSpeed;
        }

        public void SetClockwise(bool Clockwise) {
            this.Clockwise = Clockwise;
        }

        public override void Update(GameTime gameTime) {
            if(Rotate && !Stop)
                if(Clockwise)
                    RotationAngle = (RotationAngle - ((float)gameTime.ElapsedGameTime.TotalSeconds * RotationSpeed)) % (MathHelper.Pi * 2);
                else
                    RotationAngle = (RotationAngle + ((float)gameTime.ElapsedGameTime.TotalSeconds * RotationSpeed)) % (MathHelper.Pi * 2);
        }

        public void Draw(SpriteBatch spriteBatch) {
            if(Visible)
                if(Rotate)
                    spriteBatch.Draw(Texture, Position, null, Color.White, RotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
                else if(Rotate && Stop)
                    spriteBatch.Draw(Texture, Position, null, Color.White);
                else
                    spriteBatch.Draw(Texture, Position, Color.White);
        }

        public bool Intersect(Sprite sprite) {
            Rectangle me = new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width, (int)Texture.Height);
            Rectangle you = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, (int)sprite.Texture.Width, (int)sprite.Texture.Height);
            return me.Intersects(you);
        }

        public bool Intersect(Rectangle rectangle) {
            Rectangle me = new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width, (int)Texture.Height);
            return me.Intersects(rectangle);
        }

        public Vector2 Distance(Sprite sprite) {
            Rectangle me = new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width, (int)Texture.Height);
            Rectangle you = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, (int)sprite.Texture.Width, (int)sprite.Texture.Height);
            return new Vector2(Utils.GetHorizontalIntersectionDepth(me, you), Utils.GetVerticalIntersectionDepth(me, you));
        }

        public Vector2 Distance(Rectangle rectangle) {
            Rectangle me = new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width, (int)Texture.Height);
            return new Vector2(Utils.GetHorizontalIntersectionDepth(me, rectangle), Utils.GetVerticalIntersectionDepth(me, rectangle));
        }

        public static Comparison<Sprite> CompareForX = delegate(Sprite s1, Sprite s2) {
            return s1.Position.X.CompareTo(s2.Position.X);
        };

        public static Comparison<Sprite> CompareForY = delegate(Sprite s1, Sprite s2) {
            return s1.Position.Y.CompareTo(s2.Position.Y);
        };
    }
}