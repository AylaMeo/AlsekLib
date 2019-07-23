using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlsekLibShared;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace AlsekLib
{
    public class CommonFunctionsLib : BaseScript
    {
        private List<Vehicle> vehiclesList = new List<Vehicle>();
        public Vector4 getVehicleList(bool police, bool movingCheck)
        {
            if (police)
            {
                vehiclesList = World.GetAllVehicles().Where(e => VehicleClassCheck(e.Handle, true) && CheckAllPlayers(e.Handle)).ToList();
            }
            if (police && movingCheck)
            {
                vehiclesList = World.GetAllVehicles().Where(e => VehicleClassCheck(e.Handle, true) && CheckAllPlayers(e.Handle) && CheckMovingCoords(e.Handle)).ToList();
            }
            if (movingCheck)
            {
                vehiclesList = World.GetAllVehicles().Where(e => VehicleClassCheck(e.Handle, false) && CheckAllPlayers(e.Handle) && CheckMovingCoords(e.Handle)).ToList();
            }
            else
            {
                vehiclesList = World.GetAllVehicles().Where(e => VehicleClassCheck(e.Handle, false) && CheckAllPlayers(e.Handle)).ToList();
            }
            
            var random = new Random();
            int index = random.Next(vehiclesList.Count);
            /*if(vehiclesList.Count > 1)
                vehiclesList.RemoveRange(1, vehiclesList.Count - 1);
            */
            float TargetHeading = new float();
            Vector3 TargetCoords = new Vector3();

            var TargetVehicle = vehiclesList[index].Handle;
            SetEntityAsMissionEntity(TargetVehicle, true, true);
            var MaxSeats = GetVehicleMaxNumberOfPassengers(TargetVehicle);
            var ClearingSeats = -1;
            do
            {
                var TargetPed = GetPedInVehicleSeat(TargetVehicle, ClearingSeats);
                DeletePed(ref TargetPed);
                ClearingSeats++;
            } while (ClearingSeats < MaxSeats);
                
            TargetCoords = GetEntityCoords(TargetVehicle, true);
            TargetHeading = GetEntityHeading(TargetVehicle);
            DeleteVehicle(ref TargetVehicle);
            var returnValue = new Vector4(TargetCoords, TargetHeading);
            return returnValue;
            
            /*foreach (Vehicle v in vehiclesList)
            {
                var TargetVehicle = v.Handle;
                SetEntityAsMissionEntity(TargetVehicle, true, true);
                var MaxSeats = GetVehicleMaxNumberOfPassengers(TargetVehicle);
                var ClearingSeats = -1;
                do
                {
                    var TargetPed = GetPedInVehicleSeat(TargetVehicle, ClearingSeats);
                    DeletePed(ref TargetPed);
                    ClearingSeats++;
                } while (ClearingSeats < MaxSeats);
                
                TargetCoords = GetEntityCoords(TargetVehicle, true);
                TargetHeading = GetEntityHeading(TargetVehicle);
                DeleteVehicle(ref TargetVehicle);
                var returnValue = new Vector4(TargetCoords, TargetHeading);
                return returnValue;
            }
            return new Vector4(new Vector3(0, 0,0), 0); */
        }
        
        public bool CheckAllPlayers(int TargetVehicle)
        {
            foreach (Player p in Players)
            {
                if (NetworkIsPlayerActive(p.Handle))
                {
                    int playerPed = GetPlayerPed(p.Handle);
                    var playerPedCoords = GetEntityCoords(playerPed, true);
                    var LocationVec = GetEntityCoords(TargetVehicle, true);

                    if (GetDistanceBetweenCoords(playerPedCoords.X, playerPedCoords.Y, playerPedCoords.Z,
                            LocationVec.X, LocationVec.Y, LocationVec.Z, true) > 50 && !IsEntityOnScreen(TargetVehicle))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        
        public static bool VehicleClassCheck(int Vehicle, bool police)
        {
            if (police)
            {
                var VehicleClass = GetVehicleClass(Vehicle);
                if (VehicleClass != 13 & VehicleClass != 14 & VehicleClass != 15 & VehicleClass != 16 & VehicleClass != 21)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var VehicleClass = GetVehicleClass(Vehicle);
                if (VehicleClass == 1 || VehicleClass == 2 || VehicleClass == 3 || VehicleClass == 4 || VehicleClass == 5 ||
                    VehicleClass == 6 || VehicleClass == 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        #region ModelLoader
        /// <summary>
        /// Verifies a model and loads it
        /// </summary>
        /// <param name="ModelHash"></param> ModelName is the string name of the model, used only for debug msgs.
        /// <param name="ModelName"></param> ModelHash is the hash of the model this is being requested to spawn.
        /// <returns></returns>
        public static async Task<bool> ModelLoader(uint ModelHash, string ModelName)
        {
            // Check if the model exists in the game.
            if (IsModelInCdimage(ModelHash))
            {
                if (DebugLog.DebugMode)
                {
                    //Debug.Write($"AlsekLib: Valid, loading model {ModelName}!");
                    DebugLog.Log($"AlsekLib: Valid, loading model {ModelName}!", false, false, DebugLog.LogLevel.success);
                }
                // Load the model.
                RequestModel(ModelHash);
                // Wait until it's loaded.
                while (!HasModelLoaded(ModelHash))
                {
                    await BaseScript.Delay(0);
                }
                // Model is loaded, return true.
                return true;
            }
            // Model is not valid or is not loaded correctly.
            else
            {
                if (DebugLog.DebugMode)
                {
                    //Debug.Write($"AlsekLib: Model Invalid {ModelName}!");
                    DebugLog.Log($"AlsekLib: Model Invalid {ModelName}!", false, false, DebugLog.LogLevel.error);
                }
                // Return false.
                return false;
            }
        }
        #endregion
        
        #region StringToVector3
        /// <summary>
        /// Takes a string (Example 313,543,42) and converts it to vector3
        /// </summary>
        /// <param name="sVector"></param> 
        /// <returns></returns>
        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
                sVector = sVector.Substring(1, sVector.Length-2);
            }
 
            // split the items
            string[] sArray = sVector.Split(',');
 
            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));
 
            return result;
        }
        #endregion
        
        #region SeparateLocation
        /// <summary>
        /// This separates coordinates and heading from a single string (Example: 1132,523,42,180 (x,y,z,heading))
        /// </summary>
        /// <param name="sVector"></param>
        /// <returns></returns>
        public static Tuple<Vector3, float> SeparateLocation(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
                //sVector = sVector.Substring(1, sVector.Length-2);
            }
 
            // split the items
            string[] sArray = sVector.Split(',');
 
            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            float result2 = float.Parse(sArray[3]);
            
            return Tuple.Create(result,result2);
        }
        #endregion
        
        #region RandomNumber
        /// <summary>
        /// Creates a random number (More often to be actually "random" then other methods)
        /// </summary>
        public static class IntUtil
        {
            private static Random random;

            private static void Init()
            {
                if (random == null) random = new Random();
            }

            public static int Random(int min, int max)
            {
                Init();
                return random.Next(min, max);
            }
        }
        #endregion
        
        #region LocationTools
        
        #region getDistanceBetweenEntities
        /// <summary>
        /// Simply gets the distance between 2 entities
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <returns></returns>
        public static float getDistanceBetweenEntities(int entity1, int entity2)
        {
            Vector3 entity1Coords = GetEntityCoords(entity1, true);
            Vector3 entity2Coords = GetEntityCoords(entity2, true);
            return GetDistanceBetweenCoords(entity1Coords.X, entity1Coords.Y, entity1Coords.Z, entity2Coords.X, entity2Coords.Y, entity2Coords.Z, true);
        }
        #endregion
        
        #region CheckMovingCoords
        /// <summary>
        /// Checks if the target vehicle is infront of the player if the player is moving TODO: remove the speed requirement from this as it's contextual //TODO2: update anything that uses this to that
        /// </summary>
        /// <param name="TargetVehicle"></param>
        /// <returns></returns>
        public static bool CheckMovingCoords(int TargetVehicle)
        {
            if (GetVehicleDashboardSpeed(GetVehiclePedIsIn(PlayerPedId(), false)) > 25)
            {
                var TargetCoords = GetEntityCoords(TargetVehicle, true);
                
                Vector3 playerPos = GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 150, 0);
                var vector_p5 = Vector3.Zero;
                var vector_p6 = Vector3.Zero;
                var int_p7 = 0;
                var int_p8 = 0;
                var float_p9 = 0f;
                var returned_int = GetClosestRoad(playerPos.X, playerPos.Y, playerPos.Z, 1f, 1, ref vector_p5, ref vector_p6, ref int_p7, ref int_p8, ref float_p9, true);

                var LocationVec = vector_p5;
                
                if (GetDistanceBetweenCoords(TargetCoords.X, TargetCoords.Y, TargetCoords.Z, LocationVec.X,
                        LocationVec.Y, LocationVec.Z, true) < 100)
                {
                    return true;
                }
                else return false;

            }
            else return true;
        }
        #endregion
        
        #region GetNearestRoad
        /// <summary>
        /// Gets the nearest road either at the player, or Infront the player
        /// </summary>
        /// <param name="InfrontOfPlayer"></param> If true it gets 200m in front the player, if false gets nearest to player.
        /// <returns></returns>
        public static async Task<Vector3> GetNearestRoad(bool InfrontOfPlayer)
        {
            await BaseScript.Delay(0);
            
            Vector3 playerPos;
            if (InfrontOfPlayer)
            {
                playerPos = GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 200, 0);
            }
            else
            {
                playerPos = Game.PlayerPed.Position;
            }
            
            var vector_p5 = Vector3.Zero;
            var vector_p6 = Vector3.Zero;
            var int_p7 = 0;
            var int_p8 = 0;
            var float_p9 = 0f;
            var returned_int = GetClosestRoad(playerPos.X, playerPos.Y, playerPos.Z, 1f, 1, ref vector_p5, ref vector_p6, ref int_p7, ref int_p8, ref float_p9, true);

            if (DebugLog.DebugMode)
            {
                Debug.Write(
                    $"playerPos: [{playerPos}] | vector_p5 [{vector_p5}] | vector_p6 [{vector_p6}] | int_p7 [{int_p7}] | int_p8 [{int_p8}] | float_p9 [{float_p9}] | returned_int [{returned_int}]");

                var streetName = new uint();
                var crossingRoad = new uint();
                
                GetStreetNameAtCoord(vector_p5.X, vector_p5.Y, vector_p5.Z, ref streetName, ref crossingRoad);
                var stringStreetName = GetStreetNameFromHashKey(streetName);
                var stringCrossingRoad = GetStreetNameFromHashKey(crossingRoad);
                CitizenFX.Core.Debug.WriteLine(
                    $"GetStreetNameAtCoord using [vector_p5] | streetName [{streetName}] [{stringStreetName}] | crossingRoad [{crossingRoad}] [{stringCrossingRoad}]");
                
                GetStreetNameAtCoord(vector_p6.X, vector_p6.Y, vector_p6.Z, ref streetName, ref crossingRoad);
                stringStreetName = GetStreetNameFromHashKey(streetName);
                stringCrossingRoad = GetStreetNameFromHashKey(crossingRoad);
                CitizenFX.Core.Debug.WriteLine(
                    $"GetStreetNameAtCoord using [vector_p6] | streetName [{streetName}] [{stringStreetName}] | crossingRoad [{crossingRoad}] [{stringCrossingRoad}]");
            }

            return vector_p5;
        }
        #endregion
        
        #region GetVehicleInDir
        /// <summary>
        /// Gets the vehicle infront of the player
        /// </summary>
        /// <param name="OffSetY"></param> how far in front the player
        /// <returns></returns>
        public static int GetVehicleInDir(int OffSetY)
        {   
            Vector3 ForwardPosition = GetOffsetFromEntityInWorldCoords(GetVehiclePedIsIn(GetPlayerPed(PlayerId()), false), 0, OffSetY, 0);
            int player = GetPlayerPed(PlayerId());
            Vector3 playerCoords = GetEntityCoords(player, true);
            int rayCastPoint = CastRayPointToPoint(playerCoords.X, playerCoords.Y, playerCoords.Z, ForwardPosition.X, ForwardPosition.Y, ForwardPosition.Z, 10, player, 0);
            bool hit = false;
            Vector3 endCoords = new Vector3(0,0,0);
            Vector3 surfaceNormal = new Vector3(0,0,0);
            int entity = 0;
            GetRaycastResult(rayCastPoint, ref hit, ref endCoords, ref surfaceNormal, ref entity);
            return entity;
        }
        #endregion
        
        #endregion
    }
}