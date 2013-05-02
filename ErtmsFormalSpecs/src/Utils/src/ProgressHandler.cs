// -----------------------------------------------------------------------
// <copyright file="ProgressHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Utils
{
    public abstract class ProgressHandler
    {
        /// <summary>
        /// Handles the start of the thread
        /// </summary>
        /// <param name="arg"></param>
        public void TreadStart(object arg)
        {
            ProgressHandler handler = (ProgressHandler)arg;
            handler.ExecuteWork();
        }

        /// <summary>
        /// The task to be executed in background
        /// </summary>
        public abstract void ExecuteWork();
    }
}
