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
            ChallengeExternalModifiers.RegisterHandler("GameplayRestrictionsPlus", delegate (string[] modifiers)
           {
               Plugin.activateDuringIsolated = true;
               SaveOptions();
               SetToDefaultOptions();
               foreach(string arg in modifiers)
               {
                   if (arg.StartsWith("imperfectCutThreshold"))
                   {
                       Config.imperfectCutThreshold = int.TryParse(arg.Split(':')[1], out int value) ? value : Config.imperfectCutThreshold;
                       continue;
                   }
                   switch (arg)
                   {
                       case "failOnMiss":
                           Config.failOnMiss = true;
                           break;
                       case "failOnBadCut":
                           Config.failOnBadCut = true;
                           break;
                       case "failOnBomb":
                           Config.failOnBomb = true;
                           break;
                       case "failOnImperfectCut":
                           Config.failOnImperfectCut = true;
                           break;
                       case "failOnSaberClash":
                           Config.failOnSaberClash = true;
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
            failOnMiss = Config.failOnMiss;
            failOnBadCut = Config.failOnBadCut;
            failOnBomb = Config.failOnBomb;
            failOnImperfectCut = Config.failOnImperfectCut;
            failOnSaberClash = Config.failOnSaberClash;
            imperfectCutThreshold = Config.imperfectCutThreshold;
      
        }
        private static void SetToDefaultOptions()
        {
            Config.failOnMiss = false;
            Config.failOnBadCut = false;
            Config.failOnBomb = false;
            Config.failOnImperfectCut = false;
            Config.failOnSaberClash = false;
            Config.imperfectCutThreshold = 100;
        }
        private static void ReturnOptions()
        {
            Plugin.activateDuringIsolated = false;
            Config.failOnMiss = failOnMiss;
            Config.failOnBadCut = failOnBadCut;
            Config.failOnBomb = failOnBomb;
            Config.failOnImperfectCut = failOnImperfectCut;
            Config.failOnSaberClash = failOnSaberClash;
            Config.imperfectCutThreshold = imperfectCutThreshold;
        }
    }
}
