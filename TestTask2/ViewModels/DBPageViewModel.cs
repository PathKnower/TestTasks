using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Newtonsoft.Json;
using NLog;

namespace TestTask2.ViewModels
{
    public class DBPageViewModel : INotifyPropertyChanged
    {
        public ApplicationContext db = new ApplicationContext();
        public Logger logger = LogManager.GetCurrentClassLogger();

        public ObservableCollection<FileModel> FilesTable
        {
            get
            {
                logger.Trace("FilesTable: Check database");
                if (db == null)
                    db = new ApplicationContext();

                logger.Trace("FilesTable: Load data from database");
                db.Files.Load();
                return db.Files.Local;
            }
        }

        #region Commands 

        public RelayCommand Back
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    logger.Debug("Back command started!");
                    DBPageViewModel viewModel = obj as DBPageViewModel;

                    logger.Trace("Get viewmodel and try to get navigation service");
                    NavigationService navigationService = NavigationService.GetNavigationService(viewModel.Page);

                    logger.Trace("Check \'CanGoBack\'");
                    if (navigationService.CanGoBack)
                    {
                        navigationService.GoBack();
                        logger.Debug("Back command successfully finished!");
                    }
                    else
                    {
                        logger.Error("Cant go back, restart application.");
                        MessageBox.Show("Unexpected error. Application will restart");
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                });
            }
        }

        public RelayCommand Save
        {
            get
            {
                return new RelayCommand(async obj =>
                {
                    logger.Debug("Save command started!");
                    DBPageViewModel viewModel = obj as DBPageViewModel;

                    logger.Trace("Get viewmodel");
                    await viewModel.db.SaveChangesAsync();

                    logger.Info("Database successfully updated");
                    MessageBox.Show("Database successfully update");
                    logger.Debug("Save command successfully finished");
                });
            }
        }


        #endregion


        public DependencyObject Page { get; set; }

        public DBPageViewModel(DependencyObject page)
        {
            Page = page;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
