using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;


namespace AlsekLib
{
    public class VehiclesLib : BaseScript
    {
        #region SpawnVehicle
        
        public static async Task<int> SpawnVehicle(string VehicleName, Vector3 SpawnCoords, float SpawnHeading)
        {
            var NameHash = (uint)GetHashKey(VehicleName);
            uint VehicleHash = NameHash; 
            {
                bool successFull = await CommonFunctionsLib.ModelLoader(VehicleHash, VehicleName);
                if (!successFull || !IsModelAVehicle(VehicleHash))
                {
                    // Vehicle model is invalid.
                    if (CommonFunctionsLib.DebugMode)
                    {
                        Screen.ShowNotification($"~b~Debug~s~: Vehicle model is invalid. {VehicleName}!");
                    }
                    return 0;
                }
                else
                {
                    if (CommonFunctionsLib.DebugMode)
                    {
                        Screen.ShowNotification($"~b~Debug~s~: Valid, Vehicle will spawn {VehicleName}!");
                    }
                    var Vehicle = CreateVehicle(VehicleHash, SpawnCoords.X, SpawnCoords.Y, SpawnCoords.Z, SpawnHeading, true, false);
                    return Vehicle;
                }
            }
        }
        #endregion
        
        //Applies random vehicle mods to the inserted vehicle TODO: Update this to include more mods
        #region RandomVehicleMods
        public static async Task RandomVehicleMods(int Vehicle)
        {
            var randomNumberColor = CommonFunctionsLib.IntUtil.Random(0,159);
            SetVehicleColours(Vehicle, randomNumberColor, randomNumberColor);
            
            await Delay(0);
            SetVehicleModKit(Vehicle, 0);
            var VehicleMod0 = GetNumVehicleMods(Vehicle, 0);
            var VehicleMod1 = GetNumVehicleMods(Vehicle, 1);
            var VehicleMod2 = GetNumVehicleMods(Vehicle, 2);
            var VehicleMod3 = GetNumVehicleMods(Vehicle, 3);
            var VehicleMod4 = GetNumVehicleMods(Vehicle, 4);
            var VehicleMod5 = GetNumVehicleMods(Vehicle, 5);
            var VehicleMod6 = GetNumVehicleMods(Vehicle, 6);
            var VehicleMod7 = GetNumVehicleMods(Vehicle, 7);
            var VehicleMod8 = GetNumVehicleMods(Vehicle, 8);
            var VehicleMod9 = GetNumVehicleMods(Vehicle, 9);
            var VehicleMod10 = GetNumVehicleMods(Vehicle, 10);
            var VehicleMod46 = GetNumVehicleMods(Vehicle, 46);
            var VehicleMod48 = GetNumVehicleMods(Vehicle, 48);
            var randomNumberMod = 0;
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod0);
            SetVehicleMod(Vehicle, 0, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod1);
            SetVehicleMod(Vehicle, 1, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod2);
            SetVehicleMod(Vehicle, 2, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod3);
            SetVehicleMod(Vehicle, 3, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod4);
            SetVehicleMod(Vehicle, 4, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod5);
            SetVehicleMod(Vehicle, 5, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod6);
            SetVehicleMod(Vehicle, 6, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod7);
            SetVehicleMod(Vehicle, 7, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod8);
            SetVehicleMod(Vehicle, 8, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod9);
            SetVehicleMod(Vehicle, 9, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod10);
            SetVehicleMod(Vehicle, 10, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod46);
            SetVehicleMod(Vehicle, 46, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,VehicleMod48);
            SetVehicleMod(Vehicle, 48, randomNumberMod, false);
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,7);
            SetVehicleWindowTint(Vehicle, randomNumberMod);
        }
        #endregion
        
    }
}