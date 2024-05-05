/* *************************************************
*  Created:  2012-2-25 14:51:00
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System;

namespace DevBoost.Utilities {

    public interface ILogger<T>
    {
        public bool isEchoToConsole { get; }
        public bool isCaptureLog { get; }
        public void Write(string message, bool istimeStamp = true);
    }



    public static class Logger
    {
        static ILogger<FileLogger> logger;

        public static void Assign(ILogger<FileLogger> _logger)
        {
            logger = _logger;
        }

        public static void Trace(String format, params object[] args)
        {
            if (logger != null)
            {
                if (logger.isEchoToConsole)
                {
                    UnityEngine.Debug.Log(string.Format(format, args));
                    if (logger.isCaptureLog)
                        return;
                }

                logger.Write(string.Format(format, args));
            }
            //else
            //    // Fallback if the debugging system hasn't been initialized yet.
            //    UnityEngine.Debug.Log(Message);
        }
    }

}