/* *************************************************
*  Created:  7/20/2017, 2:05:05 PM
*  File:     PooledObject.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using UnityEngine;
using System;

namespace DevBoost.Utilities 
{

	public class PooledObject : MonoBehaviour 
    {

		[NonSerialized]
		ObjectPool poolInstanceForPrefab;

        /// <summary>
        /// Sets the pool handler.
        /// </summary>
        /// <value>The pool handler.</value>
		public ObjectPool poolHandler 
        { 
			private get; 
			set; 
		}

        /// <summary>
        /// Spawn this instance.
        /// </summary>
        /// <returns>The spawn.</returns>
		public PooledObject Spawn() 
        {
			if (poolInstanceForPrefab == null)
				poolInstanceForPrefab = ObjectPool.CreateObjectPool(this);
			
			return poolInstanceForPrefab.GetObject();
		}


        /// <summary>
        /// Returns to pool.
        /// </summary>
		public void ReturnToPool () 
        {
			if (poolHandler)
				poolHandler.AddObject(this);
			else
				Destroy(gameObject);
		}

        /// <summary>
        /// Ons the destroy.
        /// </summary>
        private void OnDestroy()
        {
            if (poolHandler)
                poolHandler.RemoveObject(this);
        }
    }
}