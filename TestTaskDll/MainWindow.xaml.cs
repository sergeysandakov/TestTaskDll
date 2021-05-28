using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace TestTaskDll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = textBox_1.Text.Trim();

            List<string> Dlls = new List<string>();
            
            Dlls.AddRange(Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".dll") || s.EndsWith(".DLL")));

            foreach (var dll in Dlls)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(dll);

                    foreach (var type in assembly.GetTypes())
                    {
                        dg.ItemsSource = type.Name;
                        foreach (var m in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            if (m.IsFamily || m.IsPublic)
                                dg.ItemsSource = m.Name;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Something go wrong with {0}", dll);
                    throw;
                }
            }

        }
    }
}
