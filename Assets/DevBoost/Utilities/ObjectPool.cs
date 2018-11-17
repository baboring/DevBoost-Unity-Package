﻿/* *************************************************
*  Created:  7/20/2017, 2:04:33 PM
*  File:     ObjectPool.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using UnityEngine;
using System.Collections.Generic;

namespace DevBoost.Utilities 
{
	public class ObjectPool : MonoBehaviour 
    {

		PooledObject prefab;

		List<PooledObject> availableObjects = new List<PooledObject>();

        /// <summary>
        /// Creates the object pool.
        /// </summary>
        /// <returns>The object pool.</returns>
        /// <param name="prefab">Prefab.</param>
		public static ObjectPool CreateObjectPool (PooledObject prefab) 
        {
			GameObject obj;
			ObjectPool pool;
			//if (Application.isEditor) {
			//	obj = GameObject.Find(prefab.name + " Pool");
			//	if (obj) {
			//		pool = obj.GetComponent<ObjectPool>();
			//		if (pool) {
			//			return pool;
			//		}
			//	}
			//}
			obj = new GameObject(prefab.name + " Pool");
			//DontDestroyOnLoad(obj);
			pool = obj.AddComponent<ObjectPool>();
			pool.prefab = prefab;
			return pool;
		}

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="arg">Argument.</param>
		public PooledObject GetObject (PooledObject arg = default(PooledObject)) 
        {
			PooledObject obj;
            if(arg != default(PooledObject)) {
                int index = FindIndex(arg);
                if (index > -1 && availableObjects.Count > 0) {
                    obj = availableObjects[index];
                    availableObjects.RemoveAt(index);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
            
            int lastAvailableIndex = availableObjects.Count - 1;
			if (lastAvailableIndex >= 0) {
				obj = availableObjects[lastAvailableIndex];
				availableObjects.RemoveAt(lastAvailableIndex);
			}
			else {
				obj = Instantiate<PooledObject>(prefab);
				obj.transform.SetParent(transform, false);
				obj.poolHandler = this;
            }
            obj.gameObject.SetActive(true);
            return obj;
		}

        public int FindIndex(PooledObject obj) 
        {
            return availableObjects.FindIndex((val) => { return val == obj; });
        }

        public void AddObject(PooledObject obj) 
        {
			obj.gameObject.SetActive(false);
			availableObjects.Add(obj);
		}

        public void RemoveObject(PooledObject obj)
        {
            availableObjects.Remove(obj);
        }
	}
}