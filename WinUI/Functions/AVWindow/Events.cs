using System;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        //Events
        /// <summary>
        /// Event that triggers when window close request is received
        /// </summary>
        private event EventHandler closeRequested = null;
        public EventHandler CloseRequested
        {
            get { return closeRequested; }
            set
            {
                closeRequested = null;
                closeRequested += value;
            }
        }
    }
}