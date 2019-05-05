namespace Fyp.Constant{
    public class GameConstant {
        public static readonly string GAME_VERSION = "1.0.0";
        public static readonly string GAME_NAME = "My Game~~";

        public static class ScenceName {
            public static string MAIN_MENU { get { return "MainMenu"; } }
            public static string WAITING_ROOM { get { return "WaitingRoom"; } }
        }

        public static class ToolList {

        }

        public static class WeaponList {

        }

        public static class DialogContent {
            public static string STATE_0 {
                 get {
                    return "Welcome To ";
                    }
                }
            public static string STATE_1 {
                get {
                    return "Tree is one of the Resources. Use Axe to Chop it down, \nyou will get the wood to upgrade the tools or unlock the objects";
                }
            }
            public static string STATE_2 {
                get {
                    return "Congratulations！You have learn how to collect the Resource. \nThere has 3 type of Resources. To discover, collect and build yourself.";
                }
            }
            public static string STATE_3 {
                get {
                    return "";
                }
            }
        }

        public static class UpgradeList {
            class AxeUpgrade {
                class level1 {
                    public readonly int woodNum = 10;
                    public readonly int ironNum = 10;
                    public readonly int stoneNum = 10;
                }
                class level2 {
                    public readonly int woodNum = 100;
                    public readonly int ironNum = 100;
                    public readonly int stoneNum = 100;
                }
            }
            class PickAxeUpgrade {
                class level1 {
                    public readonly int woodNum = 10;
                    public readonly int ironNum = 10;
                    public readonly int stoneNum = 10;
                }
                class level2 {
                    public readonly int woodNum = 100;
                    public readonly int ironNum = 100;
                    public readonly int stoneNum = 100;
                }
            }
            class ShovelUpgrade {
                class level1 {
                    public readonly int woodNum = 10;
                    public readonly int ironNum = 10;
                    public readonly int stoneNum = 10;
                }
                class level2 {
                    public readonly int woodNum = 100;
                    public readonly int ironNum = 100;
                    public readonly int stoneNum = 100;
                }
            }
        }
    }
}
