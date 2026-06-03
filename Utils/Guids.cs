namespace Psionics.Utils;

public static class Guids
{
    // ── Core (original random GUIDs — kept as-is) ──────────────────────────
    public const string PsychicWarriorClass = "4c8f9c4e-7f22-4d9b-a9b9-2f7a9f7e1d01";
    public const string PsychicWarriorProgression = "1e54d8d4-ec8f-4d89-91d3-62b2f70d5b11";
    public const string Proficiencies = "5c1f6b8b-e820-410a-ba6a-36b139988a0a";
    public const string BonusFeatSelection = "d2e4c1f9-0c65-4f7b-9f70-7ec822365e63";
    public const string GainPsionicFocusAbility = "b7a3d92f-18c4-4b8d-9a67-21c6130d24e5";
    public const string PsionicFocusBuff = "f8c9301a-82f5-4d7c-b3a1-79b8c4d21e85";

    // ── Spellbook / SpellList ──────────────────────────────────────────────
    public const string Spellbook = "fab46f8a-307c-4248-86a4-84886cd3f689";
    public const string SpellList = "ddaa7433-6daa-46a9-bab9-50dd6b26ca17";

    // ── Focus / Meditation ────────────────────────────────────────────────
    public const string GainPsionicFocusFeature = "dff1108a-c32d-4ccd-bc89-61d473a0ae63";
    public const string PsionicMeditationFeat = "f97adbd7-1bbb-479b-9cf0-38d344f6c855";
    public const string GainPsionicFocusMoveAbility = "dd58c7df-3eea-4691-ada3-ca3d8aa57ae4";
    public const string PsionicWeaponFeat = "19d01da1-af69-43d9-8043-fd9af97c0fe3";
    public const string PsionicWeaponBuff = "a3c72d18-bb54-4e91-9f12-8d3c04a15e27";

    // ── Path infrastructure ───────────────────────────────────────────────
    public const string PathSelectionLevel1 = "b0ff4d20-2424-4bbd-abc4-25f6ea6fba0d";
    public const string WeaponmasterPath = "b61fd2a2-9f43-4010-a6ad-239d81c5ef0a";
    public const string WeaponmasterTrance = "b637a3fe-f7a0-427e-8eac-e652b660d03c";
    public const string WeaponmasterManeuver = "abd18f11-699e-4464-8d5e-c82471bfd0df";
    public const string WeaponmasterManeuverBuff = "01c1d5ff-0a85-417d-ae0c-d9c3d5e9a230";
    public const string BrawlerPath = "1bf0e28d-9646-4719-a1c0-d822d80095d9";
    public const string BrawlerTrance = "750b8b18-682a-43d2-a1d7-89c592e65995";
    public const string BrawlerManeuver = "f8d76d74-53b4-429a-aefa-2c7611879e98";
    public const string BrawlerManeuverBuff = "5dd5f9a9-2c0b-4e53-8ea8-6c848b0da89e";
    public const string PathExpandedManeuver = "b94340e9-d1f0-4170-917e-3da650b82eee";
    public const string SecondaryPathSelection = "0bfd77af-b387-40d0-a3c5-ccbadfa121b5";
    public const string TwistingPaths = "9fd4c056-6c89-47a0-9028-e29187e1ea62";
    public const string Pathweaving = "a72ac6df-272e-4937-a991-b839fcfe4717";

    // ── 0-level powers & talents ─────────────────────────────────────────
    public const string PowerVigor = "00a09564-4df4-49a7-a99b-75eef659d615";
    public const string PowerMinorPrecognition = "5f71e319-d9f1-4381-836e-08c34cc434ca";
    public const string PowerForceScreen = "1d2e517d-f746-437f-8dc2-1ef9eba6fdcc";
    public const string PowerForceScreenBuff = "3a7f1c24-b8e5-4d92-af60-c1d34e8b2f17";
    public const string PsionicProficiency = "f4d8a570-2b01-4679-ae29-0f9329dbf800";
    public const string TalentsSelection = "bfd5eeff-e208-42e0-8caf-4789545c8d9c";
    public const string PathSkillBonusFeature = "3248434d-8b3d-4cc6-b687-ae2a8ae070bd";
    public const string PathSkillBonusSelection = "ec9a3fee-f284-4ee9-acce-4702b3faa5fb";
    public const string MartialPowerFeature = "6518212e-1063-4f6d-8cb8-6e5f49640077";
    public const string VigorBuff = "bee3e90c-578d-487d-ba14-795279891847";
    public const string PowerBurst = "8dcf9ed2-de4e-4733-9be5-6e574884ef7e";
    public const string PowerBurstBuff = "d63ea4d0-749a-45fd-aacf-335928cf6d6f";
    public const string PowerEmptyMind = "b2a09b96-1e84-46ea-8d84-02bc62bb38fd";
    public const string PowerEmptyMindBuff = "ee9643ec-2b80-42e2-b9f6-7e8ad4b240a1";
    public const string PowerValor = "a13ca553-b873-4ac4-b70b-e3ccf5335f1a";
    public const string PowerValorBuff = "93f6430d-8762-473c-95d9-3d2c1b419059";
    public const string PowerTelekineticPunch = "4bf044ba-0c52-4018-8672-3a3abf476f1d";
    public const string PowerPrecognitionDefensive = "c6dd5594-db9a-48c6-8f37-94eac53f6518";
    public const string PowerPrecognitionDefensiveBuff = "8ebda42d-c976-4e26-8983-bc79834284da";
    public const string PowerDeceleration = "babdbb18-e5eb-4113-97fa-440239b3f330";
    public const string PowerDecelerationBuff = "7cfa0e1e-86c2-4d84-a95c-f915504eadbe";
    public const string TalentMinorPrecognition = "aab32657-80d0-40fc-955f-3854faf063b4";
    public const string TalentBurst = "cdb3a75d-1eaa-4028-8898-ad592eff55d7";
    public const string TalentEmptyMind = "49e4bd8e-110e-4758-bb74-dbe6daeaff3c";
    public const string TalentValor = "a0034f81-cfc9-4be4-bca3-5dc386bfbcb6";
    public const string TalentTelekineticPunch = "11a4cb42-2176-4655-b9b4-afcaece16285";
    public const string TalentPrecognitionDefensive = "a2585e51-0ca8-41d5-a311-8665103749b2";
    public const string TalentDeceleration = "c656b5ff-d92c-4667-bcdd-4deefeff4cb6";

    // ── Path skill bonuses ────────────────────────────────────────────────
    public const string PathSkillAthletics = "199364c2-d606-43c9-8f64-be50f6b43e96";
    public const string PathSkillMobility = "d2040764-421d-4779-ac27-6697a95dcd7a";
    public const string PathSkillPerception = "b7f7a729-a9dd-4ee4-8a52-e60a54052def";
    public const string PathSkillStealth = "df0c9bc6-8295-41f0-93a6-d94a0414d5e4";

    // ── 1st-level powers ─────────────────────────────────────────────────
    public const string PowerInertialArmor = "a2d15fcd-25fc-40ba-8c9f-3c5ca2f2005d";
    public const string PowerInertialArmorBuff = "5ffe4491-3e5c-499e-878a-0bf7a7e354db";
    public const string PowerThickenSkin = "7476a916-b7a6-443f-96f8-4a8a583961d7";
    public const string PowerThickenSkinBuff = "2f2da4cc-3834-40e5-a2a8-30318665f477";
    public const string PowerBiofeedback = "15af9339-8112-4150-a6ef-6bdd0b9c8b6e";
    public const string PowerBiofeedbackBuff = "0d10511a-724b-4982-96f2-1cb94840a56d";
    public const string PowerMetaphysicalWeapon = "3a162e6f-2d77-4abf-8cac-efd7599923c2";
    public const string PowerMetaphysicalWeaponBuff = "83dc50b8-c907-47fd-af80-45741eb697a8";

    // ── Expanded maneuvers ────────────────────────────────────────────────
    public const string WeaponmasterExpandedManeuver = "191bf05c-98fd-4c2f-893a-df45d76a85ae";
    public const string WeaponmasterExpandedManeuverAbility = "3d6d9c49-685d-4c8b-b9b2-6e41b77dc127";
    public const string WeaponmasterExpandedManeuverBuff = "cc76c60e-6a5c-4c1a-8758-fd47bfcd462b";
    public const string BrawlerExpandedManeuver = "7576814f-461b-43ed-8d84-29774c9e88b1";
    public const string BrawlerExpandedManeuverAbility = "8572ebb4-1ea3-4740-a637-5222d884213f";
    public const string BrawlerExpandedManeuverBuff = "c5f73fd6-10f9-48ef-8612-f9af4d90847a";

    // ── Phase 6 paths ─────────────────────────────────────────────────────
    public const string ArcherPath = "bbd89c89-ea38-4bb9-a206-0b8ca18eca46";
    public const string ArcherTrance = "2296181d-33ce-494f-8d3f-b22a20055c6a";
    public const string ArcherManeuverBuff = "2c546663-21af-4f5b-acca-3c9e59596b5d";
    public const string ArcherManeuverAbility = "6b2ea73e-e8ea-40f6-b027-ae5db6d79c38";
    public const string ArcherExpandedBuff = "45b6c711-0bd3-4325-95ad-967541741f49";
    public const string ArcherExpandedAbility = "60847f03-9e4b-419a-9b12-b7e9a90c8f95";
    public const string ArcherExpandedFeature = "a6f10673-1ea4-415e-a0a2-4059f11dda93";

    public const string AsceticPath = "e6e03572-423e-4023-a757-3429244c6873";
    public const string AsceticTrance = "78a24005-5aa5-4784-8159-5536bed3c6e8";
    public const string AsceticManeuverBuff = "f34eb5bf-7602-4689-a990-5b03b5af5ca9";
    public const string AsceticManeuverAbility = "7799bf41-42df-42f7-a6ff-44ed08407eb3";
    public const string AsceticExpandedBuff = "d8ea76e8-7992-429a-9319-dc764b971b98";
    public const string AsceticExpandedAbility = "64c50c9f-9f13-4a14-9589-ac3cb0d70035";
    public const string AsceticExpandedFeature = "7b453131-74aa-42fb-854b-826c64ab151b";

    public const string AssassinsPath = "aca9ff81-85e0-4323-b9d9-9213501d53b7";
    public const string AssassinsTrance = "1940710e-9565-4ec4-a3e2-1de352782b2f";
    public const string AssassinsManeuverBuff = "93fd8db6-067c-4531-bcfe-6d61b21c151c";
    public const string AssassinsManeuverAbility = "039a0c6a-a1e7-4191-807e-4c28eb7fad82";
    public const string AssassinsExpandedBuff = "ee446035-2e37-489e-8c33-cbffe59b0db0";
    public const string AssassinsExpandedAbility = "3e93350a-1544-4b4c-9d04-8b12496459b0";
    public const string AssassinsExpandedFeature = "e129a6f6-eb23-479a-857a-72ae107c4bf9";

    public const string DervishPath = "f58bc750-0ad7-4edc-b012-f592a43e9147";
    public const string DervishTrance = "ece20873-5096-4550-a830-08601e7ef4db";
    public const string DervishManeuverBuff = "f34c920c-2456-4f85-8d08-44ccc09a3ab9";
    public const string DervishManeuverAbility = "b27925ef-3afe-4eab-bd59-181ec954bc9b";
    public const string DervishExpandedBuff = "5ef98d64-a521-4817-867f-3d162236ef85";
    public const string DervishExpandedAbility = "af6d87bb-a54a-447e-ba7d-4d0bfab3f5c2";
    public const string DervishExpandedFeature = "4a45a982-e2ee-4b92-b87e-c4e6df704b59";

    public const string FeralWarriorPath = "252d4841-a6a7-4574-b338-237e2129b291";
    public const string FeralWarriorTrance = "6481ec2b-a273-47a9-a615-327e6ad49304";
    public const string FeralWarriorManeuverBuff = "fbd180c7-4327-4b1c-8f8f-aca440725fa5";
    public const string FeralWarriorManeuverAbility = "d49e53b5-49d9-42db-a155-1c06d1657211";
    public const string FeralWarriorExpandedBuff = "fd28f519-3c53-4319-a3a4-e211cd99eb65";
    public const string FeralWarriorExpandedAbility = "9223e2f3-8e4a-4e8c-95df-b29151bd0854";
    public const string FeralWarriorExpandedFeature = "2966be41-92dd-48d6-aabf-0adc1c915518";

    public const string GladiatorPath = "96429534-22f1-4f2a-b9be-a1597a4de1f1";
    public const string GladiatorTrance = "826309ab-cf7a-48f0-90c4-b385b703e1f7";
    public const string GladiatorManeuverBuff = "8ab65536-3ee4-4ef8-a583-2cb6dec456d7";
    public const string GladiatorManeuverAbility = "34da3d80-5b73-4ca4-b3b8-654ab3049e30";
    public const string GladiatorExpandedBuff = "737f8471-e124-4f6f-98a8-620ec0c85864";
    public const string GladiatorExpandedAbility = "f64bb947-32da-4526-91d0-338b13bbf346";
    public const string GladiatorExpandedFeature = "033e27af-42e6-49a6-a120-f4543c36f33a";

    public const string InfiltratorPath = "e81ce535-81d6-44a3-bf8b-1b7628eba3e8";
    public const string InfiltratorTrance = "e3de5a9a-4692-446c-af0f-6b5fc543e87f";
    public const string InfiltratorManeuverBuff = "6bc15163-3c8e-4e39-a1e7-727f69650249";
    public const string InfiltratorManeuverAbility = "4ba0f2e9-03f6-4711-b344-7364ed7d44fb";
    public const string InfiltratorExpandedBuff = "6a6a53d7-0412-41ad-b51a-6ba4198b2c8f";
    public const string InfiltratorExpandedAbility = "a07a826b-f97a-4b07-8659-3ad91d687d8e";
    public const string InfiltratorExpandedFeature = "44008d26-365d-460e-870d-cea52e0d33d3";

    public const string InterceptorPath = "8864c710-9eec-407b-8cdd-bf3ea0136882";
    public const string InterceptorTrance = "491408f3-dc98-4e4c-93a1-67018a37e77c";
    public const string InterceptorManeuverBuff = "40ae133d-d2f8-420d-b933-2afd6a0239cb";
    public const string InterceptorManeuverAbility = "c13c7674-e0e5-4b04-8040-8e66c07a6f03";
    public const string InterceptorExpandedBuff = "c6fad9c5-f9e5-420d-afa1-4b7c52617d08";
    public const string InterceptorExpandedAbility = "b0153754-3bd9-4faa-8ca1-e6abef0ad06c";
    public const string InterceptorExpandedFeature = "c7b5a25c-f324-4013-834b-29becf576089";

    public const string MindKnightPath = "68af173e-3bd4-45ce-a30a-280fb549d7ca";
    public const string MindKnightTrance = "fd1bf73e-87e0-465b-9e6f-53637893ac01";
    public const string MindKnightManeuverBuff = "0889b0f8-1164-443b-89e8-39df62ce833b";
    public const string MindKnightManeuverAbility = "ca604b3c-3ab3-4d6b-b070-f4f87f95e8ae";
    public const string MindKnightExpandedBuff = "3f76285a-e64c-4797-b1a5-3e20ed421e44";
    public const string MindKnightExpandedAbility = "6315ddfe-b7d7-4247-8f25-a40011daf7d2";
    public const string MindKnightExpandedFeature = "2da09fc3-43a6-4657-9a26-d48f417a19b5";

    public const string SurvivorPath = "4f1de9f3-b5e6-45d5-8924-7cad9408b5f8";
    public const string SurvivorTrance = "125516a1-b1b7-4409-b010-07d3c0bfb0e6";
    public const string SurvivorManeuverBuff = "bbfc0ecb-78c5-48a5-bdf8-bb652853f9ad";
    public const string SurvivorManeuverAbility = "2697cd10-aa62-4c79-bc81-cb4d37c73e18";
    public const string SurvivorExpandedBuff = "7c9d212a-b12d-4047-b90d-c3bc988581b0";
    public const string SurvivorExpandedAbility = "1e1657aa-949e-4296-b10b-b00eafeecdf8";
    public const string SurvivorExpandedFeature = "2d41716e-fc02-4019-a822-6bd3f15648bb";

    // ── Phase 7: Trance buffs + activatables ─────────────────────────────
    public const string WeaponmasterTranceBuff = "d5a2e744-f6e9-4b87-97ac-ee1aff803f94";
    public const string WeaponmasterTranceActivatable = "ff8d0d38-f3bc-436d-902b-b5f2f90703b4";
    public const string BrawlerTranceBuff = "01ca5136-f40e-4089-abce-56768d8aae09";
    public const string BrawlerTranceActivatable = "47f94e0d-ef34-4c9f-a0ea-d54fa1ecf4a4";
    public const string ArcherTranceBuff = "a35dd7e4-b3d2-4b33-bc0e-e067e54c230f";
    public const string ArcherTranceActivatable = "578a76e3-f7e5-46a3-a74f-ddad581ed8fb";
    public const string AsceticTranceBuff = "74d35475-4711-4974-97eb-d7cc984703f6";
    public const string AsceticTranceActivatable = "bfd88c9a-33c3-4f96-8729-3511e214b3ea";
    public const string AssassinsTranceBuff = "fc95c1a4-d7bb-4b6e-a61b-36771f0e636b";
    public const string AssassinsTranceActivatable = "951b289b-449b-4cdb-b38c-39a06df90931";
    public const string DervishTranceBuff = "2a7ae752-4cc6-4772-90c4-1ba3ada63858";
    public const string DervishTranceActivatable = "bc67a431-62e2-4e1d-9908-937cd48e62e6";
    public const string FeralWarriorTranceBuff = "e9c624a9-8a45-4ffa-bea4-e10207bdbc8c";
    public const string FeralWarriorTranceActivatable = "4b320c0a-b605-4897-8c3c-66544631c132";
    public const string GladiatorTranceBuff = "10299f85-fc14-4b0a-98f2-806fcbb684e0";
    public const string GladiatorTranceActivatable = "814cf1ce-f90d-441a-aee0-657a5fc2592e";
    public const string InfiltratorTranceBuff = "c18ceb67-aa5f-4462-b7b6-471988c98185";
    public const string InfiltratorTranceActivatable = "45549396-8cf1-4c03-9bfa-15249361aa68";
    public const string InterceptorTranceBuff = "675ef6ad-d07f-45d1-a17d-4a82b735bbbe";
    public const string InterceptorTranceActivatable = "4744388d-23a0-4e64-9c34-50c763acaccf";
    public const string MindKnightTranceBuff = "1f6a5794-d766-4915-bb17-5804907698c5";
    public const string MindKnightTranceActivatable = "ace8f1d8-bd59-486f-8d37-1f592248f24a";
    public const string SurvivorTranceBuff = "417bbd4a-9fa8-477a-8405-ce0e55ce9c3f";
    public const string SurvivorTranceActivatable = "c524c3dd-a50a-4d31-a59b-9270398de71b";

    public static string GetTranceActivatableGuid(string baseName) => baseName switch
    {
        "Weaponmaster" => WeaponmasterTranceActivatable,
        "Brawler" => BrawlerTranceActivatable,
        "Archer" => ArcherTranceActivatable,
        "Ascetic" => AsceticTranceActivatable,
        "Assassins" => AssassinsTranceActivatable,
        "Dervish" => DervishTranceActivatable,
        "FeralWarrior" => FeralWarriorTranceActivatable,
        "Gladiator" => GladiatorTranceActivatable,
        "Infiltrator" => InfiltratorTranceActivatable,
        "Interceptor" => InterceptorTranceActivatable,
        "MindKnight" => MindKnightTranceActivatable,
        "Survivor" => SurvivorTranceActivatable,
        _ => throw new System.ArgumentException($"Unknown path baseName: {baseName}")
    };

    // ── Phase 7b: Per-path parent ability + trance toggles ────────────────
    public const string WeaponmasterPathParent = "ca4b75aa-acc4-4327-9021-8ee8743ca1c2";
    public const string WeaponmasterTranceToggleStd = "51d96180-287e-4465-a8c0-3baafdfda08f";
    public const string WeaponmasterTranceToggleSwift = "ee54006d-bf71-4081-98c0-bd23ee726711";
    public const string BrawlerPathParent = "39325aef-cbd0-4076-9f88-8b3b25bbd39a";
    public const string BrawlerTranceToggleStd = "2e10f25d-958c-456f-a7e5-f4d2bf59ad27";
    public const string BrawlerTranceToggleSwift = "11ae220a-2906-4e54-9468-63deba7eefed";
    public const string ArcherPathParent = "12ed518e-b9e2-4871-a461-d326c90eb391";
    public const string ArcherTranceToggleStd = "b29338ea-5dba-4ac8-9aa3-e452b3d9aacc";
    public const string ArcherTranceToggleSwift = "89e4b8d8-4ced-46fc-87ea-2cb3b21be383";
    public const string AsceticPathParent = "d81e4997-1fac-46db-9060-c6dd852af8de";
    public const string AsceticTranceToggleStd = "ed9c204f-74a0-4699-82fe-c9b1c08f019e";
    public const string AsceticTranceToggleSwift = "444cb470-8a8a-4dc7-8080-d1f7940966ec";
    public const string AssassinsPathParent = "4bde8086-288f-47a1-84db-11815fb8f126";
    public const string AssassinsTranceToggleStd = "af27dde8-1b21-433e-929f-525d5f28580c";
    public const string AssassinsTranceToggleSwift = "b5a5f685-f307-4c2a-92a6-e2e730b2db7c";
    public const string DervishPathParent = "0a7c9cf8-3095-45a7-b9f6-18877bd68543";
    public const string DervishTranceToggleStd = "6b0b99a4-a10b-42e7-b29d-08ff1716ee76";
    public const string DervishTranceToggleSwift = "f6ad545d-6a85-4bb7-a145-48a258885920";
    public const string FeralWarriorPathParent = "b9df9083-8809-4ae3-953b-23a53de3a15a";
    public const string FeralWarriorTranceToggleStd = "3850feff-b448-44ed-9f58-b96d4aa1abc7";
    public const string FeralWarriorTranceToggleSwift = "cc462382-02e1-4f67-95ce-6b75998bc802";
    public const string GladiatorPathParent = "21d3030f-3ef5-43e4-87a0-47d9cfd9fa67";
    public const string GladiatorTranceToggleStd = "156ef1d5-07e8-46db-9c9b-53588e373385";
    public const string GladiatorTranceToggleSwift = "890e381d-2188-49fd-9921-7fd440a313e4";
    public const string InfiltratorPathParent = "98f6c94d-c22a-46ca-b236-b27e01033fb4";
    public const string InfiltratorTranceToggleStd = "a698fbcc-f1be-41cc-aef7-73ed6609d5db";
    public const string InfiltratorTranceToggleSwift = "11321307-a752-41cb-a613-0e47f095dfeb";
    public const string InterceptorPathParent = "5bdf19f3-1e49-4cc4-bf43-3269c1935dc9";
    public const string InterceptorTranceToggleStd = "4838b757-f5d9-449e-938a-8d20642b73bc";
    public const string InterceptorTranceToggleSwift = "205160c0-a685-4064-a252-7c7854461243";
    public const string MindKnightPathParent = "3bed4cbd-0e4c-4fbb-9aee-466e43bf9d1b";
    public const string MindKnightTranceToggleStd = "ece9da41-f4cf-4c8a-bafc-255a8d6b6cf8";
    public const string MindKnightTranceToggleSwift = "70e605be-ed20-45fa-a73f-230d7bfcd95d";
    public const string SurvivorPathParent = "0d0e6499-b8eb-44a3-ab75-c12ac26a052a";
    public const string SurvivorTranceToggleStd = "c7e11645-8666-4c9d-b3a3-31916c6c8015";
    public const string SurvivorTranceToggleSwift = "acbec142-6f02-4528-91a4-1a522a7341ac";

    // ── Twisting Paths / Pathweaving / Martial Power ──────────────────────
    public const string TwistingPathsAbility = "4e6b862e-4616-41b8-8141-88fb6adaa61e";
    public const string TwistingPathsBuff = "67e30e1c-d4d7-490b-a4a7-3c836bce1412";
    public const string PathweavingAbility = "b1bc2908-4352-4dba-88bd-edb56a9f10a9";
    public const string PathweavingBuff = "84ac5c73-744a-48ba-8818-b228a8364c35";
    public const string PathweavingResource = "fd02a1e7-db1f-4152-9fda-b40faf4983c3";
    public const string MartialPowerAbility = "44a189af-d7ff-4e37-ae1e-bd34a6c9689d";
    public const string MartialPowerBuff = "3317d1d8-45a9-42b0-b488-1ac8c66628bc";
    public const string MartialPowerUsedBuff = "228c8eaa-77a6-4394-bc10-7c35aee6b152";

    public const string MartialPowerWeaponmasterManeuver = "f0cc569f-b199-4037-8898-b3384e942588";
    public const string MartialPowerWeaponmasterExpanded = "1ca01de9-87bd-4b69-8eff-f60e5304409c";
    public const string MartialPowerBrawlerManeuver = "2911922f-9939-4ce6-aafc-39fb86b7a86f";
    public const string MartialPowerBrawlerExpanded = "55403ea4-70d0-4d65-b1c3-32c01851d5ae";
    public const string MartialPowerArcherManeuver = "fa11041a-bd05-4248-8b45-cc2df58acb15";
    public const string MartialPowerArcherExpanded = "411c7184-820c-48b7-ae7a-91d549b9dd6b";
    public const string MartialPowerAsceticManeuver = "e9032567-8596-491f-900f-819313bd7d56";
    public const string MartialPowerAsceticExpanded = "c12a627e-f92c-4715-93a3-6886f89a4a9a";
    public const string MartialPowerAssassinsManeuver = "1d168ac6-81b2-435e-9ac7-7bbe4de04ad6";
    public const string MartialPowerAssassinsExpanded = "f6f8a0ff-2e60-4ccc-af3e-fda7f036f82e";
    public const string MartialPowerDervishManeuver = "a37dd655-7784-456f-857b-ccca12593813";
    public const string MartialPowerDervishExpanded = "68a77cb9-23e1-4725-beaa-4fb827ab4788";
    public const string MartialPowerFeralWarriorManeuver = "d305f1b3-7fee-4a4c-b484-59cd0289ce0f";
    public const string MartialPowerFeralWarriorExpanded = "17677572-c486-456c-a803-6894d4853706";
    public const string MartialPowerGladiatorManeuver = "feb3fc6f-08ae-4eeb-810c-17a67d9ae97d";
    public const string MartialPowerGladiatorExpanded = "b3232ad1-64c7-4c44-9c34-8a6df91a528f";
    public const string MartialPowerInfiltratorManeuver = "adfb10c5-42cc-4316-a53c-ab3ddd9a31c4";
    public const string MartialPowerInfiltratorExpanded = "05d1262c-8753-47e9-804f-5752f0a08da9";
    public const string MartialPowerInterceptorManeuver = "b0286cb1-d62c-4928-95d2-251670922db0";
    public const string MartialPowerInterceptorExpanded = "a805b186-036c-443a-ae98-a92aed739a8d";
    public const string MartialPowerMindKnightManeuver = "612e0223-a346-40de-a77a-fa28c0606809";
    public const string MartialPowerMindKnightExpanded = "bb3efee4-c159-4f50-aa7a-ebeb0a4f4a15";
    public const string MartialPowerSurvivorManeuver = "f30c4b59-0abf-4d59-8050-7aa0593f6a9e";
    public const string MartialPowerSurvivorExpanded = "48b33683-30ab-4fb0-9fb7-02d7df80d245";

    public const string MartialPowerSelectedWeaponmasterManeuver = "f5bef40b-ace3-471d-9658-589b690ca174";
    public const string MartialPowerSelectedWeaponmasterExpanded = "211d878e-1076-48a7-a6d8-166f2513dd00";
    public const string MartialPowerSelectedBrawlerManeuver = "17490f91-3738-4919-84b2-072ec4352997";
    public const string MartialPowerSelectedBrawlerExpanded = "3fe57ef5-4a3b-4193-a041-ba40b28495f9";
    public const string MartialPowerSelectedArcherManeuver = "f26f0902-34ae-4568-bb85-b2a255df8dd5";
    public const string MartialPowerSelectedArcherExpanded = "86b83783-5018-49b0-9a65-fa09e25fc42c";
    public const string MartialPowerSelectedAsceticManeuver = "e22f2977-7fc0-458f-b4fc-24451f245ec7";
    public const string MartialPowerSelectedAsceticExpanded = "4b118c2f-4638-4d2f-9f1d-7ed7266b1206";
    public const string MartialPowerSelectedAssassinsManeuver = "f57667e8-989e-45be-97f6-9e832b9f24a4";
    public const string MartialPowerSelectedAssassinsExpanded = "b7babb51-2c0d-4f36-ad47-efeed43e59db";
    public const string MartialPowerSelectedDervishManeuver = "c33fdc5c-ea79-40ba-9143-ba63d2847f7e";
    public const string MartialPowerSelectedDervishExpanded = "bfbdf5bd-6020-44a1-b42b-15bfb18b949e";
    public const string MartialPowerSelectedFeralWarriorManeuver = "3a25a851-59a0-44b5-9685-c21a065b5475";
    public const string MartialPowerSelectedFeralWarriorExpanded = "03dbc45e-115d-4919-a277-22ade8418e5a";
    public const string MartialPowerSelectedGladiatorManeuver = "5700aa18-ecd4-4a83-8a55-1b8c0a42034c";
    public const string MartialPowerSelectedGladiatorExpanded = "c44e97dc-488e-4e2f-a4fa-0f19c3585611";
    public const string MartialPowerSelectedInfiltratorManeuver = "766d89b3-81f8-4721-8390-a3461bf17bca";
    public const string MartialPowerSelectedInfiltratorExpanded = "b5faeff5-5254-4e25-9a9d-a2e096137933";
    public const string MartialPowerSelectedInterceptorManeuver = "7d44cfc3-9b4c-4bf2-b58a-a118df864022";
    public const string MartialPowerSelectedInterceptorExpanded = "7550328d-dafe-491b-8452-b66b88cc3b76";
    public const string MartialPowerSelectedMindKnightManeuver = "b5c35fc5-5aa8-4fcd-9d02-c0c081687073";
    public const string MartialPowerSelectedMindKnightExpanded = "135c20aa-65c6-4bce-b1e5-ae59f1e649e5";
    public const string MartialPowerSelectedSurvivorManeuver = "755d902b-1952-4afd-bdae-81284e4ff8f4";
    public const string MartialPowerSelectedSurvivorExpanded = "22f22602-4aaf-4640-816d-1123519232d6";

    public const string MartialPowerActivatable = "9b862cda-0b5e-4b6d-a1b3-4a8c03bd8818";
    public const string MartialPowerActivatableBuff = "ccdf480c-4a35-4f4f-b35d-023fffb3f905";

    // ── 2nd-level powers ─────────────────────────────────────────────────
    public const string PowerBodyAdjustment = "83316df3-4170-4c0d-8c41-22c4ff66dfc1";
    public const string PowerBodyPurification = "30494eaa-f4d0-47a0-a86b-e768106f385e";
    public const string PowerStrengthOfMyEnemy = "96fae0a3-50d1-4c6b-bc10-419d7c661f48";
    public const string PowerStrengthOfMyEnemyEnemyDebuff = "9936cafd-93d7-4d09-af24-cf6bd701f4d6";
    public const string PowerStrengthOfMyEnemyCasterBuff = "34a25994-c048-41f4-9804-8f975f57da10";
    public const string PowerStrengthOfMyEnemyBuff = "122e8f25-5625-432a-aead-5f5e2e617a1a";
    public const string PowerAnimalAffinity = "c5663cc3-7342-4c38-856d-68ee0ff5d293";
    public const string PowerAnimalAffinityStrength = "cbf55898-6412-4398-b155-05e439b55e42";
    public const string PowerAnimalAffinityStrengthBuff = "db152423-2b99-4f92-ba14-ba331626eaf9";
    public const string PowerAnimalAffinityDexterity = "271f361f-c843-430d-bf8d-9778471b3a30";
    public const string PowerAnimalAffinityDexterityBuff = "a402c0e2-6d7c-42e6-844e-29c61ae647a9";
    public const string PowerAnimalAffinityConstitution = "2fe7412e-b203-4123-9ad3-b72a02ac79a1";
    public const string PowerAnimalAffinityConstitutionBuff = "0fd7c1ea-970a-414d-9da0-b5e1057d1436";
    public const string PowerAnimalAffinityIntelligence = "cc1c7023-054d-4ae2-b951-9457f8e0d223";
    public const string PowerAnimalAffinityIntelligenceBuff = "b7047590-27ae-45c0-8cf8-1cebb8d11d2b";
    public const string PowerAnimalAffinityWisdom = "88cc26d6-e926-4982-bf16-58bf861a3b1a";
    public const string PowerAnimalAffinityWisdomBuff = "ca068add-ae99-44e8-97cf-d03145ed15de";
    public const string PowerAnimalAffinityCharisma = "af1ff05b-8cf8-4a11-b118-2eda401be5f2";
    public const string PowerAnimalAffinityCharismaBuff = "386b06e4-15c4-45d3-a309-95a7920e9493";
    public const string PowerDetectHostileIntent = "44f1db5d-9e6f-4fec-8876-a8ca7c95fd21";
    public const string PowerDetectHostileIntentBuff = "0680497e-d936-4201-ae93-794eca8f9029";
    public const string PowerHustle = "02bf4471-1e88-411f-a072-e9628a2e5390";
    public const string PowerHustleBuff = "29cba4bc-4346-4374-87be-c8ea806ef797";

    // ── Psionic feats ─────────────────────────────────────────────────────
    public const string PsionicBodyFeat = "772711d2-1c37-4afb-852a-94124d1e2e64";
    public const string SpeedOfThoughtFeat = "3d8c44ec-fa7d-440d-acbc-6e458bef180a";
    public const string SpeedOfThoughtBuff = "21ecf3d4-21db-43bb-a3ce-fbb895af0b98";
    public const string PsionicDodgeFeat = "18e62323-cc52-4f81-982d-57de8addccb6";
    public const string PsionicDodgeBuff = "9f4194e8-3c40-4317-a826-cf218e3b2472";
    public const string CriticalRefocusFeat = "86f7987c-caee-4fa6-a8c8-c0e52b984a43";
    public const string AdvancedWeaponmasterPathFeat = "a1c3e5f7-b2d4-4e68-9f0a-bc12de34fa56";
    public const string AdvancedAsceticPathFeat = "c2d4f6a8-e3b5-4f79-a0b1-cd23ef45ab67";
    public const string AdvancedArcherPathFeat = "d3e5a7b9-f4c6-4a8b-b1c2-de34fa56bc78";
    public const string AdvancedAssassinPathFeat = "e4f6a8b0-a5c7-4e8b-c2d3-ef45fa67cd89";
    public const string AdvancedAssassinStaggerBuff = "f5a7b9c1-b6d8-4f9c-d3e4-fa56ab78de90";
    public const string AdvancedBrawlerPathFeat = "a6b8c0d2-c7e9-4f0a-e4f5-ab67bc89ef01";
    public const string AdvancedDervishPathFeat = "b7c9d1e3-d8f0-4a1b-f5a6-bc78cd90fa12";
    public const string AdvancedFeralWarriorPathFeat = "c8d0e2f4-e9a1-4b2c-a6b7-cd89de01ab23";
    public const string AdvancedMindKnightPathFeat = "d9e1f3a5-fa02-4c3d-b7c8-de90ef12bc34";
    public const string GreaterPsionicWeaponFeat = "531a4c41-01e6-4860-a3dc-b2c124e8111d";
    public const string GreaterPsionicWeaponBuff = "b4d83e29-cc65-5fa2-af23-9e4d15b26f38";
    public const string PsionicFistFeat = "ed7161df-4b58-41f8-ada9-a46168fa1861";
    public const string GreaterPsionicFistFeat = "316556a0-a097-4c6a-afe3-9680776e2557";
    public const string PsionicShotFeat = "bb0fdacf-2da5-4ed5-91ab-83c7995ad819";
    public const string GreaterPsionicShotFeat = "d4f72fed-c0ed-4b69-b658-0af2ac9f9099";
    public const string RapidMetabolismFeat = "591d3b3c-acff-4456-b0f4-c05cdce78d49";
    public const string RapidMetabolismBuff = "b138ef5a-e6b6-48d6-ab41-3212bead7f19";
    public const string OverchannelFeat = "1be4b762-c6cd-4607-8908-912a2b30f483";
    public const string CombatManifestationFeat = "646c15c2-d708-475a-be38-3a72917d3966";
    public const string DeepImpactFeat = "3df4b129-a2c4-4073-ac10-11441462c900";
    public const string DeepImpactDebuff = "b2249006-f5fd-461c-926e-832ab24072f6";
    public const string ExpandedKnowledgeFeat = "f84cf294-c32a-4568-914f-a649dab112dd";
    public const string UpTheWallsFeat = "74be44c5-d82f-468e-8b0e-19bb945d210b";
    public const string UpTheWallsBuff = "855de559-b08f-43a3-b112-8f85e387796e";
    public const string PsionicEndowmentFeat = "9561b13f-d4fd-492f-83ae-85d0f8f9be2f";
    public const string PsionicEndowmentBuff = "75d54bb0-8061-4a60-a33c-832f6ed1f62f";
    public const string GreaterPsionicEndowmentFeat = "a1c2e3f4-1b2c-4d3e-9f80-1a2b3c4d5e60";
    public const string GreaterPsionicEndowmentBuff = "b2d3f4a5-2c3d-4e4f-8a91-2b3c4d5e6f71";
    public const string ExtraBladeSkillFeat = "c3e4a5b6-3d4e-4f50-8b02-3c4d5e6f7a82";
    public const string IntuitiveShotFeat = "d4f5b6c7-4e5f-4061-9c13-4d5e6f7a8b93";
    public const string PsionicCriticalFeat = "6c2ec38d-0160-4c01-951a-06a6066cb581";
    public const string WoundingAttackFeat = "ba41ef92-8813-441a-836d-95f58a07ba05";
    public const string WoundingAttackBuff = "1af891ea-7562-441d-84bb-4cd7a8f7a4c1";
    public const string WoundingAttackAbility = "c9d26a2a-fa57-4213-ac0e-81039ceaa8d7";
    public const string FellShotFeat = "ba1a97c4-f569-417d-9fd6-a025a157b51e";
    public const string FellShotDebuff = "ab458948-67c7-4790-8ebd-2bd9379aade7";
    public const string UnavoidableStrikeFeat = "32e4108c-d82a-424d-8950-7138296c970b";
    public const string UnavoidableStrikeDebuff = "727c624c-2353-4d3f-8d4a-b356f6d103c7";
    public const string IntuitiveFightingFeat = "27843897-fb9c-41a6-9a1c-4d6fb260502e";
    public const string IntuitiveFightingBuff = "b14091f0-df95-44f5-aabc-a0dcb1c19786";
    public const string RecklessOffenseFeat = "f29283ed-f696-4834-b003-85abc9feafe1";
    public const string RecklessOffenseBuff = "627e4a21-5b45-4cdf-8f55-796c3365eae9";
    public const string RecklessOffenseActivatable = "fbf97a24-8130-4aa2-9604-c4d9aacfd509";
    public const string AlignedAttackFeat = "717a2789-20a5-40d6-acb0-6d8b58f65583";

    // ── 5th-level powers ─────────────────────────────────────────────────
    public const string PowerTrueMetabolism = "c0e70249-6f5e-4923-aa24-020ef2ed07ef";
    public const string PowerTrueMetabolismBuff = "1080c524-a095-4d0c-8b4d-a7e79c0032af";
    public const string PowerAdaptBody = "bc924bf7-e311-4a97-bb1e-7fc1bfe57f46";
    public const string PowerAdaptBodyFire = "bfae2183-295d-4b4b-a4aa-2cb5c02a88c1";
    public const string PowerAdaptBodyFireBuff = "f09ec9a3-ba4b-43d1-afdf-554bd95613ae";
    public const string PowerAdaptBodyCold = "6f44b565-56ed-49bd-8014-4d5381e8bfb9";
    public const string PowerAdaptBodyColdBuff = "9e791313-0c37-46ed-9c86-136fd86353ac";
    public const string PowerAdaptBodyElectric = "712158cd-bf95-4447-8348-d74396d11ed5";
    public const string PowerAdaptBodyElectricBuff = "bf33b2ba-0d23-4956-b881-142f81b1a434";
    public const string PowerAdaptBodyAcid = "34c81b18-c6d1-4f11-b92a-e0ecb0af535c";
    public const string PowerAdaptBodyAcidBuff = "ce7b332d-cedb-41de-bcf9-78a467b22faa";
    public const string PowerTrueSeeing = "799e9be1-99dc-4fb3-a91b-2831551c5c01";

    // ── 6th-level powers ─────────────────────────────────────────────────
    public const string PowerDisintegratePsionic = "0bb920ad-1bbe-43f5-81da-9176dcbc339b";
    public const string PowerMindBlankPersonal = "d12ed557-70a5-420d-8e7d-747c90b31850";
    public const string PowerMindBlankPersonalBuff = "fb166fb5-8022-4f1f-a5c7-2d4b9adea33f";
    public const string PowerOakBody = "3fd38783-2c14-453c-9f5f-2ae2b0a1f0de";
    public const string PowerOakBodyBuff = "815c7a00-18e0-474a-938e-c89d72ad1b79";
    public const string PowerBodyOfIron = "8b69fe2e-406d-48ce-875b-c67b6d59588f";
    public const string PowerBodyOfIronBuff = "ab8b0ced-6d61-4871-8b6a-3090ef4b15af";

    // ── Premade build ─────────────────────────────────────────────────────
    public const string PrebuildPsychicWarriorFeatureList = "a1b2c3d4-e5f6-7890-abcd-ef1234567890";

    // ── Archetypes ────────────────────────────────────────────────────────
    public const string ThunderJarlArchetype = "c9d1e2f3-a4b5-6789-cdef-012345678901";

    // ── Level 20 capstone ─────────────────────────────────────────────────
    public const string EternalWarriorFeature = "51d67668-4bfa-4976-8717-7d35acab9fbd";
    public const string EternalWarriorAbility = "afd53a7f-8704-4eb3-9c6b-b57afc5f8ef2";
    public const string EternalWarriorBuff = "8421551e-e2bb-4289-bb8e-56c42f3e7347";
    public const string EternalWarriorResource = "3d89fce0-b5e5-415e-8d13-d6d0a5d00037";

    // ── 1st-level powers (new batch) ─────────────────────────────────────
    public const string PowerMetaphysicalClaw = "fcaa4483-e0d0-4638-99d8-90ea99352180";
    public const string PowerMetaphysicalClawBuff = "2218eadb-ae6f-4174-ba56-72de38ab156e";
    public const string PowerExpansion = "8ab23579-fdfc-434f-b701-3090e0a8dd6d";
    public const string PowerCompression = "7e1f61e0-f6b6-4385-86c9-b45959fa8333";
    public const string PowerConcealingAmorpha = "a71580af-d6be-47a6-a610-296c2f0e7f81";

    // ── 2nd-level powers (new batch) ─────────────────────────────────────
    public const string PowerPsionicLionsCharge = "d8fe2e16-820f-4ad5-aa51-cc754d6e7a02";
    public const string PowerPsionicLionsChargeBuff = "3c9c366f-6f48-49b5-9638-9398ac879ad5";

    // ── 3rd-level powers ─────────────────────────────────────────────────
    public const string PowerEvadeBurst = "3e9a8b05-3cc4-40dd-9134-ff223f356672";
    public const string PowerEvadeBurstBuff = "624bc4b5-1162-42c6-98a9-efe61d3335f9";
    public const string PowerUbiquitousVision = "19c28276-ab11-46f5-8bc0-2fe66f9c2e61";
    public const string PowerUbiquitousVisionBuff = "d8c8786c-3df8-42a1-80e7-b6656741a519";
    public const string PowerPhysicalAcceleration = "f80b477a-1884-4a94-9fe5-56f5f515e43b";
    public const string PowerDimensionSlide = "5fb915a0-231a-434c-9d49-e5f680b1165e";
    public const string PowerVampiricBlade = "c7808c67-47cf-4e61-bf02-38af86449443";
    public const string PowerVampiricBladeBuff = "cfd1514b-176f-4738-9df6-f6d0620878a9";
    public const string PowerVampiricBladeTempHPBuff = "5426f158-37bb-4a9b-8508-eeaae72eea0d";
    public const string PowerMentalBarrier = "70fe6322-ad11-4e49-9bac-59cde353a81f";
    public const string PowerMentalBarrierBuff = "94156c7d-06bc-4e7d-bc99-5606ac165e87";
    public const string PowerConcealingAmorphaGreater = "d2b04088-6bd7-4248-8e8c-686ee5aaedf9";
    public const string PowerConcealingAmorphaGreaterBuff = "49d6736d-7b2d-4c63-8c7c-13393b7402e2";
    public const string PowerGraftWeapon = "8cf00295-01ba-4ef7-9c45-677e0788bced";
    public const string PowerGraftWeaponBuff = "4f50d23a-9121-4e3e-9733-17a21ff9f746";
    public const string PowerKeenEdgePsionic = "52e52c80-24fd-44e0-83ee-6b68fb994dbd";

    // ── 4th-level powers ─────────────────────────────────────────────────
    public const string PowerInertialBarrier = "b82ece7f-25bd-4980-b79e-77d02e2ad842";
    public const string PowerInertialBarrierBuff = "df58d06b-e6f6-4444-a4d8-66433032a003";
    public const string PowerZealousFury = "8712a2e8-8fb8-45e3-a572-9fb1a4888b84";
    public const string PowerZealousFuryBuff = "9d815a02-c93f-4832-94b2-2d3124b42ebe";
    public const string PowerEnergyAdaptation = "2faa6fd6-87c9-4835-be69-32588b105fec";
    public const string PowerEnergyAdaptationFire = "6b680694-0adb-459c-bc67-3e14506f6521";
    public const string PowerEnergyAdaptationFireBuff = "3c9e0aad-3b02-4fe5-95e6-07331629bf58";
    public const string PowerEnergyAdaptationCold = "9683fb6c-3616-4255-b4f5-e8f581acfd70";
    public const string PowerEnergyAdaptationColdBuff = "30cde910-205f-4ec5-8432-aa22fb5fd9f1";
    public const string PowerEnergyAdaptationElec = "f3f1702e-909a-49c9-8339-d8d72654e6d3";
    public const string PowerEnergyAdaptationElecBuff = "714eb70b-d628-488b-bf2e-44b75670fec8";
    public const string PowerEnergyAdaptationAcid = "e65b1556-5068-4e00-81f1-822cd1c92f14";
    public const string PowerEnergyAdaptationAcidBuff = "82426e3e-875a-4126-9105-f128fb543cb3";
    public const string PowerEnergyAdaptationSonic = "99b02cba-a615-4c86-8cfb-4948f524521b";
    public const string PowerEnergyAdaptationSonicBuff = "5e2ac24c-984b-4400-88b1-79912036f2ac";
    public const string PowerBattleTransformation = "a8878087-dc8d-4a52-baff-d9439f8416cf";
    public const string PowerBattleTransformationBuff = "994096a0-fe24-4fc4-9b09-c92cca160ae6";
    public const string PowerDimensionDoor = "6094f147-9dcd-48e4-bd75-58a501348122";
    public const string PowerFreedomOfMovement = "730865f7-8759-4134-9eea-0933634f7c8c";
    public const string PowerWeaponOfEnergy = "bdaa6681-3ed7-48b6-b741-5b38be008174";
    public const string PowerWeaponOfEnergyFire = "aa7c6d5e-9cf4-44ed-9d64-6933e75aab56";
    public const string PowerWeaponOfEnergyFireBuff = "9d3aac95-a16f-48ee-95c5-ca2d1d0e8c57";
    public const string PowerWeaponOfEnergyCold = "8165bbac-0a9e-451c-b267-fcb615c0e6ee";
    public const string PowerWeaponOfEnergyColdBuff = "3c68a35a-15da-4b2c-acbe-58be9b06720f";
    public const string PowerWeaponOfEnergyElectric = "97c8ae63-d847-4b0c-aa77-73638cdf5695";
    public const string PowerWeaponOfEnergyElectricBuff = "6009198e-8133-4301-aa09-28b79682e312";
    public const string PowerWeaponOfEnergyAcid = "57ba6d94-bfa9-45ac-85db-36b06918f745";
    public const string PowerWeaponOfEnergyAcidBuff = "e82e1bb9-9e3c-44c8-9dc4-9f6bb8fb8349";
    public const string PowerSteadfastPerception = "a736af3c-b7c8-4b83-998f-1d150627d914";
    public const string PowerSteadfastPerceptionBuff = "56cc7a10-19a3-4ee3-b1de-cd072c7de8ef";

    // ── SoulKnife class ──────────────────────────────────────────────────────
    public const string SoulKnifeClass               = "7c4a2f91-e3b8-4d65-a017-58c91b2e3f40";
    public const string SoulKnifeProgression         = "8d5b3aa2-f4c9-4e76-b128-69da2c3f4a51";
    public const string SoulKnifePrebuild            = "9f8e7d6c-5b4a-4938-8271-60c5f4e3d2b1";
    public const string SoulKnifeProficiencies       = "9e6c4bb3-a5da-4f87-c239-7aeb3d4a5b62";

    // ── Mind Blade ────────────────────────────────────────────────────────────
    // Per-weapon-entry GUIDs are generated deterministically via MD5 in MindBlade.Configure().
    public const string MindBladeFeature             = "af7d5cc4-b6eb-4198-d34a-8bfc4e5b6c73";

    // Custom weapon type blueprints (fixed stats, mutable visuals)
    public const string MindBladeLightWeaponType     = "b19f8ee7-d91f-4a73-c34b-9c1e6f7d8e95";
    public const string MindBladeOneHandedWeaponType = "c2a09ff8-ea20-4b84-d45c-ad2f7a8e9fa6";
    public const string MindBladeTwoHandedWeaponType = "d3b1a009-fb31-4c95-e56d-be3a8b9fadb7";

    // Custom item weapon blueprints
    public const string MindBladeLightWeapon         = "e4c2b11a-0c42-4da6-f67e-cf4b9cafc8c8";
    public const string MindBladeOneHandedWeapon     = "f5d3c22b-1d53-4eb7-079f-d05c0dbfd9d9";
    public const string MindBladeTwoHandedWeapon     = "06e4d33c-2e64-4fc8-180a-e16d1ece0aea";

    // Enhanced Mind Blade (enhancement pool + special-ability toggles)
    // Per-ability toggle/buff GUIDs are generated deterministically via MD5 in EnhancedMindBlade.Configure().
    public const string EnhancedMindBladeFeature       = "1a2b3c4d-5e6f-4a7b-8c9d-0e1f2a3b4c5d";
    public const string EnhancedMindBladeEnhanceToggle = "2b3c4d5e-6f7a-4b8c-9d0e-1f2a3b4c5d6e";
    public const string EnhancedMindBladeEnhanceBuff   = "3c4d5e6f-7a8b-4c9d-0e1f-2a3b4c5d6e7f";
    public const string EnhancedMindBladePoolResource  = "7a8b9c0d-1e2f-4a3b-4c5d-6e7f8a9b0c1d";
    // Ability-tier features, granted at the level each tier of special abilities unlocks.
    public const string EnhancedMindBladeAbilities5    = "4d5e6f7a-8b9c-4d0e-1f2a-3b4c5d6e7f8a";
    public const string EnhancedMindBladeAbilities7    = "5e6f7a8b-9c0d-4e1f-2a3b-4c5d6e7f8a9b";
    public const string EnhancedMindBladeAbilities12   = "6f7a8b9c-0d1e-4f2a-3b4c-5d6e7f8a9b0c";

    // Form buffs and toggles
    public const string MindBladeLightBuff           = "4ac8177a-62a8-43ac-5c4e-25ab5c2a4e2e";
    public const string MindBladeOneHandedBuff       = "5bd9288b-73b9-44bd-6d5f-36bc6d3b5f3f";
    public const string MindBladeTwoHandedBuff       = "6cea399c-84ca-45ce-7e6a-47cd7e4c6040";
    public const string MindBladeLightToggle         = "7dfb4aad-95db-46df-8f7b-58de8f5d7151";
    public const string MindBladeOneHandedToggle     = "8eac5bbe-a6ec-47ea-9a8c-69ef9a6e8262";
    public const string MindBladeTwoHandedToggle     = "9fbd6ccf-b7fd-48fb-ab9d-7afa0b7f9373";

    // Double-weapon form (two-bladed sword, double axe, urgrosh, hooked hammer)
    public const string MindBladeDoubleWeaponType    = "a0fce11b-c81f-4b1a-9c2d-3e4f5a6b7c8d";
    public const string MindBladeDoublePrimaryWeapon = "b1adf22c-d92a-4c2b-ad3e-4f5a6b7c8d9e";
    public const string MindBladeDoubleOffhandWeapon = "c2bef33d-ea3b-4d3c-be4f-5a6b7c8d9eaf";
    public const string MindBladeDoubleBuff          = "d3caf44e-fb4c-4e4d-cf5a-6b7c8d9eafb0";
    public const string MindBladeDoubleToggle        = "e4dbf55f-ac5d-4f5e-da6b-7c8d9eafb0c1";

    // ── Psychic Strike ────────────────────────────────────────────────────────
    public const string PsychicStrikeFeature         = "d2a08ff7-e91e-440c-067d-be2f718e9fa6";
    public const string PsychicStrikeChargeBuff      = "e3b19008-fa2f-450d-178e-cf30829fab7b";
    public const string PsychicStrikeChargeAbility   = "f4c2a119-0b30-460e-29af-d04193ac8c2c";
    public const string PsychicStrikeRechargeAbility = "05d3b22a-1c41-470f-3ab0-e152040dde3d";

    // ── Blade Skills ──────────────────────────────────────────────────────────
    public const string BladeSkillsSelection         = "16e4c33b-2d52-481a-4bc1-f263050ece4e";
    public const string BladeSkillFocusedOffense     = "27f5d44c-3e63-492b-5cd2-374160fdf55f";
    public const string BladeSkillFocusedOffenseBuff = "38068c6e-4f74-4a3c-6ee3-485271fe66e0";
    public const string BladeSkillEvasion            = "38068c5d-4f74-4b4d-7ef4-596381907771";
    public const string BladeSkillPowerfulStrikes    = "490a9d7e-5085-4c5e-8f05-6a74920188b2";
    public const string BladeSkillFocusedDefense     = "5a1bb08f-6196-4e5c-9d16-8b85a42399c4";
    public const string BladeSkillFocusedDefenseBuff = "6b2cc190-72a7-4f6d-ae27-9c96b53400e5";
    public const string BladeSkillImprovedEvasion    = "6b2cc19a-72a7-4f6d-ae27-9c96b53400d5";
    public const string BladeSkillImprovedEnhancement= "7c3dd2ab-83b8-407e-bf38-ad07c64511e6";
    public const string BladeSkillVampiricBlade      = "8d4ee3bc-94c9-4180-c049-be18d75622f7";
    public const string BladeSkillPsionicTraining    = "9e5ff4cd-a5da-4291-d15a-cf29e86733a8";
    public const string BladeSkillTelekineticEdge    = "af600506-b6eb-43a2-e26b-d03af97844b9";
    public const string BladeSkillTrapfinder         = "b0711617-c7fc-44b3-f37c-e14b0a8955ca";
    public const string BladeSkillMindBladeFinesse   = "c1822728-d80d-45c4-048d-f25c1b9a66db";
    public const string BladeSkillWeaponSpecial      = "d2933839-e91e-46d5-159e-036d2cab77ec";
    public const string BladeSkillReapersBlade       = "e3a4494a-fa2f-4706-26af-147e3dbc88fd";
    public const string BladeSkillTelekineticAthleticism = "f4b55a5b-0b30-4817-37b0-258f4ecd990e";
    public const string BladeSkillDeadlyBlow         = "05c66b7d-1c41-4a39-48c1-369f5fde1b30";
    public const string BladeSkillGhostStep          = "16d78c8e-2d52-4b4a-59d2-470f60fe2c41";
    public const string BladeSkillGhostStepAbility   = "27e89d9f-3e63-4c5b-6ae3-581f71ff3d52";
    public const string BladeSkillCleaveSpace        = "38f9ae0a-4f74-4d6c-7bf4-692f820f4e63";
    public const string BladeSkillCleaveSpaceAbility = "490abf1b-5085-4e7d-8c05-7a3f931f5f74";
    public const string BladeSkillReachingBlade      = "5a1bd02c-6196-4f8e-9d16-8b4fa42f6085";
    public const string BladeSkillReachingBladeToggle= "6b2ce13d-72a7-4a9f-ae27-9c5fb53f7196";
    public const string BladeSkillReachingBladeBuff  = "7c3df24e-83b8-4ba0-bf38-ad6fc64f82a7";

    // ── Blade Skills: status-effect strikes ───────────────────────────────────
    public const string BladeSkillStunningBlade      = "8d4f035f-94c9-4cb1-c049-be7fd75f93b8";
    public const string BladeSkillStunningBladeAbility = "9e500460-a5da-4dc2-d15a-cf8fe86fa4c9";
    public const string BladeSkillStunnedBuff        = "af611571-b6eb-4ed3-e26b-d09ff970b5da";
    public const string BladeSkillDazzlingBlade      = "b0722682-c7fc-4fe4-f37c-e1aff081c6eb";
    public const string BladeSkillDazzlingBladeAbility = "c1833793-d80d-40f5-048d-f2b002f2d7fc";
    public const string BladeSkillDazzledBuff        = "d29448a4-e91e-4106-159e-03c113f3e80d";
    public const string BladeSkillWingClip           = "e3a559b5-fa2f-4217-26af-14d224f4f91e";
    public const string BladeSkillWingClipAbility    = "f4b66ac6-0b30-4328-37b0-25e335f50a2f";
    public const string BladeSkillWingClipBuff       = "05c77bd7-1c41-4439-48c1-36f446f61b30";

    // ── Blade Skills: psychic-strike riders ────────────────────────────────────
    public const string BladeSkillKnifeToTheSoul     = "16d88ce8-2d52-454a-59d2-47f557f72c41";
    public const string BladeSkillDevastatingBlade   = "27e99df9-3e63-465b-6ae3-58f668f83d52";
    public const string BladeSkillExplodingCritical  = "38faae0a-4f74-476c-7bf4-69f779f94e63";
    public const string BladeSkillDispellingStrike   = "490bbf1b-5085-487d-8c05-7a0880fa5f74";
    public const string BladeSkillDispellingStrikeAbility = "5a1cd02c-6196-499e-9d16-8b1991fb6085";
    public const string BladeSkillPsychokineticBlast = "6b2de13d-72a7-4aaf-ae27-9c2aa20c7196";
    public const string BladeSkillPsychokineticBlastAbility = "7c3ef24e-83b8-4bb0-bf38-ad3bb31d8207";
    public const string BladeSkillMarkOfTheChallenger = "8d40035f-94c9-4cc1-c049-be4cc42e9318";
    public const string BladeSkillMarkOfTheChallengerAbility = "9e511460-a5da-4dd2-d15a-cf5dd53f0429";
    public const string BladeSkillMarkOfTheChallengerBuff = "af622571-b6eb-4ee3-e26b-d06ee640153a";

    // ── Blade Skills: Energy Blades (Fire/Ice/Lightning/Thunder) ───────────────
    public const string BladeSkillFireBlade          = "05c66b6c-1c41-4928-48c1-369f5fde0a1f";
    public const string BladeSkillFireBladeToggle    = "16d77c7d-2d52-4a39-59d2-470f60ef1b20";
    public const string BladeSkillFireBladeBuff      = "27e88d8e-3e63-4b4a-6ae3-581f71f02c31";
    public const string BladeSkillIceBlade           = "38f99e9f-4f74-4c5b-7bf4-692f82f13d42";
    public const string BladeSkillIceBladeToggle     = "490aaf0a-5085-4d6c-8c05-7a3f93f24e53";
    public const string BladeSkillIceBladeBuff       = "5a1bc01b-6196-4e7d-9d16-8b4fa4f35f64";
    public const string BladeSkillLightningBlade     = "6b2cd12c-72a7-4f8e-ae27-9c5fb5f46075";
    public const string BladeSkillLightningBladeToggle = "7c3de23d-83b8-4a9f-bf38-ad6fc6f57186";
    public const string BladeSkillLightningBladeBuff = "8d4ef34e-94c9-4ba0-c049-be7fd7f68297";
    public const string BladeSkillThunderBlade       = "9e5fa45f-a5da-4cb1-d15a-cf8fe8f793a8";
    public const string BladeSkillThunderBladeToggle = "af60b56a-b6eb-4dc2-e26b-d09ff9f8a4b9";
    public const string BladeSkillThunderBladeBuff   = "b071c67b-c7fc-4ed3-f37c-e1afaaf9b5ca";

    // Energy blade secondary riders (require the matching energy blade)
    public const string BladeSkillFirestorm          = "c182d78c-d80d-4ef4-048d-f2bac20c6edb";
    public const string BladeSkillFreezingIce        = "d293e89d-e91e-40f5-159e-03cbd31d7fec";
    public const string BladeSkillLightningArc       = "e3a4f9ae-fa2f-4106-26af-14dce42e80fd";
    public const string BladeSkillResoundingThunder  = "f4b60abf-0b30-4217-37b0-25edf53f910e";

    // Reach / charge skills
    public const string BladeSkillExtendedStrike       = "05c71bc0-1c41-4328-48c1-360ad64fa21f";
    public const string BladeSkillExtendedStrikeAbility= "16d82cd1-2d52-4439-59d2-471be75fb320";
    public const string BladeSkillExtendedStrikeBuff   = "27e93de2-3e63-454a-6ae3-582cf86fc431";
    public const string BladeSkillFuriousCharge        = "38fa4ef3-4f74-465b-7bf4-693da97fd542";

    // ── SoulKnife bonus feat ──────────────────────────────────────────────────
    public const string SoulKnifeBonusFeat           = "5a1bae8f-6196-4d6f-9016-7b85a31299c3";
    public const string WeaponFocusMindBlade         = "6b2cbf90-7297-4e70-a127-8c96b42300d4";
    public const string GreaterWeaponFocusMindBlade  = "e5a6c7d8-5f60-4172-9d24-5e6f7a8b9c04";
    public const string ImprovedCriticalMindBlade    = "f6b7d8e9-6a71-4283-8e35-6f7a8b9c0d15";
    public const string WeaponSpecializationMindBlade        = "a7c8e9f0-7b82-4394-9f46-7a8b9c0d1e26";
    public const string GreaterWeaponSpecializationMindBlade = "b8d9f0a1-8c93-44a5-8a57-8b9c0d1e2f37";

    // ── Call Weaponry (Mind Knight path) ─────────────────────────────────────
    // Per-weapon GUIDs are generated deterministically via MD5 in CallWeaponry.Configure().
    public const string CallWeaponrySelection = "49aa17bd-e8f4-4dea-9cb9-b903e1f869a0";
    public const string CallWeaponryVisualEnchantment = "c3a17f92-e841-4db5-a923-87d0f1b6c204";
    public const string MindKnightCallWeaponryProgression = "b2e4f1a3-9c07-4d58-8e6a-123f45d67890";
}
