# Psionics — WotR Mod

A *Pathfinder: Wrath of the Righteous* mod that adds psionic classes based on the Pathfinder 1e
**Psionics Unleashed** rules. Instead of spell slots, psionics run on **Psionic Focus** — a resource you
gain and expend to power abilities and feats. The mod currently includes two full classes:

- **Psychic Warrior** — a martial manifester who augments combat with psionic powers and Warrior Paths.
- **Soulknife** — a warrior who manifests a weapon of pure mental energy and customizes it through Blade Skills.

Both share a common library of psionic feats and the focus system.

---

## Psychic Warrior

A full-BAB martial class (d8) that manifests psychic powers through a Wisdom-flavored, focus-based
spellbook (0–6th level). It trades raw caster power for durability and battlefield presence.

**Core features**
- **Psionic Focus** — gain it with an action; expend it to fuel powers and many feats.
- **Powers** — a broad library from 0–6th level (buffs, defenses, mobility, and damage), chosen as you level.
- **Warrior Paths** — 12 themed paths (Weaponmaster, Brawler, Archer, Ascetic, Assassin, Dervish, Feral,
  Gladiator, Infiltrator, Interceptor, Mind Knight, Survivor), each granting a trance, a maneuver, and a
  signature feature at 1st/3rd/7th path level. **Advanced Path** feats and **Twisting Paths** pathweaving
  expand and combine them later on.
- **Bonus feats** — combat and psionic feats as you advance.
- **Eternal Warrior** — 20th-level capstone.

---

## Soulknife

A precision warrior (d10, full BAB, good Reflex & Will, light/medium armor and shields) who forms a
**mind blade** and sharpens it with Blade Skills. Wisdom drives much of its combat.

**Core features**
- **Form Mind Blade** — at 1st level, choose any weapon; your blade manifests as that weapon, inheriting its
  look, reach, and category, with damage/crit normalized by form (light 1d6, one-handed 1d8, two-handed 2d6;
  all 19‑20/×2). It's always magic, you're always proficient, and it summons/dismisses instantly.
- **Psychic Strike** — charge the blade to deal bonus psychic damage on your next hit; recharge as a swift
  action by expending focus.
- **Blade Skills** — a customization pick every even level from a large list: Focused Offense/Defense
  (Wisdom to attack & damage / AC), Deadly Blow (×3 crit), Exploding Critical, energy blades, Mind Blade
  Finesse, Telekinetic Edge, and many more.
- **Enhanced Mind Blade** — from 3rd level, a reconfigurable enhancement **pool** that grows with level.
  Allocate it on the fly between a direct enhancement bonus and weapon special abilities (Flaming, Keen,
  Brilliant Energy, the +2 bursts, etc.), each a toggle that shows its cost and the remaining pool.
- **Mind-blade combat feats** — mind-blade versions of Greater Weapon Focus, Improved Critical, Weapon
  Specialization, and Greater Weapon Specialization.
- A **recommended build** is available on the class-selection screen.

---

## Notable WotR adaptations

A few tabletop rules were adapted to engine limits — most visibly:

- **No power points.** Manifesting uses a per-day, spellbook-style economy; manifester level still drives all
  scaling. Metapsionic feats (which edit power-point cost) are therefore not included.
- **Gaining focus needs no skill check.**
- **Psionic Weapon/Fist/Shot** bonus damage shows as its own combat-log entry rather than folding into the hit.
- The **mind blade** normalizes damage and crit by form, so weapon-type feats use mind-blade-specific
  versions (e.g. Improved Critical (Mind Blade)).

---

## Build & Deploy

**Prerequisites**
- [Unity Mod Manager](https://www.nexusmods.com/site/mods/21), patched into the game.
- .NET SDK (net472 target) or Visual Studio 2022.
- Game at the default Steam path; otherwise edit `<WrathPath>` in [Psionics.csproj](Psionics.csproj).

**Build**
```
dotnet build
```
The PostBuild step copies `Psionics.dll`, `Info.json`, and `BlueprintCore.dll` into
`$(WrathPath)\Mods\PsychicWarrior\`. Launch through Steam; UMM loads the mod on startup. Mod log lines in
`Player.log` are prefixed `[Psionics]`.

---

## Save compatibility & renaming

The mod's **UMM id stays `PsychicWarrior`** even though it now displays as "Psionics" — UMM tracks saves by
id, so keeping it means existing characters load with no "missing mod" warning. Only the **DisplayName**
(and the C# project/namespaces/DLL) became *Psionics*; those are safe because mod-created blueprints are
rebuilt from code every launch and are never persisted in saves.

**The one rule that must never break:** never change a GUID in [Guids.cs](Utils/Guids.cs). Saves store
blueprint references by GUID — change one and the matching feat/buff/feature is silently stripped from any
character that had it. Renames are recoverable; GUID changes are not.
