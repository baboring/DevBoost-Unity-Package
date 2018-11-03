﻿/* *************************************************
*  Created:  2018-3-28 19:46:32
*  File:     UIViewController.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace ActionBehaviour {



    public class UIViewController : Execute {
    
		[SerializeField]
		protected StartOption startType;

		void Start() {
			
			if( StartOption.AutoStart == startType )
				base.ExecuteInvoke();
		}

        // called by outside
        public void RunController()
        {

            base.ExecuteInvoke();
        }

		
	}

}
