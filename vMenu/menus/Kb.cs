using MenuAPI;

using CitizenFX.Core;

namespace vMenuClient.menus
{
    public class Kb : BaseScript
    {
        private Menu menu;

        private MenuItem Key1;
        private MenuItem Key2;
        private MenuItem Key3;
        private MenuItem Key4;

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

            Key1 = new MenuItem("F1 Open vMenu", "This will open up vMenu");
            Key2 = new MenuItem("F2 No-clip", "This will enable Noclip");
            Key3 = new MenuItem("F3 Open Dyno Menu", "Opens the Dyno UI");
            Key4 = new MenuItem("F4 Open Engine Manager", "Opens Engine Manager");

            menu.AddMenuItem(version);
            menu.AddMenuItem(credits);
            menu.AddMenuItem(Key1);
            menu.AddMenuItem(Key2);
            menu.AddMenuItem(Key3);
            menu.AddMenuItem(Key4);

            // event triggers
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == Key1)
                {
                    TriggerEvent("");
                }
                else if (item == Key2)
                {
                    TriggerEvent("txcl:setPlayerMode", "noclip", true);
                }
                else if (item == Key3)
                {
                    TriggerEvent("");
                }
                else if (item == Key4)
                {
                    TriggerEvent("");
                }
            };
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
