using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class FilesViewModel :INotifyPropertyChanged
    {
        /// <summary>
        /// Populate File Type ComboBox From XML Config File
        /// </summary>
        public FilesViewModel()
        {
            IList<FileType> list = new List<FileType>();
            _files = new CollectionView(list);
            foreach (var item in getData())
            {
                list.Add(new FileType(item));
            }
        }
        //Load XML Document
        //Retreive Data as Array
        public static string[] getData()
        {
            var document = XDocument.Load(@"C:\Users\ps-it4\source\repos\WpfApp1\WpfApp1\config.xml");
            var fileType = from file in document.Descendants("files")
                           select (string)file.Element("type");

            return fileType.ToArray();
        }
        private readonly CollectionView _files;
        private string _fileType;      

        public CollectionView Files
        {
            get { return _files; }
        }

        public string FileType
        {
            get { return _fileType; }
            set
            {
                if (_fileType == value) return;
                _fileType = value;
                OnPropertyChanged("FileType");
            }
        }
        #region INotifyPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
