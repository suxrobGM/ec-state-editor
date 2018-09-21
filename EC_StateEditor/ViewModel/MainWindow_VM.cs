using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.IO;
using Prism.Mvvm;
using Prism.Commands;
using EC_StateEditor.Model;


namespace EC_StateEditor.ViewModel
{
    class MainWindow_VM : BindableBase
    {
        private int progressCount;
        private int progressMaxValue;
        private string modPath;

        public ObservableCollection<State> States { get; }
        public string ModPath { get => modPath; }
        public int ProgressPercentage { get => GetPercentage(progressCount, progressMaxValue); }
        public DelegateCommand LoadCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand SetModPathCommand { get; }

        public MainWindow_VM()
        {
            States = new ObservableCollection<State>();

            LoadCommand = new DelegateCommand(() =>
            {
                LoadData(modPath + @"\history\states");
            });

            SaveCommand = new DelegateCommand(() =>
            {

            });

            SetModPathCommand = new DelegateCommand(() =>
            {
                var dialog = new FolderBrowserDialog
                {
                    Description = "Please set mod folder",
                    SelectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive", "Hearts of Iron IV", "mod")
                };
                dialog.ShowDialog();
                modPath = dialog.SelectedPath;
                RaisePropertyChanged("ModPath");
            });
        }

        private void LoadData(string pathToStatesFolder)
        {
            var files = Directory.GetFiles(pathToStatesFolder, "*.txt", SearchOption.TopDirectoryOnly);
            progressMaxValue = files.Length;
            progressCount = 0;

            foreach (var file in files)
            {
                progressCount++;
                var buffer = File.ReadAllLines(file);
                States.Add(new State
                {
                    Id = State.ParseId(buffer),
                    Manpower = State.ParseManpower(buffer),
                    Name = State.ParseName(file),
                    Owner = State.ParseOwner(buffer),
                    Religion = State.ParseReligion(buffer),
                    StateCategory = State.ParseStateCategory(buffer)
                });
                RaisePropertyChanged(nameof(this.ProgressPercentage));
            }
        }

        private void SaveData(string pathToStatesFolder)
        {

        }

        private int GetPercentage(int value, int maxValue)
        {
            if (maxValue < 1)
                return 0;
            return value * 100 / maxValue;
        }
    }
}
