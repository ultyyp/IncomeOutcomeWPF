using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace IncomeOutcomeWPF
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
            if (ComboBox.SelectedIndex == 0)
            {
                decimal income = 0m, outcome = 0m, total = 0m;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var calculateTask = Task.Run(() =>
                {
                    string incomePath = @"C:\Payments\income.txt";
                    string outcomePath = @"C:\Payments\outcome.txt";



                    var incomeTask = Task.Run(() =>
                    {
                        string[] incomeLines = File.ReadAllLines(incomePath);
                        foreach (string line in incomeLines)
                        {
                            income += decimal.Parse(line);
                        }
                    });

                    var outcomeTask = Task.Run(() =>
                    {
                        string[] outcomeLines = File.ReadAllLines(outcomePath);
                        foreach (string line in outcomeLines)
                        {

                            outcome += decimal.Parse(line);
                        }
                    });
                    Task.WaitAll(incomeTask, outcomeTask);

                });

                var changeTask = Task.Run(() =>
                {
                    Task.WaitAll(calculateTask);
                    total = income - outcome;
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    Dispatcher.Invoke(() =>
                    {
                        IncomeBox.Text = "Income: " + total;
                        TimeBlock.Text = "Time Elapsed: " + ts;
                    });
                });
            }


            else if(ComboBox.SelectedIndex==1)
            {
                decimal income = 0m, outcome = 0m, total = 0m;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string incomePath = @"C:\Payments\income.txt";
                string outcomePath = @"C:\Payments\outcome.txt";
                void outcomeAction()
                {
                    string[] outcomeLines = File.ReadAllLines(outcomePath);
                    foreach (string line in outcomeLines)
                    {

                        outcome += decimal.Parse(line);
                    }
                }
                void incomeAction()
                {
                    string[] incomeLines = File.ReadAllLines(incomePath);
                    foreach (string line in incomeLines)
                    {
                        income += decimal.Parse(line);
                    }
                }
                Parallel.Invoke(incomeAction, outcomeAction);
                Parallel.Invoke(() => 
                {
                    total = income - outcome;
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    Dispatcher.Invoke(() =>
                    {
                        IncomeBox.Text = "Income: " + total;
                        TimeBlock.Text = "Time Elapsed: " + ts;
                    });
                });

            }
        }
    }
}
