
using System;
using System.IO;
using System.Reflection;
using System.Xml;
using BepInEx;
using BrutalAPI;
using FiendishFools.Characters;
using FiendishFools.StatusEffects;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace FiendishFools
{

    [BepInPlugin("OverheatingNuclearReactor.", "FiendishFools", "1.2.1")]
    [BepInDependency("BrutalOrchestra.BrutalAPI", BepInDependency.DependencyFlags.HardDependency)]
    public class FiendishMain : BaseUnityPlugin
    {

        public void Awake()
        {
            ExtraUtils.SetUp();
            LoadStatusEffect();
            LoadCharacters();
        }

        public void LoadCharacters()
        {
            Samuel.Add();
            Fiender.Add();
            Thanatos.Add();
            Thithar.Add();
            Grove.Add();
            Mugleh.Add();
            Daymon.Add();
            Zelith.Add();
        }

        public void LoadStatusEffect()
        {
            DeathsTouchSE_SO.SetUp();
            EmpoweredSE_SO.SetUp();
        }

        public static AssetBundle assetBundle;

        public static int ThanatosInCombat;
    }
}
