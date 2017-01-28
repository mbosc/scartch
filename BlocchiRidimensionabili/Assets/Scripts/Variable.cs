using System.Collections;
using System;

public abstract class Variable {
	private string name;

	public string Name {
		get { return name; }
		set { 
			// verificare che non sia in collisione con il namespace
			// per questo potrebbe convenire aggiungere un riferimento al dominio di definizione
			name = value; 
		}
	}
	public abstract string EvaluateAsString();

}
