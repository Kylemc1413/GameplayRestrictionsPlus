using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace GameplayRestrictionsPlus
{
    public class Config
    {
        public string FilePath { get; }

        public bool restartOnFail = false;
        public bool crashOnFail = false;

        public bool failOnMiss = false;
        public bool failOnBadCut = false;
        public bool failOnBomb = false;
        public bool failOnSaberClash = false;
        public bool failOnImperfectCut = false;
       
        public int imperfectCutThreshold = 100;

        private readonly FileSystemWatcher _configWatcher;
        public event Action<Config> ConfigChangedEvent;
        private bool _saving;


        public Config(String filePath)
        {
            FilePath = filePath;

            if (File.Exists(FilePath))
            {
                Load();
            }
            else
            {
                Save();
            }

            if (restartOnFail && crashOnFail)
                crashOnFail = false;

            _configWatcher = new FileSystemWatcher($"{Environment.CurrentDirectory}\\UserData")
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "GameplayModifiersPlusChatSettings.ini",
                EnableRaisingEvents = true
            };
            _configWatcher.Changed += _configWatcher_Changed;

        }

        private void _configWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (_saving)
            {
                _saving = false;
                return;
            }

            Load();

            ConfigChangedEvent?.Invoke(this);
        }

        public void Save()
        {
            _saving = true;
            if (restartOnFail && crashOnFail)
                crashOnFail = false;

            ConfigSerializer.SaveConfig(this, FilePath);
        }

        public void Load()
        {
            ConfigSerializer.LoadConfig(this, FilePath);
        }

    }
}
