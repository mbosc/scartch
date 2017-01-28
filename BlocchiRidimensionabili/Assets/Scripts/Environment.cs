using System.Collections;
using System;

public class Environment {

	#region Singleton Pattern Code
	private static Environment instance;

	private Environment () {}

	public static Environment Instance {
		get {
			if (instance == null) {
				instance = new Environment ();
			}
			return instance;
		}
	}
	#endregion

}
