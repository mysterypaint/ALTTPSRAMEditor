// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace Library.Classes;

public static class Constants
{
    public const int srm_size = 0x2000;
    public const int srm_randomizer_size = 16 * 1024;
    public const int srm_randomizer_size_2 = 32 * 1024;

    // Various events
    public const int birdStatueKakariko = 0x298; // Freed from Statue if set to 0x20 (Maybe?)

    // Items and Equipment (All these values are relative, just subtract 0x340 from their actual SRAM values
    public const int bow = 0x0;
    public const int boomerang = 0x1;
    public const int hookshot = 0x2;
    public const int bombCount = 0x3;
    public const int mushroomPowder = 0x4;
    public const int fireRod = 0x5;
    public const int iceRod = 0x6;
    public const int bombosMedallion = 0x7;
    public const int etherMedallion = 0x8;
    public const int quakeMedallion = 0x9;
    public const int lamp = 0xA;
    public const int magicHammer = 0xB;
    public const int shovelFlute = 0xC;
    public const int bugNet = 0xD;
    public const int book = 0xE;
    public const int bottle = 0xF;
    public const int caneOfSomaria = 0x10;
    public const int caneOfByrna = 0x11;
    public const int magicCape = 0x12;
    public const int magicMirror = 0x13;
    public const int gloves = 0x14;
    public const int pegasusBoots = 0x15;
    public const int zorasFlippers = 0x16;
    public const int moonPearl = 0x17;
    public const int skipthis = 0x18;
    public const int sword = 0x19;
    public const int shield = 0x1A;
    public const int armor = 0x1B;
    public const int bottle1Contents = 0x1C;
    public const int bottle2Contents = 0x1D;
    public const int bottle3Contents = 0x1E;
    public const int bottle4Contents = 0x1F;
    public const int wallet = 0x20; // 2 bytes
    public const int rupees = 0x22; // 2 bytes

    public const int abilityFlags = 0x39;
    public const int arrowCount = 0x37;
    public const int bombUpgrades = 0x30;
    public const int arrowUpgrades = 0x31;
    public const int magicPower = 0x2E;
    public const int magicUpgrades = 0x3B;
    public const int maxHearts = 0x2C;
    public const int currHearts = 0x2D;

    public const int greenPendant = 0x2;
    public const int bluePendant = 0x1;
    public const int redPendant = 0x0;

    public const int crystalPoD = 0x1;
    public const int crystalSP = 0x4;
    public const int crystalSW = 0x6;
    public const int crystalTT = 0x5;
    public const int crystalIP = 0x2;
    public const int crystalMM = 0x0;
    public const int crystalTR = 0x3;
}
