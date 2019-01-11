using BeatSaberDailyChallenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayRestrictionsPlus
{
    class ChallengeIntegration
    {
        //These are where the options get saved to be returned to what they were before the challenge
        public static bool failOnMiss = false;
        public static bool failOnBadCut = false;
        public static bool failOnBomb = false;
        public static bool failOnSaberClash = false;
        public static bool failOnImperfectCut = false;

        public static int imperfectCutThreshold = 100;
        public static void AddListeners()
        {
            ChallengeExternalModifiers.onChallengeFailedToLoad += ReturnOptions;
            ChallengeExternalModifiers.onChallengeEnd += ReturnOptions;
            ChallengeExternalModifiers.RegisterHandler("GameplayModifiersPlus", delegate (string[] modifiers)
           {
               Plugin.activateDuringIsolated = true;
               SaveOptions();
               SetToDefaultOptions();
               foreach(string arg in modifiers)
               {
                   if (arg.StartsWith("imperfectCutThreshold"))
                   {
                       Plugin.Config.imperfectCutThreshold = int.Parse(arg.Split(':')[1]);
                       continue;
                   }
                   switch (arg)
                   {
                       case "failOnMiss":
                           Plugin.Config.failOnMiss = true;
                           break;
                       case "failOnBadCut":
                           Plugin.Config.failOnBadCut = true;
                           break;
                       case "failOnBomb":
                           Plugin.Config.failOnBomb = true;
                           break;
                       case "failOnImperfectCut":
                           Plugin.Config.failOnImperfectCut = true;
                           break;
                       case "failOnSaberClash":
                           Plugin.Config.failOnSaberClash = true;
                           break;
                       default:
                           return false;
                   }
               }
               return true;
           });
        }
        private static void SaveOptions()
        {
            failOnMiss = Plugin.Config.failOnMiss;
            failOnBadCut = Plugin.Config.failOnBadCut;
            failOnBomb = Plugin.Config.failOnBomb;
            failOnImperfectCut = Plugin.Config.failOnImperfectCut;
            failOnSaberClash = Plugin.Config.failOnSaberClash;
            imperfectCutThreshold = Plugin.Config.imperfectCutThreshold;
      
        }
        private static void SetToDefaultOptions()
        {
            Plugin.Config.failOnMiss = false;
            Plugin.Config.failOnBadCut = false;
            Plugin.Config.failOnBomb = false;
            Plugin.Config.failOnImperfectCut = false;
            Plugin.Config.failOnSaberClash = false;
            Plugin.Config.imperfectCutThreshold = 100f;
        }
        private static void ReturnOptions()
        {
            Plugin.activateDuringIsolated = false;
            Plugin.Config.failOnMiss = failOnMiss;
            Plugin.Config.failOnBadCut = failOnBadCut;
            Plugin.Config.failOnBomb = failOnBomb;
            Plugin.Config.failOnImperfectCut = failOnImperfectCut;
            Plugin.Config.failOnSaberClash = failOnSaberClash;
            Plugin.Config.imperfectCutThreshold = imperfectCutThreshold;
        }
    }
}
