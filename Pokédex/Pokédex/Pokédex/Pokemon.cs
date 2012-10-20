using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pokédex {
    public class Pokemon : Sprite {
        public List<RNGReporter.Pokemon> dex;
        public int Num;
        public float Speed;

        public Pokemon(int Num, Texture2D Texture, Vector2 Position, float Speed, bool Rotate, Game game)
            : base(Texture, Position, Rotate, game) {
            this.Num = Num;
            this.Speed = Speed;
            dex = RNGReporter.Pokemon.PokemonCollection();
        }

        public Pokemon(int Num, Texture2D Texture, Vector2 Position, float Speed, Game game)
            : base(Texture, Position, game) {
            this.Num = Num;
            this.Speed = Speed;
            dex = RNGReporter.Pokemon.PokemonCollection();
        }

        public override void Update(GameTime gameTime) {
            if(Visible)
                Position.X -= Speed;
            base.Update(gameTime);
        }

        public static Comparison<Pokemon> CompareForNum = delegate(Pokemon p1, Pokemon p2) {
            return p1.Num.CompareTo(p2.Num);
        };
    }
}