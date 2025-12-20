using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CitizenFX.Core;
using CitizenFX.Core.Native;

using MenuAPI;

using Newtonsoft.Json;

using vMenuClient.data;

using static CitizenFX.Core.Native.API;
using static vMenuClient.CommonFunctions;
using static vMenuShared.PermissionsManager;

namespace vMenuClient.menus
{
    public class VehicleSpawner
    {
        private Menu menu;
        private Menu addonMenu;
        private bool addonMenuAccessible = false;
        private MenuItem addonBtn;

        public static Dictionary<string, uint> AddonVehicles;
        public static List<bool> allowedCategories;

        public bool SpawnInVehicle { get; private set; } = UserDefaults.VehicleSpawnerSpawnInside;
        public bool ReplaceVehicle { get; private set; } = UserDefaults.VehicleSpawnerReplacePrevious;

        #region ADDON VEHICLE CONFIG (MERGED)

        private class AddonVehicleConfig
        {
            public Dictionary<int, string> Categories { get; set; }
            public List<AddonVehicleEntry> Vehicles { get; set; }
        }

        private class AddonVehicleEntry
        {
            public string SpawnName { get; set; }
            public string DisplayName { get; set; }
            public int Category { get; set; }
        }

        private static AddonVehicleConfig LoadAddonVehicleConfig()
        {
            try
            {   
                var json = LoadResourceFile("vMenu", "config/addon_vehicles.json");

                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<AddonVehicleConfig>(json);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        private void CreateMenu()
        {
            #region INITIAL MENU SETUP

            menu = new Menu(Game.Player.Name, "Vehicle Spawner");

            var spawnByName = new MenuItem(
                "Spawn Vehicle By Model Name",
                "Enter the name of a vehicle to spawn."
            );

            var spawnInVeh = new MenuCheckboxItem(
                "Spawn Inside Vehicle",
                "Teleport into vehicle on spawn.",
                SpawnInVehicle
            );

            var replacePrev = new MenuCheckboxItem(
                "Replace Previous Vehicle",
                "Delete previously spawned vehicle.",
                ReplaceVehicle
            );

            if (IsAllowed(Permission.VSSpawnByName))
                menu.AddMenuItem(spawnByName);

            menu.AddMenuItem(spawnInVeh);
            menu.AddMenuItem(replacePrev);

            #endregion

            #region ADDON VEHICLES

            var addonConfig = LoadAddonVehicleConfig();

            addonBtn = new MenuItem(
                "Addon Vehicles",
                "Spawn server-streamed addon vehicles"
            )
            { Label = "→→→" };

            menu.AddMenuItem(addonBtn);

            bool hasPermission = IsAllowed(Permission.VSAddon);

            if (!hasPermission || addonConfig == null)
            {
                addonBtn.Enabled = false;
                addonBtn.LeftIcon = MenuItem.Icon.LOCK;
            }
            else
            {
                addonMenuAccessible = true;

                // Create the addon menu
                addonMenu = new Menu("Addon Vehicles", "Spawn custom addon vehicles");

                MenuController.AddSubmenu(menu, addonMenu);
                MenuController.BindMenuItem(menu, addonMenu, addonBtn);

                var categoryMenus = new Dictionary<int, Menu>();

                if (addonConfig.Vehicles != null)
                {
                    foreach (var veh in addonConfig.Vehicles)
                    {
                        var hash = (uint)GetHashKey(veh.SpawnName);

                        if (!categoryMenus.TryGetValue(veh.Category, out var categoryMenu))
                        {
                            var categoryName =
                                addonConfig.Categories != null &&
                                addonConfig.Categories.ContainsKey(veh.Category)
                                    ? addonConfig.Categories[veh.Category]
                                    : $"Category {veh.Category}";

                            categoryMenu = new Menu("Addon Vehicles", categoryName);
                            categoryMenus.Add(veh.Category, categoryMenu);

                            var catBtn = new MenuItem(categoryName) { Label = "→→→" };
                            addonMenu.AddMenuItem(catBtn);

                            MenuController.AddSubmenu(addonMenu, categoryMenu);
                            MenuController.BindMenuItem(addonMenu, categoryMenu, catBtn);
                        }

                        var vehBtn = new MenuItem(veh.DisplayName)
                        {
                            Label = $"({veh.SpawnName})",
                            ItemData = veh.SpawnName
                        };

                        if (!IsModelInCdimage(hash))
                        {
                            vehBtn.Enabled = false;
                            vehBtn.LeftIcon = MenuItem.Icon.LOCK;
                            vehBtn.Description = "Vehicle not streamed correctly.";
                        }

                        categoryMenu.AddMenuItem(vehBtn);
                    }
                }

                foreach (var kvp in categoryMenus)
                {
                    kvp.Value.OnItemSelect += async (m, item, index) =>
                    {
                        await SpawnVehicle(
                            item.ItemData.ToString(),
                            SpawnInVehicle,
                            ReplaceVehicle
                        );
                    };
                }
            }

            #endregion

            #region STOCK VEHICLE CLASSES

            for (var vehClass = 0; vehClass < 23; vehClass++)
            {
                var className = GetLabelText($"VEH_CLASS_{vehClass}");

                var btn = new MenuItem(
                    className,
                    $"Spawn a vehicle from the {className} class."
                )
                { Label = "→→→" };

                var vehicleClassMenu = new Menu("Vehicle Spawner", className);

                menu.AddMenuItem(btn);
                MenuController.AddSubmenu(menu, vehicleClassMenu);

                if (!allowedCategories[vehClass])
                {
                    btn.Enabled = false;
                    btn.LeftIcon = MenuItem.Icon.LOCK;
                    continue;
                }

                MenuController.BindMenuItem(menu, vehicleClassMenu, btn);

                foreach (var veh in VehicleData.Vehicles.VehicleClasses[className])
                {
                    var displayName =
                        GetVehDisplayNameFromModel(veh) != "NULL"
                            ? GetVehDisplayNameFromModel(veh)
                            : veh;

                    var item = new MenuItem(displayName)
                    {
                        Label = $"({veh.ToLower()})",
                        ItemData = veh
                    };

                    if (!DoesModelExist(veh))
                    {
                        item.Enabled = false;
                        item.RightIcon = MenuItem.Icon.LOCK;
                    }

                    vehicleClassMenu.AddMenuItem(item);
                }

                vehicleClassMenu.OnItemSelect += async (m, item, index) =>
                {
                    await SpawnVehicle(
                        item.ItemData.ToString(),
                        SpawnInVehicle,
                        ReplaceVehicle
                    );
                };
            }

            #endregion

            #region EVENTS

            menu.OnItemSelect += async (sender, item, index) =>
            {
                if (item == spawnByName)
                {
                    await SpawnVehicle("custom", SpawnInVehicle, ReplaceVehicle);
                }
            };

            menu.OnCheckboxChange += (sender, item, index, value) =>
            {
                if (item == spawnInVeh)
                    SpawnInVehicle = value;
                else if (item == replacePrev)
                    ReplaceVehicle = value;
            };

            #endregion
        }

        public Menu GetMenu()
        {
            if (menu == null)
                CreateMenu();

            return menu;
        }

        public Menu GetAddonMenu()
        {
            if (menu == null)
                CreateMenu();

            if (addonMenuAccessible && addonMenu != null)
            {
                return addonMenu;
            }
            else
            {
                return null;
            }
        }

        public void OpenAddonMenuDirect()
        {
            var menuToOpen = GetAddonMenu();

            if (menuToOpen != null)
            {
                menuToOpen.OpenMenu();
            }
            else
            {
                GetMenu().OpenMenu();
            }
        }
    }
}