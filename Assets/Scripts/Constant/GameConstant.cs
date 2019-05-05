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
                    return "Welcome To Wanderer! \nIn this tutorial mode we will introduce how to Collect resources and Attack. You can then move on to multi-player. \n\n\nSlide your finger to right to move on to next action.";
                    }
                }
            public static string STATE_1 {
                get {
                    return "Please find the Axe nearby by walking to it. \nIt will be your melee weapon and can be used with chopping gesture.";
                    //return "Tree is one of the Resources. Use Axe to Chop it down, \nyou will get the wood to upgrade the tools or unlock the objects";
                }
            }
            public static string STATE_2 {
                get {
                    return "Now find a tree and starting chopping it. \nIt will become wood material which you can pick up just like the Axe";
                    //return "Congratulations！You have learn how to collect the Resource. \nThere has 3 type of Resources. To discover, collect and build yourself.";
                }
            }
            public static string STATE_3 {
                get {
                    return "Nice! You have learnt how to collect Resources. \nNow we will move on to Gun usage. Find and pickup a Gun nearby";
                    //return "";
                }
            }
            public static string STATE_4
            {
                get
                {
                    return "Now that you have picked up a Gun, use it to shoot the Animals. \n\nPerform the gun shooting gesture aiming toward an Animal.";
                }
            }
            public static string STATE_5
            {
                get
                {
                    return "Awesome! You have finished the Tutorial Mode. \nTo move on point your face palm towardsd yourself and click Quit button on the Palm Menu";
                    //return "Now that you have picked up a Gun, use it to shoot the Animals. Perform the gun shooting gesture aiming toward an Animal.";
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
