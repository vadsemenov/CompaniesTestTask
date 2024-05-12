using System;
using School.MainProgram.View;
using System.Windows;
using School.DataAccess.Model;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace School.MainProgram.ViewModel;

public class SchoolViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Subject> _subjects = new();

    public Subject? CurrentSubject { get; set; }

    public ObservableCollection<Subject> Subjects
    {
        get
        {
            _subjects.Clear();

            _subjects = new ObservableCollection<Subject>(_dbContext!.Subjects.ToList());

            return _subjects;
        }
    }

    private ObservableCollection<Student> _students = new();

    public Student? CurrentStudent { get; set; }

    public ObservableCollection<Student> Students
    {
        get
        {
            _students.Clear();

            _students = new ObservableCollection<Student>(_dbContext!.Students.ToList());

            // OnPropertyChanged(nameof(Students));

            return _students;
        }
    }

    public string StudentFirstName { get; set; } = "asdfasdf";

    public string StudentLastName { get; set; } = "asdfasdf";

    public string SubjectName { get; set; } = string.Empty;

    public ushort Mark { get; set; }

    private readonly IServiceProvider _services;

    private readonly SchoolDbContext _dbContext;

    public SchoolViewModel(IServiceProvider serviceProvider,
        SchoolDbContext dbContext)
    {
        _services = serviceProvider;

        _dbContext = dbContext;

        AddFakeSubjects(_dbContext);
    }

    public void AddFakeSubjects(SchoolDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Students.AddRange(new Student { FirstName = "Иван", LastName = "Иванов" }, new Student { FirstName = "Иван", LastName = "Иванов" });

        context.Subjects.AddRange(new Subject { Name = "sdfsdf" }, new Subject { Name = "sdfsdf" });

        var assdfsdf = context.Assessments.ToList();

        context.SaveChanges();
    }

    public RelayCommand AddNewStudent
    {
        get
        {
            return new RelayCommand(obj =>
            {
                var window = obj as Window;

                if (string.IsNullOrEmpty(StudentFirstName) || string.IsNullOrEmpty(StudentLastName))
                {
                    MessageBox.Show("Должны быть введены имя и фамилия", "Ошибка!");
                }
                else
                {
                    var student = new Student { FirstName = StudentFirstName, LastName = StudentLastName };

                    _dbContext!.Students.Add(student);

                    _students.Add(student);

                    _dbContext.SaveChanges();

                    window?.Close();

                    // OnPropertyChanged();
                    // OnPropertyChanged(nameof(Students));
                }
            });
        }
    }

    public RelayCommand AddNewSubject
    {
        get
        {
            return new RelayCommand(obj =>
            {
                var window = obj as Window;

                if (string.IsNullOrEmpty(SubjectName))
                {
                    MessageBox.Show("Должны быть введены имя и фамилия", "Ошибка!");
                }
                else
                {
                    var subject = new Subject { Name = SubjectName };

                    _dbContext!.Subjects.Add(subject);

                    _subjects.Add(subject);

                    _dbContext.SaveChanges();

                    window?.Close();

                    // OnPropertyChanged();
                    // OnPropertyChanged(nameof(Students));
                }
            });
        }
    }

    public RelayCommand AddNewAssessment
    {
        get
        {
            return new RelayCommand(obj =>
            {
                var window = obj as Window;

                if (CurrentStudent == null || CurrentSubject == null)
                {
                    MessageBox.Show("Должен быть выбран студент и дисциплина", "Ошибка!");
                }
                else if (Mark == 0 || Mark > 5)
                {
                    MessageBox.Show("Оценка может быть от 1 до 5", "Ошибка!");
                }
                else
                {
                    var assessment = new Assessment
                    {
                        StudentId = CurrentStudent.Id,
                        // Student = CurrentStudent,
                        SubjectId = CurrentSubject.Id,
                        // Subject = CurrentSubject,
                        Mark = Mark
                    };

                    _dbContext!.Assessments.Add(assessment);

                    _dbContext.SaveChanges();

                    CurrentStudent = null;
                    CurrentSubject = null;

                    Mark = 0;

                    window?.Close();
                }

                // if (!string.IsNullOrEmpty(SubjectName))
                // {
                //     var subject = new Subject { Name = SubjectName };
                //
                //     _dbContext!.Subjects.Add(subject);
                //
                //     _subjects.Add(subject);
                //
                //     window?.Close();
                //
                //     // OnPropertyChanged();
                //     // OnPropertyChanged(nameof(Students));
                // }
            });
        }
    }

    public RelayCommand OpenAddNewStudentWindow
    {
        get
        {
            return new RelayCommand(_ =>
            {
                var window = _services.GetRequiredService<AddStudentWindow>();

                SetCenterPositionAndOpen(window);
            });
        }
    }

    public RelayCommand OpenAddNewSubjectWindow
    {
        get
        {
            return new RelayCommand(_ =>
            {
                var window = _services.GetRequiredService<AddSubjectWindow>();

                SetCenterPositionAndOpen(window);
            });
        }
    }

    public RelayCommand OpenAddNewAssessmentWindow
    {
        get
        {
            return new RelayCommand(_ =>
            {
                var window = _services.GetRequiredService<AddAssessmentWindow>();

                SetCenterPositionAndOpen(window);
            });
        }
    }

    private void SetCenterPositionAndOpen(Window window)
    {
        window.Owner = Application.Current.MainWindow;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        window.Show();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}