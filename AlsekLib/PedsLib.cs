using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core;

namespace AlsekLib
{
    public class PedsLib
    {
        #region SpawnPed
        public static async Task<int> SpawnPed(string PedVariable, bool inVehicle, int Vehicle)
        {
            var PedHash = (uint)GetHashKey(PedVariable);
            bool successFullPed = await CommonFunctionsLib.ModelLoader(PedHash, PedVariable);
            if (!successFullPed)
            {
                // Ped model is invalid.
                if (CommonFunctionsLib.DebugMode)
                {
                    //Debug.Write($"AlsekLib: Model invalid, Ped not spawning {PedVariable}!");
                    DebugLog.Log($"AlsekLib: Model invalid, Ped not spawning {PedVariable}!",false, DebugLog.LogLevel.error);
                }
                return 0;
            }
            else
            {
                if (CommonFunctionsLib.DebugMode)
                {
                    //Debug.Write($"AlsekLib: Model valid, Ped will spawn {PedVariable}!");
                    DebugLog.Log($"AlsekLib: Model valid, Ped will spawn {PedVariable}!",false, DebugLog.LogLevel.success);
                }
                if (inVehicle && AreAnyVehicleSeatsFree(Vehicle))
                {
                    var SeatFree = false;
                    var SeatFreeNum = -1;
                    var MaxSeats = GetVehicleMaxNumberOfPassengers(Vehicle);
                    do
                    {
                        if (!IsVehicleSeatFree(Vehicle, SeatFreeNum))
                        {
                            ++SeatFreeNum;
                            if (CommonFunctionsLib.DebugMode)
                            {
                                //Debug.Write($"Seatfree:: {SeatFreeNum}");
                                DebugLog.Log($"Seatfree:: {SeatFreeNum}",false, DebugLog.LogLevel.info);
                            }
                        }
                        else
                        {
                            SeatFree = true;
                            var NPC = CreatePedInsideVehicle(Vehicle, 1, PedHash, SeatFreeNum, true, true);
                            return NPC;
                        }
                        
                    } while (!SeatFree && SeatFreeNum < MaxSeats);
                }

                if (inVehicle && AreAnyVehicleSeatsFree(Vehicle) == false)
                {
                    //failed
                    return 0;
                }
                else
                {
                    //Make this area?
                    return 0;
                }
            }
        }
        #endregion

        #region PedSettings

        public static void PedSettings(int Ped, string PedWeapon, string PedNameString, uint Relations) //Ped is the the variable of the specific entity that this is being called on. PedWeapon is the weapon it is to get(example: WEAPON_SMG_MK2). PedNameString is the string name of the ped being spawned, used only for debug msgs.
        {
            if (CommonFunctionsLib.DebugMode)
            {
                //Debug.Write($"AlsekLib: Applying settings to ped:{PedNameString}");
                DebugLog.Log($"AlsekLib: Applying settings to ped:{PedNameString}",false, DebugLog.LogLevel.info);
            }
            
            var WeaponHash = (uint)GetHashKey(PedWeapon);
            SetPedShootRate(Ped, 700);
            AddArmourToPed(Ped, GetPlayerMaxArmour(Ped) - GetPedArmour(Ped));
            SetPedAlertness(Ped, 100);
            SetPedAccuracy(Ped, 100);
            SetPedCanSwitchWeapon(Ped, true);
            SetEntityHealth(Ped, 200);
            SetPedFleeAttributes(Ped, 2, true);
            SetPedCombatAttributes(Ped, 46, true);
            SetPedCombatAbility(Ped, 2);
            SetPedCombatRange(Ped, 50);
            SetPedPathAvoidFire(Ped, true);
            SetPedPathCanUseLadders(Ped, true);
            SetPedPathCanDropFromHeight(Ped, true);
            SetPedPathPreferToAvoidWater(Ped, true);
            SetPedGeneratesDeadBodyEvents(Ped, true);
            GiveWeaponToPed(Ped, WeaponHash, 5000, true, true);
            SetPedRelationshipGroupHash(Ped, Relations); // Hash is from https://gtaforums.com/topic/853451-request-hash-values-for-relationship-groups/ //
        }
        #endregion
    }
}