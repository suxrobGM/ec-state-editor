using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Prism.Mvvm;
using Prism.Commands;
using EC_StateEditor.Model;


namespace EC_StateEditor.ViewModel
{
    class MainWindow_VM : BindableBase
    {
        private int progressPercentage;
        private string modPath;
        private string settingsXmlFileName = "Settings.xml";
        private SettingsXML settingsXML;

        public ObservableCollection<State> States { get; }
        public string ModPath
        {
            get
            {            
                return modPath;
            }
            set
            {
                modPath = value;
                settingsXML.ModPath = value;
                RaisePropertyChanged(nameof(ModPath));
            }
        }
        public int ProgressPercentage
        {
            get
            {
                return progressPercentage;
            }
            set
            {             
                progressPercentage = value;
                RaisePropertyChanged(nameof(ProgressPercentage));     
            }
        }
        public Version AppVersion { get => Assembly.GetExecutingAssembly().GetName().Version; }
        public DelegateCommand LoadCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand SetModPathCommand { get; }
        public DelegateCommand ReloadCommand { get; }
        public DelegateCommand WindowsLoadEventCommand { get; }

        public MainWindow_VM()
        {
            States = new ObservableCollection<State>();

            LoadCommand = new DelegateCommand(() =>
            {
                LoadData(modPath + @"\history\states");
            });

            SaveCommand = new DelegateCommand(() =>
            {
                SaveData(modPath + @"\history\states");
            });

            ReloadCommand = new DelegateCommand(() =>
            {
                if(States.Count != 0)
                {
                    States.Clear();
                    LoadData(modPath + @"\history\states");
                }
            });

            SetModPathCommand = new DelegateCommand(() =>
            {
                var dialog = new FolderBrowserDialog
                {
                    Description = "Please set mod folder",
                    SelectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive", "Hearts of Iron IV", "mod")
                };
                dialog.ShowDialog();
                ModPath = dialog.SelectedPath;              
            });

            WindowsLoadEventCommand = new DelegateCommand(() =>
            {              
                settingsXML = new SettingsXML(settingsXmlFileName);
                ModPath = settingsXML.ModPath;
            });
        }

        private void LoadData(string pathToStatesFolder)
        {
            var files = Directory.GetFiles(pathToStatesFolder, "*.txt", SearchOption.TopDirectoryOnly);
            int progressMaxValue = files.Length;
            int progressCount = 0;

            foreach (var file in files)
            {          
                var buffer = File.ReadAllLines(file);
                States.Add(new State
                {
                    FileName = State.ParseFileName(file),
                    Id = State.ParseId(buffer),
                    LocalizationToken = State.ParseLocalizationToken(buffer),
                    Manpower = State.ParseManpower(buffer),
                    Name = State.ParseName(file),
                    Owner = State.ParseOwner(buffer),
                    Religion = State.ParseReligion(buffer),
                    StateCategory = State.ParseStateCategory(buffer)
                });
                progressCount++;
                ProgressPercentage = GetPercentage(progressCount, progressMaxValue);              
            }
        }

        private void SaveData(string pathToStatesFolder)
        {
            Task.Run(()=> 
            {               
                int progressMaxValue = States.Count;
                int progressCount = 0;

                foreach (var state in States)
                {
                    state.SaveContent(pathToStatesFolder);
                    progressCount++;
                    ProgressPercentage = GetPercentage(progressCount, progressMaxValue);                    
                }
            });
        }

        private int GetPercentage(int value, int maxValue)
        {
            if (maxValue < 1)
                return 0;
            return value * 100 / maxValue;
        }
    }
}
