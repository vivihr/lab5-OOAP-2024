using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;

namespace PictureEditing
{


    public partial class Form1 : Form
    {
        private List<PictureBox> pictureBoxes;
        private ImageFacade imageFacade;
        public Form1()
        {
            InitializeComponent();
            InitializePictureBoxes();
            imageFacade = new ImageFacade(pictureBoxes);
        }

        private void InitializePictureBoxes()
        {
            pictureBoxes = new List<PictureBox> { pictureBox1, pictureBox2, pictureBox3 };
        }

        private void UpdateListBox()
        {
            listBox1.Items.Clear();
            foreach (var pictureBox in pictureBoxes)
            {
                listBox1.Items.Add($"Name: {pictureBox.Name}, Size: {pictureBox.Image.Size}");
            }
        }

        private void ApplyFilter(IFilter filter)
        {
            foreach (var pictureBox in pictureBoxes)
            {
                pictureBox.Image = filter.Apply(pictureBox.Image);
            }
            UpdateListBox();
        }

        private void blurButton_Click(object sender, EventArgs e)
        {
            ApplyFilter(new BlurFilter());
        }

        private void clarityButton_Click(object sender, EventArgs e)
        {
            ApplyFilter(new ClarityFilter());
        }

        private void saturationButton_Click(object sender, EventArgs e)
        {
            ApplyFilter(new SaturationFilter());
        }

        private void noiseButton_Click(object sender, EventArgs e)
        {
            ApplyFilter(new NoiseFilter());
        }
        
        //фільтри
        interface IFilter
        {
            Image Apply(Image image);
        }

        //клас для керування рисунками
        class ImageFacade
        {
            private readonly List<PictureBox> pictureBoxes;

            public ImageFacade(List<PictureBox> pictureBoxes)
            {
                this.pictureBoxes = pictureBoxes;
            }

            public void ApplyFilter(IFilter filter)
            {
                foreach (var pictureBox in pictureBoxes)
                {
                    pictureBox.Image = filter.Apply(pictureBox.Image);
                }
            }
        }

        //фільтри
        class BlurFilter : IFilter
        {
            public Image Apply(Image image)
            {
                //розмиття
                return ApplyBlurFilter(image);
            }

            private Image ApplyBlurFilter(Image image)
            {
                Bitmap bitmap = new Bitmap(image);
                //логіка розмиття
                AForge.Imaging.Filters.Blur blurFilter = new AForge.Imaging.Filters.Blur();
                return blurFilter.Apply(bitmap);
            }
        }


        class ClarityFilter : IFilter
        {
            public Image Apply(Image image)
            {
                //підвищення чіткості
                return ApplyClarityFilter(image);
            }

            private Image ApplyClarityFilter(Image image)
            {
                Bitmap bitmap = new Bitmap(image);

                //AForge.NET для підвищення чіткості
                AForge.Imaging.Filters.Sharpen clarityFilter = new AForge.Imaging.Filters.Sharpen();
                return clarityFilter.Apply(bitmap);
            }
        }

        class SaturationFilter : IFilter
        {
            public Image Apply(Image image)
            {
                return ApplySaturationFilter(image);
            }

            private Image ApplySaturationFilter(Image image)
            {
                Bitmap bitmap = new Bitmap(image);
                AForge.Imaging.Filters.SaturationCorrection saturationFilter = new AForge.Imaging.Filters.SaturationCorrection();

                return saturationFilter.Apply(bitmap);
            }
        }

        class NoiseFilter : IFilter
        {
            public Image Apply(Image image)
            {
                //приклад простого додавання шуму
                return ApplyNoiseFilter(image);
            }

            private Image ApplyNoiseFilter(Image image)
            {
                Bitmap bitmap = new Bitmap(image);

                //переведення зображення до підтримуваного формату
                if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                {
                    bitmap = AForge.Imaging.Image.Clone(bitmap, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }

                //створення фільтру шуму
                AForge.Imaging.Filters.AdditiveNoise noiseFilter = new AForge.Imaging.Filters.AdditiveNoise();

                //застосування фільтру до зображення
                return noiseFilter.Apply(bitmap);
            }
        }

    }
}
