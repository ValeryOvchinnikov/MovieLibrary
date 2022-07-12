using MovieLibrary.Commands.Pagination;
using MovieLibrary.Commands;
using MovieLibrary.Models;
using MovieLibrary.Repositories;
using MovieLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using ClosedXML.Excel;
using System.Data;
using LinqKit;
using MovieLibrary.Interfaces;

namespace MovieLibrary.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private IRepository<Director> _directorRepository;
        private IRepository<Movie> _movieRepository;
        private IMovieService _movieService;
        private IDialogService _dialogService;
        private ICommand _openCSVCommand;
        private ICommand _writeXMLCommand;
        private ICommand _writeXLSCommand;
        private ICommand _applyFilterCommand;
        private ICommand _resetFilterCommand;
        private string _filterMovieName;
        private string _filterMovieYear;
        private string _filterDirectorFirstName;
        private string _filterDirectorLastName;
        private string _filterMovieRating;
        private int _itemPerPage = 50;
        private int _itemcount;
        private int _totalPages;
        private int _currentPageIndex;
        private ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();
        public event PropertyChangedEventHandler? PropertyChanged;
        public ExpressionStarter<Movie> filter;
        public CollectionViewSource ViewList { get; set; }
        public ReadOnlyObservableCollection<Movie> MovieList
        {
            get { return new ReadOnlyObservableCollection<Movie>(_movieList); }
        }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand FirstCommand { get; private set; }
        public ICommand LastCommand { get; private set; }


        public string FilterMovieName
        {
            get
            {
                return _filterMovieName;
            }
            set
            {
                _filterMovieName = value;
                RaisePropertyChanged("FilterMovieName");
            }
        }


        public string FilterMovieYear
        {
            get
            {
                return _filterMovieYear;
            }
            set
            {
                _filterMovieYear = value;
                RaisePropertyChanged("FilterMovieYear");
            }
        }

        public string FilterDirectorFirstName
        {
            get
            {
                return _filterDirectorFirstName;
            }
            set
            {
                _filterDirectorFirstName = value;
                RaisePropertyChanged("FilterDirectorFirstName");
            }
        }

        public string FilterDirectorLastName
        {
            get
            {
                return _filterDirectorLastName;
            }
            set
            {
                _filterDirectorLastName = value;
                RaisePropertyChanged("FilterDirectorLastName");
            }
        }

        public string FilterMovieRating
        {
            get
            {
                return _filterMovieRating;
            }
            set
            {
                _filterMovieRating = value;
                RaisePropertyChanged("FilterMovieRating");
            }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                _currentPageIndex = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public int CurrentPage
        {
            get { return _currentPageIndex + 1; }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            private set
            {
                _totalPages = value;
                OnPropertyChanged("TotalPage");
            }
        }



        public void ShowNextPage()
        {
            CurrentPageIndex++;
            ViewList.View.Refresh();
        }

        public void ShowPreviousPage()
        {
            CurrentPageIndex--;
            ViewList.View.Refresh();
        }

        public void ShowFirstPage()
        {
            CurrentPageIndex = 0;
            ViewList.View.Refresh();
        }

        public void ShowLastPage()
        {
            CurrentPageIndex = TotalPages - 1;
            ViewList.View.Refresh();
        }


        public ICommand OpenCSVCommand
        {
            get
            {
                if (_openCSVCommand == null) _openCSVCommand = new RelayCommand(p => OpenCSV());
                return _openCSVCommand;
            }
        }

        public ICommand WriteXMLCommand
        {
            get
            {
                if (_writeXMLCommand == null) _writeXMLCommand = new RelayCommand(p => WriteXML());
                return _writeXMLCommand;
            }
        }

        public ICommand WriteXLSCommand
        {
            get
            {
                if (_writeXLSCommand == null) _writeXLSCommand = new RelayCommand(p => WriteXLS());
                return _writeXLSCommand;
            }
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                if (_applyFilterCommand == null) _applyFilterCommand = new RelayCommand(p => ApplyFilters());
                return _applyFilterCommand;
            }
        }

        public ICommand ResetFilterCommand
        {
            get
            {
                if (_resetFilterCommand == null) _resetFilterCommand = new RelayCommand(p => ResetFilters());
                return _resetFilterCommand;
            }
        }


        public MainWindowViewModel(IDialogService dialogService, IMovieService movieService, IRepository<Director> directorRepository, IRepository<Movie> movieRepository)
        {
            _directorRepository = directorRepository;
            _movieRepository = movieRepository;
            _dialogService = dialogService;
            _movieService = movieService;


            ViewList = new CollectionViewSource();
            NextCommand = new NextPageCommand(this);
            PreviousCommand = new PreviousPageCommand(this);
            FirstCommand = new FirstPageCommand(this);
            LastCommand = new LastPageCommand(this);
            ViewList.Filter += new FilterEventHandler(PageFilter);

            CurrentPageIndex = 0;
        }

        private void PageFilter(object sender, FilterEventArgs e)
        {
            int index = _movieList.IndexOf((Movie)e.Item);
            if (index >= _itemPerPage * CurrentPageIndex && index < _itemPerPage * (CurrentPageIndex + 1))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void CalculateTotalPages()
        {
            _itemcount = ViewList.View.SourceCollection.Cast<Movie>().Count();
            if (_itemcount % _itemPerPage == 0)
            {
                TotalPages = (_itemcount / _itemPerPage);
            }
            else
            {
                TotalPages = (_itemcount / _itemPerPage) + 1;
            }
        }

        private void OpenCSV()
        {
            var file = _dialogService.OpenFileDialog(".csv", "Doc (.csv)|*.csv*");
            ReadCSV(file);
            _movieList = _movieService.GetAllMovies();
            ViewList.Source = MovieList;
            CalculateTotalPages();
        }

        public void ReadCSV(string fileName)
        {
            try
            {
                using (var file = new System.IO.StreamReader(fileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] data = line.Split(';');
                        Director newDirector = new Director(data[0], data[1]);
                        _directorRepository.Create(newDirector);
                        var directors = _directorRepository.GetAll();
                        var director = directors.Find(d => d.FirstName == newDirector.FirstName && d.LastName == newDirector.LastName);
                        var newMovie = new Movie(data[2], Convert.ToInt32(data[3]), Convert.ToInt32(data[4]));
                        director.Movies.Add(newMovie);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception Caught",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void WriteXML()
        {
            try
            {
                var file = _dialogService.SaveFileDiaolog(".xml", "Doc (.xml)|*.xml*");

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                var movies = _movieRepository.GetAll();
                XmlWriter textWriter = XmlWriter.Create(file, settings);
                textWriter.WriteStartDocument();
                textWriter.WriteStartElement("Movies");

                foreach (Movie movie in movies)
                {

                    textWriter.WriteStartElement("Record");
                    textWriter.WriteAttributeString("id", movie.Id.ToString());

                    textWriter.WriteStartElement("FirstName");
                    textWriter.WriteString(movie.Director.FirstName);
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("LastName");
                    textWriter.WriteString(movie.Director.LastName);
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("MovieName");
                    textWriter.WriteString(movie.Name);
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("MovieYear");
                    textWriter.WriteString(movie.Year.ToString());
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("MovieRating");
                    textWriter.WriteString(movie.Rating.ToString());
                    textWriter.WriteEndElement();

                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();
                textWriter.WriteEndDocument();
                textWriter.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception Caught",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void WriteXLS()
        {
            try
            {
                var file = _dialogService.SaveFileDiaolog(".xlsx", "Doc (.xlsx)|*.xlsx*");

                var movies = _movieRepository.GetAll();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Movies");
                    worksheet.Cell("A1").Value = "FirstName";
                    worksheet.Cell("B1").Value = "LastName";
                    worksheet.Cell("C1").Value = "MovieName";
                    worksheet.Cell("D1").Value = "Year";
                    worksheet.Cell("E1").Value = "Rating";


                    for (int i = 0; i < movies.Count; i++)
                    {
                        var movie = movies[i];
                        worksheet.Cell(i + 2, 1).Value = movie.Director.FirstName;
                        worksheet.Cell(i + 2, 2).Value = movie.Director.LastName;
                        worksheet.Cell(i + 2, 3).Value = movie.Name;
                        worksheet.Cell(i + 2, 4).Value = movie.Year;
                        worksheet.Cell(i + 2, 5).Value = movie.Rating;
                    }
                    workbook.SaveAs(file);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception Caught",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ApplyFilters()
        {
            filter = PredicateBuilder.New<Movie>().And(m => true);
            if (!string.IsNullOrEmpty(FilterMovieName))
            {
                filter.And(m => m.Name.Contains(FilterMovieName));
            }
            if (!string.IsNullOrEmpty(FilterMovieYear))
            {
                filter.And(m => m.Year.ToString().Contains(FilterMovieYear));
            }
            if (!string.IsNullOrEmpty(FilterDirectorFirstName))
            {
                filter.And(m => m.Director.FirstName.Contains(FilterDirectorFirstName));
            }
            if (!string.IsNullOrEmpty(FilterDirectorLastName))
            {
                filter.And(m => m.Director.LastName.Contains(FilterDirectorLastName));
            }
            if (!string.IsNullOrEmpty(FilterMovieRating))
            {
                filter.And(m => m.Rating.ToString() == FilterMovieRating.ToString());
            }

            _movieList = _movieService.GetFilteredMovies(filter);
            ViewList.Source = MovieList;
            CurrentPageIndex = 0;
            CalculateTotalPages();

            ViewList.View.Refresh();
        }

        public void ResetFilters()
        {
            FilterMovieName = "";
            FilterMovieYear = "";
            FilterDirectorFirstName = "";
            FilterDirectorLastName = "";
            FilterMovieRating = "";
            _movieList = _movieService.GetAllMovies();
            ViewList.Source = MovieList;
            CurrentPageIndex = 0;
            CalculateTotalPages();
            ViewList.View.Refresh();
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
