using CitizenFX.Core;
using CitizenFX.Core.Native;

using MenuAPI;

namespace vMenuClient.menus
{
    public class Kb : BaseScript
    {
        private Menu menu;


        // Separators
        private MenuItem KeySep1;
        private MenuItem KeySep2;

        // Keybinds (FuntionKeys)
        private MenuItem FunctionF1;
        private MenuItem FunctionF2;
        private MenuItem FunctionF3;
        private MenuItem FunctionF4;
        private MenuItem FunctionF9;

        // Keybinds (Keybinds)
        //private MenuItem replace_me;
        private MenuItem KeyLeftShift;
        private MenuItem KeyLeftControl;
        private MenuItem KeyR;
        private MenuItem KeyG;



        private void CreateMenu()
        {
            menu = new Menu("vMenu", "Car Haven Keybinds");

            var version = new MenuItem(
                "Car Haven Version",
                "Car Haven is on Version ~b~~h~v1.0.2~h~~s~.")
            {
                Label = "~h~v1.0.2~h~"
            };

            var credits = new MenuItem(
                "Car Haven Credits",
                "Car Haven is Lead Developed by @Aweetumn With help from @ricky3207 & @muttt_");

            KeySep1 = new MenuItem("=========================================", "");
            FunctionF1 = new MenuItem("Open vMenu", "This will open up vMenu")
            {
                Label = "F1"
            };
            FunctionF2 = new MenuItem("No-clip", "This will enable Noclip")
                {
                Label = "F2"
            };
            FunctionF3 = new MenuItem("Open Dyno Menu", "Opens the Dyno UI")
                {
                Label = "F3"
            };
            FunctionF4 = new MenuItem("Open Engine Manager", "Opens Engine Manager")
                {
                Label = "F4"
            };
            FunctionF9 = new MenuItem("Toggle Sepdometer", "This Opens/Closes the Spedometer when in a vehicle")
                {
                Label = "F9"
            };
            KeySep2 = new MenuItem("=========================================", "");
            //replace_me = new MenuItem("", "");
            KeyLeftShift = new MenuItem("Shift Up Gear", "If your vehicle has a manual transmission this is how you change your gear")
            {
                Label = "Left Shift"
            };
            KeyLeftControl = new MenuItem("Shift Down Gear", "If your vehicle has a manual transmission this is how you change your gear")
                {
                Label = "Left Control"
            };
            KeyR = new MenuItem("Pressing or Holding will activate Nitrous", "If your vehicle has nitrous equiped to the vehicle and your have nitrous in the tank you will utilise the nitrous")
                {
                Label = "R"
            };
            KeyG = new MenuItem("open Car Radio", "By pressing G you will open and utilise our car radio system")
                {
                Label = "G"
            };

            menu.AddMenuItem(version);
            menu.AddMenuItem(credits);
            // Separator
            menu.AddMenuItem(KeySep1);
            // Keybinds
            menu.AddMenuItem(FunctionF1);
            menu.AddMenuItem(FunctionF2);
            menu.AddMenuItem(FunctionF3);
            menu.AddMenuItem(FunctionF4);
            menu.AddMenuItem(FunctionF9);
            // Separator
            menu.AddMenuItem(KeySep2);
            //menu.AddMenuItem(replace_me);
            menu.AddMenuItem(KeyLeftShift);
            menu.AddMenuItem(KeyLeftControl);
            menu.AddMenuItem(KeyR);
            menu.AddMenuItem(KeyG);


            // event triggers
            //menu.OnItemSelect += (sender, item, index) =>
            //{
            //    if (item == Key1)
            //    {
            //        TriggerEvent("");
            //    }
            //    else if (item == Key2)
            //    {
            //        TriggerEvent("txcl:setPlayerMode", "noclip", true);
            //    }
            //    else if (item == Key3)
            //    {
            //        TriggerEvent("");
            //    }
            //    else if (item == Key4)
            //    {
            //        TriggerEvent("");
            //    }
            //};
        }

        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }
    }
}
