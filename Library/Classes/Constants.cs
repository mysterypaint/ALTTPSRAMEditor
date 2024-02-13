// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace Library.Classes;

public static class Constants
{
    public const int srm_size = 0x2000;
    public const int srm_randomizer_size = 16 * 1024;
    public const int srm_randomizer_size_2 = 32 * 1024;

    // Various events
    public const int BirdStatueKakarikoAddress = 0x298; // Freed from Statue if set to 0x20 (Maybe?)

    // Items and Equipment (All these values are relative, just subtract 0x340 from their actual SRAM values
    public const int BowAddress = 0x0;
    public const int BoomerangAddress = 0x1;
    public const int HookshotAddress = 0x2;
    public const int BombCountAddress = 0x3;
    public const int MushroomPowderAddress = 0x4;
    public const int FireRodAddress = 0x5;
    public const int IceRodAddress = 0x6;
    public const int BombosMedallionAddress = 0x7;
    public const int EtherMedallionAddress = 0x8;
    public const int QuakeMedallionAddress = 0x9;
    public const int LampAddress = 0xA;
    public const int MagicHammerAddress = 0xB;
    public const int ShovelFluteAddress = 0xC;
    public const int BugNetAddress = 0xD;
    public const int BookAddress = 0xE;
    public const int BottleAddress = 0xF;
    public const int CaneOfSomariaAddress = 0x10;
    public const int CaneOfByrnaAddress = 0x11;
    public const int MagicCapeAddress = 0x12;
    public const int MagicMirrorAddress = 0x13;
    public const int GlovesAddress = 0x14;
    public const int PegasusBootsAddress = 0x15;
    public const int ZorasFlippersAddress = 0x16;
    public const int MoonPearlAddress = 0x17;
    public const int SwordAddress = 0x19;
    public const int ShieldAddress = 0x1A;
    public const int ArmorAddress = 0x1B;
    public const int Bottle1ContentsAddress = 0x1C;
    public const int Bottle2ContentsAddress = 0x1D;
    public const int Bottle3ContentsAddress = 0x1E;
    public const int Bottle4ContentsAddress = 0x1F;
    public const int WalletAddress = 0x20; // 2 bytes
    public const int RupeesAddress = 0x22; // 2 bytes

    public const int AbilityFlagsAddress = 0x39;
    public const int ArrowCountAddress = 0x37;
    public const int BombUpgradesAddress = 0x30;
    public const int ArrowUpgradesAddress = 0x31;
    public const int MagicPowerAddress = 0x2E;
    public const int MagicLeftToFillAddress = 0x33;
    public const int MagicUpgradesAddress = 0x3B;
    public const int HeartPiecesAddress = 0x2B;
    public const int MaxHeartsAddress = 0x2C;
    public const int CurrHeartsAddress = 0x2D;

    public const int GreenPendantAddress = 0x2;
    public const int BluePendantAddress = 0x1;
    public const int RedPendantAddress = 0x0;

    public const int CrystalPoD = 0x1;
    public const int CrystalSPAddress = 0x4;
    public const int CrystalSWAddress = 0x6;
    public const int CrystalTTAddress = 0x5;
    public const int CrystalIPAddress = 0x2;
    public const int CrystalMMAddress = 0x0;
    public const int CrystalTRAddress = 0x3;
}
