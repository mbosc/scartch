using System.Collections;
using System;
namespace model
{
    public class Sound
    {
		private string name;

		public Sound(string name){
			this.name = name;
		}
		public string Name {
			get {
				return name;
			}
		}
    }
}