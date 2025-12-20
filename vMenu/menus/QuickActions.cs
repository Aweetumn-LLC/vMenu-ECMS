using CitizenFX.Core;
using CitizenFX.Core.Native;

using MenuAPI;

using static CitizenFX.Core.Native.API;
using static vMenuClient.data.VehicleData;

namespace vMenuClient.menus
{
    public class QuickActions : BaseScript
    {
        private Menu menu;
        private MenuItem RepairVehicle;
        private MenuItem OpenRadio;
        private MenuItem VehicleSpawner;

        private void CreateMenu()
        {
            menu = new Menu("vMenu", "Car Haven Actions");

            RepairVehicle = new MenuItem("Repair Vehicle", "This repairs your vehicle to full health.")
            {
                Label = ""
            };

            OpenRadio = new MenuItem("Open Radio", "This will open the radio within your Vehicles.")
            {
                Label = ""
            };

            VehicleSpawner = new MenuItem("Open Addon Vehicles", "Spawn custom addon vehicles!")
            {
                Label = ""
            };

            menu.AddMenuItem(RepairVehicle);
            menu.AddMenuItem(VehicleSpawner);
            menu.AddMenuItem(OpenRadio);

            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == RepairVehicle)
                {
                    int veh = API.GetVehiclePedIsIn(API.PlayerPedId(), false);

                    if (veh != 0)
                    {
                        API.SetVehicleFixed(veh);
                        API.SetVehicleDeformationFixed(veh);
                        API.SetVehicleDirtLevel(veh, 0f);
                        API.SetVehicleEngineHealth(veh, 1000f);
                    }
                }
                else if (item == OpenRadio)
                {
                    API.ExecuteCommand("openradiocarG");
                }
                else if (item == VehicleSpawner)
                {
                    var vehicleSpawner = new VehicleSpawner();
                    MenuController.CloseAllMenus();
                    vehicleSpawner.OpenAddonMenuDirect();
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