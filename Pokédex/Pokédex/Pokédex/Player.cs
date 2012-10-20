using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pokédex {
    class Player : Sprite {
        public Vector2 Speed, Friction, Velocity;

        public Player(Texture2D Texture, Vector2 Position, Vector2 Speed, Vector2 Friction, bool Rotate, Game game)
            : base(Texture, Position, Rotate, game) {
            this.Speed = Speed;
            this.Friction = Friction;
        }

        public Player(Texture2D Texture, Vector2 Position, Vector2 Speed, Vector2 Friction, Game game)
            : base(Texture, Position, game) {
            this.Speed = Speed;
            this.Friction = Friction;
        }

        private enum Direction { Unknown, Left, Right, Up, Down }
        private enum Status { Walking, Jumping }

        private Direction HandleDirection(KeyboardState state) {
            Direction direction = Direction.Unknown;
            if(state.IsKeyDown(Keys.Left))
                direction = Direction.Left;
            else if(state.IsKeyDown(Keys.Right))
                direction = Direction.Right;
            else if(state.IsKeyDown(Keys.Up))
                direction = Direction.Up;
            else if(state.IsKeyDown(Keys.Down))
                direction = Direction.Down;
            return direction;
        }

        private void HandleMovements(Direction direction) {
            Vector2 flipY = new Vector2(1, -1);
            Velocity += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left * Speed * flipY;
            if(direction == Direction.Left)
                Velocity.X -= Speed.X;
            if(direction == Direction.Right)
                Velocity.X += Speed.X;
            if(direction == Direction.Up)
                Velocity.Y -= Speed.Y;
            if(direction == Direction.Down)
                Velocity.Y += Speed.Y;
            Position += Velocity;
            Velocity *= Friction;
        }

        private void HandleCollision(Direction direction, List<Pokemon> pokemon) {
            foreach(Pokemon p in pokemon)
                if(Intersect(p)) {
                    Velocity = new Vector2(0, 0);
                    if(direction == Direction.Left)
                        Position.X += Velocity.X + Distance(p).X;
                    if(direction == Direction.Right)
                        Position.X -= Velocity.X - Distance(p).X;
                    if(direction == Direction.Up)
                        Position.Y += Velocity.Y + Distance(p).Y;
                    if(direction == Direction.Down)
                        Position.Y -= Velocity.Y - Distance(p).Y;
                }
        }

        private void HandleCollisionWithBorders(Screen screen) {
            int MaxX = screen.Width - Texture.Width;
            int MinX = 0;
            int MaxY = screen.Height - Texture.Height;
            int MinY = 0;

            if(Position.X > MaxX)
                Position.X = MaxX;
            else if(Position.X < MinX)
                Position.X = MinX;

            if(Position.Y > MaxY)
                Position.Y = MaxY;
            else if(Position.Y < MinY)
                Position.Y = MinY;
        }

        public void Update(List<Pokemon> pokemon, KeyboardState state, Screen screen, GameTime gameTime) {
            if(Visible) {
                Direction direction = HandleDirection(state);
                HandleMovements(direction);
                HandleCollision(direction, pokemon);
                HandleCollisionWithBorders(screen);
            }
            base.Update(gameTime);
        }
    }
}
