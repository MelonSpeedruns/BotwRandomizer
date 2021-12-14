using System;
using System.Collections.Generic;

namespace BotwRandoLib
{
    internal class BotwObjects
    {
        public Dictionary<List<string>, bool> OverworldObjects;

        public BotwObjects()
        {
            OverworldObjects = GenerateOverworldObjectList();
        }

        private static Dictionary<List<string>, bool> GenerateOverworldObjectList()
        {
            Dictionary<List<string>, bool> objectList = new Dictionary<List<string>, bool>();

            List<string> swordList = GenerateSwordList();
            List<string> lSwordList = GenerateLSwordList();
            List<string> spearList = GenerateSpearList();
            List<string> shieldList = GenerateShieldList();
            List<string> bowList = GenerateBowList();
            List<string> enemyList = GenerateEnemyList();
            List<string> keeseList = GenerateKeeseList();
            List<string> keeseAllDayList = GenerateKeeseAllDayList();
            List<string> chuchulist = GenerateChuchuList();
            List<string> littleGolemsList = GenerateLittleGolemList();
            List<string> subBossList = GenerateSubBossList();
            List<string> lynelList = GenerateLynelList();
            List<string> enemyGuardList = GenerateEnemyGuardList();
            List<string> enemyGuardAmbushList = GenerateEnemyGuardAmbushList();
            List<string> enemyTreehouseGuardList = GenerateEnemyTreeHouseGuardList();
            List<string> singleArrowList = GenerateSingleArrowList();
            List<string> wizzrobeList = GenerateWizzrobeList();
            List<string> sandwormList = GenerateSandwormList();
            List<string> staticGuardianList = GenerateStaticGuardianList();
            List<string> movingGuardianList = GenerateMovingGuardianList();
            List<string> miniGuardianList = GenerateMiniGuardianList();
            List<string> octorokList = GenerateOctorokList();
            List<string> materialList = GenerateMaterialList();
            List<string> meatList = GenerateMeatList();
            List<string> mannequinList = GenerateMannequinList();
            List<string> mushroomList = GenerateMushroomList();
            List<string> insectList = GenerateInsectList();
            List<string> frogList = GenerateFrogList();     // https://pastebin.com/raw/NevkzWMF
            List<string> plantList = GeneratePlantList();
            List<string> fishList = GenerateFishList();
            List<string> AnimalList = GenerateAnimalList();
            List<string> fruitList = GenerateFruitList();
            List<string> oreList = GenerateOreList();
            List<string> rupeeList = GenerateRupeeList();
            List<string> arrowBundle1List = GenerateArrowBundle1List();
            List<string> arrowBundle2List = GenerateArrowBundle2List();
            List<string> mineralList = GenerateMineralList();

            objectList.Add(swordList, false);
            objectList.Add(lSwordList, false);
            objectList.Add(spearList, false);
            objectList.Add(shieldList, false);
            objectList.Add(bowList, false);
            objectList.Add(enemyList, false);
            objectList.Add(keeseList, false);
            objectList.Add(keeseAllDayList, false);
            objectList.Add(chuchulist, false);
            objectList.Add(littleGolemsList, false);
            objectList.Add(lynelList, false);
            objectList.Add(subBossList, false);
            objectList.Add(enemyGuardList, false);
            objectList.Add(enemyGuardAmbushList, false);
            objectList.Add(enemyTreehouseGuardList, false);
            objectList.Add(singleArrowList, false);
            objectList.Add(wizzrobeList, false);
            objectList.Add(sandwormList, false);
            objectList.Add(staticGuardianList, false);
            objectList.Add(movingGuardianList, false);
            objectList.Add(miniGuardianList, false);
            objectList.Add(octorokList, false);
            objectList.Add(materialList, false);
            objectList.Add(meatList, false);
            objectList.Add(mannequinList, true);
            objectList.Add(mushroomList, false);
            objectList.Add(insectList, false);
            objectList.Add(frogList, false);
            objectList.Add(plantList, false);
            objectList.Add(fishList, false);
            objectList.Add(AnimalList, false);
            objectList.Add(fruitList, false);
            objectList.Add(oreList, false);
            objectList.Add(rupeeList, false);
            objectList.Add(arrowBundle1List, false);
            objectList.Add(arrowBundle2List, false);
            objectList.Add(mineralList, false);

            return objectList;
        }

        private static List<string> GenerateLynelList()
        {
            return new List<string>()
            {
                "Enemy_Lynel_Dark",
                "Enemy_Lynel_Junior",
                "Enemy_Lynel_Middle",
                "Enemy_Lynel_Senior",
                "Enemy_Lynel_Gold"
            };
        }

        private static List<string> GenerateSpearList()
        {
            return new List<string>
            {
                "Weapon_Spear_001",
                "Weapon_Spear_002",
                "Weapon_Spear_003",
                "Weapon_Spear_004",
                "Weapon_Spear_005",
                "Weapon_Spear_006",
                "Weapon_Spear_007",
                "Weapon_Spear_008",
                "Weapon_Spear_009",
                "Weapon_Spear_010",
                "Weapon_Spear_011",
                "Weapon_Spear_012",
                "Weapon_Spear_013",
                "Weapon_Spear_014",
                "Weapon_Spear_015",
                "Weapon_Spear_016",
                "Weapon_Spear_017",
                "Weapon_Spear_018",
                "Weapon_Spear_021",
                "Weapon_Spear_022",
                "Weapon_Spear_023",
                "Weapon_Spear_024",
                "Weapon_Spear_025",
                "Weapon_Spear_027",
                "Weapon_Spear_028",
                "Weapon_Spear_029",
                "Weapon_Spear_030",
                "Weapon_Spear_031",
                "Weapon_Spear_032",
                "Weapon_Spear_033",
                "Weapon_Spear_034",
                "Weapon_Spear_035",
                "Weapon_Spear_036",
                "Weapon_Spear_037",
                "Weapon_Spear_038",
                "Weapon_Spear_047",
                "Weapon_Spear_049",
                "Weapon_Spear_050"
            };
        }

        private static List<string> GenerateLSwordList()
        {
            return new List<string>
            {
                "Weapon_Lsword_001",
                "Weapon_Lsword_002",
                "Weapon_Lsword_003",
                "Weapon_Lsword_004",
                "Weapon_Lsword_005",
                "Weapon_Lsword_006",
                "Weapon_Lsword_010",
                "Weapon_Lsword_011",
                "Weapon_Lsword_012",
                "Weapon_Lsword_013",
                "Weapon_Lsword_014",
                "Weapon_Lsword_015",
                "Weapon_Lsword_016",
                "Weapon_Lsword_017",
                "Weapon_Lsword_018",
                "Weapon_Lsword_019",
                "Weapon_Lsword_020",
                "Weapon_Lsword_023",
                "Weapon_Lsword_024",
                "Weapon_Lsword_027",
                "Weapon_Lsword_029",
                "Weapon_Lsword_030",
                "Weapon_Lsword_031",
                "Weapon_Lsword_032",
                "Weapon_Lsword_033",
                "Weapon_Lsword_034",
                "Weapon_Lsword_035",
                "Weapon_Lsword_036",
                "Weapon_Lsword_037",
                "Weapon_Lsword_038",
                "Weapon_Lsword_041",
                "Weapon_Lsword_045",
                "Weapon_Lsword_047",
                "Weapon_Lsword_051",
                "Weapon_Lsword_054",
                "Weapon_Lsword_055",
                "Weapon_Lsword_056",
                "Weapon_Lsword_074"
            };
        }

        private static List<string> GenerateSwordList()
        {
            return new List<string>
            {
                "Weapon_Sword_001",
                "Weapon_Sword_002",
                "Weapon_Sword_003",
                "Weapon_Sword_004",
                "Weapon_Sword_005",
                "Weapon_Sword_006",
                "Weapon_Sword_007",
                "Weapon_Sword_008",
                "Weapon_Sword_009",
                "Weapon_Sword_013",
                "Weapon_Sword_014",
                "Weapon_Sword_015",
                "Weapon_Sword_016",
                "Weapon_Sword_017",
                "Weapon_Sword_018",
                "Weapon_Sword_019",
                "Weapon_Sword_020",
                "Weapon_Sword_021",
                "Weapon_Sword_022",
                "Weapon_Sword_023",
                "Weapon_Sword_024",
                "Weapon_Sword_025",
                "Weapon_Sword_027",
                "Weapon_Sword_029",
                "Weapon_Sword_030",
                "Weapon_Sword_031",
                "Weapon_Sword_033",
                "Weapon_Sword_034",
                "Weapon_Sword_035",
                "Weapon_Sword_040",
                "Weapon_Sword_041",
                "Weapon_Sword_043",
                "Weapon_Sword_044",
                "Weapon_Sword_047",
                "Weapon_Sword_048",
                "Weapon_Sword_049",
                "Weapon_Sword_050",
                "Weapon_Sword_051",
                "Weapon_Sword_052",
                "Weapon_Sword_053",
                "Weapon_Sword_060",
                "Weapon_Sword_061",
                "Weapon_Sword_062",
                "Weapon_Sword_073"
            };
        }

        private static List<string> GenerateShieldList()
        {
            return new List<string>
            {
                "Weapon_Shield_001",
                "Weapon_Shield_002",
                "Weapon_Shield_003",
                "Weapon_Shield_004",
                "Weapon_Shield_005",
                "Weapon_Shield_006",
                "Weapon_Shield_007",
                "Weapon_Shield_008",
                "Weapon_Shield_009",
                "Weapon_Shield_013",
                "Weapon_Shield_014",
                "Weapon_Shield_015",
                "Weapon_Shield_016",
                "Weapon_Shield_017",
                "Weapon_Shield_018",
                "Weapon_Shield_021",
                "Weapon_Shield_022",
                "Weapon_Shield_023",
                "Weapon_Shield_025",
                "Weapon_Shield_026",
                "Weapon_Shield_030",
                "Weapon_Shield_031",
                "Weapon_Shield_032",
                "Weapon_Shield_033",
                "Weapon_Shield_034",
                "Weapon_Shield_035",
                "Weapon_Shield_036",
                "Weapon_Shield_037",
                "Weapon_Shield_038",
                "Weapon_Shield_040",
                "Weapon_Shield_041",
                "Weapon_Shield_042"
            };
        }

        private static List<string> GenerateBowList()
        {
            return new List<string>
            {
                "Weapon_Bow_001",
                "Weapon_Bow_002",
                "Weapon_Bow_003",
                "Weapon_Bow_004",
                "Weapon_Bow_006",
                "Weapon_Bow_009",
                "Weapon_Bow_011",
                "Weapon_Bow_013",
                "Weapon_Bow_014",
                "Weapon_Bow_015",
                "Weapon_Bow_016",
                "Weapon_Bow_017",
                "Weapon_Bow_023",
                "Weapon_Bow_026",
                "Weapon_Bow_027",
                "Weapon_Bow_028",
                "Weapon_Bow_029",
                "Weapon_Bow_030",
                "Weapon_Bow_032",
                "Weapon_Bow_033",
                "Weapon_Bow_035",
                "Weapon_Bow_036",
                "Weapon_Bow_038",
                "Weapon_Bow_040"
            };
        }

        private static List<string> GenerateEnemyList()
        {
            return new List<string>()
            {
                // Bokos
                "Enemy_Bokoblin_Junior",
                "Enemy_Bokoblin_Middle",
                "Enemy_Bokoblin_Senior",
                "Enemy_Bokoblin_Dark",
                "Enemy_Bokoblin_Gold",
                "Enemy_Bokoblin_Bone_Junior_AllDay",
                // Lialfos
                "Enemy_Lizalfos_Junior",
                "Enemy_Lizalfos_Middle",
                "Enemy_Lizalfos_Senior",
                "Enemy_Lizalfos_Dark",
                "Enemy_Lizalfos_Gold",
                "Enemy_Lizalfos_Electric",
                "Enemy_Lizalfos_Fire",
                "Enemy_Lizalfos_Ice",
                // Moriblins
                "Enemy_Moriblin_Junior",
                "Enemy_Moriblin_Middle",
                "Enemy_Moriblin_Senior",
                "Enemy_Moriblin_Dark",
                "Enemy_Moriblin_Gold",
                // Mini Guardians
                "Enemy_Guardian_Mini_Baby",
                "Enemy_Guardian_Mini_Junior",
                "Enemy_Guardian_Mini_Middle",
                "Enemy_Guardian_Mini_Senior"
            };
        }

        private static List<string> GenerateKeeseList()
        {
            return new List<string>()
            {
                "Enemy_Keese",
                "Enemy_Keese_Electric",
                "Enemy_Keese_Fire",
                "Enemy_Keese_Ice",
                "Enemy_Keese_Swarm"
            };
        }

        private static List<string> GenerateKeeseAllDayList()
        {
            return new List<string>()
            {
                "Enemy_Keese_AllDay",
                "Enemy_Keese_Electric_AllDay",
                "Enemy_Keese_Fire_AllDay",
                "Enemy_Keese_Ice_AllDay"
            };
        }

        private static List<string> GenerateChuchuList()
        {
            return new List<string>()
            {
                "Enemy_Chuchu_Electric_Junior",
                "Enemy_Chuchu_Electric_Middle",
                "Enemy_Chuchu_Electric_Senior",
                "Enemy_Chuchu_Fire_Junior",
                "Enemy_Chuchu_Fire_Middle",
                "Enemy_Chuchu_Fire_Senior",
                "Enemy_Chuchu_Ice_Junior",
                "Enemy_Chuchu_Ice_Middle",
                "Enemy_Chuchu_Ice_Senior",
                "Enemy_Chuchu_Junior",
                "Enemy_Chuchu_Middle",
                "Enemy_Chuchu_Senior"
            };
        }

        private static List<string> GenerateLittleGolemList()
        {
            return new List<string>()
            {
                "Enemy_Golem_Little",
                "Enemy_Golem_Little_Ice",
                "Enemy_Golem_Little_Fire"
            };
        }

        private static List<string> GenerateSubBossList()
        {
            return new List<string>()
            {
                "Enemy_Golem_Fire",
                "Enemy_Golem_Ice",
                "Enemy_Golem_Junior",
                "Enemy_Golem_Middle",
                "Enemy_Golem_Senior",
                "Enemy_Giant_Junior",
                "Enemy_Giant_Middle",
                "Enemy_Giant_Senior",
                "Enemy_Guardian_A"
            };
        }

        private static List<string> GenerateEnemyGuardList()
        {
            return new List<string>()
            {
                "Enemy_Bokoblin_Guard_Junior",
                "Enemy_Bokoblin_Guard_Middle",
                "Enemy_Lizalfos_Guard_Junior",
                "Enemy_Lizalfos_Guard_Middle"
            };
        }

        private static List<string> GenerateEnemyGuardAmbushList()
        {
            return new List<string>()
            {
                "Enemy_Lizalfos_Middle_Guard_Ambush",
                "Enemy_Lizalfos_Fire_Guard_Ambush",
                "Enemy_Lizalfos_Junior_Guard_Ambush",
                "Enemy_Bokoblin_Guard_Junior_Ambush",
                "Enemy_Bokoblin_Guard_Middle_Ambush"
            };
        }

        private static List<string> GenerateEnemyTreeHouseGuardList()
        {
            return new List<string>()
            {
                "Enemy_Bokoblin_Guard_Junior_TreeHouseTop",
                "Enemy_Bokoblin_Guard_Middle_TreeHouseTop"
            };
        }

        private static List<string> GenerateSingleArrowList()
        {
            return new List<string>()
            {
                "NormalArrow",
                "FireArrow",
                "IceArrow",
                "AncientArrow",
                "ElectricArrow",
                "BombArrow_A",
                "BrightArrow"
            };
        }

        private static List<string> GenerateWizzrobeList()
        {
            return new List<string>()
            {
                "Enemy_Wizzrobe_Electric",
                "Enemy_Wizzrobe_Electric_Senior",
                "Enemy_Wizzrobe_Fire",
                "Enemy_Wizzrobe_Fire_Senior",
                "Enemy_Wizzrobe_Ice",
                "Enemy_Wizzrobe_Ice_Senior"
            };
        }

        private static List<string> GenerateSandwormList()
        {
            return new List<string>()
            {
                "Enemy_Sandworm",
                "Enemy_SandwormR"
            };
        }

        private static List<string> GenerateStaticGuardianList()
        {
            return new List<string>()
            {
                "Enemy_Guardian_B",
                "Enemy_Guardian_A_Fixed_Moss",
                "Enemy_Guardian_A_Fixed_Sand",
                "Enemy_Guardian_A_Fixed_Snow"
            };
        }

        private static List<string> GenerateMovingGuardianList()
        {
            return new List<string>()
            {
                "Enemy_Guardian_A",
                "Enemy_Guardian_A_Moss",
                "Enemy_Guardian_A_Sand",
                "Enemy_Guardian_A_Snow"
            };
        }

        private static List<string> GenerateMiniGuardianList()
        {
            return new List<string>()
            {
                "Enemy_Guardian_Mini_Baby",
                "Enemy_Guardian_Mini_Baby_Dark",
                "Enemy_Guardian_Mini_Junior",
                "Enemy_Guardian_Mini_Junior_Dark",
                "Enemy_Guardian_Mini_Middle",
                "Enemy_Guardian_Mini_Middle_Dark",
                "Enemy_Guardian_Mini_Practice",
                "Enemy_Guardian_Mini_Senior",
                "Enemy_Guardian_Mini_Senior_Dark"
            };
        }

        private static List<string> GenerateOctorokList()
        {
            return new List<string>()
            {
                "Enemy_Octarock",
                "Enemy_Octarock_Desert",
                "Enemy_Octarock_Forest",
                "Enemy_Octarock_Snow",
                "Enemy_Octarock_Stone"
            };
        }

        private static List<string> GenerateMaterialList()
        {
            return new List<string>()
            {
                "Item_Material_01",
                "Item_Material_02",
                "Item_Material_03",
                "Item_Material_04",
                "Item_Material_05",
                "Item_Material_06",
                "Item_Material_07",
                "Item_Material_08"
            };
        }

        private static List<string> GenerateMeatList()
        {
            return new List<string>()
            {
                "Item_Meat_01",
                "Item_Meat_02",
                "Item_Meat_06",
                "Item_Meat_07",
                "Item_Meat_11",
                "Item_Meat_12"
            };
        }

        private static List<string> GenerateMannequinList()
        {
            return new List<string>()
            {
                "Mannequin_004_Lower",
                "Mannequin_004_Head",
                "Mannequin_004_Upper",
                "Mannequin_007_Upper",
                "Mannequin_008_Upper",
                "Mannequin_007_Lower",
                "Mannequin_006_Lower",
                "Mannequin_008_Head",
                "Mannequin_006_Upper",
                "Mannequin_006_Head",
                "Mannequin_007_Head",
                "Mannequin_008_Lower",
                "Mannequin_005_Head",
                "Mannequin_005_Upper",
                "Mannequin_005_Lower",
                "Mannequin_001_Lower",
                "Mannequin_003_Lower",
                "Mannequin_003_Upper",
                "Mannequin_001_Head",
                "Mannequin_001_Upper",
                "Mannequin_003_Head",
                "Mannequin_006_Lower",
                "Mannequin_006_Upper",
                "Mannequin_006_Head",
                "Mannequin_009_Upper",
                "Mannequin_001_Upper",
                "Mannequin_001_Head",
                "Mannequin_002_Head",
                "Mannequin_002_Upper",
                "Mannequin_001_Lower",
                "Mannequin_002_Lower"
            };
        }

        private static List<string> GenerateMushroomList()
        {
            return new List<string>()
            {
                "Item_Mushroom_A",
                "Item_Mushroom_B",
                "Item_Mushroom_C",
                "Item_Mushroom_D",
                "Item_Mushroom_E",
                "Item_Mushroom_F",
                "Item_Mushroom_H",
                "Item_Mushroom_J",
                "Item_Mushroom_L",
                "Item_Mushroom_M",
                "Item_Mushroom_N",
                "Item_Mushroom_O"
            };
        }

        private static List<string> GenerateInsectList()
        {
            return new List<string>()
            {
                "Animal_Insect_A",
                "Animal_Insect_AA",
                "Animal_Insect_AB",
                "Animal_Insect_B",
                "Animal_Insect_C",
                "Animal_Insect_E",
                "Animal_Insect_G",
                "Animal_Insect_H",
                "Animal_Insect_I",
                "Animal_Insect_M",
                "Animal_Insect_N",
                "Animal_Insect_P",
                "Animal_Insect_Q",
                "Animal_Insect_R",
                "Animal_Insect_S",
                "Animal_Insect_T",
                "Animal_Insect_X"
            };
        }

        private static List<string> GenerateFrogList()
        {
            return new List<string>()
            {
                "Animal_Insect_K",
                "Animal_Insect_O",
                "Animal_Insect_Z",
                "Animal_Frog"
            };
        }

        private static List<string> GeneratePlantList()
        {
            return new List<string>()
            {
                "Item_Plant_A",
                "Item_Plant_B",
                "Item_Plant_C",
                "Item_Plant_E",
                "Item_Plant_F",
                "Item_Plant_G",
                "Item_Plant_H",
                "Item_Plant_I",
                "Item_Plant_J",
                "Item_Plant_L",
                "Item_Plant_M",
                "Item_Plant_O",
                "Item_Plant_Q"
            };
        }

        private static List<string> GenerateFishList()
        {
            return new List<string>()
            {
                "Animal_Fish_A",
                "Animal_Fish_B",
                "Animal_Fish_C",
                "Animal_Fish_D",
                "Animal_Fish_E",
                "Animal_Fish_F",
                "Animal_Fish_G",
                "Animal_Fish_H",
                "Animal_Fish_I",
                "Animal_Fish_J",
                "Animal_Fish_K",
                "Animal_Fish_L",
                "Animal_Fish_M",
                "Animal_Fish_X",
                "Animal_Fish_Z"
            };
        }

        private static List<string> GenerateAnimalList()
        {
            return new List<string>()
            {
                "Animal_Bear_A",
                "Animal_Bear_B",
                "Animal_Boar_A",
                "Animal_Boar_B",
                "Animal_Boar_C",
                "Animal_Bull_A",
                "Animal_Cassowary_A",
                "Animal_Deer_A",
                "Animal_Deer_C",
                "Animal_Doe_A",
                "Animal_Elk_A",
                "Animal_Fox_A",
                "Animal_Fox_B",
                "Animal_Rhino_A",
                "Animal_RupeeRabbit_A",
                "Animal_Squirrel_A",
                "Animal_WildDuck_A",
                "Animal_WildGoat_A",
                "Animal_Wolf_A",
                "Animal_Wolf_B",
                "Animal_Wolf_C"
            };
        }

        private static List<string> GenerateFruitList()
        {
            return new List<string>()
            {
                "Item_Fruit_A",
                "Item_Fruit_B",
                "Item_Fruit_C",
                "Item_Fruit_D",
                "Item_Fruit_E",
                "Item_Fruit_F",
                "Item_Fruit_G",
                "Item_Fruit_H",
                "Item_Fruit_I",
                "Item_Fruit_J",
                "Item_Fruit_K",
                "Item_Fruit_L"
            };
        }

        private static List<string> GenerateOreList()
        {
            return new List<string>()
            {
                "Item_Ore_A",
                "Item_Ore_B",
                "Item_Ore_C",
                "Item_Ore_D",
                "Item_Ore_E",
                "Item_Ore_F",
                "Item_Ore_G",
                "Item_Ore_H",
                "Item_Ore_I",
                "Item_Ore_J"
            };
        }

        private static List<string> GenerateRupeeList()
        {
            return new List<string>()
            {
                "PutRupee",
                "PutRupee_Blue",
                "PutRupee_Gold",
                "PutRupee_Purple",
                "PutRupee_Red",
                "PutRupee_Silver"
            };
        }

        private static List<string> GenerateArrowBundle1List()
        {
            return new List<string>()
            {
                "Obj_ArrowBundle_A_01",
                "Obj_ArrowBundle_A_02",
                "Obj_BombArrow_A_02",
                "Obj_BombArrow_A_03",
                "Obj_BombArrow_A_04",
                "Obj_ElectricArrow_A_02",
                "Obj_ElectricArrow_A_03",
                "Obj_FireArrow_A_02",
                "Obj_FireArrow_A_03",
                "Obj_IceArrow_A_02",
                "Obj_IceArrow_A_03"
            };
        }

        private static List<string> GenerateArrowBundle2List()
        {
            return new List<string>()
            {
                "Obj_ArrowNormal_A_01",
                "Obj_BombArrow_A_01",
                "Obj_ElectricArrow_A_01",
                "Obj_FireArrow_A_01",
                "Obj_IceArrow_A_01"
            };
        }

        private static List<string> GenerateMineralList()
        {
            return new List<string>()
            {
                "Obj_Mineral_A_01",
                "Obj_Mineral_B_01",
                "Obj_Mineral_C_01"
            };
        }
    }
}