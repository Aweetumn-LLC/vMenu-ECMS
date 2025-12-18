using CitizenFX.Core;
using CitizenFX.Core.Native;

using MenuAPI;

namespace vMenuClient.menus
{
    public class QuickActions : BaseScript
    {
        private Menu menu;

        private MenuItem Replace;



        private void CreateMenu()
        {
            menu = new Menu("vMenu", "Car Haven Actions");

            Replace = new MenuItem("Placeholder", "Placeholder")
            {
                Label = ""
            };

            // Separator
            menu.AddMenuItem(Replace);


            // event triggers
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == Replace)
                {
                    TriggerEvent("");
                }
                //else if (item == Key2)
                //{
                //    TriggerEvent("txcl:setPlayerMode", "noclip", true);
                //}
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
