using System.Collections.Generic;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        /// <summary>
        /// Event that triggers when window close request is received and cancels closing
        /// </summary>
        public delegate void EventCloseRequested();
        private event EventCloseRequested closeRequested = null;
        public EventCloseRequested CloseRequested
        {
            get { return closeRequested; }
            set
            {
                closeRequested = null;
                closeRequested += value;
            }
        }

        /// <summary>
        /// Event that triggers when files have been dropped in window
        /// </summary>
        public delegate void EventFilesDropped(List<string> droppedFiles);
        private event EventFilesDropped filesDropped = null;
        public EventFilesDropped FilesDropped
        {
            get { return filesDropped; }
            set
            {
                filesDropped = null;
                filesDropped += value;
            }
        }

        /// <summary>
        /// Event that forwards all window messages with bool to set handled
        /// </summary>
        public delegate void EventForwardMessage(ref WindowMessage windowMessage, ref bool messageHandled);
        private event EventForwardMessage forwardMessage;
        public EventForwardMessage ForwardMessage
        {
            get { return forwardMessage; }
            set
            {
                forwardMessage = null;
                forwardMessage += value;
            }
        }
    }
}