/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     ActionController.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace ActionBehaviour 
{

	// deprecated ( ActionStarter instead )
    public class ActionController : Execute 
    {
    
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
