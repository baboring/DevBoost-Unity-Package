using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Core
{
	public class ObjectPool<T>
	{

		#region Delegate

		/// <summary>
		/// Delegate for creating new objects for this pool.
		/// </summary>
		/// <returns>A new object of the correct type for this pool.</returns>
		public delegate T PoolObjectCreationDelegate();

		/// <summary>
		/// Delegate for resetting an object to an initial state.
		/// </summary>
		/// <param name="objectToReset">Object that should reset.</param>
		public delegate void CleanupObjectDelegate(ref T objectToReset);

		#endregion

		#region Data

		/// <summary>
		/// Callback function for creating a new object.
		/// </summary>
		private PoolObjectCreationDelegate createObject = null;

		/// <summary>
		/// Function for cleaning up an object in this pool.
		/// </summary>
		private CleanupObjectDelegate cleanupObject = null;

		/// <summary>
		/// Queue of free objects.
		/// </summary>
		private Queue<T> freeObjects = null;

		/// <summary>
		/// Linked list of active objects.
		/// </summary>
		private LinkedList<T> activeObjects = null;

		/// <summary>
		/// The maximum pool size.
		/// </summary>
		private int maxPoolSize = int.MaxValue;

		/// <summary>
		/// The number of objects that are currently being managed by this pool.
		/// </summary>
		public int ObjectCount
		{
			get
			{
				return this.freeObjects.Count + this.activeObjects.Count;
			}
		}

        /// <summary>
        /// Get all active objects
        /// </summary>
        public IEnumerable<T> ActiveObjects => activeObjects;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor for an ObjectPool.
		/// </summary>
		/// <param name="createFunction">Function that will create new objects.</param>
		/// <param name="cleanupFunction">Function that will reset objects to the correct state for them in the pool.</param>
		/// <param name="initialPoolSize">The initial number of objects to create for the pool.</param>
		public ObjectPool(PoolObjectCreationDelegate createFunction, CleanupObjectDelegate cleanupFunction, int initialPoolSize = 0, int maxPoolSize = int.MaxValue)
		{
			this.createObject = createFunction;
			this.cleanupObject = cleanupFunction;

			this.freeObjects = new Queue<T>(initialPoolSize);
			this.activeObjects = new LinkedList<T>();

			for (int i = 0; i < initialPoolSize; ++i)
			{
				this.freeObjects.Enqueue(this.createObject());
			}
		}

		#endregion

		#region Pool Logic

		/// <summary>
		/// Get an object from this pool.
		/// </summary>
		/// <returns>An object of the requested type, either new or re-used.</returns>
		public T RequestObject()
		{
			T requestedObject = default(T);
			if (freeObjects.Count > 0)
			{
				requestedObject = freeObjects.Dequeue();
			}
			else
			{
				requestedObject = this.createObject();
			}

			this.activeObjects.AddLast(requestedObject);
			return requestedObject;
		}

		/// <summary>
		/// This will release an object from the active pool.
		/// </summary>
		/// <param name="obj">The object to release.</param>
		public bool Release(T obj)
		{
			// If the object is a part of this pool we need to reset it and clean it up.
			if (this.activeObjects.Remove(obj))
			{
				this.cleanupObject(ref obj);

				// Only push the object back if the pool has room for it, otherwise let it fall out of scope.
				if (this.ObjectCount < this.maxPoolSize)
				{
					this.freeObjects.Enqueue(obj);
					return true;
				}
			}
			return false;
		}


        public bool Exist(T obj)
        {
            return activeObjects.Contains(obj);
        }

		/// <summary>
		/// Add resource into
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool AddFreeObject(T obj)
        {
			if (obj != null)
			{
				if (Exist(obj))
					return Release(obj);

				this.cleanupObject(ref obj);

				// Only push the object back if the pool has room for it, otherwise let it fall out of scope.
				if (this.ObjectCount < this.maxPoolSize)
				{
					this.freeObjects.Enqueue(obj);
					return true;
				}
			}
			return false;
		}
		#endregion

	}
}
