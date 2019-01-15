using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomUI.GameplaySettings;
namespace GameplayRestrictionsPlus
{
    class UI
    {
        public static void CreateUI()
        {
            var restrictions1Menu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersLeft, "Gameplay Restrictions", "MainMenu", "restrictions1", "Set various restrictions for gameplay");

            var restartFailOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Restart On Fail", "restrictions1", "Restart the song on failing.");
            restartFailOption.GetValue = Plugin.Config.restartOnFail;
            restartFailOption.OnToggle += (value) => { Plugin.Config.restartOnFail = value; Plugin.Config.Save(); };
            restartFailOption.AddConflict("Crash On Fail");

            var crashFailOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Crash On Fail", "restrictions1", "Crash the game on failing.");
            crashFailOption.GetValue = Plugin.Config.crashOnFail;
            crashFailOption.OnToggle += (value) => { Plugin.Config.crashOnFail = value; Plugin.Config.Save(); };

            var restrictions2Menu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersLeft, "Restrictions", "restrictions1", "restrictions2", "Set restrictions here");

            var failOnSaberClashOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Fail On Saber Clash", "restrictions2", "Better not Cross those sabers.");
            failOnSaberClashOption.GetValue = Plugin.Config.failOnSaberClash;
            failOnSaberClashOption.OnToggle += (value) => { Plugin.Config.failOnSaberClash = value; Plugin.Config.Save(); };

            var failOnMissOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Fail On Miss", "restrictions2", "Fail the Song on missing a note.");
            failOnMissOption.GetValue = Plugin.Config.failOnMiss;
            failOnMissOption.OnToggle += (value) => { Plugin.Config.failOnMiss = value; Plugin.Config.Save(); };

            var failOnBadCutOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Fail On Bad Cut", "restrictions2", "Fail the Song on cutting a note the wrong way.");
            failOnBadCutOption.GetValue = Plugin.Config.failOnBadCut;
            failOnBadCutOption.OnToggle += (value) => { Plugin.Config.failOnBadCut = value; Plugin.Config.Save(); };

            var failOnBombOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Fail On Bomb", "restrictions2", "Fail the Song on hitting a bomb.");
            failOnBombOption.GetValue = Plugin.Config.failOnBomb;
            failOnBombOption.OnToggle += (value) => { Plugin.Config.failOnBomb = value; Plugin.Config.Save(); };

            var failOnImperfectOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Fail On Imperfect Cut", "restrictions2", "Fail the Song on getting a cut below a threshold, can be changed manually in the config, default is 100.");
            failOnImperfectOption.GetValue = Plugin.Config.failOnImperfectCut;
            failOnImperfectOption.OnToggle += (value) => { Plugin.Config.failOnImperfectCut = value; Plugin.Config.Save(); };
            
            var stricterAnglesOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Stricter Angles", "restrictions2", "Unfinished strict angles modifier from the game, might not even do anything. \r\n <b><#FF0000>Disables Score Submission</color></b>");
            stricterAnglesOption.GetValue = Config.stricterAngles;
            stricterAnglesOption.OnToggle += (value) => { Config.stricterAngles = value; Plugin.Config.Save(); };
            
        }
    }
}
