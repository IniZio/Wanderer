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
