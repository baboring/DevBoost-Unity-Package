using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Serialization {

	/// <summary>
	/// Interface for implementing a serializable component.
	/// </summary>
	public interface ISerializable {

		/// <summary>
		/// Serialize this component into a data object that can be serialized with minijson.
		/// </summary>
		/// <returns></returns>
		Dictionary<string, object> Serialize();

		/// <summary>
		/// Deserialize a provided dictionary into this object.
		/// </summary>
		/// <param name="serializedObject">Parsed, serialized version of the object to deserialize.</param>
		void Deserialize(Dictionary<string, object> serializedObject); 
		
	}

}
