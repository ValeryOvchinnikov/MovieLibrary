using Microsoft.EntityFrameworkCore;
using MovieLibrary.DBContext;
using MovieLibrary.Models;
using MovieLibrary.Repositories;
using MovieLibrary.Services;
using MovieLibrary.Utility;
using MovieLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace MovieLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MovieLibraryContext movieLibraryContext = new MovieLibraryContext();
        DirectorRepository directorRepository = new DirectorRepository(new MovieLibraryContext());
        MovieRepository movieRepository = new MovieRepository(new MovieLibraryContext());
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new DialogService(), new MovieService(directorRepository, movieRepository), new DirectorRepository(movieLibraryContext), new MovieRepository(movieLibraryContext));
        }
    }
}
