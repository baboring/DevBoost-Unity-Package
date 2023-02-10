using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevBoost.Core {

	#region Attribute Class

		/// <summary>
		/// This attribute is used to determine what service a given object type represents.
		/// </summary>
		[AttributeUsage(AttributeTargets.Class, Inherited = true)]
		public class ServiceAttribute : Attribute {

			#region Data

			private string serviceID = string.Empty;
			/// <summary>
			/// Gets the service ID of this 
			/// </summary>
			public string ServiceID {
				get {
					return this.serviceID;
				}
			}

			#endregion
			
			#region Constructor

			/// <summary>
			/// Create a new service attribute.
			/// </summary>
			/// <param name="serviceID">The ID for this service attribute.</param>
			public ServiceAttribute(string serviceID) {
				this.serviceID = serviceID;
			}

			#endregion
		}

	#endregion

	public class ServiceLocator : MonoBehaviour {

		#region Data

		/// <summary>
		/// Reference to the ServiceLocator singleton object.
		/// </summary>
		private static ServiceLocator instance = null;

		/// <summary>
		///  The map of service attribute ID to instantiated object.
		/// </summary>
		/// <typeparam name="string">Service ID of the object.</typeparam>
		/// <typeparam name="object">Reference to the bound object.</typeparam>
		private static Dictionary<string, object> serviceMap = new Dictionary<string, object>();

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// When this class first initalizes register the first one as the singleton and move it outside the standard scene management settings.
		/// Destroy any other instances.
		/// </summary>
		private void Awake() {
			this.InitializeServiceLocator();
		}

		#endregion

		#region Service Locator

		/// <summary>
		/// Service locator initialization.
		/// </summary>
		private void InitializeServiceLocator() {
			if (instance != null && instance != this) {
				GameObject.Destroy(this.gameObject);
			} else {
				ServiceLocator.instance = this;
				// Add this to the don't destroy on load area.
				GameObject.DontDestroyOnLoad(this);
				this.Initialize();
			}
		}

		/// <summary>
		/// Setup the service locator instance.
		/// Override in subclasses for initalizations.
		/// </summary>
		protected virtual void Initialize() {
			// Override in subclasses for game specific service initalizations if required.
		}

		#endregion

		#region Static ServiceLocator

		/// <summary>
		/// Binds an object to the service locator.
		/// </summary>
		/// <param name="service">The new bound object to attach to this service locator.</param>
		public static void Bind<T>(T service) where T : class {
			if (service == null) {
				Debug.LogError("Cannot bind an empty service object.");
				return;
			}

			string serviceKey = ServiceLocator.GetServiceID(service.GetType());

			if (string.IsNullOrEmpty(serviceKey)) {
				Debug.LogError(service.GetType().ToString() + " is not a valid Service object. Did you add the ServiceAttribute to it?");
				return;
			}

			if (ServiceLocator.serviceMap.ContainsKey(serviceKey)) {
				Debug.LogError("ServiceLocator already contains " + service.GetType().ToString());
				return;
			}

			ServiceLocator.serviceMap.Add(serviceKey, service);
		}

		/// <summary>
		/// Take a prefab object, instantiate an instance of it as a child of the service locator and then bind that reference to this service locator.
		/// </summary>
		/// <param name="prefab">The prefab to bind to the service locator.</param>
		public static void BindPrefab<T>(T prefab) where T : MonoBehaviour {
			T monoService = DevBoost.Core.UnityObjectUtility.Instantiate<T>(prefab, ServiceLocator.instance.transform);
			ServiceLocator.Bind(monoService);
		}

		/// <summary>
		/// Checks if this service locator has a bound service of the correct type.
		/// </summary>
		/// <typeparam name="T">The type of the service we are searching for.</typeparam>
		public static bool Has<T>() where T : class {
			string serviceID = ServiceLocator.GetServiceID(typeof(T));
			Debug.Assert(!string.IsNullOrEmpty(serviceID), typeof(T).ToString() + " is not a ServiceAttribute.");

			return (ServiceLocator.serviceMap.ContainsKey(serviceID));
		}

		/// <summary>
		/// Returns a reference to the request service type.
		/// </summary>
		/// <typeparam name="T">The service type we are trying to access.</typeparam>
		/// <returns>The reference to the correct service type or null if there is no matching bound service.</returns>
		public static T Get<T>() where T : class {
			string serviceID = ServiceLocator.GetServiceID(typeof(T));
			Debug.Assert(!string.IsNullOrEmpty(serviceID), typeof(T).ToString() + " is does not have a ServiceAttribute.");
			
			if (!ServiceLocator.serviceMap.ContainsKey(serviceID)) {
				Debug.LogError("ServiceLocator does not contain a service with the type of: " + typeof(T).ToString());
				return null;
			} else {
				return (T)ServiceLocator.serviceMap[serviceID];
			}
		}

		/// <summary>
		/// Gets the service attribute for the provided object.
		/// </summary>
		/// <param name="t">The object type to check for service attributes.</param>
		/// <returns>The first service attribute assigned to the given type.</returns>
		private static string GetServiceID(Type t) {
			System.Attribute[] attributes = System.Attribute.GetCustomAttributes(t, true);
			string foundAttribute = string.Empty;
			for (int i = 0; i < attributes.Length; ++i) {
				if (attributes[i] is ServiceAttribute) {
					foundAttribute = ((ServiceAttribute)attributes[i]).ServiceID;
					break;
				}
			}

			return foundAttribute;
		}

		#endregion

		#region Object

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="ServiceLocator"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="ServiceLocator"/>.</returns>
		public override string ToString() {
			System.Text.StringBuilder info = new System.Text.StringBuilder();
			
			info.Append("ServiceLocator: ");
			info.Append(ServiceLocator.serviceMap.Count.ToString());
			info.AppendLine(" Services");
			
			foreach (KeyValuePair<string, object> service in ServiceLocator.serviceMap) {
				info.Append(service.Key);
				info.Append(" - ");
				info.AppendLine(service.Value.ToString());
			}
			
			return info.ToString();
		}

		#endregion

	}
}
