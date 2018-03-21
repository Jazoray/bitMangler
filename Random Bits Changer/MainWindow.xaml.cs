using System;
using System.Collections;
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

namespace Random_Bits_Changer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string FilePath { get; set; }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.                
                FilePath = files.First();

                FileNameLabel.Content = FilePath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] file = System.IO.File.ReadAllBytes(FilePath);

            if (Int32.TryParse(bitsCounter.Text, out int iterations))
            {
                Random rnd = new Random(System.Environment.TickCount);

                BitArray bitArray = new BitArray(file);

                for (int i = 0; i < iterations; i++)
                {
                    int offset = rnd.Next(bitArray.Count - 1);
                    bitArray[offset] = !bitArray[offset];
                }
                byte[] result = BitArrayToByteArray(bitArray);

                var directoryPath = System.IO.Path.GetDirectoryName(FilePath);
                var fileName = System.IO.Path.GetFileNameWithoutExtension(FilePath);
                var extension = System.IO.Path.GetExtension(FilePath);

                var outputFileName = directoryPath +"\\"+ fileName + bitsCounter.Text + "Output" + extension;

                System.IO.File.WriteAllBytes(outputFileName, result);

                outputLabel.Content = "your bitflipped file was saved to " +System.Environment.NewLine + outputFileName;
            }

        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}
