using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MathNet;
using CBIR;

namespace CBIR
{

    public partial class Form1 : Form
    {
        private static string fileNamePath;

        private static string[] filesToCompare;

        public static int lambdaPassedToMethod = 64;

        public static int pixelDividedByLambda = 256 / lambdaPassedToMethod;

        public int pixelsInImage;

        public static int degrees = 30;

        public static int sizeM = (360 / degrees) + 1;
        //maski
        private static int[,] xPrewitt = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

        private static int[,] yPrewitt = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };

        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //make an empty bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = original.GetPixel(i, j);

                    //create the grayscale version of the pixel
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                        + (originalColor.B * .11));

                    //create the color object
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    //set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }

        public RGB[] getRGBfromImage(Image im, int lambda)
        {
            int imageSize = im.Height * im.Width;
            pixelsInImage = imageSize;
            Dictionary<int, int> countOfSpecificVetorList = new Dictionary<int, int>();
            using (Bitmap bmp = new Bitmap(im))
            {
                Color clr;
                //int[, ,] listOfRGB = new int[im.Width, im.Height, 3];
                RGB[] listOfRGBVectorsFromImage = new RGB[imageSize];
                int j = 0;
                for (int i = 0; i < imageSize; i++)
                {
                    listOfRGBVectorsFromImage[i] = new RGB();
                }

                for (int i = 0; i < imageSize; i++)
                {
                    //clr = bmp.GetPixel(x, y);

                }
                for (int x = 0; x < im.Width; x++)
                {
                    for (int y = 0; y < im.Height; y++)
                    {
                        clr = bmp.GetPixel(x, y);
                        listOfRGBVectorsFromImage[j].R = clr.R / lambda;
                        listOfRGBVectorsFromImage[j].G = clr.G / lambda;
                        listOfRGBVectorsFromImage[j].B = clr.B / lambda;

                        //resultsTextBox.AppendText(j + " - RGB: " + "(" + listOfRGBVectorsFromImage[j].R + " " + listOfRGBVectorsFromImage[j].G + " " + listOfRGBVectorsFromImage[j].B + ")" + Environment.NewLine);

                        //chart1.Series["Series1"].Points.AddXY(listOfRGBVectorsFromImage[j],j);
                        j++;
                    }


                    //listOfRGB[x, y, 0] = clr.R;
                    //listOfRGB[x, y, 1] = clr.G;
                    //listOfRGB[x, y, 2] = clr.B;
                }

                //listOfRGBVectorsFromImage = listOfRGBVectorsFromImage.OrderBy(x => x.R);
                return listOfRGBVectorsFromImage;
            }

        }

        public HSV[] getHSVfromImage(Image im, int lambdaH, int lambdaSV)
        {
            int imageSize = im.Height * im.Width;
            pixelsInImage = imageSize;
            Dictionary<int, int> countOfSpecificVetorList = new Dictionary<int, int>();
            using (Bitmap bmp = new Bitmap(im))
            {
                Color clr;
                //int[, ,] listOfRGB = new int[im.Width, im.Height, 3];
                HSV[] listOfHSVVectorsFromImage = new HSV[imageSize];
                int j = 0;
                for (int i = 0; i < imageSize; i++)
                {
                    listOfHSVVectorsFromImage[i] = new HSV();
                }

                for (int i = 0; i < imageSize; i++)
                {
                    //clr = bmp.GetPixel(x, y);
                }
                for (int x = 0; x < im.Width; x++)
                {
                    for (int y = 0; y < im.Height; y++)
                    {
                        clr = bmp.GetPixel(x, y);
                        int max = Math.Max(clr.R, Math.Max(clr.G, clr.B));
                        int min = Math.Min(clr.R, Math.Min(clr.G, clr.B));

                        listOfHSVVectorsFromImage[j].H = Math.Round(clr.GetHue() / lambdaH);
                        listOfHSVVectorsFromImage[j].S = Math.Round((((max == 0) ? 0 : 1d - (1d * min / max)) / lambdaSV) * 100);
                        listOfHSVVectorsFromImage[j].V = Math.Round(((max / 255d) / lambdaSV) * 100);

                        //resultsTextBox.AppendText(j + " - RGB: " + "(" + listOfRGBVectorsFromImage[j].R + " " + listOfRGBVectorsFromImage[j].G + " " + listOfRGBVectorsFromImage[j].B + ")" + Environment.NewLine);

                        //chart1.Series["Series1"].Points.AddXY(listOfRGBVectorsFromImage[j],j);
                        j++;
                    }


                    //listOfRGB[x, y, 0] = clr.R;
                    //listOfRGB[x, y, 1] = clr.G;
                    //listOfRGB[x, y, 2] = clr.B;
                }

                //listOfRGBVectorsFromImage = listOfRGBVectorsFromImage.OrderBy(x => x.R);
                return listOfHSVVectorsFromImage;
            }

        }

        public HistogramRGB[] getAllPossibleConfigurationsOfRGBVectors(int maxValueOfEachElement)
        {
            int countOfVectorsMatrix = maxValueOfEachElement * maxValueOfEachElement * maxValueOfEachElement;
            HistogramRGB[] hst = new HistogramRGB[countOfVectorsMatrix];
            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                hst[i] = new HistogramRGB();
                hst[i].Vector = new RGB();
            }

            int l = 0;
            for (int i = 0; i < maxValueOfEachElement; i++)
            {
                for (int j = 0; j < maxValueOfEachElement; j++)
                {
                    for (int k = 0; k < maxValueOfEachElement; k++, l++)
                    {
                        hst[l].Count = 0; //ilosc wystapien danego vectora
                        hst[l].Vector.R = i;
                        hst[l].Vector.G = j;
                        hst[l].Vector.B = k;
                    }
                }
            }
            return hst;
        }

        public HistogramHSV[] getAllPossibleConfigurationsOfHSVVectors(int maxValueOfEachElement, int pixelDividedByLambdaHSV)
        {
            int countOfVectorsMatrix = pixelDividedByLambdaHSV * maxValueOfEachElement * maxValueOfEachElement;
            HistogramHSV[] hst = new HistogramHSV[countOfVectorsMatrix];
            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                hst[i] = new HistogramHSV();
                hst[i].Vector = new HSV();
            }

            int l = 0;
            for (int i = 0; i < pixelDividedByLambdaHSV; i++)
            {
                for (int j = 0; j < maxValueOfEachElement; j++)
                {
                    for (int k = 0; k < maxValueOfEachElement; k++, l++)
                    {
                        hst[l].Count = 0; //ilosc wystapien danego vectora
                        hst[l].Vector.H = i;
                        hst[l].Vector.S = j;
                        hst[l].Vector.V = k;
                    }
                }
            }
            return hst;
        }

        public HistogramRGB[] getRGBHistogramFromImage(Image im, int lambda, int maxValueOfEachElement)
        {
            int countOfVectorsMatrix = maxValueOfEachElement * maxValueOfEachElement * maxValueOfEachElement;

            int lambdaPassedToMethod = lambda;
            int pixelDividedByLambda = 256 / lambdaPassedToMethod;
            //int countOfVectorsMatrix = pixelDividedByLambda * pixelDividedByLambda * pixelDividedByLambda;
            RGB[] allPossibleVectorsFiledFromImage = new RGB[countOfVectorsMatrix];
            //RGB[] sortedList = new RGB[countOfVectorsMatrix];
            HistogramRGB[] RBGtemp = new HistogramRGB[countOfVectorsMatrix];
            List<HistogramRGB> lisHist = new List<HistogramRGB>();
            HistogramRGB[] allHstVectors = new HistogramRGB[countOfVectorsMatrix];

            HistogramHSV[] HSVtemp = new HistogramHSV[countOfVectorsMatrix];
            HistogramHSV[] allHSVVectors = new HistogramHSV[countOfVectorsMatrix];

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                RBGtemp[i] = new HistogramRGB();
                RBGtemp[i].Vector = new RGB();

                allHstVectors[i] = new HistogramRGB();
                allHstVectors[i].Vector = new RGB();

                //HSVtemp[i] = new HistogramHSV();
                //HSVtemp[i].Vector = new HSV();

                //allHSVVectors[i] = new HistogramHSV();
                //allHSVVectors[i].Vector = new HSV();
            }

            try
            {

                if (!fileNamePath.Equals(null))
                {
                    //Image colorImage = Image.FromFile(fileNamePath);

                    //Bitmap monoCromaticImage = MakeGrayscale(new Bitmap(colorImage));

                    #region RGB
                    if ("RGB".Equals(RGBHSVCombobox.SelectedItem))
                    {
                        var listOfUniqueVectors = getRGBfromImage(im, lambdaPassedToMethod).GroupBy(l => new { l.R, l.G, l.B }).Select(g => new
                        {
                            Vector = g.Key,
                            Count = g.Select(l => new { l.R, l.G, l.B }).Count()
                        });

                        var sortedList = listOfUniqueVectors.OrderBy(r => r.Vector.R).ThenBy(g => g.Vector.G).ThenBy(b => b.Vector.B);

                        chart1.Series[0].SmartLabelStyle.Enabled = false;
                        chart1.Series[0].LabelAngle = 90;
                        //chart1.Series[0].IsVisibleInLegend = false;
                        //chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        //chart1.ChartAreas[0].AxisX.Interval = 1;


                        //przepisanie na nieanonimowy typ
                        int it = 0;
                        foreach (var element in sortedList)
                        {
                            RBGtemp[it].Vector.R = element.Vector.R;
                            RBGtemp[it].Vector.G = element.Vector.G;
                            RBGtemp[it].Vector.B = element.Vector.B;
                            RBGtemp[it].Count = element.Count;
                            it++;
                        }

                        //add empty vectors
                        allHstVectors = getAllPossibleConfigurationsOfRGBVectors(pixelDividedByLambda);

                        //rewrite vectors into empty matrix
                        for (int i = 0; i < countOfVectorsMatrix; i++)
                        {
                            foreach (var item in sortedList)
                            {
                                if (item.Vector.R == allHstVectors[i].Vector.R && item.Vector.G == allHstVectors[i].Vector.G && item.Vector.B == allHstVectors[i].Vector.B)
                                {
                                    allHstVectors[i].Vector.R = item.Vector.R;
                                    allHstVectors[i].Vector.G = item.Vector.G;
                                    allHstVectors[i].Vector.B = item.Vector.B;
                                    allHstVectors[i].Count = item.Count;
                                }
                            }
                        }
                        ;
                        //foreach (var element in allHstVectors)
                        //{
                        //    //hstElement.vector.R = element.R;
                        //    //hstElement.vector.G = element.G;
                        //    //hstElement.vector.B = element.B;
                        //    //unique_items.Add(element);
                        //    //element.Vector.R;
                        //    chart1.Series["Series1"].Points.AddXY(element.Vector.ToString(), element.Count);
                        //}
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return allHstVectors;
        }

        public HistogramHSV[] getHSVHistogramFromImage(Image im, int lambda, int maxValueOfEachElement)
        {


            int lambdaPassedToMethod = 25;
            int lambdaPassedToMethodH = 72;
            int pixelDividedByLambda = 4; //4 poziomy S i V - 100/25
            int pixelDividedByLambdaHSV = 5; //H levels - 360/72

            int countOfVectorsMatrix = (pixelDividedByLambdaHSV + 1) * (pixelDividedByLambda + 1) * (pixelDividedByLambda + 1);

            //int countOfVectorsMatrix = pixelDividedByLambda * pixelDividedByLambda * pixelDividedByLambda;
            RGB[] allPossibleVectorsFiledFromImage = new RGB[countOfVectorsMatrix];
            //RGB[] sortedList = new RGB[countOfVectorsMatrix];
            //HistogramRGB[] RBGtemp = new HistogramRGB[countOfVectorsMatrix];
            //List<HistogramRGB> lisHist = new List<HistogramRGB>();
            //HistogramRGB[] allHstVectors = new HistogramRGB[countOfVectorsMatrix];

            HistogramHSV[] HSVtemp = new HistogramHSV[countOfVectorsMatrix];
            HistogramHSV[] allHSVVectors = new HistogramHSV[countOfVectorsMatrix];

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                //RBGtemp[i] = new HistogramRGB();
                //RBGtemp[i].Vector = new RGB();

                //allHstVectors[i] = new HistogramRGB();
                //allHstVectors[i].Vector = new RGB();

                HSVtemp[i] = new HistogramHSV();
                HSVtemp[i].Vector = new HSV();

                allHSVVectors[i] = new HistogramHSV();
                allHSVVectors[i].Vector = new HSV();
            }

            try
            {

                if (!fileNamePath.Equals(null))
                {
                    //Image colorImage = Image.FromFile(fileNamePath);

                    //Bitmap monoCromaticImage = MakeGrayscale(new Bitmap(colorImage));

                    var listOfUniqueVectors = getHSVfromImage(im, lambdaPassedToMethodH, lambdaPassedToMethod).GroupBy(l => new { l.H, l.S, l.V }).Select(g => new
                    {
                        Vector = g.Key,
                        Count = g.Select(l => new { l.H, l.S, l.V }).Count()
                    });

                    var sortedList = listOfUniqueVectors.OrderBy(h => h.Vector.H).ThenBy(s => s.Vector.S).ThenBy(v => v.Vector.V);

                    //chart1.Series[0].SmartLabelStyle.Enabled = false;
                    //chart1.Series[0].LabelAngle = 90;
                    //chart1.Series[0].IsVisibleInLegend = false;
                    //chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                    //chart1.ChartAreas[0].AxisX.Interval = 1;


                    //przepisanie na nieanonimowy typ
                    int it = 0;
                    //uwaga na indexoutofrange!
                    foreach (var element in sortedList)
                    {
                        HSVtemp[it].Vector.H = element.Vector.H;
                        HSVtemp[it].Vector.S = element.Vector.S;
                        HSVtemp[it].Vector.V = element.Vector.V;
                        HSVtemp[it].Count = element.Count;
                        it++;
                        Console.WriteLine(it.ToString());
                    }

                    //add empty vectors - za mala macierz
                    allHSVVectors = getAllPossibleConfigurationsOfHSVVectors(pixelDividedByLambda + 1, pixelDividedByLambdaHSV + 1);

                    //rewrite vectors into empty matrix
                    for (int i = 0; i < countOfVectorsMatrix; i++)
                    {
                        foreach (var item in HSVtemp)
                        {
                            if (Math.Round(item.Vector.H) == allHSVVectors[i].Vector.H && Math.Round(item.Vector.S) == allHSVVectors[i].Vector.S && Math.Round(item.Vector.V) == allHSVVectors[i].Vector.V)
                            {
                                allHSVVectors[i].Vector.H = item.Vector.H;
                                allHSVVectors[i].Vector.S = item.Vector.S;
                                allHSVVectors[i].Vector.V = item.Vector.V;
                                allHSVVectors[i].Count = item.Count;

                            }
                        }
                        Console.WriteLine(i);
                    }
                }


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return allHSVVectors;
        }

        public HistogramGrey[] getAllAngles()
        {
            // od -180 do 180 stopni co 30 stopni
            int size = 360 / degrees;
            int start = -180 / degrees;

            HistogramGrey[] hst = new HistogramGrey[size + 1];
            HistogramGrey tempHst = new HistogramGrey();

            for (int i = 0; i < size + 1; i++)
            {
                hst[i] = new HistogramGrey();
                hst[i].pixel = new GrayPixel();
            }

            for (int i = 0; i < size + 1; i++, start++)
            {
                hst[i].pixel.angle = start;
            }

            return hst;
        }

        public HistogramGrey[] getAnglesHistogram(Image sourceImage, int windowSize)
        {
            int size = 360 / degrees;
            int matrixSize = sourceImage.Height * sourceImage.Width;
            //Image im = Image.FromFile(@"image.jpg");
            //Bitmap myBitmap = new Bitmap(sourceImage.Width, sourceImage.Height);
            HistogramGrey[] histogram = new HistogramGrey[size + 1];

            try
            {
                if (windowSize % 2 != 0)
                {
                    //odczyt obrazu
                    using (Bitmap bmp = new Bitmap(sourceImage))
                    {
                        Color originalColor;
                        Color framePixelColor;
                        GrayPixel[] _pixels = new GrayPixel[matrixSize];
                        GrayPixel _framePixelTemp = new GrayPixel();
                        GrayPixel[] _framePixels = new GrayPixel[(windowSize) * (windowSize)];
                        GrayPixel[,] _xDerivative = new GrayPixel[(windowSize), (windowSize)];
                        GrayPixel[,] _yDerivative = new GrayPixel[(windowSize), (windowSize)];

                        int[,] tempTableValues = new int[windowSize, windowSize];


                        int j = 0;
                        int counter = 0;
                        int _xSum = 0, _ySum = 0;
                        //int counter2, counter3;
                        int distance = Convert.ToInt32((windowSize - 1) * 0.5);
                        //int _maxTempValue = 0;
                        //int _minTempValue = 0;
                        for (int i = 0; i < matrixSize; i++)
                        {
                            _pixels[i] = new GrayPixel();
                        }

                        for (int i = 0; i < (windowSize) * (windowSize); i++)
                        {
                            _framePixels[i] = new GrayPixel();

                        }

                        for (int i = 0; i < windowSize; i++)
                        {
                            for (int k = 0; k < windowSize; k++)
                            {
                                _xDerivative[i, k] = new GrayPixel();
                                _yDerivative[i, k] = new GrayPixel();
                                tempTableValues[i, k] = new int();
                            }

                        }
                        for (int i = 0; i < size; i++)
                        {
                            histogram[i] = new HistogramGrey();
                            histogram[i].pixel = new GrayPixel();
                        }

                        #region
                        for (int x = 0; x < sourceImage.Width; x++)
                        {
                            for (int y = 0; y < sourceImage.Height; y++)
                            {
                                originalColor = bmp.GetPixel(x, y);
                                //create the grayscale version of the pixel - mapa pixeli
                                _pixels[j].value = (int)((originalColor.R * .3) + (originalColor.G * .59) + (originalColor.B * .11));
                                _pixels[j].R = originalColor.R;
                                _pixels[j].G = originalColor.G;
                                _pixels[j].B = originalColor.B;
                                _pixels[j].x = x;
                                _pixels[j].y = y;

                                //tworzymy liste o wielkosci okna naokolo pixela 
                                if (x >= distance && x < sourceImage.Width - distance)
                                {
                                    if (y >= distance && y < sourceImage.Height - distance)
                                    {
                                        counter = 0;
                                        //i,k - obramowanie okna po ktorym sie poruszam naokolo danego pixela
                                        //pobieram pixele naokolo pixela i tworze macierz
                                        //ptrzymana macierze mnoze poprzez maski w poziomie i pionie
                                        //majac pochodne pixela w punkcie w kierunku OX i OY moge policzyc macierz gradientu - ?
                                        //za pomoca pochodnych moge obliczyc kat nachylenia kazdego pixela w macierzy - zapisywac to w info o pixelu

                                        for (int i = x - distance, xx = 0; i <= x + distance; i++, xx++)
                                        {
                                            for (int k = y - distance, yy = 0; k <= y + distance; k++, yy++)
                                            {
                                                ;
                                                //pobieram wartosci dla kazdego pixela z okna i odrazu licze prog lokalny dla obecnego pixela(centralnego z okna)
                                                framePixelColor = bmp.GetPixel(i, k);
                                                _framePixels[counter].value = (int)((framePixelColor.R * .3) + (framePixelColor.G * .59) + (framePixelColor.B * .11));
                                                _framePixels[counter].x = i;
                                                _framePixels[counter].y = k;
                                                tempTableValues[xx, yy] = _framePixels[counter].value;
                                                counter++;
                                            }
                                        }


                                        ;//wyczysc
                                        for (int a = 0; a < windowSize; a++)
                                        {
                                            for (int b = 0; b < windowSize; b++)//x
                                            {
                                                _xDerivative[b, a].value = 0;
                                                _yDerivative[b, a].value = 0;
                                            }
                                        }


                                        ////mnozenie okna przez maske w poziomie, a potem w pionie
                                        ////zwykle mnozenie macierzy
                                        //for (int a = 0; a < windowSize; a++)
                                        //{
                                        //    for (int b = 0; b < windowSize; b++)//x
                                        //    {
                                        //        for (int c = 0; c < windowSize; c++)//y
                                        //        {
                                        //            //majac okno naokolo pixela (macierze), moge pomnozyc ja przez maski w poziomie i pionie
                                        //            _xDerivative[b, a].value += tempTableValues[c, a] * xPrewitt[b, c];
                                        //            _yDerivative[b, a].value += tempTableValues[c, a] * yPrewitt[b, c];
                                        //            counter++;
                                        //        }
                                        //    }
                                        //}

                                        //mnozenie tablic - nie macierzy.. 



                                        _xSum = 0;
                                        _ySum = 0;
                                        for (int a = 0; a < windowSize; a++)
                                        {
                                            for (int b = 0; b < windowSize; b++)//x
                                            {
                                                _xSum += tempTableValues[b, a] * xPrewitt[b, a];//_xDerivative[a, b].value;
                                                _ySum += tempTableValues[b, a] * yPrewitt[b, a];//_yDerivative[a, b].value;
                                            }
                                        }

                                        //-> otrzymane wartosci pochodnych zsumowac i policzyc kat dla danego pixela + kwantyzacja co 30 stopni
                                        //angle(x,y) = arctan(dy/dx)

                                        if (_xSum != 0 || _ySum != 0)
                                        {
                                            _pixels[j].angle = Convert.ToInt32((Math.Atan2(_ySum, _xSum) * (180 / Math.PI)) / degrees);
                                            //Console.WriteLine("Angle: " + _pixels[j].angle + " j: " + j);
                                        }
                                        else
                                        {
                                            _pixels[j].angle = 0;
                                        }

                                        //Console.WriteLine("Xsum = 0! ");
                                        //_maxTempValue = _framePixels.Max(max => max.value); //max
                                        //var temp = MathNet.Numerics.Fn.Factorial(_xSum) / MathNet.Numerics.Fn.Factorial(_ySum) * MathNet.Numerics.Fn.Factorial(_xSum - _ySum);
                                        ;
                                        //Console.WriteLine(temp);
                                        // min
                                        //_minTempValue = _framePixels.Min(min => min.value);
                                        // oblicz prog dla danego globalnego pixela

                                        //if (_maxTempValue - _minTempValue <= eps)
                                        //{
                                        //    _pixels[j].threshold = 121;//Convert.ToInt32((_maxTempValue + _minTempValue) * 0.5);
                                        //}
                                        //else
                                        //{
                                        //    _pixels[j].threshold = Convert.ToInt32((_maxTempValue + _minTempValue) * 0.5);
                                        //}
                                    }
                                }
                                else
                                {
                                    //tu trafiaja przypadki, kiedy pixel jest za blisko brzegu obrazka - nie ruszac pixeli i wziac je z oryginalnego obrazka

                                }


                                j++;
                            }
                        }
                        #endregion

                        var listOfUniqueAngles = _pixels.GroupBy(l => l.angle).Select(g => new
                        {
                            Angle = g.Key,
                            Count = g.Select(l => new { l.value }).Count()
                        });

                        var sortedList = listOfUniqueAngles.OrderBy(r => r.Angle);

                        histogram = getAllAngles();


                        for (int i = 0; i < size + 1; i++)
                        {
                            foreach (var item in sortedList)
                            {
                                if (item.Angle == histogram[i].pixel.angle)
                                {
                                    histogram[i].pixel.angle = item.Angle;
                                    histogram[i].Count = item.Count;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
            return histogram;
        }

        #region metody porownujace

        public double ManhattanRGB(HistogramRGB[] H1, HistogramRGB[] H2, int countOfVectorsMatrix)
        {
            double diversity = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                diversity += Math.Abs(H1[i].Count - H2[i].Count);
            }


            return diversity;
        }

        public double ManhattanHSV(HistogramHSV[] H1, HistogramHSV[] H2, int countOfVectorsMatrix)
        {
            double diversity = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                diversity += Math.Abs(H1[i].Count - H2[i].Count);
            }
            return diversity;
        }

        public double Manhattan(HistogramGrey[] H1, HistogramGrey[] H2, int countOfVectorsMatrix)
        {
            double diversity = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                diversity += Math.Abs(H1[i].Count - H2[i].Count);
            }


            return diversity;
        }

        public double EuklidesRGB(HistogramRGB[] H1, HistogramRGB[] H2, int countOfVectorsMatrix)
        {
            double diversity = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                diversity += Math.Sqrt((H1[i].Count - H2[i].Count) * (H1[i].Count - H2[i].Count));
            }
            return diversity;
        }

        public double EuklidesHSV(HistogramHSV[] H1, HistogramHSV[] H2, int countOfVectorsMatrix)
        {
            double diversity = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                diversity += Math.Sqrt((H1[i].Count - H2[i].Count) * (H1[i].Count - H2[i].Count));
            }
            return diversity;
        }

        public double Euklides(HistogramGrey[] H1, HistogramGrey[] H2, int countOfVectorsMatrix)
        {
            double diversity = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                diversity += Math.Sqrt((H1[i].Count - H2[i].Count) * (H1[i].Count - H2[i].Count));
            }
            return diversity;
        }

        public double MPHRGB(HistogramRGB[] H1, HistogramRGB[] H2, int countOfVectorsMatrix, int pixels)
        {
            double min = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                min += Math.Min(H1[i].Count, H2[i].Count);
            }
            return 1 - (min / pixels);
        }

        public double MPHHSV(HistogramHSV[] H1, HistogramHSV[] H2, int countOfVectorsMatrix, int pixels)
        {
            double min = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                min += Math.Min(H1[i].Count, H2[i].Count);
            }
            return 1 - (min / pixels);
        }

        public double JeffreyaRGB(HistogramRGB[] H1, HistogramRGB[] H2, int countOfVectorsMatrix)
        {
            double div = 0;
            double p = new double();
            double q = 0;
            double m = 0;

            //m = (p+q)/2
            // p/q - prawdopodobienstwo wystapienia pixela w obrazie -> h.count / ilosc pixeli w obrazie
            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                //min += Math.Min(H1[i].Count, H2[i].Count);
                p = (double)H1[i].Count / (double)pixelsInImage;
                q = (double)H2[i].Count / (double)pixelsInImage;
                m = (p + q) * 0.5;
                if (m != 0)
                {
                    if (q == 0)
                        div += p * Math.Log(p / m, 2);
                    else if (p == 0)
                        div += q * Math.Log(q / m, 2);
                    else if (p != 0 && q != 0)
                        div += p * Math.Log(p / m, 2) + q * Math.Log(q / m, 2);
                }
            }
            return div;
        }

        public double JeffreyaHSV(HistogramHSV[] H1, HistogramHSV[] H2, int countOfVectorsMatrix)
        {
            double div = 0, p = 0, q = 0, m = 0;

            //m = (p+q)/2
            // p/q - prawdopodobienstwo wystapienia pixela w obrazie -> h.count / ilosc pixeli w obrazie
            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                //min += Math.Min(H1[i].Count, H2[i].Count);
                p = (double)H1[i].Count / (double)pixelsInImage;
                q = (double)H2[i].Count / (double)pixelsInImage;
                m = (p + q) * 0.5;
                if (m != 0)
                {
                    if (q == 0)
                        div += p * Math.Log(p / m, 2);
                    else if (p == 0)
                        div += q * Math.Log(q / m, 2);
                    else if (p != 0 && q != 0)
                        div += p * Math.Log(p / m, 2) + q * Math.Log(q / m, 2);
                }
            }
            return div;
        }

        public double KullbackaLeibleraRGB(HistogramRGB[] H1, HistogramRGB[] H2, int countOfVectorsMatrix)
        {
            double div = 0;
            double p = new double();
            double q = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                p = (double)H1[i].Count / (double)pixelsInImage;
                q = (double)H2[i].Count / (double)pixelsInImage;

                if (p == 0)
                    div += 0;
                else if (q == 0)
                    div += 1;
                else if (p != 0 && q != 0)
                    div += p * Math.Log(p / q, 2);
            }
            return div;
        }

        public double KullbackaLeibleraHSV(HistogramHSV[] H1, HistogramHSV[] H2, int countOfVectorsMatrix)
        {
            double div = 0;
            double p = new double();
            double q = 0;

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                p = (double)H1[i].Count / (double)pixelsInImage;
                q = (double)H2[i].Count / (double)pixelsInImage;

                if (p == 0)
                    div += 0;
                else if (q == 0)
                    div += 1;
                else if (p != 0 && q != 0)
                    div += p * Math.Log(p / q, 2);
            }
            return div;
        }

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void loadImage_Click(object sender, EventArgs e)
        {

            #region data definitions

            HistogramRGB hstElement = new HistogramRGB();



            double pixelDividedByLambdaHSV = 360 / lambdaPassedToMethod;
            int countOfVectorsMatrix = pixelDividedByLambda * pixelDividedByLambda * pixelDividedByLambda;
            int countHSVMatrix = 150;
            RGB[] allPossibleVectorsFiledFromImage = new RGB[countOfVectorsMatrix];
            //RGB[] sortedList = new RGB[countOfVectorsMatrix];
            //HistogramRGB[] hst = new HistogramRGB[countOfVectorsMatrix];
            //List<HistogramRGB> lisHist = new List<HistogramRGB>();
            HistogramRGB[] allRGBVectors = new HistogramRGB[countOfVectorsMatrix];
            HistogramRGB[] RGBHistogramtmp = new HistogramRGB[countOfVectorsMatrix];


            //HistogramHSV[] HSVtemp = new HistogramHSV[countOfVectorsMatrix];
            HistogramHSV[] allHSVVectors = new HistogramHSV[countOfVectorsMatrix];

            HistogramGrey[] greyHistogram = new HistogramGrey[sizeM];
            //Files oneFileTemp = new Files();
            //List<Files> filesWithDiv = new List<Files>();
            Files[] filesWithDiv = new Files[filesToCompare.Length];

            for (int i = 0; i < countOfVectorsMatrix; i++)
            {
                //hst[i] = new HistogramRGB();
                //hst[i].Vector = new RGB();

                allRGBVectors[i] = new HistogramRGB();
                allRGBVectors[i].Vector = new RGB();

                RGBHistogramtmp[i] = new HistogramRGB();
                RGBHistogramtmp[i].Vector = new RGB();

                //HSVtemp[i] = new HistogramHSV();
                //HSVtemp[i].Vector = new HSV();

                //allHSVVectors[i] = new HistogramHSV();
                //allHSVVectors[i].Vector = new HSV();
            }

            for (int i = 0; i < sizeM; i++)
            {
                greyHistogram[i] = new HistogramGrey();
                greyHistogram[i].pixel = new GrayPixel();
            }


            for (int i = 0; i < filesToCompare.Length; i++)
            {
                filesWithDiv[i] = new Files();
            }

            #endregion

            try
            {

                if (!fileNamePath.Equals(null))
                {
                    Image colorImage = Image.FromFile(fileNamePath);
                    Image compareImage;
                    //Bitmap monoCromaticImage = MakeGrayscale(new Bitmap(colorImage));

                    #region RGB
                    if ("RGB".Equals(RGBHSVCombobox.SelectedItem))
                    {
                        chart1.Series[0].IsVisibleInLegend = false;
                        chart1.Series["Series1"].Points.Clear();
                        allRGBVectors = getRGBHistogramFromImage(colorImage, lambdaPassedToMethod, pixelDividedByLambda);
                        foreach (var element in allRGBVectors)
                        {
                            chart1.Series["Series1"].Points.AddXY(element.Vector.ToString(), element.Count);
                        }

                        //Image img = Image.FromFile(filesToCompareComboBox.SelectedItem.ToString());

                        ////metoda zle pobiera histogram dla drugiego obrazka - debug - zly obrazek byl wczytywany w funkcji
                        //chart2.Series["Series1"].Points.Clear();
                        //RGBHistogramtmp = getRGBHistogramFromImage(img, lambdaPassedToMethod, pixelDividedByLambda);
                        //foreach (var item in RGBHistogramtmp)
                        //{
                        //    chart2.Series["Series1"].Points.AddXY(item.Vector.ToString(), item.Count);
                        //}


                        //do the math here
                        #region math

                        switch (MethodCombobox.SelectedItem.ToString())
                        {
                            case "Manhattan":

                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = ManhattanRGB(allRGBVectors, getRGBHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countOfVectorsMatrix);
                                }
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }

                                //wystwietlic histogram dla selected image z combobox + dla kazdego nowego selected niech sie generuje nowy histogram
                                //rozwiazanie -> histogram generowany za pomoca buttona -> zmniejsza czas czekania na wyniki
                                break;

                            case "Euklidesa":
                                //dorobic reszte metod + analogicznie dla HSV - problemy z rozmiarem wartosci H wziac pod uwage
                                //H max -> 360
                                //++++zrobic button, ktory generuje sam histogram na wykresie bez obliczen
                                //+dla obrazka selected z listy z folderu (128x128)

                                //dodatkowo zaimplementowac pozostale metody porownywania histogramow i je tu wlozyc + debug.

                                //EuklidesRGB
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = EuklidesRGB(allRGBVectors, getRGBHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countOfVectorsMatrix);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            case "przekroju histogramow":
                                //MPHRGB
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = MPHRGB(allRGBVectors, getRGBHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countOfVectorsMatrix, pixelsInImage);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            case "Jeffreya":
                                //JeffreyaRGB
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = JeffreyaRGB(allRGBVectors, getRGBHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countOfVectorsMatrix);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            case "Kullbacka-Leiblera":

                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = KullbackaLeibleraRGB(allRGBVectors, getRGBHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countOfVectorsMatrix);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            default:
                                break;
                        }

                        #endregion

                    }
                    #endregion

                    #region HSV
                    else if ("HSV".Equals(RGBHSVCombobox.SelectedItem))
                    {

                        chart1.Series["Series1"].Points.Clear();
                        allHSVVectors = getHSVHistogramFromImage(colorImage, lambdaPassedToMethod, pixelDividedByLambda);
                        foreach (var element in allHSVVectors)
                        {
                            chart1.Series["Series1"].Points.AddXY(element.Vector.ToString(), element.Count);
                        }

                        ///do the math here
                        #region math

                        switch (MethodCombobox.SelectedItem.ToString())
                        {
                            case "Manhattan":

                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = ManhattanHSV(allHSVVectors, getHSVHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countHSVMatrix);
                                }
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }

                                //wystwietlic histogram dla selected image z combobox + dla kazdego nowego selected niech sie generuje nowy histogram
                                //rozwiazanie -> histogram generowany za pomoca buttona -> zmniejsza czas czekania na wyniki
                                break;

                            case "Euklidesa":
                                //dorobic reszte metod + analogicznie dla HSV - problemy z rozmiarem wartosci H wziac pod uwage
                                //H max -> 360
                                //++++zrobic button, ktory generuje sam histogram na wykresie bez obliczen
                                //+dla obrazka selected z listy z folderu (128x128)

                                //dodatkowo zaimplementowac pozostale metody porownywania histogramow i je tu wlozyc + debug.

                                //EuklidesRGB
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = EuklidesHSV(allHSVVectors, getHSVHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countHSVMatrix);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            case "przekroju histogramow":
                                //MPHRGB
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = MPHHSV(allHSVVectors, getHSVHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countHSVMatrix, pixelsInImage);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            case "Jeffreya":
                                //JeffreyaRGB
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = JeffreyaHSV(allHSVVectors, getHSVHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countHSVMatrix);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            case "Kullbacka-Leiblera":

                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = KullbackaLeibleraHSV(allHSVVectors, getHSVHistogramFromImage(compareImage, lambdaPassedToMethod, pixelDividedByLambda), countHSVMatrix);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            default:
                                break;
                        }

                        #endregion

                    }
                    #endregion

                    #region grayscale
                    else if ("Grayscale".Equals(RGBHSVCombobox.SelectedItem))
                    {
                        chart1.Series["Series1"].Points.Clear();

                        greyHistogram = getAnglesHistogram(colorImage, 3);
                        foreach (var element in greyHistogram)
                        {
                            chart1.Series["Series1"].Points.AddXY(element.pixel.angle.ToString(), element.Count);
                        }

                        ///do the math here
                        #region math

                        switch (MethodCombobox.SelectedItem.ToString())
                        {
                            case "Manhattan":

                                
                                //getAnglesHistogram(colorImage, 3);
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    //wrzucic tu histogramy wygenerowane za pomoca wektorow do metody

                                    filesWithDiv[i].div = Manhattan(greyHistogram, getAnglesHistogram(compareImage, 3), sizeM);
                                }
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }

                                //wystwietlic histogram dla selected image z combobox + dla kazdego nowego selected niech sie generuje nowy histogram
                                //rozwiazanie -> histogram generowany za pomoca buttona -> zmniejsza czas czekania na wyniki
                                break;

                            case "Euklidesa":
                                //dorobic reszte metod + analogicznie dla HSV - problemy z rozmiarem wartosci H wziac pod uwage
                                //H max -> 360
                                //++++zrobic button, ktory generuje sam histogram na wykresie bez obliczen
                                //+dla obrazka selected z listy z folderu (128x128)

                                //dodatkowo zaimplementowac pozostale metody porownywania histogramow i je tu wlozyc + debug.

                                //EuklidesRGB
                                for (int i = 0; i < filesToCompare.Length; i++)
                                {
                                    compareImage = Image.FromFile(filesToCompare[i].ToString());
                                    filesWithDiv[i].path = filesToCompare[i].ToString();
                                    filesWithDiv[i].div = Euklides(greyHistogram, getAnglesHistogram(compareImage, 3), sizeM);
                                }

                                //sorting and view data
                                filesWithDiv = filesWithDiv.OrderBy(si => si.div).ToArray<Files>();
                                //wyswietlic posegregowana liste w combobox + div
                                filesToCompareComboBox.Items.Clear();
                                foreach (var image in filesWithDiv)
                                {
                                    filesToCompareComboBox.Items.Add("DIV: " + image.div + ", " + image.path);
                                }
                                break;

                            default:
                                MessageBox.Show("Gray scale only for Manhattan/Euklides methods.");
                                break;
                        }

                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }

            
        }


        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Jpeg Files|*.jpg";
                openFileDialog1.Title = "Select a jpeg File";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileNamePath = openFileDialog1.FileName;
                }



                MethodCombobox.Items.Clear();
                MethodCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
                MethodCombobox.Items.Add("Manhattan");
                MethodCombobox.Items.Add("Euklidesa");
                MethodCombobox.Items.Add("przekroju histogramow");
                MethodCombobox.Items.Add("Jeffreya");
                MethodCombobox.Items.Add("Kullbacka-Leiblera");
                MethodCombobox.SelectedItem = "Manhattan";

                RGBHSVCombobox.Items.Clear();
                RGBHSVCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
                RGBHSVCombobox.Items.Add("RGB");
                RGBHSVCombobox.Items.Add("HSV");
                RGBHSVCombobox.Items.Add("Grayscale");
                //RGBHSVCombobox.SelectedItem
                RGBHSVCombobox.SelectedItem = "Grayscale";


                selectFolderButton.Enabled = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();

                //string[] files = Directory.GetFiles(fbd.SelectedPath);
                filesToCompare = Directory.GetFiles(fbd.SelectedPath);

                filesToCompareComboBox.Items.Clear();
                filesToCompareComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                foreach (var file in filesToCompare)
                {
                    filesToCompareComboBox.Items.Add(file);
                }
                filesToCompareComboBox.SelectedItem = filesToCompare[0];

                loadImageButton.Enabled = true;
                generateHistogram2Button.Enabled = true;
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }

        }

        private void generateHistogram2Button_Click(object sender, EventArgs e)
        {
            try
            {
                Image colorImage;
                if (filesToCompareComboBox.SelectedItem != null)
                {
                    if (filesToCompareComboBox.SelectedItem.ToString().Split().Length > 1)
                        colorImage = Image.FromFile(filesToCompareComboBox.SelectedItem.ToString().Split()[2]);
                    else
                        colorImage = Image.FromFile(filesToCompareComboBox.SelectedItem.ToString());

                    chart2.Series[0].IsVisibleInLegend = false;
                    chart2.Series["Series1"].Points.Clear();

                    if ("RGB".Equals(RGBHSVCombobox.SelectedItem))
                        foreach (var item in getRGBHistogramFromImage(colorImage, lambdaPassedToMethod, pixelDividedByLambda))
                            chart2.Series["Series1"].Points.AddXY(item.Vector.ToString(), item.Count);

                    else if ("HSV".Equals(RGBHSVCombobox.SelectedItem))
                        foreach (var item in getHSVHistogramFromImage(colorImage, lambdaPassedToMethod, pixelDividedByLambda))
                            chart2.Series["Series1"].Points.AddXY(item.Vector.ToString(), item.Count);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        private static System.Drawing.Size GetSizeAndSaveFile(string imageUrl, int Height, int Width, string pathToSave, int numberOfFile)
        {
            System.Drawing.Size size = new System.Drawing.Size();
            int start, end;
            string newFile;
            try
            {
                Console.WriteLine(imageUrl);
                var request = System.Net.WebRequest.Create(imageUrl);

                request.Method = "GET";
                request.ContentType = "image/jpeg";
                request.Timeout = 400;
                var response = (System.Net.HttpWebResponse)request.GetResponse();
                var s = response.GetResponseStream();

                var bmp = new System.Drawing.Bitmap(s);
                size.Height = bmp.Height;
                size.Width = bmp.Width;

                string lowcasestr = imageUrl.ToLower();


                

                //save file to folder
                if (size.Height == Height && size.Width == Width)
                {

                    bmp.Save(Application.StartupPath + "\\" + pathToSave + "\\" + numberOfFile + ".jpg"); 
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return size;

        }
        //debug dla crawlera
        private void debugButton_Click(object sender, EventArgs e)
        {
            //pozyskaj liste obrazkow rozmiaru 128x128 - pobrac rozmiar obrazka wzorcowego
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string subPath = @"ImagesPath"; 
            string[] imagesToCompare;
            Image im;
            int i = 0;
            System.Drawing.Size size = new System.Drawing.Size();
            List<Uri> images = new List<Uri>();
            Crawler crawler = new Crawler();
            openFileDialog1.Filter = "Jpeg Files|*.jpg";
            openFileDialog1.Title = "Select a jpeg File";
            bool isExists = System.IO.Directory.Exists(subPath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(subPath);

            var files = Directory.GetFiles(subPath);

            if (files.Length != 0)
            {
                foreach (string filePath in files)
                    File.Delete(filePath);
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileNamePath = openFileDialog1.FileName;
            }

            im = Image.FromFile(fileNamePath);
            images = crawler.Run();
            filesToCompare = new string[images.Capacity];
            foreach (var item in images)
            {
                size = GetSizeAndSaveFile(item.ToString(), im.Height, im.Width, subPath, i);
                //if (size.Height == im.Height && size.Width == im.Width)
                //{
                    //filesToCompare[i] = item.ToString();
                    i++;
                //}
            }
            filesToCompare = Directory.GetFiles(subPath);
            filesToCompare = filesToCompare.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            loadImageButton.Enabled = true;
            //trzeba pobrac te obrazki do porownania..
            

            

            Console.WriteLine("Taki sobie przerywnik ;)");
        }

    }

    public class RGB : IComparable<RGB>
    {


        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public RGB() { }

        public RGB(int r, int g, int b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public int CompareTo(RGB value)
        {
            return this.R.CompareTo(value.R);
        }
    }

    public class HSV
    {

        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }
    }

    public class HistogramRGB
    {
        public RGB Vector;
        public int Count;
    }

    public class HistogramHSV
    {
        public HSV Vector;
        public int Count;
    }

    public class Files
    {
        public double div { get; set; }
        public string path { get; set; }
    }

    public class GrayPixel
    {
        public int value { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        //public int threshold { get; set; } //prog lokalny dla danego pixela
        public int angle { get; set; } //kat w stopniach
    }

    public class HistogramGrey
    {
        public GrayPixel pixel { get; set; }
        public int Count { get; set; }
    }
}
