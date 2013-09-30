using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlBooster
{
    public class Lock
    {
        private bool locked = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public Lock()
        {
        }

        /// <summary>
        /// Gets the corresponding lock
        /// </summary>
        /// <returns>false if the lock is already locked</returns>
        public bool GetLock()
        {
            bool retVal = !locked;

            if (!locked)
            {
                locked = true;
            }

            return retVal;
        }

        /// <summary>
        /// Unlocks the lock
        /// </summary>
        public void UnLock()
        {
            locked = false;
        }
    }

    /// <summary>
    /// Listens to a specific event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListener<T>
        where T : class
    {
        /// <summary>
        /// Handles the change event
        /// </summary>
        /// <param name="sender"></param>
        void HandleChangeEvent(T sender);

        /// <summary>
        /// Handles a change event
        /// </summary>
        /// <param name="aLock"></param>
        /// <param name="sender"></param>
        void HandleChangeEvent(Lock aLock, T sender);
    }

    /// <summary>
    /// Listens to a specific event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Listener<T> : IListener<T>
        where T : class
    {
        /// <summary>
        /// Handles the change event
        /// </summary>
        /// <param name="sender"></param>
        public abstract void HandleChangeEvent(T sender);

        /// <summary>
        /// Handles a change event
        /// </summary>
        /// <param name="aLock"></param>
        /// <param name="sender"></param>
        public void HandleChangeEvent(Lock aLock, T sender)
        {
            HandleChangeEvent(this, aLock, sender);
        }

        /// <summary>
        /// Handles a change event
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="aLock"></param>
        /// <param name="sender"></param>
        public static void HandleChangeEvent(IListener<T> listener, Lock aLock, T sender)
        {
            if (aLock == null)
            {
                listener.HandleChangeEvent(sender);
            }
            else
            {
                if (aLock.GetLock())
                {
                    try
                    {
                        listener.HandleChangeEvent(sender);
                    }
                    finally
                    {
                        aLock.UnLock();
                    }
                }
            }
        }
    }

    /// <summary>
    /// A model element controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Listener"></typeparam>
    public class Controller<T, Listener>
        where T : class
        where Listener : IListener<T>
    {
        /// <summary>
        /// Indicates that notifications should be sent to the listeners
        /// </summary>
        private int notifyCount = 1;

        /// <summary>
        /// Activates the notifications to the listeners
        /// </summary>
        public void ActivateNotification()
        {
            notifyCount += 1;
        }

        /// <summary>
        /// Deactivates the notifications to the listeners
        /// </summary>
        public void DesactivateNotification()
        {
            notifyCount -= 1;
        }

        /// <summary>
        /// The listeners
        /// </summary>
        private List<IListener<T>> listeners = new List<IListener<T>>();
        public List<IListener<T>> Listeners
        {
            get { return listeners; }
        }

        /// <summary>
        /// Alerts all listeners that a change occured
        /// </summary>
        /// <param name="aLock"></param>
        /// <param name="sender"></param>
        public void alertChange(Lock aLock, T sender)
        {
            if (notifyCount > 0)
            {
                foreach (IListener<T> listener in Listeners)
                {
                    listener.HandleChangeEvent(aLock, sender);
                }
            }
        }
    }
}

