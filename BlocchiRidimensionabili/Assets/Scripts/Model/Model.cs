using System.Collections;
using System;
namespace model
{
    public class Model
    {
		private string name;

		public Model(string name){
			this.name = name;
		}
		public string Name {
			get {
				return name;
			}
		}
    }
}