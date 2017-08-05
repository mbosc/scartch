using System.Collections;
using System.Collections.Generic;
using System;

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
    }
}