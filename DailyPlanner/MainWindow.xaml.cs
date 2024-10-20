using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace DailyPlanner
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Task> Tasks { get; set; }
        private Task selectedTask;

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<Task>();
            TaskList.ItemsSource = Tasks;

            Tasks.CollectionChanged += Tasks_CollectionChanged;
            UpdateEmptyMessageVisibility();
        }

        private void UpdateEmptyMessageVisibility()
        {
            if (Tasks.Count == 0)
            {
                EmptyMessage.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyMessage.Visibility = Visibility.Collapsed;
            }
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateEmptyMessageVisibility();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TaskInput.Text) && TaskDate.SelectedDate.HasValue)
            {
                Tasks.Add(new Task { Description = TaskInput.Text, Date = TaskDate.SelectedDate.Value });
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните задачу и выберите дату.");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var taskToRemove = button.Tag as Task;
            Tasks.Remove(taskToRemove);
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            selectedTask = button.Tag as Task;

            if (selectedTask != null)
            {
                TaskInput.Text = selectedTask.Description;
                TaskDate.SelectedDate = selectedTask.Date;

                AddButton.Visibility = Visibility.Collapsed;
                UpdateButton.Visibility = Visibility.Visible;
            }
        }

        
        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask != null && !string.IsNullOrEmpty(TaskInput.Text) && TaskDate.SelectedDate.HasValue)
            {
                selectedTask.Description = TaskInput.Text;
                selectedTask.Date = TaskDate.SelectedDate.Value;

                TaskList.Items.Refresh();

                ClearInputFields();
                AddButton.Visibility = Visibility.Visible;
                UpdateButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните задачу и выберите дату.");
            }
        }

        
        private void ClearInputFields()
        {
            TaskInput.Clear();
            TaskDate.SelectedDate = null;
        }
    }

    public class Task
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
