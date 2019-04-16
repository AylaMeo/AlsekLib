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
        
    }
}