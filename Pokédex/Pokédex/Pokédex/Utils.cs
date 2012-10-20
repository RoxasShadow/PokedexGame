using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pokédex {
    public static class Utils {
        public static RNGReporter.Language Language = RNGReporter.Language.English;

        public static string GetCurrentDirectory() {
            return Directory.GetCurrentDirectory();
        }

        public static Texture2D LoadTexture(GraphicsDevice graphicsDevice, string file) {
            FileStream stream = new FileStream(file, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(graphicsDevice, stream);
            stream.Close();
            RenderTarget2D result = new RenderTarget2D(graphicsDevice, texture.Width, texture.Height);
            graphicsDevice.SetRenderTarget(result);
            graphicsDevice.Clear(Color.Black);
            BlendState blendColor = new BlendState();
            blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;
            blendColor.AlphaDestinationBlend = Blend.Zero;
            blendColor.ColorDestinationBlend = Blend.Zero;
            blendColor.AlphaSourceBlend = Blend.SourceAlpha;
            blendColor.ColorSourceBlend = Blend.SourceAlpha;
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
            spriteBatch.Draw(texture, texture.Bounds, Color.White);
            spriteBatch.End();
            BlendState blendAlpha = new BlendState();
            blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;
            blendAlpha.AlphaDestinationBlend = Blend.Zero;
            blendAlpha.ColorDestinationBlend = Blend.Zero;
            blendAlpha.AlphaSourceBlend = Blend.One;
            blendAlpha.ColorSourceBlend = Blend.One;
            spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
            spriteBatch.Draw(texture, texture.Bounds, Color.White);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
            return result as Texture2D;
        }

        public static SoundEffect LoadSound(string file) {
            FileStream stream = new FileStream(file, FileMode.Open);
            SoundEffect sound = SoundEffect.FromStream(stream);
            stream.Close();
            return sound;
        }

        public static float GetHorizontalIntersectionDepth(Rectangle rectA, Rectangle rectB) {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;

            // Calculate centers.
            float centerA = rectA.Left + halfWidthA;
            float centerB = rectB.Left + halfWidthB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA - centerB;
            float minDistanceX = halfWidthA + halfWidthB;

            // If we are not intersecting at all, return (0, 0).
            if(Math.Abs(distanceX) >= minDistanceX)
                return 0f;

            // Calculate and return intersection depths.
            return distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        }

        public static float GetVerticalIntersectionDepth(Rectangle rectA, Rectangle rectB) {
            // Calculate half sizes.
            float halfHeightA = rectA.Height / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            float centerA = rectA.Top + halfHeightA;
            float centerB = rectB.Top + halfHeightB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceY = centerA - centerB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if(Math.Abs(distanceY) >= minDistanceY)
                return 0f;

            // Calculate and return intersection depths.
            return distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
        }

        public static List<Pokemon> Randomize(List<Pokemon> inputList) {
            List<Pokemon> output = new List<Pokemon>();
            Random randomizer = new Random();

            while(inputList.Count > 0) {
                int index = randomizer.Next(inputList.Count);
                output.Add(inputList[index]);
                inputList.RemoveAt(index);
            }
            return output;
        }

        public static int[] ListOfInt(int length, int min, int max) {
            int[] list = new int[length];
            Random rand = new Random();
            for(int i = 0; i < length; ++i)
                list[i] = rand.Next(min, max);
            return list;
        }
    }
}