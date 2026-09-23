<div align="center">

# 📍 Random Spawn Point *Bruh!*

### *Customized player spawn positioning, dynamic biome distance ranging, and biome starting kits for Valheim.*

[![GitHub Release](https://img.shields.io/github/v/release/Vapok/RandomSpawnPointBruh?include_prereleases&logo=github&style=for-the-badge)](https://github.com/Vapok/RandomSpawnPointBruh/releases)
[![Thunderstore Version](https://img.shields.io/thunderstore/v/Vapok/RandomSpawnPointBruh?logo=thunderstore&style=for-the-badge)](https://thunderstore.io/c/valheim/p/Vapok/RandomSpawnPointBruh/)
[![Nexus Mods](https://img.shields.io/badge/Nexus_Mods-Available-da8e35?logo=nexusmods&style=for-the-badge)](https://www.nexusmods.com/valheim/mods/2544)
<br>
[![Discord](https://img.shields.io/badge/Discord-Join%20Community-7289da?logo=discord&logoColor=white&style=for-the-badge)](https://discord.gg/5YAJkRFBXt)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

---

</div>

**Random Spawn Point Bruh!** gives server administrators and solo players control over player spawn locations and initial survival loadouts. The mod supports randomized biome spawns with mathematical distance bands, fixed coordinates for community hubs, special trader POI protection buffers, terrain altitude validation, and optional starting equipment kits tailored to each biome.

---

<div align="center">

[![Survival Servers](https://raw.githubusercontent.com/Vapok/RandomSpawnPointBruh/main/images/survivalservers_banner.png)](https://www.survivalservers.com/services/game_servers/valheim/?ref=vapok)

</div>

---

## 🧭 Spawn Modes & Features

| Spawn Mode | Description |
| :--- | :--- |
| **`Random`** | Places new players at a randomized location within targeted biome boundaries and search ranges. |
| **`Static`** | Spawns all incoming players at a fixed, predefined coordinate. Ideal for servers with a central starter town or community hub. |
| **`Vanilla`** | Preserves the default sacrificial stones circle at the center of the world. |

### Smart Spawn Placement & Safeguards
* **Natural Biome Targeting**: Spawn generation respects authentic world geography, naturally placing Ashlands spawns in the far south and Deep North in the far north.
* **Trader Protection**: Automatically keeps spawn points and opening flight paths away from Haldor, Hildir, and the Bog Witch so their map markers aren't accidentally revealed early.
* **Safe Ground Validation**: Ensures players always spawn on dry land at a safe elevation above the water—never underwater, in deep marshland, or embedded in cliffsides.
* **Player & Base Separation**: Keeps new spawns at a configurable distance away from active players, player bases, beds, and wards.
* **Flight Path Fog-of-War**: Keeps the opening Valkyrie flight corridor covered by fog of war until you touch down.

---

## 🎒 Biome Starting Kits

When spawning outside the peaceful Meadows, surviving the elements and local creatures requires immediate equipment. The optional **Biome Starting Kits** system delivers gear, tools, and provisions upon touchdown.

* **Server-Synced & Optional**: Disabled by default (`Enable Starting Kits = false`) to preserve vanilla progression until enabled by the server admin.
* **First Spawn Only**: Kits are delivered only once when a new character first arrives in the world. They are never repeated on deaths, bed respawns, or world re-joins.
* **Auto-Equip Gear**: Armor, shields, weapons, and utility belts are automatically equipped upon landing (`Auto-Equip Gear = true`).
* **Auto-Consume Foods & Meads**: Immediately consumes one of each food and protective mead in the kit upon touchdown (`Auto-Consume Foods and Meads = true`), granting instant health, stamina, and resistances (e.g. Frost, Poison, or Fire Resistance) before local hazards take effect.
* **Admin / Testing Command**: Server admins and solo testers can run `rspb_resetkit` (requires `devcommands`) in the in-game console to re-queue starting kit delivery on demand.

---

## 📋 Starting Kit Loadouts by Biome

Each kit is fully customizable via server configuration. The default loadouts are detailed below:

### Meadows

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/RagTunic.png" width="24" height="24" alt="Rag Tunic" /> | **Rag Tunic** (`ArmorRagsChest`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/RagPants.png" width="24" height="24" alt="Rag Pants" /> | **Rag Pants** (`ArmorRagsLegs`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/club.png" width="24" height="24" alt="Club" /> | **Club** (`Club`) | 1 | Level 1 | Weapon - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/torch.png" width="24" height="24" alt="Torch" /> | **Torch** (`Torch`) | 1 | Level 1 | Tool |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/wood.png" width="24" height="24" alt="Wood" /> | **Wood** (`Wood`) | 10 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/stone.png" width="24" height="24" alt="Stone" /> | **Stone** (`Stone`) | 6 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/raspberry.png" width="24" height="24" alt="Raspberry" /> | **Raspberry** (`Raspberry`) | 5 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/meat_cooked.png" width="24" height="24" alt="Cooked Meat" /> | **Cooked Meat** (`CookedMeat`) | 3 | Level 1 | Food (1 Auto-consumed) |

### Black Forest

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorLeatherChest.png" width="24" height="24" alt="Leather Tunic" /> | **Leather Tunic** (`ArmorLeatherChest`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorLeatherLegs.png" width="24" height="24" alt="Leather Pants" /> | **Leather Pants** (`ArmorLeatherLegs`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HelmetLeather.png" width="24" height="24" alt="Leather Helmet" /> | **Leather Helmet** (`HelmetLeather`) | 1 | Level 1 | Armor (Helmet) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CapeDeerHide.png" width="24" height="24" alt="Deer Hide Cape" /> | **Deer Hide Cape** (`CapeDeerHide`) | 1 | Level 1 | Armor (Cape) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/club.png" width="24" height="24" alt="Club" /> | **Club** (`Club`) | 1 | Level 1 | Weapon - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/shield_wood0.png" width="24" height="24" alt="Wood Shield" /> | **Wood Shield** (`ShieldWood`) | 1 | Level 1 | Shield - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/torch.png" width="24" height="24" alt="Torch" /> | **Torch** (`Torch`) | 1 | Level 1 | Tool |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HardAntler.png" width="24" height="24" alt="Hard Antler" /> | **Hard Antler** (`HardAntler`) | 1 | Level 1 | Progression (Antler Pickaxe crafting) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/wood.png" width="24" height="24" alt="Wood" /> | **Wood** (`Wood`) | 10 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/stone.png" width="24" height="24" alt="Stone" /> | **Stone** (`Stone`) | 6 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/deer_meat_cooked.png" width="24" height="24" alt="Cooked Deer Meat" /> | **Cooked Deer Meat** (`CookedDeerMeat`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/blueberries.png" width="24" height="24" alt="Blueberries" /> | **Blueberries** (`Blueberries`) | 5 | Level 1 | Food (1 Auto-consumed) |

### Swamp

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorTrollLeatherChest.png" width="24" height="24" alt="Troll Leather Tunic" /> | **Troll Leather Tunic** (`ArmorTrollLeatherChest`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorTrollLeatherLegs.png" width="24" height="24" alt="Troll Leather Pants" /> | **Troll Leather Pants** (`ArmorTrollLeatherLegs`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HelmetTrollLeather.png" width="24" height="24" alt="Troll Leather Helmet" /> | **Troll Leather Helmet** (`HelmetTrollLeather`) | 1 | Level 1 | Armor (Helmet) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CapeTrollHide.png" width="24" height="24" alt="Troll Hide Cape" /> | **Troll Hide Cape** (`CapeTrollHide`) | 1 | Level 1 | Armor (Cape) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MaceBronze.png" width="24" height="24" alt="Bronze Mace" /> | **Bronze Mace** (`MaceBronze`) | 1 | Level 1 | Weapon - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/shield_bronzebuckler.png" width="24" height="24" alt="Bronze Buckler" /> | **Bronze Buckler** (`ShieldBronzeBuckler`) | 1 | Level 1 | Shield - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/pickaxe_antler.png" width="24" height="24" alt="Antler Pickaxe" /> | **Antler Pickaxe** (`PickaxeAntler`) | 1 | Level 1 | Tool / Mining |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/axe_flint.png" width="24" height="24" alt="Flint Axe" /> | **Flint Axe** (`AxeFlint`) | 1 | Level 1 | Tool / Woodcutting |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/hoe.png" width="24" height="24" alt="Hoe" /> | **Hoe** (`Hoe`) | 1 | Level 1 | Tool / Path clearing |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/hammer.png" width="24" height="24" alt="Hammer" /> | **Hammer** (`Hammer`) | 1 | Level 1 | Tool / Base building |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/potion_poisonresist.png" width="24" height="24" alt="Poison Resistance Mead" /> | **Poison Resistance Mead** (`MeadPoisonResist`) | 4 | Level 1 | Mead (1 Auto-consumed, 3 reserve) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CarrotSoup.png" width="24" height="24" alt="Carrot Soup" /> | **Carrot Soup** (`CarrotSoup`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/deer_meat_cooked.png" width="24" height="24" alt="Cooked Deer Meat" /> | **Cooked Deer Meat** (`CookedDeerMeat`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/QueensJam.png" width="24" height="24" alt="Queen's Jam" /> | **Queen's Jam** (`QueensJam`) | 3 | Level 1 | Food (1 Auto-consumed) |

### Mountain

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorIronChest.png" width="24" height="24" alt="Iron Scale Mail" /> | **Iron Scale Mail** (`ArmorIronChest`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorIronLegs.png" width="24" height="24" alt="Iron Greaves" /> | **Iron Greaves** (`ArmorIronLegs`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HelmetIron.png" width="24" height="24" alt="Iron Helmet" /> | **Iron Helmet** (`HelmetIron`) | 1 | Level 1 | Armor (Helmet) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MaceIron.png" width="24" height="24" alt="Iron Mace" /> | **Iron Mace** (`MaceIron`) | 1 | Level 1 | Weapon - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/shield_banded0.png" width="24" height="24" alt="Banded Shield" /> | **Banded Shield** (`ShieldBanded`) | 1 | Level 1 | Shield - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/bow_huntsman.png" width="24" height="24" alt="Huntsman Bow" /> | **Huntsman Bow** (`BowHuntsman`) | 1 | Level 1 | Ranged Weapon |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/arrow_fire.png" width="24" height="24" alt="Fire Arrow" /> | **Fire Arrow** (`ArrowFire`) | 30 | Level 1 | Ammunition |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/pickaxe_antler.png" width="24" height="24" alt="Antler Pickaxe" /> | **Antler Pickaxe** (`PickaxeAntler`) | 1 | Level 1 | Tool / Mining |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/axe_bronze.png" width="24" height="24" alt="Bronze Axe" /> | **Bronze Axe** (`AxeBronze`) | 1 | Level 1 | Tool / Woodcutting |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/hammer.png" width="24" height="24" alt="Hammer" /> | **Hammer** (`Hammer`) | 1 | Level 1 | Tool / Base building |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/potion_frostresist.png" width="24" height="24" alt="Frost Resistance Mead" /> | **Frost Resistance Mead** (`MeadFrostResist`) | 4 | Level 1 | Mead (1 Auto-consumed, 3 reserve) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/Sausages.png" width="24" height="24" alt="Sausages" /> | **Sausages** (`Sausages`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/TurnipStew.png" width="24" height="24" alt="Turnip Stew" /> | **Turnip Stew** (`TurnipStew`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/deer_meat_cooked.png" width="24" height="24" alt="Cooked Deer Meat" /> | **Cooked Deer Meat** (`CookedDeerMeat`) | 3 | Level 1 | Food (1 Auto-consumed) |

### Plains

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorWolfChest.png" width="24" height="24" alt="Wolf Armor Chest" /> | **Wolf Armor Chest** (`ArmorWolfChest`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorwolfLegs.png" width="24" height="24" alt="Wolf Armor Legs" /> | **Wolf Armor Legs** (`ArmorWolfLegs`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HelmetDrake.png" width="24" height="24" alt="Drake Helmet" /> | **Drake Helmet** (`HelmetDrake`) | 1 | Level 1 | Armor (Helmet) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CapeWolf.png" width="24" height="24" alt="Wolf Fur Cape" /> | **Wolf Fur Cape** (`CapeWolf`) | 1 | Level 1 | Armor (Cape) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MaceBronze.png" width="24" height="24" alt="Bronze Mace" /> | **Bronze Mace** (`MaceBronze`) | 1 | Level 4 | Weapon - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/shield_bronzebuckler.png" width="24" height="24" alt="Bronze Buckler" /> | **Bronze Buckler** (`ShieldBronzeBuckler`) | 1 | Level 4 | Shield - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/bow_draugrfang.png" width="24" height="24" alt="Draugr Fang" /> | **Draugr Fang** (`BowDraugrFang`) | 1 | Level 1 | Ranged Weapon |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/arrow_obsidian.png" width="24" height="24" alt="Obsidian Arrow" /> | **Obsidian Arrow** (`ArrowObsidian`) | 30 | Level 1 | Ammunition |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/pickaxe_antler.png" width="24" height="24" alt="Antler Pickaxe" /> | **Antler Pickaxe** (`PickaxeAntler`) | 1 | Level 1 | Tool / Mining |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/axe_bronze.png" width="24" height="24" alt="Bronze Axe" /> | **Bronze Axe** (`AxeBronze`) | 1 | Level 1 | Tool / Woodcutting |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/hammer.png" width="24" height="24" alt="Hammer" /> | **Hammer** (`Hammer`) | 1 | Level 1 | Tool / Base building |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/wolf_skewer.png" width="24" height="24" alt="Wolf Skewer" /> | **Wolf Skewer** (`WolfMeatSkewer`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/eyescream.png" width="24" height="24" alt="Eyescream" /> | **Eyescream** (`Eyescream`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/Sausages.png" width="24" height="24" alt="Sausages" /> | **Sausages** (`Sausages`) | 3 | Level 1 | Food (1 Auto-consumed) |

### Mistlands

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorPaddedCuirass.png" width="24" height="24" alt="Padded Cuirass" /> | **Padded Cuirass** (`ArmorPaddedCuirass`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorPaddedGreaves.png" width="24" height="24" alt="Padded Greaves" /> | **Padded Greaves** (`ArmorPaddedGreaves`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HelmetPadded.png" width="24" height="24" alt="Padded Helmet" /> | **Padded Helmet** (`HelmetPadded`) | 1 | Level 1 | Armor (Helmet) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CapeLox.png" width="24" height="24" alt="Lox Cape" /> | **Lox Cape** (`CapeLox`) | 1 | Level 1 | Armor (Cape) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/demister.png" width="24" height="24" alt="Wisplight" /> | **Wisplight** (`Demister`) | 1 | Level 1 | Utility (Wisplight) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MaceSilver.png" width="24" height="24" alt="Frostner" /> | **Frostner** (`MaceSilver`) | 1 | Level 1 | Weapon (Frostner) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/shield_blackmetal00.png" width="24" height="24" alt="Black Metal Shield" /> | **Black Metal Shield** (`ShieldBlackmetal`) | 1 | Level 1 | Shield - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/pickaxe_antler.png" width="24" height="24" alt="Antler Pickaxe" /> | **Antler Pickaxe** (`PickaxeAntler`) | 1 | Level 1 | Tool / Mining |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/axe_blackmetal.png" width="24" height="24" alt="Black Metal Axe" /> | **Black Metal Axe** (`AxeBlackMetal`) | 1 | Level 1 | Tool / Woodcutting |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/hammer.png" width="24" height="24" alt="Hammer" /> | **Hammer** (`Hammer`) | 1 | Level 1 | Tool / Base building |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/finewood.png" width="24" height="24" alt="Fine Wood" /> | **Fine Wood** (`FineWood`) | 30 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/deerhide.png" width="24" height="24" alt="Deer Hide" /> | **Deer Hide** (`DeerHide`) | 10 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/resin.png" width="24" height="24" alt="Resin" /> | **Resin** (`Resin`) | 20 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/bronzenails.png" width="24" height="24" alt="Bronze Nails" /> | **Bronze Nails** (`BronzeNails`) | 80 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/barleywine.png" width="24" height="24" alt="Fire Resistance Barley Wine" /> | **Fire Resistance Barley Wine** (`BarleyWine`) | 4 | Level 1 | Mead (Fire Resistance; 1 Auto-consumed, 3 reserve) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/LoxPie.png" width="24" height="24" alt="Lox Meat Pie" /> | **Lox Meat Pie** (`LoxPie`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/BloodPudding.png" width="24" height="24" alt="Blood Pudding" /> | **Blood Pudding** (`BloodPudding`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/Bread.png" width="24" height="24" alt="Bread" /> | **Bread** (`Bread`) | 3 | Level 1 | Food (1 Auto-consumed) |

### Ashlands

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorCarapaceChest.png" width="24" height="24" alt="Carapace Breastplate" /> | **Carapace Breastplate** (`ArmorCarapaceChest`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorCarapaceLegs.png" width="24" height="24" alt="Carapace Greaves" /> | **Carapace Greaves** (`ArmorCarapaceLegs`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HelmetCarapace.png" width="24" height="24" alt="Carapace Helmet" /> | **Carapace Helmet** (`HelmetCarapace`) | 1 | Level 1 | Armor (Helmet) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CapeLox.png" width="24" height="24" alt="Lox Cape" /> | **Lox Cape** (`CapeLox`) | 1 | Level 1 | Armor (Cape) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/BeltStrength.png" width="24" height="24" alt="Megingjord" /> | **Megingjord** (`BeltStrength`) | 1 | Level 1 | Utility (Megingjord) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/SwordMistwalker.png" width="24" height="24" alt="Mistwalker" /> | **Mistwalker** (`SwordMistwalker`) | 1 | Level 1 | Weapon (Mistwalker) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/shield_carapace.png" width="24" height="24" alt="Carapace Shield" /> | **Carapace Shield** (`ShieldCarapace`) | 1 | Level 1 | Shield - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/pickaxe_antler.png" width="24" height="24" alt="Antler Pickaxe" /> | **Antler Pickaxe** (`PickaxeAntler`) | 1 | Level 1 | Tool / Mining |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/axe_blackmetal.png" width="24" height="24" alt="Black Metal Axe" /> | **Black Metal Axe** (`AxeBlackMetal`) | 1 | Level 1 | Tool / Woodcutting |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/hammer.png" width="24" height="24" alt="Hammer" /> | **Hammer** (`Hammer`) | 1 | Level 1 | Tool / Base building |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/blackmarble.png" width="24" height="24" alt="Black Marble" /> | **Black Marble** (`BlackMarble`) | 10 | Level 1 | Ashlands Outpost Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/BlackCore.png" width="24" height="24" alt="Black Core" /> | **Black Core** (`BlackCore`) | 5 | Level 1 | Portal / Station Component |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CeramicPlate.png" width="24" height="24" alt="Ceramic Plate" /> | **Ceramic Plate** (`CeramicPlate`) | 30 | Level 1 | Ashlands Outpost Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ironnails.png" width="24" height="24" alt="Iron Nails" /> | **Iron Nails** (`IronNails`) | 100 | Level 1 | Ashlands Outpost Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/finewood.png" width="24" height="24" alt="Fine Wood" /> | **Fine Wood** (`FineWood`) | 50 | Level 1 | Ashlands Outpost Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/yggdrasilwood.png" width="24" height="24" alt="Yggdrasil Wood" /> | **Yggdrasil Wood** (`YggdrasilWood`) | 35 | Level 1 | Ashlands Outpost Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/barleywine.png" width="24" height="24" alt="Fire Resistance Barley Wine" /> | **Fire Resistance Barley Wine** (`BarleyWine`) | 4 | Level 1 | Mead (Fire Resistance; 1 Auto-consumed, 3 reserve) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MisthareSupreme.png" width="24" height="24" alt="Misthare Supreme" /> | **Misthare Supreme** (`MisthareSupreme`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MeatPlatter.png" width="24" height="24" alt="Meat Platter" /> | **Meat Platter** (`MeatPlatter`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/Salad.png" width="24" height="24" alt="Salad" /> | **Salad** (`Salad`) | 3 | Level 1 | Food (1 Auto-consumed) |

### Deep North

| Icon | Item | Quantity | Level | Purpose / Role |
| :---: | :--- | :---: | :---: | :--- |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorFlametalChest.png" width="24" height="24" alt="Flametal Breastplate" /> | **Flametal Breastplate** (`ArmorFlametalChest`) | 1 | Level 1 | Armor (Chest) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/ArmorFlametalLegs.png" width="24" height="24" alt="Flametal Greaves" /> | **Flametal Greaves** (`ArmorFlametalLegs`) | 1 | Level 1 | Armor (Legs) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/HelmetFlametal.png" width="24" height="24" alt="Flametal Helmet" /> | **Flametal Helmet** (`HelmetFlametal`) | 1 | Level 1 | Armor (Helmet) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/CapeLox.png" width="24" height="24" alt="Lox Cape" /> | **Lox Cape** (`CapeLox`) | 1 | Level 1 | Armor (Cape) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/bow_ashlands.png" width="24" height="24" alt="Ash Fang" /> | **Ash Fang** (`BowAshlands`) | 1 | Level 1 | Ranged Weapon (Ash Fang) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/shield_flametal00.png" width="24" height="24" alt="Flametal Shield" /> | **Flametal Shield** (`ShieldFlametal`) | 1 | Level 1 | Shield - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/SwordMistwalker.png" width="24" height="24" alt="Mistwalker" /> | **Mistwalker** (`SwordMistwalker`) | 1 | Level 1 | Weapon (Mistwalker) - Auto-equipped |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/arrow_fire.png" width="24" height="24" alt="Fire Arrow" /> | **Fire Arrow** (`ArrowFire`) | 40 | Level 1 | Ammunition |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/pickaxe_antler.png" width="24" height="24" alt="Antler Pickaxe" /> | **Antler Pickaxe** (`PickaxeAntler`) | 1 | Level 1 | Tool / Mining |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/axe_blackmetal.png" width="24" height="24" alt="Black Metal Axe" /> | **Black Metal Axe** (`AxeBlackMetal`) | 1 | Level 1 | Tool / Woodcutting |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/hammer.png" width="24" height="24" alt="Hammer" /> | **Hammer** (`Hammer`) | 1 | Level 1 | Tool / Base building |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/wood.png" width="24" height="24" alt="Wood" /> | **Wood** (`Wood`) | 20 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/finewood.png" width="24" height="24" alt="Fine Wood" /> | **Fine Wood** (`FineWood`) | 30 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/deerhide.png" width="24" height="24" alt="Deer Hide" /> | **Deer Hide** (`DeerHide`) | 10 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/resin.png" width="24" height="24" alt="Resin" /> | **Resin** (`Resin`) | 20 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/bronzenails.png" width="24" height="24" alt="Bronze Nails" /> | **Bronze Nails** (`BronzeNails`) | 80 | Level 1 | Building Material |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MeatPlatter.png" width="24" height="24" alt="Meat Platter" /> | **Meat Platter** (`MeatPlatter`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/MisthareSupreme.png" width="24" height="24" alt="Misthare Supreme" /> | **Misthare Supreme** (`MisthareSupreme`) | 3 | Level 1 | Food (1 Auto-consumed) |
| <img src="https://raw.githubusercontent.com/Vapok/ValheimDifferences/main/assets/icons/items/Salad.png" width="24" height="24" alt="Salad" /> | **Salad** (`Salad`) | 3 | Level 1 | Food (1 Auto-consumed) |


---

## ⚙️ Configuration Reference

All settings in the table below are **Server-Synced** on dedicated servers using Jotunn ServerSync:

| Section | Setting Name | Default | Acceptable Values | Description |
| :--- | :--- | :---: | :---: | :--- |
| **`[Random Spawn Settings (Synced)]`** | `Spawn Method` | `Random` | `Random`, `Static`, `Vanilla` | Determines the spawn selection algorithm. |
| **`[Random Spawn Settings (Synced)]`** | `Spawn Biome` | `Meadows` | Any Biome Name | Biome target for random spawn selection. |
| **`[Random Spawn Settings (Synced)]`** | `Custom Spawn Point` | `(0, 0, 0)` | Vector3 | Fixed coordinates used when `Spawn Method = Static`. |
| **`[Random Spawn Settings (Synced)]`** | `Min Search Range` | `100.0` | 0.0 – 10000.0 | Minimum radius from world origin to begin searching. |
| **`[Random Spawn Settings (Synced)]`** | `Max Search Range` | `2000.0` | 100.0 – 10000.0 | Maximum radius from world origin for candidate points. |
| **`[Random Spawn Settings (Synced)]`** | `Range Increment` | `100` | 10 – 1000 | Step distance added per search pass when locating valid terrain. |
| **`[Random Spawn Settings (Synced)]`** | `Max Search Attempts` | `250` | 10 – 1000 | Maximum candidate points tested before falling back to temple. |
| **`[Random Spawn Settings (Synced)]`** | `Min Altitude Above Water` | `4.0` | 1.0 – 50.0 | Minimum elevation in meters above sea level to accept a spawn point. |
| **`[Random Spawn Settings (Synced)]`** | `Player Separation Distance` | `200.0` | 0.0 – 2000.0 | Minimum distance from other players, bases, and active wards. |
| **`[Random Spawn Settings (Synced)]`** | `Special POI Buffer Distance` | `100.0` | 50.0 – 500.0 | Safety buffer added beyond trader reveal radius and flight path. |
| **`[Starting Kits (Synced)]`** | `Enable Starting Kits` | `false` | `true`, `false` | Master toggle to enable biome-based starting kits. |
| **`[Starting Kits (Synced)]`** | `Clear Vanilla Starting Items` | `true` | `true`, `false` | Removes default rags and torch on initial spawn. |
| **`[Starting Kits (Synced)]`** | `Auto-Equip Gear` | `true` | `true`, `false` | Automatically equips weapons, shields, armor, and utility belts. |
| **`[Starting Kits (Synced)]`** | `Auto-Consume Foods and Meads` | `true` | `true`, `false` | Consumes 1 of each food and mead included in the kit on landing. |
| **`[Starting Kits (Synced)]`** | `Use Biome Specific Starter Kit` | `true` | `true`, `false` | When enabled, awards the kit for the landing biome. When disabled, uses the Default Starter Kit. |
| **`[Starting Kits (Synced)]`** | `Default Starter Kit` | `Meadows` | Any Biome Name | The starting kit to award when biome-specific kits are disabled or unavailable. |
| **`[Starting Kits: <Biome> (Synced)]`** | `Enabled` | `true` | `true`, `false` | Enables the starting kit for that specific biome. |
| **`[Starting Kits: <Biome> (Synced)]`** | `Kit Items` | *(See tables)* | String | Comma-separated list of items: `PrefabName:Count[:Quality]`. |

---

## 📥 Installation & Server Setup

### Mod Manager Installation (Recommended)
1. Install via **Thunderstore Mod Manager** or **r2modman**.
2. Dependencies (`BepInExPack`, `Jotunn (JVL)`, `YamlDotNetDetector`) are managed automatically.

### Dedicated Server Requirements
* **Install on Both Server & Clients**: To enforce synchronized spawn positions and starting kit rules, install the mod on the dedicated server and all connecting clients.
* **Server Authority**: The dedicated server configuration controls all synced settings. Client settings in synced categories are overwritten upon connection.

---

## 🔒 Anonymous Telemetry, Error Reporting & Privacy

RandomSpawnPointBruh includes lightweight, privacy-first telemetry and error reporting to help monitor mod stability, diagnose unhandled exceptions, and track active version adoption across Valheim updates.

* **100% Anonymous**: No personal data, Steam IDs, IP addresses, character names, or local file system paths are collected. Stack traces are sanitized to strip local user directories.
* **Granular Control**:
  * **Anonymous Telemetry (Opt-In)**: Tracks version adoption and session launches. Defaults to **disabled** (`Enable Anonymous Telemetry = false`).
  * **Error Reporting (Opt-Out)**: Captures sanitized mod crash diagnostics to rapidly identify and fix bugs. Defaults to **enabled** (`Send Error Reports = true`) with one-click opt-out.
  * **Data Disclaimers**: Hover over any toggle in the startup splash modal for interactive tooltip disclaimers detailing what data is transmitted.
* **In-Game & Online Privacy Policy**: View the complete policy directly in-game via **`[ PRIVACY POLICY ]`** on the startup splash screen, or online at [vapok.io/privacy-policy](https://vapok.io/privacy-policy/).
* **Configuration Files**: Settings can be adjusted in-game via the startup modal, through the BepInEx Configuration Manager (<kbd>F1</kbd>), or under `[Local Config]` in `BepInEx/config/vapok.mods.RandomSpawnPointBruh.cfg`.

---

<div align="center">

### 👨‍💻 Created by Vapok Gaming

[![Vapok Gaming](https://avatars.githubusercontent.com/u/1264136?s=120&v=4)](https://github.com/Vapok)

**Author**: [Vapok](https://github.com/Vapok)  
**Source Code**: [GitHub Repository](https://github.com/Vapok/RandomSpawnPointBruh)  
**Community & Support**: [Discord Server](https://discord.gg/5YAJkRFBXt)  
**Changelog**: [Release Notes](https://github.com/Vapok/RandomSpawnPointBruh/blob/main/CHANGELOG.md)

</div>
