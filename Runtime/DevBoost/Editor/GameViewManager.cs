/* ---------------------------------------------------------------------
 * Created on Mon Fev 08 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : GameView Editor to handle screen View
--------------------------------------------------------------------- */
namespace DevBoost
{

    using System;
    using System.Reflection;
    using UnityEditor;

    public class GameViewEditor
    {
        private static GameViewSizeGroupType currentPlatform
        {
            get {
                return GameViewSizeManager.GetCurrentGroupType();
            }
        }

        [MenuItem("PopReach/Tool/SetGameView FaceBook")]
        public static void SetGamesFaceBook()
        {
            int index = GameViewSizeManager.FindSize(currentPlatform, "Facebook");
            GameViewSizeManager.SelectViewAspect(index);
        }
        [MenuItem("PopReach/Tool/SetGameView IPhone Pro12")]
        public static void SetGamesViewMobile()
        {
            int index = GameViewSizeManager.FindSize(currentPlatform, "IPhone Pro12");
            GameViewSizeManager.SelectViewAspect(index);
        }

        [MenuItem("PopReach/Tool/Add All Size")]
        public static void AddAllSize()
        {
            var targets = new Spect[] {
            new Spect() { label = "Facebook", with = 760, height = 590},
            new Spect() { label = "IPhone Pro12", with = 2532, height = 1170 },
            new Spect() { label = "Note20 Ultra", with = 3088, height = 1440},
            new Spect() { label = "UI Match", with = 1276, height = 590},
            new Spect() { label = "IPad", with = 2732, height = 2048},
        };
            foreach (var spec in targets)
            {
                int index = GameViewSizeManager.FindSize(currentPlatform, spec.label);
                if (index == -1)
                    GameViewSizeManager.AddCustomSize(GameViewSizeManager.GameViewSizeType.FixedResolution, currentPlatform, spec.with, spec.height, spec.label);
            }
        }

        public struct Spect
        {
            public string label;
            public int with;
            public int height;
        }

    }

    public class GameViewSizeManager
    {
        private static object gameViewSizesInstance;
        private static MethodInfo getGroupMethod;

        static GameViewSizeManager()
        {
            var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            getGroupMethod = sizesType.GetMethod("GetGroup");
            gameViewSizesInstance = instanceProp.GetValue(null, null);
        }

        public enum GameViewSizeType
        {
            AspectRatio,
            FixedResolution
        }

        public static void SelectViewAspect(int index)
        {
            var gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            var gameViewWindow = EditorWindow.GetWindow(gameViewType);
            var sizeSelectionCallback = gameViewType.GetMethod("SizeSelectionCallback");
            sizeSelectionCallback.Invoke(gameViewWindow, new object[] { index, null });
        }

        public static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        {
            Type type = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");

            var group = GetGroup(sizeGroupType);
            var addCustomSize = getGroupMethod.ReturnType.GetMethod("AddCustomSize");
            var gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");

            var constructor = gameViewSizeType.GetConstructor(new Type[] { type, typeof(int), typeof(int), typeof(string) });
            var newSize = constructor.Invoke(new object[] { (int)viewSizeType, width, height, text });

            addCustomSize.Invoke(group, new object[] { newSize });
        }

        public static bool SizeExists(GameViewSizeGroupType sizeGroupType, string text)
        {
            return FindSize(sizeGroupType, text) != -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
        {
            var group = GetGroup(sizeGroupType);
            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
            for (int i = 0; i < displayTexts.Length; i++)
            {
                string display = displayTexts[i];
                int pren = display.IndexOf('(');
                if (pren != -1)
                    display = display.Substring(0, pren - 1);
                if (display == text)
                    return i;
            }

            return -1;
        }

        public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            return FindSize(sizeGroupType, width, height) != -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            var group = GetGroup(sizeGroupType);
            var groupType = group.GetType();

            var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            var getCustomCount = groupType.GetMethod("GetCustomCount");

            int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);

            var getGameViewSize = groupType.GetMethod("GetGameViewSize");
            var gvsType = getGameViewSize.ReturnType;

            var widthProp = gvsType.GetProperty("width");
            var heightProp = gvsType.GetProperty("height");

            var indexValue = new object[1];
            for (int i = 0; i < sizesCount; i++)
            {
                indexValue[0] = i;
                var size = getGameViewSize.Invoke(group, indexValue);
                int sizeWidth = (int)widthProp.GetValue(size, null);
                int sizeHeight = (int)heightProp.GetValue(size, null);
                if (sizeWidth == width && sizeHeight == height)
                    return i;
            }

            return -1;
        }

        static object GetGroup(GameViewSizeGroupType type)
        {
            return getGroupMethod.Invoke(gameViewSizesInstance, new object[] { (int)type });
        }

        public static GameViewSizeGroupType GetCurrentGroupType()
        {
            var getCurrentGroupTypeProp = gameViewSizesInstance.GetType().GetProperty("currentGroupType");
            return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(gameViewSizesInstance, null);
        }
    }
}