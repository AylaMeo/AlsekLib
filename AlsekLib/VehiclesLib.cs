using System.Threading.Tasks;
using AlsekLibShared;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace AlsekLib
{
    public class VehiclesLib
    {
        //GetVehicle function, gets vehicle ped is in
        #region getVehicle
        public static Vehicle GetVehicle(bool lastVehicle = false)
        {
            if (lastVehicle)
            {
                return Game.PlayerPed.LastVehicle;
            }
            else
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    return Game.PlayerPed.CurrentVehicle;
                }
            }
            return null;
        }
        #endregion
            
        //spawnVehicle function, returns a cfx "vehicle". vehicleName = model name of vehicle (example:"adder") pos = vector3 coords for spawn, spawnHeading = float for vehicle heading when spawned.
        #region spawnVehicle
        public static async Task<Vehicle> SpawnVehicle(string vehicleName, Vector3 pos, float spawnHeading)
        {
            //makes it return 0 if fails
            Vehicle vehicle = new Vehicle(0);
            //gets the name hash
            var nameHash = (uint)GetHashKey(vehicleName);
            uint vehicleHash = nameHash;
            //loads the vehicle model
            bool successFull = await CommonFunctionsLib.ModelLoader(vehicleHash, vehicleName);
            if (!successFull || !IsModelAVehicle(vehicleHash))
            {
                // Vehicle model is invalid.
                if (DebugLog.DebugMode)
                {
                    //Debug.Write($"AlsekLib: Model invalid, Vehicle will not spawn. {vehicleName}!");
                    DebugLog.Log($"AlsekLib: Model invalid, Vehicle will not spawn. {vehicleName}!",false, false, DebugLog.LogLevel.error);
                }
                //returns the 0
                return vehicle;
            }
            else
            {
                if (DebugLog.DebugMode)
                {
                    //Debug.Write($"AlsekLib: Model valid, Vehicle will spawn {vehicleName}!");
                    DebugLog.Log($"AlsekLib: Model valid, Vehicle will spawn {vehicleName}!",false, false, DebugLog.LogLevel.success);
                }
                //actually creates the vehicle
                vehicle = new Vehicle(CreateVehicle(vehicleHash, pos.X, pos.Y, pos.Z + 1f, spawnHeading, true, false))
                {
                    NeedsToBeHotwired = false,
                    PreviouslyOwnedByPlayer = true,
                    IsPersistent = true,
                    IsStolen = false,
                    IsWanted = false
                };
                return vehicle;
            }
        }
        #endregion
        
        //Applies random vehicle mods to the inserted vehicle
        #region randomVehicleMods
        public static async Task RandomVehicleMods(Vehicle vehicle, bool allowHorn)
        {
            await BaseScript.Delay(0);
            int randomNumberMod = 0;
            
            //applies random vehicle paint
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,159);
            SetVehicleColours(vehicle.Handle, randomNumberMod, randomNumberMod);
            //applies random window tint (Disabled allowing the oldgen window tints (limo/green))
            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,4);
            SetVehicleWindowTint(vehicle.Handle, randomNumberMod);
            //sets the modkit to 0 so the vehicle can be modified
            SetVehicleModKit(vehicle.Handle, 0);
            // Get all mods available on this vehicle.
            VehicleMod[] mods = vehicle.Mods.GetAllMods();
            // Loop through all the mods.
            foreach (var mod in mods)
            {
                // Get the current item index ({current}/{max upgrades})
                var currentItem = $"[1/{ mod.ModCount + 1}]";
                // Loop through all available upgrades for this specific mod type.
                for (var x = 0; x < mod.ModCount; x++)
                {
                    //if allowHorn = true, allows the horn to be changed, if allowHorn = false it stops the horn from being changed.
                    if (allowHorn)
                    {
                        randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,x);
                        SetVehicleMod(vehicle.Handle, x, randomNumberMod, false);
                    }
                    else
                    {
                        if (x == 14)
                        {
                            break;
                        }
                        else
                        {
                            randomNumberMod = CommonFunctionsLib.IntUtil.Random(0,x);
                            SetVehicleMod(vehicle.Handle, x, randomNumberMod, false);
                        }
                    }   
                }
            }
        }
        #endregion
        
    }
}