using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Mecani {
	[RequireComponent(typeof(Animator))]
	public class MecanimParticleSystem : MonoBehaviour {

		#region Data

		/// <summary>
		/// The particle systems that should be controllable by the animator associated with this component.
		/// </summary>
		[SerializeField]
		private ParticleSystem[] trackingParticleSystems = null;

		/// <summary>
		/// Lookup map of particle system by particle system gameobject name.
		/// </summary>
		private Dictionary<string, ParticleSystem> particleSystemMap = null;

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// When the particle system starts up log the systems by thier gameobject id.
		/// </summary>
		private void Awake() {
			this.particleSystemMap = new Dictionary<string, ParticleSystem>(this.trackingParticleSystems.Length);

			for (int i = 0; i < this.trackingParticleSystems.Length; i++) {
				this.particleSystemMap.Add(this.trackingParticleSystems[i].gameObject.name, this.trackingParticleSystems[i]);
			}
		}

		#endregion

		#region Components

		/// <summary>
		/// Passthrough function to play particle systems.
		/// </summary>
		/// <param name="particleSystemName">Specific gameobject name of the particle system to pause.</param>
		public void PlayParticles(string particleSystemName = null) {
			// Using no specified particle system means apply to all particle systems associated with this component.
			if (string.IsNullOrEmpty(particleSystemName)) {
				foreach (ParticleSystem system in this.trackingParticleSystems) {
					system.Play(true);
				}
			} else {
				ParticleSystem foundSystem = null;
				if (this.particleSystemMap.TryGetValue(particleSystemName, out foundSystem)) {
					foundSystem.Play(true);
				} else {
					Debug.LogErrorFormat("Could not find particle system with gameobject name of {0} registered with MecanimParticleSystem on {1}", particleSystemName, this.name);
				}
			}
		}

		/// <summary>
		/// Passthrough function to pause particle systems.
		/// </summary>
		/// <param name="particleSystemName">Specific gameobject name of the particle system to pause.</param>
		public void PauseParticleSystem(string particleSystemName = null) {
			// Using no specified particle system means apply to all particle systems associated with this component.
			if (string.IsNullOrEmpty(particleSystemName)) {
				foreach (ParticleSystem system in this.trackingParticleSystems) {
					system.Pause(true);
				}
			} else {
				ParticleSystem foundSystem = null;
				if (this.particleSystemMap.TryGetValue(particleSystemName, out foundSystem)) {
					foundSystem.Pause(true);
				} else {
					Debug.LogErrorFormat("Could not find particle system with gameobject name of {0} registered with MecanimParticleSystem on {1}", particleSystemName, this.name);
				}
			}
		}

		/// <summary>
		/// Passthrough function to stop particle systems.
		/// </summary>
		/// <param name="particleSystemName">Specific gameobject name of the particle system to stop.</param>
		public void StopParticleSystem(string particleSystemName = null) {
			// Using no specified particle system means apply to all particle systems associated with this component.
			if (string.IsNullOrEmpty(particleSystemName)) {
				foreach (ParticleSystem system in this.trackingParticleSystems) {
					system.Stop(true);
				}
			} else {
				ParticleSystem foundSystem = null;
				if (this.particleSystemMap.TryGetValue(particleSystemName, out foundSystem)) {
					foundSystem.Stop(true);
				} else {
					Debug.LogErrorFormat("Could not find particle system with gameobject name of {0} registered with MecanimParticleSystem on {1}", particleSystemName, this.name);
				}
			}
		}

		/// <summary>
		/// Passthrough function to clear particle systems.
		/// </summary>
		/// <param name="particleSystemName">Specific gameobject name of the particle system to clear.</param>
		public void ClearParticleSystems(string particleSystemName = null) {
			if (string.IsNullOrEmpty(particleSystemName)) {
				foreach (ParticleSystem system in this.trackingParticleSystems) {
					system.Clear(true);
				}
			} else {
				ParticleSystem foundSystem = null;
				if (this.particleSystemMap.TryGetValue(particleSystemName, out foundSystem)) {
					foundSystem.Clear(true);
				} else {
					Debug.LogErrorFormat("Could not find particle system with gameobject name of {0} registered with MecanimParticleSystem on {1}", particleSystemName, this.name);
				}
			}
		}

		#endregion

	}
}

