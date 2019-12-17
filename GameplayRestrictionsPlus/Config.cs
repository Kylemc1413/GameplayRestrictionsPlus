using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace GameplayRestrictionsPlus
{
    public static class Config
    {
        public static string FilePath { get; }

        public static bool restartOnFail = false;
        public static bool crashOnFail = false;

        public static bool failOnMiss = false;
        public static bool failOnBadCut = false;
        public static bool failOnBomb = false;
        public static bool failOnSaberClash = false;
        public static bool failOnImperfectCut = false;
        public static bool failOnMissWithinFrame = false;
        //Modes: Note Count, Note Percentage, Song Percentage
        public static string frameMode = "NotePercent";
        public static float frameValue = 15;
        internal static bool stricterAngles = false;
        public static int imperfectCutThreshold = 100;




        public static void Save()
        {
            Plugin.ModConfig.SetBool("Fail Modifiers", "Restart on Fail", restartOnFail);
            Plugin.ModConfig.SetBool("Restrictions", "Fail on Miss", failOnMiss);
            Plugin.ModConfig.SetBool("Restrictions", "Fail on Bad Cut", failOnBadCut);
            Plugin.ModConfig.SetBool("Restrictions", "Fail on Imperfect Cut", failOnImperfectCut);
            Plugin.ModConfig.SetInt("Restrictions", "Imperfect Cut Threshold", imperfectCutThreshold);
            Plugin.ModConfig.SetBool("Restrictions", "Fail on Miss Within Frame", failOnMissWithinFrame);
            Plugin.ModConfig.SetString("Restrictions", "Miss Within Frame Mode (NoteCount, NotePercent, SongPercent)", frameMode);
            Plugin.ModConfig.SetFloat("Restrictions", "Miss Within Frame Value (15% - 15, 15 Blocks - 15)", frameValue);
            Plugin.ModConfig.SetBool("Restrictions", "Strict Angles (Disables Submission)", stricterAngles);
        }

        public static void Load()
        {
            restartOnFail = Plugin.ModConfig.GetBool("Fail Modifiers", "Restart on Fail", false, true);
            failOnMiss = Plugin.ModConfig.GetBool("Restrictions", "Fail on Miss", false, true);
            failOnBadCut = Plugin.ModConfig.GetBool("Restrictions", "Fail on Bad Cut", false, true);
            failOnImperfectCut = Plugin.ModConfig.GetBool("Restrictions", "Fail on Imperfect Cut", false, true);
            imperfectCutThreshold = Plugin.ModConfig.GetInt("Restrictions", "Imperfect Cut Threshold", 100, true);
            failOnMissWithinFrame = Plugin.ModConfig.GetBool("Restrictions", "Fail on Miss Within Frame", false, true);
            frameMode = Plugin.ModConfig.GetString("Restrictions", "Miss Within Frame Mode (NoteCount, NotePercent, SongPercent)", "NotePercent", true);
            frameValue = Plugin.ModConfig.GetFloat("Restrictions", "Miss Within Frame Value (15% - 15, 15 Blocks - 15)", 15, true);
            stricterAngles = Plugin.ModConfig.GetBool("Restrictions", "Strict Angles (Disables Submission)", false, true);

        }

    }
}
