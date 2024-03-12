using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace StudentTestPicker.Models
{
    class AllClasses
    {
        public ObservableCollection<Klasa> Classes { get; set; } = new ObservableCollection<Klasa>();

        public AllClasses()
        {
            LoadClasses();
        }

        public void LoadClasses()
        {
            Classes.Clear();

            string appDataPath = FileSystem.AppDataDirectory;

            IEnumerable<Klasa> classes = Directory
                .EnumerateFiles(appDataPath, "*.cls.txt")
                .Select(filename => new Klasa()
                {
                    Filename = filename,
                    ClassNumber = File.ReadAllText(filename).Split('\n')[0],
                    Text = File.ReadAllText(filename)
                });

            foreach (Klasa klasa in classes)
            {
                Classes.Add(klasa);
            }
        }

        public int AddClass(string name)
        {
            string appDataPath = FileSystem.AppDataDirectory;
            string className = $"{name}.cls.txt";

            if (File.Exists(Path.Combine(appDataPath, className)))
            {
                return 1;
            }

            if (name.Split("\t").Length > 1)
                return 2;
            File.WriteAllText(Path.Combine(appDataPath, className), $"{name}");

            return 0;
        }

        public int DeleteClass(string klasa)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{klasa}.cls.txt");
            if (!File.Exists(path)) return 1;

            File.Delete(path);

            return 0;
        }
    }
}
