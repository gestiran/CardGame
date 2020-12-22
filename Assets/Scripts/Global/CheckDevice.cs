using UnityEngine;

namespace CardGame
{
    public static class CheckDevice
    {
        public enum DeviceType
        {
            Phone,
            Tablet
        }

        private const string _prefName = "CurrentDeviceType";
        
        public static DeviceType GetCurrentType()
        {
            if (
        #if UNITY_EDITOR
                false
        #else
                PlayerPrefs.HasKey(_prefName)
        #endif
                ) return LoadType();
            else
            {
                DeviceType currentType;
                
                if (((Screen.height > Screen.width) ? Screen.width : Screen.height) / Screen.dpi < 3.1) currentType = DeviceType.Phone;
                else currentType = DeviceType.Tablet;
                
                SaveType(currentType);
                return currentType;
            }
        }

        private static void SaveType(DeviceType currentType) => PlayerPrefs.SetInt(_prefName, (int)currentType);

        private static DeviceType LoadType() => (DeviceType)PlayerPrefs.GetInt(_prefName);
    }
}