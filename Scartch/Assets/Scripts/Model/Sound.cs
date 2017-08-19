using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Model
{
    public class Sound
    {
        private string name;

        private Sound(string name)
        {
            this.name = name;
        }

        public static Sound[] sounds;

        public static Sound GetSound(int num)
        {
            if (sounds == null)
            {
                sounds = new Sound[3];
                sounds[0] = new Sound("Sound 1");
                sounds[1] = new Sound("Sound 2");
                sounds[2] = new Sound("Sound 3");
            }
            if (num > sounds.Length)
                throw new ArgumentException("invalid sound number");
            else
                return sounds[num];
        }
        public static int IndexOfSound(Sound snd)
        {
            if (sounds == null)
            {
                sounds = new Sound[3];
                sounds[0] = new Sound("Sound 1");
                sounds[1] = new Sound("Sound 2");
                sounds[2] = new Sound("Sound 3");
            }
            if (!sounds.Contains(snd))
                throw new ArgumentException("invalid actormodel");
            else
                return sounds.ToList().IndexOf(snd);
        }

        public static UnityEngine.AudioClip GetClip(Sound sound)
        {
            return ScartchResourceManager.instance.sounds[IndexOfSound(sound)];
        }
    }
}