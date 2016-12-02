using System;
using System.Drawing;
using System.Windows.Forms;
using SLICOSuperpixelsCSharp.Properties;

namespace SLICOSuperpixelsCSharp
{
    public partial class MainForm : Form
    {
        
        // scource image and superpixel segmented image
        //
        private Bitmap imgBitmap = Resources.IR_Building;
        private Bitmap imgSuperpixel = null;

        // SLICO related members
        //
        private SLICO _slico = new SLICO();     // SLICO 
        private int[] _klabels;                  // SLICO segment label vector for all pixels
        private int _numlabels;                  // number of resulted labels
        private int K;                          // user specified segment numbers

        // Flag to control the selection of images
        //
        private int _picFlag = 0;
        private int NUM_PIC = 3;                // number of pictures for demonstration purpose

        // Propertie to control the image selection
        //
        private int MpicFlag
        {
            get
            {
                _picFlag = _picFlag%NUM_PIC;
                return _picFlag;
            }
        }
        private Bitmap BitmapSelected
        {
            get
            {
                switch (MpicFlag)
                {
                    case 0:
                        return Resources.IR_Building;
                    case 1:
                        return Resources.MRI_Prostate;
                    case 2:
                        return Resources.MRI_Chest_Axial;
                    default:
                        return Resources.MRI_Prostate;
                }
            }
        }

        private String StrBitmapName
        {
            get
            {
                switch (MpicFlag)
                {
                    case 0:
                        return "Thermal Image - Building ";
                    case 1:
                        return "MRI Image - Prostate";
                    case 2:
                        return "MRI Image - Chest Axial";
                    default:
                        return "MRI Image - Prostate";
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // initialize controls
            splitContainer1.SplitterDistance = splitContainer1.Width - 218;
            splitContainer2.SplitterDistance = (int) (splitContainer2.Width*0.5);

            comboBox1.SelectedIndex = 9;

            // PictureBox
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = imgBitmap;

            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            textBox1.Text = Resources.strProgramInfo + Resources.strHelpInfo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _picFlag ++;

            imgBitmap = this.BitmapSelected;
            pictureBox1.Image = imgBitmap;
            pictureBox2.Image = null;

            textBox1.Text += (Resources.strImageReload + "\r\n");
            textBox1.Text += (StrBitmapName + "\r\n");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (null == comboBox1.SelectedText)
            {
                textBox1.Text += @"\nSelect Superpixel Number First!\n";
                return;
            }
            else
            {
                K = int.Parse(comboBox1.SelectedItem.ToString());
            }
            if (null != imgBitmap)
            {
                // Perform SLIO to input image
                imgSuperpixel = _slico.PerformSLICO_ForGivenK(ref imgBitmap, out _klabels, out _numlabels, K, Color.Red, 10);
            }

            // update Pixturebox2
            pictureBox2.Image = imgSuperpixel;

            textBox1.Text += (Resources.strSegmentResult + _numlabels.ToString() + "\r\n");
        }
    }
}
