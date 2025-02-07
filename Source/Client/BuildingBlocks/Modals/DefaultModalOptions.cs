using Blazored.Modal;

namespace Client.BuildingBlocks.Modals
{
    public class DefaultModalOptions
    {
        public static ModalOptions SignUpLoginModal
        {
            get
            {
                return new ModalOptions
                {
                    OverlayCustomClass = "modaloverlay",
                    Class = "defaultModal",
                    Position = ModalPosition.Middle,
                    HideCloseButton = true,
                    HideHeader = true
                };
            }
        }

        public static ModalOptions DefaultModal
        {
            get
            {
                return new ModalOptions
                {
                    OverlayCustomClass = "modaloverlay",
                    Class = "defaultModal",
                    Position = ModalPosition.Middle,
                    HideCloseButton = true,
                    HideHeader = true,
                    AnimationType = ModalAnimationType.None,
                    DisableBackgroundCancel = false
                };
            }
        }
    }
}
