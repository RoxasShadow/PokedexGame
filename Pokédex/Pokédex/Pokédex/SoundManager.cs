using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Pokédex {
    public class SoundManager {
        private SoundEffect sound;
        public SoundEffectInstance soundInstance;

        public enum Sound { Shot, Denied, Main, Main2, Zelda }

        public SoundManager(Sound sound, bool Loop) {
            Load(sound);
            soundInstance.IsLooped = Loop;
        }

        private void Load(Sound sound) {
            string file = Utils.GetCurrentDirectory() + "\\Data\\Sound\\" + sound.ToString() + ".wav";
            this.sound = Utils.LoadSound(file);
            soundInstance = this.sound.CreateInstance();
        }
    }
}
