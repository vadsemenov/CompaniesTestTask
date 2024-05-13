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
using School.DataAccess.Repositories;
using School.DataAccess.UnitOfWork;

namespace School.MainProgram.ViewModel;

public class SchoolViewModel : INotifyPropertyChanged
{
    private ObservableCollection<AverageAssessment>? _assessments = new();

    public ObservableCollection<AverageAssessment> Assessments
    {
        get
        {
            if (CurrentSubject != null)
            {
                _assessments =
                    new ObservableCollection<AverageAssessment>(
                        _assessmentRepository.GetAverageAssessments(CurrentSubject));
            }

            return _assessments!;
        }
    }

    public double AverageMark => !_assessments!.Any() ? 0 : _assessments!.Average(a => a.SubjectMark);

    private Subject? _currentSubject;

    public Subject? CurrentSubject
    {
        get => _currentSubject;
        set
        {
            _currentSubject = value;

            _assessments =
                new ObservableCollection<AverageAssessment>(_assessmentRepository.GetAverageAssessments(value));

            OnPropertyChanged(nameof(AverageMark));
            OnPropertyChanged(nameof(Assessments));
            OnPropertyChanged(nameof(CurrentSubject));
        }
    }

    private ObservableCollection<Subject> _subjects = new();

    public ObservableCollection<Subject> Subjects
    {
        get
        {
            _subjects = new ObservableCollection<Subject>(_subjectRepository.GetAll().ToList());

            return _subjects;
        }
    }

    private ObservableCollection<Student> _students = new();

    private Student? _student;

    public Student? CurrentStudent
    {
        get => _student;
        set
        {
            _student = value;
            OnPropertyChanged(nameof(CurrentStudent));
        }
    }

    public ObservableCollection<Student> Students
    {
        get
        {
            _students = new ObservableCollection<Student>(_studentRepository.GetAll().ToList());

            return _students;
        }
    }

    public string StudentFirstName { get; set; } = string.Empty;

    public string StudentLastName { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public ushort Mark { get; set; }

    private readonly IServiceProvider _services;

    private readonly IAssessmentRepository _assessmentRepository;

    private readonly IStudentRepository _studentRepository;

    private readonly ISubjectRepository _subjectRepository;

    private readonly IUnitOfWork _unitOfWork;

    public SchoolViewModel(IServiceProvider serviceProvider,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        _assessmentRepository = unitOfWork.GetRepository<IAssessmentRepository>();

        _studentRepository = unitOfWork.GetRepository<IStudentRepository>();

        _subjectRepository = unitOfWork.GetRepository<ISubjectRepository>();

        // Это конечно антипаттерн Service Locator, но иногда его используют
        _services = serviceProvider;

        AddFakeObjects();
    }

    public void AddFakeObjects()
    {
        var student1 = new Student { FirstName = "Иван", LastName = "Иванов" };

        var student2 = new Student { FirstName = "Петр", LastName = "Петров" };

        _studentRepository.Create(new Collection<Student>() { student1, student2 });
        _unitOfWork.Save();

        var subject1 = new Subject { Name = "Информатика" };

        var subject2 = new Subject { Name = "Физика" };

        _subjectRepository.Create(new Collection<Subject>() { subject1, subject2 });
        _unitOfWork.Save();

        var assessment1 = new Assessment { StudentId = student1.Id, SubjectId = subject1.Id, Mark = 3 };
        var assessment2 = new Assessment { StudentId = student2.Id, SubjectId = subject1.Id, Mark = 4 };
        var assessment3 = new Assessment { StudentId = student2.Id, SubjectId = subject2.Id, Mark = 5 };

        _assessmentRepository.Create(new Collection<Assessment> { assessment1, assessment2, assessment3 });

        _unitOfWork.Save();
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

                {
                    var student = new Student { FirstName = StudentFirstName, LastName = StudentLastName };

                    _studentRepository.Create(student);

                    _students.Add(student);

                    _unitOfWork.Save();

                    window?.Close();

                    OnPropertyChanged(nameof(Students));
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

                    _subjectRepository.Create(subject);

                    _subjects.Add(subject);

                    _unitOfWork.Save();

                    window?.Close();

                    OnPropertyChanged(nameof(Subjects));
                    OnPropertyChanged(nameof(AverageMark));
                    OnPropertyChanged(nameof(CurrentSubject));
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
                        SubjectId = CurrentSubject.Id,
                        Mark = Mark
                    };

                    _assessmentRepository.Create(assessment);

                    _unitOfWork.Save();

                    Mark = 0;

                    window?.Close();

                    OnPropertyChanged(nameof(AverageMark));
                    OnPropertyChanged(nameof(Assessments));
                    OnPropertyChanged(nameof(CurrentSubject));
                }
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
        window.ShowDialog();
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