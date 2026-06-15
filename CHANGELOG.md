# Changelog

## [0.2.2] - 2026-06-14

### Fixed
- Fixed the mod failing to initialize when other installed mods load an older version of BlueprintCore first. Previously this caused both classes to appear in the class list but be unselectable/unbuildable (none of the powers, paths, blade skills, or other features would configure). BlueprintCore is now merged directly into the mod assembly, so the mod always uses its own correct version regardless of load order or what other mods ship.

## [0.2.1] - 2026-06-06

### Changed
- Psychic Strike (and Exploding Critical) now deal force damage instead of physical, bypassing damage reduction and better fitting the psychic theme
- Psychic Warrior path selection now shows a full breakdown of what each path grants (trance with its scaling, maneuver, and expanded maneuver) instead of a one-line summary

### Fixed
- Mind Blade form classification: one-handed weapons (longsword, scimitar, battleaxe, flail, etc.) were wrongly manifesting as light (1d6) instead of one-handed (1d8). The form is now determined by the weapon's light flag, with everything non-light/non-two-handed correctly treated as one-handed
- Psychic Warrior trance bonus now begins at +1 when the trance unlocks at 3rd level (previously it was +0 until 6th level due to an off-by-one in the scaling), correctly reaching +5 (or +6 for Assassin/Gladiator) by 19th

## [0.2.0] - 2026-06-01

### Added
- Soulknife class (Phase 1 skeleton): full BAB, d10 HD, good Reflex and Will saves, appears in character creation and levels to 20
- Form Mind Blade: at 1st level, choose the weapon your mind blade manifests as. The blade's damage and critical range are set by the weapon's form — light (1d6), one-handed (1d8), or two-handed (2d6), all 19-20/×2 — and the blade takes on the chosen weapon's appearance. Toggle it on your action bar to summon or dismiss the weapon, which is bound to your primary hand while active. You are always proficient with your mind blade, and it counts as a magic weapon.
- Enhanced Mind Blade: at 3rd level the mind blade gains an enhancement pool that grows with level (up to +9 points). Allocate it between weapon special abilities (one toggle each) and a direct enhancement bonus via the "Enhance Mind Blade" eat-all toggle (capped at the level's max direct bonus: +1 at 3rd, +2 at 7th, +3 at 9th, +4 at 13th, +5 at 15th). Special abilities unlock by level: +1 abilities (Flaming, Frost, Shock, Corrosive, Keen, Ghost Touch, Vicious, Agile) at 5th level, +2 abilities (Flaming Burst, Icy Burst, Shocking Burst, Corrosive Burst, Holy, Unholy, Anarchic, Axiomatic) at 7th, and Brilliant Energy at 12th. Toggle abilities first, then enable the enhancement toggle to spend the remaining points
- Weapon Focus (Mind Blade): available as a 1st-level soulknife bonus feat, granting +1 to attack rolls with your mind blade
- Soulknife bonus feat at 1st level: choose Weapon Focus (Mind Blade), Power Attack, or Two-Weapon Fighting (Two-Weapon Fighting may be chosen without meeting its normal Dexterity requirement)
- Psychic Strike: imbue the mind blade with destructive psychic energy (1d8 at 3rd level, scaling every four levels); charge persists through misses and can be recharged as a swift action by expending psionic focus
- Blade Skills selection (every even level), including: Focused Offense (Wisdom to attack and damage), Focused Defense (Wisdom dodge AC), Evasion and Improved Evasion, Mind Blade Finesse (Dexterity to hit in any form), Deadly Blow (×3 critical multiplier), Exploding Critical, Powerful Strikes, Telekinetic Edge (ignore DR), Improved Enhancement, Knife to the Soul / Devastating Blade, Reaper's Blade, Vampiric Blade, Trapfinder, Telekinetic Athleticism, Psionic Training (gain a psionic feat); energy blades (Fire/Ice/Lightning/Thunder + their riders); and status/utility strikes (Stunning, Dazzling, Wing Clip, Dispelling Strike, Psychokinetic Blast, Mark of the Challenger, Reaching Blade, Furious Charge)
- Soulknife recommended build: the class-selection screen now shows a "Premade Build Balance" chart and a "Use Recommended Build" button (a two-handed mind-blade striker)
- Mind-blade combat feats: Greater Weapon Focus, Improved Critical, Weapon Specialization, and Greater Weapon Specialization — versions that work with the mind blade (require Weapon Focus (Mind Blade); fighter-level prerequisites relaxed to base attack bonus so mind-blade classes can take them)
- New psionic feats: Greater Psionic Endowment (+2 to power save DCs while focused), Extra Blade Skill (gain an additional blade skill, repeatable), and Intuitive Shot (add your Wisdom modifier to ranged damage while focused)

### Changed
- Renamed the mod to **Psionics** to reflect its expanded scope (Psychic Warrior and Soulknife). Existing saves and installs are unaffected — the mod's internal id is unchanged; only the display name changed
- All psionic toggles now activate **and deactivate instantly** instead of at the end of the round: mind blade form (summon/dismiss), the Enhanced Mind Blade enhancement/ability toggles, Psychic Warrior trances, Mind Knight's Call Weaponry, Reckless Offense, Reaching Blade, and Martial Power
- Enhanced Mind Blade ability toggles now show the remaining enhancement-pool points on their icons and darken when you can't afford them; the "Enhance Mind Blade" toggle also displays the pool and now defaults to **off**
- Enhanced Mind Blade ability toggles now describe what each weapon special ability actually does, rather than just naming it

### Fixed
- Enhanced Mind Blade: turning an ability off now refunds its pool points immediately; the pool updates correctly on level-up (previously stuck until you rested); and ability toggles can be switched on/off reliably
- Fixed a crash when a soulknife reached 3rd level (duplicate localization key in the Enhanced Mind Blade setup)
- Enhanced Mind Blade special abilities now apply and clear instantly when toggled

## [0.1.4] - 2026-05-31

### Added
- Advanced Mind Knight Path feat: when using the Mind Knight Maneuver, gain a competence bonus to attack rolls equal to +1 per four psychic warrior levels
- Advanced Feral Path feat: while in Feral Trance, the trance competence bonus also applies to damage rolls with natural attacks and unarmed strikes
- Advanced Weaponmaster Path feat: while in Weaponmaster Trance, your trance competence bonus also applies to weapon damage rolls
- Advanced Ascetic Path feat: while in Ascetic Trance, your trance competence bonus also applies to all saving throws
- Advanced Archer Path feat: while in Archer Trance, half the trance competence bonus applies to damage rolls with ranged and thrown weapons
- Advanced Dervish Path feat: while in Dervish Trance and dual-wielding, the trance competence bonus also applies to damage rolls
- Advanced Brawler Path feat: while in Brawler Trance, gain a competence bonus to grapple checks equal to +1 per 3 psychic warrior levels
- Advanced Assassin Path feat: while in Assassin's Trance, attacks against flat-footed or flanked targets deal bonus precision damage scaling with your psychic warrior level; when using the Assassin's Maneuver, successful hits may stagger the target

## [0.1.3] - 2026-05-28

### Fixed
- Mind Knight Path: Call Weaponry weapon picks at 3rd, 7th, 11th, 15th, and 19th level no longer appear for non-Mind Knight psychic warriors; picks are now granted via a Mind Knight-owned sub-progression and correctly prompt the player during level-up

## [0.1.2] - 2026-05-27

### Added
- Mind Knight Path: Call Weaponry feature — at 3rd, 7th, 11th, 15th, and 19th level, choose a weapon to psionically call to your primary hand at will
  - Supports every weapon the character is proficient with; exotic weapons (e.g. Elven Curved Blade) appear automatically once the matching exotic weapon proficiency is taken
  - Only one called weapon may be active at a time; switching calls dismisses the previous weapon
  - First weapon choice granted at 1st level (when the path is taken); additional choices at 3rd, 7th, 11th, 15th, and 19th level (six total)
  - Enhancement bonus scales with psychic warrior level: +1 at 3rd, +2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th
  - Called weapon is locked in the primary hand slot while active and cannot be unequipped
- Mind Knight Path: Called weapon now displays an astral glow visual effect (borrowed from Brilliant Energy FX) while active
- Mind Knight Trance: attack roll bonus now only applies when attacking with the currently called weapon (primary hand); secondary hand attacks and attacks without an active called weapon are unaffected. Initiative bonus remains unconditional.

### Fixed
- All paths: trance toggle and maneuver menu now correctly appear at 3rd level (previously appeared immediately at 1st level when the path was selected)
- Mind Knight Path: Call Weaponry first weapon selection now correctly appears immediately when the path is chosen at 1st level — the path is now itself a feature selection, which causes the engine to cascade into the weapon pick automatically
- Mind Knight Path: Called weapon now correctly appears in the primary hand slot when a Call Weaponry toggle is activated (previously the buff and slot lock applied but no weapon appeared)
- Mind Knight Path: Called weapon no longer appears as unidentified when summoned
- Mind Knight Path: Additional weapon selections at 3rd, 7th, 11th, 15th, and 19th level now correctly prompt the player during level-up (previously the selections were silently granted without showing the pick UI)
- Mind Knight Path: Called weapon enhancement bonus now correctly applies to both attack and damage rolls (previously only applied to damage; now uses a proper weapon enchantment so the bonus appears on the weapon itself)
- Mind Knight Path: Called weapon enhancement bonus now automatically upgrades when the character gains a psychic warrior level while the weapon is active (no longer requires toggling the weapon off and on after leveling)

## [0.1.1] - 2026-05-25

### Fixed
- Critical Refocus: scoring a critical hit now correctly regains psionic focus for the attacker (previously had no effect)
- Martial Power: on-hit maneuver effects and the once-per-round cooldown now correctly apply to the attacker (previously had no effect)
- Martial Power: maneuver buffs now last 2 rounds so they are active at the start of your next turn
- Aligned Attack: extra 2d6 damage now correctly bypasses all alignment-based damage reduction (Good/Evil/Law/Chaos)
- Psionic Critical: extra 1d8 damage now inherits the weapon's material and enhancement for damage reduction bypass, matching the weapon attack it extends
- Aligned Attack / Psionic Critical: when dual-wielding, each hand's extra damage now correctly uses that hand's weapon properties (previously always used the primary hand, negating all damage if the enemy was immune to that material)
- Psionic Weapon: bonus damage now correctly scales with manifester level (1d6 → 2d6 → 3d6 → 4d6) instead of always dealing 1d6
- Greater Psionic Weapon: same scaling fix as Psionic Weapon
- Psionic Weapon / Greater Psionic Weapon: multiple separate dice rolls now combined into a single Xd6 roll in the combat log
- Rapid Metabolism / Intuitive Fighting / Psionic Endowment: buffs now apply immediately when the feat is taken while already psionically focused (previously required toggling focus off and back on)
- Dervish Trance: attack bonus now shows in the character sheet attack tooltip and correctly fires on the same rule as the Two-Weapon Fighting penalty
- Dervish Trance: attack bonus now correctly only applies when wielding two weapons (secondary hand has a weapon, not a shield, and grip is not two-handed)
- Dervish Trance: attack bonus no longer applies when unarmed or using natural weapons in the secondary hand
- Secondary path: maneuver menu (expandable action bar button) now correctly appears when a path is selected as a secondary path at level 9
- Secondary path: expanded maneuver is now correctly granted at level 9 alongside the secondary path selection (fixes split/corrupted maneuver menu icon and missing expanded maneuver option)
- All paths: selecting the same path as both primary and secondary is now correctly prevented (greyed out in the secondary selection if already chosen as primary)
- All paths: expandable maneuver button now shows a unified icon instead of a diagonal split (both maneuver and expanded maneuver now use the same icon)
- Weaponmaster Trance: attack bonus now correctly applies to all manufactured weapon attacks
- Feral Warrior Trance: attack bonus now correctly applies to natural weapon and unarmed attacks
- Martial Power: each variant now has a distinct icon matching its path maneuver (instead of all showing the same hammer icon)

## [0.1.0] - 2026-05-24

### Fixed
- All path maneuvers: maneuver menu now only appears at 3rd level (when Expanded Path Maneuver is granted), not at 1st level
- Archer Trance: attack bonus now only applies to ranged and thrown weapons (not natural weapons)
- Feral Warrior Trance: attack bonus now only applies to natural weapon attacks
- Weaponmaster Trance: attack bonus now only applies to manufactured melee weapons (not natural weapons)
- All path trances: trance bonuses now scale with psychic warrior level starting at 3rd level, as per tabletop text ("beginning at 3rd level, +1 every four levels")
  - Weaponmaster, Archer, Feral Warrior, Ascetic: attack/AC bonus now +1 at 3rd, scaling to +5 at 19th
  - Mind Knight: Initiative and attack bonuses now scale identically
  - Interceptor: attack and damage bonuses now scale identically
  - Assassin's, Gladiator: bonuses start at +2 at 3rd level, scaling to +6 at 19th
  - Infiltrator: Persuasion stays +1 higher than damage at all levels, both scaling from 3rd
- Dervish Trance: attack bonus now correctly requires two weapons equipped
- Dervish Trance: attack bonus now scales correctly starting at 3rd level (+1 at 3rd, +2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th)