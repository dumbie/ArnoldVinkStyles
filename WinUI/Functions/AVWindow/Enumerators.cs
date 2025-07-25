namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        //Enumerators
        public enum AVWindowState : int
        {
            Normal,
            Minimized,
            Maximized
        }

        public enum AVWindowLocation : int
        {
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        public enum CORE_WINDOW_TYPE
        {
            IMMERSIVE_BODY,
            IMMERSIVE_DOCK,
            IMMERSIVE_HOSTED,
            IMMERSIVE_TEST,
            IMMERSIVE_BODY_ACTIVE,
            IMMERSIVE_DOCK_ACTIVE,
            NOT_IMMERSIVE
        }
    }
}