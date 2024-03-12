using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StudentTestPicker.Models
{
    class AllStudents
    {
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
        public string classNumber;

        public AllStudents(string klasa)
        {
            classNumber = klasa;
            LoadStudents(klasa);

        }

        public string getClassNumber()
        {
            return classNumber;
        }

        public void LoadStudents(string klasa)
        {
            Students.Clear();

            string path = FileSystem.AppDataDirectory;
            List<Student> students = new List<Student>();

            if (File.Exists(Path.Combine(path, $"{klasa}.cls.txt")))
            {
                string text = File.ReadAllText(Path.Combine(path, $"{klasa}.cls.txt"));
                string[] studentsData = text.Split('\n');
                foreach (string s in studentsData)
                {
                    string[] studentData = s.Split("\t");
                    if (studentData.Length > 3)
                    {
                        students.Add(new Student()
                        {
                            Number = int.Parse(studentData[0]),
                            Name = studentData[1],
                            Surname = studentData[2],
                            ClassNumber = studentData[3]
                        });
                    }
                }

                foreach (Student student in students)
                {
                    Students.Add(student);
                }
            }
        }


        public Student DrawStudent(string classNumber)
        {
            List<Student> classStudents = new List<Student>();
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{classNumber}.cls.txt");
            foreach (Student student in Students)
            {
                if (student.ClassNumber == classNumber)
                {
                    classStudents.Add(student);
                }
            }
            if (classStudents.Count == 0)
            {
                return new Student();
            }

            Random rnd = new Random();
            int randomIndex = rnd.Next(classStudents.Count);
            return classStudents[randomIndex];
        }


        public int AddStudent(string klasa, string name, string surname)
        {
            string path = FileSystem.AppDataDirectory;
            Student student = new Student()
            {
                Number  = Students.Count + 1,
                Name = name,
                Surname = surname,
                ClassNumber=klasa

            };

            if (!File.Exists(Path.Combine(path, $"{klasa}.cls.txt")))
                return 1;
            string text = File.ReadAllText(Path.Combine(path, $"{klasa}.cls.txt"));
            string studentData = text + $"\n{student.Number}\t{student.Name}\t{student.Surname}\t{student.ClassNumber}";

            File.WriteAllText(Path.Combine(path, $"{klasa}.cls.txt"), studentData);
            Students.Add(student);

            return 0;
        }

        public int DeleteStudent(string klasa, string name, string surname)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{klasa}.cls.txt");
            string new_text = "";
            string text = File.ReadAllText(path);
            string[] studentData = text.Split("\n");
            int counter = 1;
            for (int i = 0; i < studentData.Length; i++)
            {
                Student s;
                string[] stD = studentData[i].Split("\t");
                if (stD.Length == 1)
                {
                    new_text += $"{stD[0]}";
                }
                else if (stD.Length > 2 && (stD[1] != name || stD[2] != surname))
                {
                    new_text += $"\n{counter}\t{stD[1]}\t{stD[2]}\t{stD[3]}";
                    counter++;
                }
            }

            File.WriteAllText(path, new_text);

            return 1;
        }
    }
}