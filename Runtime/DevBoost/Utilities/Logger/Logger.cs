/* *************************************************
*  Created:  2023-2-23 14:51:00
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.IO;
using log4net;
using log4net.Config;


namespace DevBoost.Utilities
{
    public static class Logger
    {
        static public ILog Dev { get; private set; }

        static public bool Init(FileInfo fileInfo)
        {
            if ((fileInfo?.Length ?? -1) != -1)
            {
                XmlConfigurator.Configure(fileInfo);
                Dev = log4net.LogManager.GetLogger("Dev");
                return true;
            }

            return false;
        }
    }
}