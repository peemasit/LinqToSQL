using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinqToSQL {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        LinqToSqlDataClassesDataContext dataContext;
        
        public MainWindow() {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQL.Properties.Settings.PeemasitDBConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);
            //InsertUniversities();
            //InsertStudent();
            //InsertLecture();
            //InsertStudentLectureAssociations();
            //GetUniversityOfJohn();
            //GetLectureFromJohn();
            //GetAllStudentFromYale();
            //GetAllUniversitiesWithTransgenders();
            //GetAllLectureFromCambridge();
            //UpdateJohn();
            //DeleteJame();
        }

        public void InsertUniversities() {
            dataContext.ExecuteCommand("delete from University");
            University yale = new University();
            yale.Name = "Yale";
            University cambridge = new University();
            cambridge.Name = "Cambridge";
            dataContext.Universities.InsertOnSubmit(yale);
            dataContext.Universities.InsertOnSubmit(cambridge);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudents() {
            Student john = new Student();
            john.Name = "John";
            john.Gender = "Male";
            john.UniversityId = 4;
            dataContext.Students.InsertOnSubmit(john);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertStudent() {
            University yale = dataContext.Universities.First(un => un.Name.Equals("Yale"));
            University cambridge = dataContext.Universities.First(un => un.Name.Equals("Cambridge"));
            List<Student> students = new List<Student>();
            students.Add(new Student { Name = "Peem", Gender = "male", UniversityId = cambridge.Id });
            students.Add(new Student { Name = "Jame", Gender = "trans-gender", UniversityId = yale.Id });
            students.Add(new Student { Name = "Judith", Gender = "female", University = cambridge });
            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLecture() {
            List<Lecture> lectures = new List<Lecture>();
            lectures.Add(new Lecture { Name = "Math" });
            lectures.Add(new Lecture { Name = "C# Programming" });
            lectures.Add(new Lecture { Name = "Node.JS" });
            dataContext.Lectures.InsertAllOnSubmit(lectures);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations() {
            Student john = dataContext.Students.First(st => st.Name.Equals("John"));
            Student peem = dataContext.Students.First(st => st.Name.Equals("Peem"));
            Student jame = dataContext.Students.First(st => st.Name.Equals("Jame"));
            Student judith = dataContext.Students.First(st => st.Name.Equals("Judith"));
            Lecture math = dataContext.Lectures.First(lc => lc.Name.Equals("Math"));
            Lecture csharp = dataContext.Lectures.First(lc => lc.Name.Equals("C# Programming"));
            Lecture nodejs = dataContext.Lectures.First(lc => lc.Name.Equals("Node.JS"));
            List<StudentLecture> studentLectures = new List<StudentLecture>();
            studentLectures.Add(new StudentLecture { Student = john, Lecture = math });
            studentLectures.Add(new StudentLecture { Student = peem, Lecture = nodejs });
            studentLectures.Add(new StudentLecture { Student = jame, Lecture = csharp });
            studentLectures.Add(new StudentLecture { Student = judith, Lecture = nodejs });
            dataContext.StudentLectures.InsertAllOnSubmit(studentLectures);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.StudentLectures;
        }

        public void GetUniversityOfJohn() {
            Student John = dataContext.Students.First(st => st.Name.Equals("John"));
            University JohnUniversity = John.University;
            List<University> universities = new List<University>();
            universities.Add(JohnUniversity);
            MainDataGrid.ItemsSource = universities;
        }
        public void GetLectureFromJohn() {
            Student John = dataContext.Students.First(st => st.Name.Equals("John"));
            var johnLecture = from sl in John.StudentLectures select sl.Lecture;
            MainDataGrid.ItemsSource = johnLecture;
        }

        public void GetAllStudentFromYale() {
            var studentsFromYale = from st in dataContext.Students where st.University.Name == "Cambridge" select st;
            MainDataGrid.ItemsSource = studentsFromYale;
        }

        public void GetAllUniversitiesWithTransgenders() {
            var transgenderUniversities = from student in dataContext.Students
                                          join university in dataContext.Universities
                                          on student.University equals university
                                          where student.Gender == "trans-gender"
                                          select university;
            MainDataGrid.ItemsSource = transgenderUniversities;
        }

        public void GetAllLectureFromCambridge() {
            var lectureFromCambridge = from sl in dataContext.StudentLectures
                                     join student in dataContext.Students
                                     on sl.Student equals student
                                     where student.University.Name == "Cambridge"
                                     select sl.Lecture ;
            MainDataGrid.ItemsSource = lectureFromCambridge;
        }

        public void UpdateJohn() {
            Student john = dataContext.Students.FirstOrDefault(st => st.Name == "John");
            john.Name = "Peter";
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteJame() {
            Student jame = dataContext.Students.FirstOrDefault(st => st.Name.Equals("Jame"));
            dataContext.Students.DeleteOnSubmit(jame);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;

        }
    }
}
