using UnityEngine;

namespace CardGame
{
    public class OrientDevice : MonoBehaviour
    {
        private void Start() => ChangeOrientation();

        private void ChangeOrientation()
        {
            switch (CheckDevice.GetCurrentType())
            {
                case CheckDevice.DeviceType.Phone:
                    Screen.orientation = ScreenOrientation.Portrait;
                    Screen.orientation = ScreenOrientation.AutoRotation;

                    Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = true;
                    Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = false;
                    break;
                
                case CheckDevice.DeviceType.Tablet:
                    Screen.orientation = ScreenOrientation.Landscape;
                    Screen.orientation = ScreenOrientation.AutoRotation;

                    Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
                    Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;
                    break;
            }
        }
    }
}