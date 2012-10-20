using Microsoft.Xna.Framework;

namespace Pokédex {
    public class Screen {
        public int Width;
        public int Height;
        public Vector2 Vector;

        public Screen(int w, int h) {
            Width = w;
            Height = h;
            Vector = new Vector2(Width, Height);
        }
    }
}