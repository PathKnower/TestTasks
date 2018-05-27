using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows;
using System.Xml;
using System.Windows.Navigation;
using TestTask2.Pages;
using Newtonsoft.Json;
using NLog;

namespace TestTask2.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ApplicationContext db = new ApplicationContext();
        public Logger logger = LogManager.GetCurrentClassLogger();

        string selectedFileLabel = "No one file selected", filePath;
        bool uploadButtonEnable = false;
        Visibility infoPanelVisibility = Visibility.Collapsed;

        FileModel model;

        #region functions

        /// <summary>
        /// First way to parse
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<bool> NameFormatCheck(string name)
        {
            try
            {
                logger.Debug("NameFormatCheck action started with name = " + name);
                string filename = (name.Split(new string[] { ".xml" }, StringSplitOptions.None)).First();

                string[] nameParts = filename.Split(new char[] { '_' }, StringSplitOptions.None);
                logger.Trace($"Split filename for {name.Length} parts");

                if (nameParts.Length < 3)
                {
                    logger.Debug("NameFormatCheck action exit. Exit condition: File name must contains \'_\' char 2 or more times");
                    MessageBox.Show("File name must contains \'_\' char 2 or more times", "File name validation message", MessageBoxButton.OK);
                    return false;
                }

                int index = 0;
                if (!filename.StartsWith("_"))
                {
                    logger.Trace("Check for russian letters");
                    if (nameParts[0].Length > 100)
                    {
                        logger.Debug("NameFormatCheck action exit. Exit condition: First part of file name must be less than 100 letters");
                        MessageBox.Show("First part of file name must be less than 100 letters", "File name validation message", MessageBoxButton.OK);
                        return false;
                    }

                    foreach (char letter in nameParts[0])
                    {
                        int code = (int)letter;

                        if (code < 1040 || code > 1103) // 1040 to 1103
                        {
                            logger.Debug("NameFormatCheck action exit. Exit condition: One of letters is not russian letter");
                            MessageBox.Show("One of letters is not russian letter", "File name validation message", MessageBoxButton.OK);
                            return false; //not russian letter
                        }
                    }

                    index++;
                }

                logger.Trace("Check for numeric chars");
                if (nameParts[index].Length == 1 || nameParts[index].Length == 10 ||
                    (nameParts[index].Length >= 14 && nameParts[index].Length <= 20))
                {
                    foreach (char number in nameParts[index])
                    {
                        if (int.TryParse(number.ToString(), out int x))
                        {
                            if (x < 0)
                            {
                                logger.Debug("NameFormatCheck action exit. Exit condition: Number cannot less than 0");
                                MessageBox.Show("Number cannot less than 0", "File name validation message", MessageBoxButton.OK);
                                return false;
                            }
                        }
                        else
                        {
                            logger.Debug("NameFormatCheck action exit. Exit condition: Failed to parse one of char in second part");
                            MessageBox.Show("Failed to parse one of char in second part", "File name validation message", MessageBoxButton.OK);
                            return false;
                        }
                    }
                }
                else
                {
                    logger.Debug("NameFormatCheck action exit. Exit condition: Second part contains wrong amount of numbers");
                    MessageBox.Show("Second part contains wrong amount of numbers", "File name validation message", MessageBoxButton.OK);
                    return false;
                }

                string filter = $"{nameParts[0]}_{nameParts[1]}_";
                string lastPart = (filename.Split(new string[] { filter, ".xml" }, StringSplitOptions.RemoveEmptyEntries)).First();

                if (lastPart.Length > 7)
                {
                    logger.Debug("NameFormatCheck action exit. Exit condition: Last part contains more chars than allowed");
                    MessageBox.Show("Last part contains more chars than allowed", "File name validation message", MessageBoxButton.OK);
                    return false;
                }

                logger.Debug("NameFormatCheck action end successfully!");
                return true;
            }
            catch (Exception e)
            {
                logger.Debug(e, "Something goes wrong while method checking filename");
                return false;
            }
        }

        /// <summary>
        /// Simple way
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<bool> SimpleNameFormatCheck(string name)
        {
            try
            {
                string filename = (name.Split(new string[] { ".xml" }, StringSplitOptions.None)).First();

                int firstCount = 0, secondCount = 0, thirdCount = -1;

                bool firstpart = false, secondpart = false, thirdpart = false;

                if (filename.StartsWith("_"))
                    firstpart = true;

                for (int i = 0; i < filename.Length; i++)
                {
                    if (!firstpart)
                    {
                        if (filename[i] != '_' && ++firstCount <= 100)
                        {
                            int code = (int)filename[i];

                            if (code < 1040 || code > 1103) // 1040 to 1103
                            {
                                MessageBox.Show("One of letters is not russian", "File name validation message", MessageBoxButton.OK);
                                return false; //not russian letter
                            }
                            continue;
                        }

                        if (firstCount > 100)
                        {
                            MessageBox.Show("First part of file name must be less than 100 letters", "File name validation message", MessageBoxButton.OK);
                            return false;
                        }


                        if (filename[i] == '_')
                            firstpart = true;
                    }

                    if (!secondpart)
                    {
                        if (filename[i] != '_')
                        {
                            secondCount++;

                            if (int.TryParse(filename[i].ToString(), out int x))
                            {
                                if (x < 0)
                                {
                                    MessageBox.Show("Number cannot less than 0", "File name validation message", MessageBoxButton.OK);
                                    return false;
                                }
                                continue;
                            }
                            else
                            {
                                MessageBox.Show("Failed to parse one of char in second part", "File name validation message", MessageBoxButton.OK);
                                return false;
                            }
                        }

                        if (filename[i] == '_')
                        {
                            if (secondCount == 1 || secondCount == 10 ||
                                    (secondCount >= 14 && secondCount <= 20))
                                secondpart = true;
                            else
                            {
                                MessageBox.Show("Second part contains wrong amount of numbers", "File name validation message", MessageBoxButton.OK);
                                return false;
                            }
                        }
                    }

                    if (!thirdpart)
                        thirdCount++;
                }

                if (thirdCount > 7)
                {
                    MessageBox.Show("Last part contains more chars than allowed", "File name validation message", MessageBoxButton.OK);
                    return false;
                }
                else if (thirdCount >= 0)
                    thirdpart = true;


                if (firstpart && secondpart && thirdpart)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<bool> XmlFormatCheck(Stream reader)
        {
            try
            {
                logger.Debug("XmlFormatCheck action started!");
                logger.Trace("Load xml document");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(reader);

                logger.Trace("Get the root of xml document");
                XmlElement xRoot = xmlDocument.DocumentElement;

                logger.Trace("Check root name");
                if (xRoot.Name != "File")
                {
                    logger.Debug($"XmlFormatCheck action exit. Exit condition: Name = {xRoot.Name} of root element is not allowed");
                    reader.Close();
                    return false;
                }

                logger.Trace("Check root attribute");
                if(xRoot.Attributes.Count != 1 || xRoot.Attributes.Item(0).Name != "FileVersion")
                {
                    logger.Debug($"XmlFormatCheck action exit. Exit condition: FileVersion attribute not found or count of root attributes is not allowed");
                    reader.Close();
                    return false;
                }

                logger.Trace("Create new file model");
                model = new FileModel
                {
                    FileVersion = xRoot.Attributes.Item(0).Value
                };

                if(xRoot.ChildNodes.Count != 2)
                {
                    logger.Debug("XmlFormatCheck action exit. Exit condition: Child nodes of xml file must equal 2");
                    reader.Close();
                    return false;
                }

                if(xRoot.FirstChild.Name != "Name")
                {
                    logger.Debug("XmlFormatCheck action exit. Exit condition: First child node must has name = \'Name\'");
                    reader.Close();
                    return false;
                }
                model.Name = xRoot.FirstChild.InnerText;
                logger.Trace("Set model name = " + model.Name);

                if (xRoot.LastChild.Name != "DateTime")
                {
                    logger.Debug("XmlFormatCheck action exit. Exit condition: Last child node must has name = \'DateTime\'");
                    reader.Close();
                    return false;
                }
                model.ModifyDate = DateTime.Parse(xRoot.LastChild.InnerText);
                logger.Trace("Set model ModifyDate = " + model.ModifyDate.ToString());

                logger.Debug("XmlFormatCheck action finish successfully");
                reader.Close();
                return true;
            }
            catch(FormatException e)
            {
                logger.Debug(e, "XmlFormatCheck action exit. Exit condition: error while DateTime parse");
                reader.Close();
                return false;
            }
            catch (Exception e)
            {
                logger.Debug(e, "XmlFormatCheck action exit. Exit condition: Error was catched!");
                reader.Close();
                return false;
            }
        }

        private async Task<bool> ExtractDB(string name)
        {
            try
            {
                logger.Debug("ExtractDB action started! Path to extract = " + name);
                logger.Trace("Open file stream");
                using (StreamWriter sw = new StreamWriter(name))
                {
                    logger.Trace("Serialize for human-readable json and write to file");
                    await sw.WriteLineAsync(JsonConvert.SerializeObject(db.Files.ToArray(), Newtonsoft.Json.Formatting.Indented));
                }

                logger.Debug("ExtractDB action successfully finish");
                return true;
            }
            catch(Exception e)
            {
                logger.Error(e, "ExtractDB action catched error");
                return false;
            }
        }

        public void UpdateModelLabel()
        {
            logger.Trace("Update 3 model properties on view");
            OnPropertyChanged("ModelName");
            OnPropertyChanged("ModelVersion");
            OnPropertyChanged("ModelDate");
        }

        #endregion

        #region Commands 

        public RelayCommand SelectFile
        {
            get
            {
                return new RelayCommand(async obj =>
                {
                    logger.Debug("SelectFile command started!");
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        Multiselect = false,
                        DefaultExt = "Xml files (*.xml) | *.xml",
                        Filter = "Xml files (*.xml) | *.xml"
                    };

                    logger.Trace("Show file select dialog window");
                    if (fileDialog.ShowDialog() != true)
                        return;

                    logger.Debug("New file selected. Start testing...");
                    bool namePass, xmlPass;
                    namePass = await NameFormatCheck(fileDialog.SafeFileName);
                    xmlPass = await XmlFormatCheck(fileDialog.OpenFile());

                    if (namePass && xmlPass)
                    {
                        logger.Debug("New file pass all tests.");
                        UploadButtonEnable = true;
                        logger.Trace("Turn on visibility of info panel");
                        InfoPanelVisibility = Visibility.Visible;
                        UpdateModelLabel();
                    }
                    else
                    {
                        if (!namePass)
                            logger.Debug($"New file with name \"{fileDialog.FileName.Split('\\').Last()}\" do not pass the NAME test");
                        if (!xmlPass)
                            logger.Debug($"New file with name \"{fileDialog.FileName.Split('\\').Last()}\" do not pass the XML test");

                        UploadButtonEnable = false;
                        logger.Trace("Turn off visibility of info panel");
                        InfoPanelVisibility = Visibility.Collapsed;
                        UpdateModelLabel();
                    }
                    logger.Debug("SelectFile command ends");
                });
            }
        }

        public RelayCommand UploadFile
        {
            get
            {
                return new RelayCommand(async obj =>
                {
                    logger.Debug("Upload file command started!");
                    try
                    {
                        logger.Debug("Check DB");
                        if (db == null)
                            db = new ApplicationContext();
                    }
                    catch (Exception e)
                    {
                        logger.Fatal(e, "Cannot connect or create DB");
                        MessageBox.Show("Unable connect to database");
                        return;
                    }

                    if(model == null)
                    {
                        logger.Error("Uploadable file cannot be null");
                        MessageBox.Show("Uploadable file cannot be null");
                        return;
                    }

                    if (db.Files.Any(x => x.Name == model.Name))
                    {
                        logger.Trace($"Database already know file with name {model.Name}");
                        MessageBox.Show("Database already know that file");
                        return;
                    }

                    logger.Debug("Try to upload new file to DB");
                    db.Files.Add(model);
                    await db.SaveChangesAsync();
                    logger.Info($"New file successfully uploaded with Name = {model.Name} and FileVersion = {model.FileVersion}");

                    MessageBox.Show("Successfull upload model to database");

                    logger.Trace("Refresh UI after file upload");
                    model = null;
                    UploadButtonEnable = false;
                    InfoPanelVisibility = Visibility.Collapsed;
                    UpdateModelLabel();
                    logger.Debug("Upload file command finish successfully");
                });
            }
        }

        public RelayCommand ShowDB
        {
            get
            {
                return new RelayCommand(obj => 
                {
                    logger.Debug("ShowDB command started!");
                    MainPageViewModel viewModel = obj as MainPageViewModel;

                    logger.Trace("Get view model from UI");
                    logger.Trace("Try to get navigation service from page");
                    NavigationService navigationService = NavigationService.GetNavigationService(viewModel.Page);
                    logger.Debug("Navigate to DB page");
                    navigationService.Navigate(new DBPage());
                });
            }
        }

        public RelayCommand ExtractDatabase
        {
            get
            {
                return new RelayCommand(async obj =>
                {
                    logger.Debug("ExtractDatabase command started!");
                    logger.Trace("Prepare save file dialog");
                    SaveFileDialog fileDialog = new SaveFileDialog
                    {
                        DefaultExt = "JSON files (*.json) | *.json",
                        Filter = "JSON files (*.json) | *.json",
                        InitialDirectory = Environment.CurrentDirectory,
                        FileName = "DbBackup"
                    };
                    
                    logger.Trace("Show save file dialog");
                    if (fileDialog.ShowDialog() != true)
                        return;

                    logger.Debug("Try to extract database");
                    if (await ExtractDB(fileDialog.FileName))
                    {
                        MessageBox.Show("Extract successfully!");
                        logger.Info("Successfully extract database to file = " + fileDialog.FileName);
                        return;
                    }

                    logger.Warn("Database extract finish unsuccessfully. Check lower level logs");
                    MessageBox.Show("Something goes wrong");
                });
            }
        }

        #endregion


        #region Properties 

        public bool UploadButtonEnable
        {
            get { return uploadButtonEnable; }
            set
            {
                uploadButtonEnable = value;
                OnPropertyChanged("UploadButtonEnable");
            }
        }

        public Visibility InfoPanelVisibility
        {
            get { return infoPanelVisibility; }
            set
            {
                infoPanelVisibility = value;
                OnPropertyChanged("InfoPanelVisibility");
            }
        }

        public string ModelName
        {
            get
            {
                if (model == null)
                    return "";

                return model.Name;
            }
        }

        public string ModelVersion
        {
            get
            {
                if (model == null)
                    return "";

                return model.FileVersion;
            }
        }

        public string ModelDate
        {
            get
            {
                if (model == null)
                    return "";

                return model.ModifyDate.ToString();
            }
        }

        #endregion

        public DependencyObject Page { get; set; }

        public MainPageViewModel(DependencyObject page)
        {
            logger.Trace("Initializing MainPageViewModel");
            Page = page;
            model = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            logger.Trace($"Property on MainPageViewModel {prop} has updated!");
        }
    }
}
