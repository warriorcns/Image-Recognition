using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace CBIR
{
 

    class Edge
    {
        public string from { get; set; }
        public string to { get; set; }
        public int weight { get; set; }
    }

    class Results
    {
        public string URL { get; set; }
    }

    class Crawler
    {


        // Find a link in a content string.
        static string FindLink(string htmlstr, ref int startloc)
        {
            int i;
            int start, end;
            string uri = null;
            string lowcasestr = htmlstr.ToLower();
            i = lowcasestr.IndexOf("href=\"http", startloc);
            if (i != -1)
            {
                start = htmlstr.IndexOf('"', i) + 1;
                end = htmlstr.IndexOf('"', start);
                uri = htmlstr.Substring(start, end - start);
                startloc = end;
            }
            //Console.WriteLine(uri);
            return uri;
        }

        //szuka zadanego stringa w contencie strony
        //private bool FindString(string htmlstr, string searchString, ref int startLoc)
        //{
        //    string str = String.Empty;
        //    int i;
        //    int start, end;
        //    bool exists = false;
        //    string lowcasestr = htmlstr.ToLower();
        //    i = lowcasestr.IndexOf("href=\"http", startLoc);
        //    if (i != -1)
        //    {
        //        start = htmlstr.IndexOf('"', i) + 1;
        //        end = htmlstr.IndexOf(searchString, start);
        //        str = htmlstr.Substring(start, end - start);
        //        startLoc = end;
        //        exists = true;
        //    }
        //    return exists;
        //}

        //override
        private static string FindString(string htmlstr, string searchString, ref int startLoc)
        {
            string str = String.Empty;
            //int i;
            //int start, end;
            //string uri = null;
            //string searchStartString = "src=\"";
            //string lowcasestr = htmlstr.ToLower();
            //i = lowcasestr.IndexOf("href=\"http", startLoc);
            //if (i != -1)
            //{
            //    //trzeba znalezc poprawny start linku do zdjecia

            //    start = htmlstr.IndexOf(searchStartString, i) + 1;
            //    end = htmlstr.IndexOf(searchString, start);
            //    if(end != -1)
            //        str = htmlstr.Substring(start + 4, end - start - 1);
            //    startLoc = end;
            //}

            return str;
        }

        private static List<Uri> getImages(string httpstring)
        {
            List<Uri> links = new List<Uri>();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            //string regexImgSrc = @"<img.*?src=""(.*?)""";
            
            //Regex rg = new Regex(@"<img.*?src=""(.*?)""", RegexOptions.IgnoreCase);
            MatchCollection matchesImgSrc = Regex.Matches(httpstring, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                links.Add(new Uri(href));
            }
            return links;
        }
        //do not use
        //public static string getWebPageContent(string uri)
        //{
        //    int curloc;
        //    string content = string.Empty;
        //    Console.WriteLine("Linking to " + uri);
        //    // Create a WebRequest to the specified URI.
        //    HttpWebRequest req = (HttpWebRequest)
        //    WebRequest.Create(uri);
        //    uri = null; // disallow further use of this URI
        //    // Send that request and return the response.
        //    HttpWebResponse resp = (HttpWebResponse)
        //    req.GetResponse();
        //    // From the response, obtain an input stream.
        //    Stream istrm = resp.GetResponseStream();
        //    // Wrap the input stream in a StreamReader.
        //    StreamReader rdr = new StreamReader(istrm);
        //    // Read in the entire page.
        //    content = rdr.ReadToEnd();
        //    int d = content.Length;
        //    curloc = 0;
        //    resp.Close();

        //    return content;
        //}

        ////tworzy krawedz z zadanej strony poczatkowej, koncowej i wagi
        //public static Edge doEdge(string parentLink, string childLink, int depthLevel)
        //{
        //    Edge edge = new Edge();

        //    edge.from = parentLink;
        //    edge.to = childLink;
        //    edge.weight = depthLevel;

        //    return edge;
        //}

        //retrieve URLS from mother URL

        private static List<string> getURLsFromURL(string motherURL)
        {
            List<string> results = new List<string>();
            string str = string.Empty;
            int curloc;
            string link = string.Empty;
            //string content = getWebPageContent(motherURL);
            try
            {
                do
                {
                    //Console.WriteLine("Linking to " + motherURL);
                    // Create a WebRequest to the specified URI.
                    HttpWebRequest req = (HttpWebRequest)
                    WebRequest.Create(motherURL);
                    motherURL = null; // disallow further use of this URI
                    // Send that request and return the response.
                    HttpWebResponse resp = (HttpWebResponse)
                    req.GetResponse();
                    // From the response, obtain an input stream.
                    Stream istrm = resp.GetResponseStream();
                    // Wrap the input stream in a StreamReader.
                    StreamReader rdr = new StreamReader(istrm);
                    // Read in the entire page.
                    str = rdr.ReadToEnd();
                    curloc = 0;
                    int d = str.Length;
                    string content = string.Empty;
                    //link contains a web page link
                    // Close the response.
                    resp.Close();
                } while (motherURL != null);

                do
                {
                    link = FindLink(str, ref curloc);
                    results.Add(link);
                }
                while (link != null);
            }
            catch (WebException exc)
            {
                Console.WriteLine("Network Error: " + exc.Message +
                "\nStatus code: " + exc.Status);
            }
            catch (ProtocolViolationException exc)
            {
                Console.WriteLine("Protocol Error: " + exc.Message);
            }
            catch (UriFormatException exc)
            {
                Console.WriteLine("URI Format Error: " + exc.Message);
            }
            catch (NotSupportedException exc)
            {
                Console.WriteLine("Unknown Protocol: " + exc.Message);
            }
            catch (IOException exc)
            {
                Console.WriteLine("I/O Error: " + exc.Message);
            }
            return results;
        }

        //czy string istnieje w contencie strony
        private static List<Uri> getImageFromURLStringContent (string motherURL)
        {
            List<Uri> imagesList = new List<Uri>();
            string str = string.Empty;
            int curloc;
            bool exists = false;
            string imageUrl = string.Empty;
            int i = 0;
            try
            {
                do
                {
                    //Console.WriteLine("Linking to " + motherURL);
                    // Create a WebRequest to the specified URI.
                    HttpWebRequest req = (HttpWebRequest)
                    WebRequest.Create(motherURL);
                    motherURL = null; // disallow further use of this URI
                    // Send that request and return the response.
                    HttpWebResponse resp = (HttpWebResponse)
                    req.GetResponse();
                    // From the response, obtain an input stream.
                    Stream istrm = resp.GetResponseStream();
                    // Wrap the input stream in a StreamReader.
                    StreamReader rdr = new StreamReader(istrm);
                    // Read in the entire page.
                    str = rdr.ReadToEnd();
                    curloc = 0;
                    int d = str.Length;
                    string content = string.Empty;
                    //link contains a web page link
                    // Close the response.
                    resp.Close();
                } while (motherURL != null);

                //imageUrl = FindString(str, ".jpg", ref curloc);
                imagesList = getImages(str);
            }
            catch (WebException exc)
            {
                Console.WriteLine("Network Error: " + exc.Message +
                "\nStatus code: " + exc.Status);
            }
            catch (ProtocolViolationException exc)
            {
                Console.WriteLine("Protocol Error: " + exc.Message);
            }
            catch (UriFormatException exc)
            {
                Console.WriteLine("URI Format Error: " + exc.Message);
            }
            catch (NotSupportedException exc)
            {
                Console.WriteLine("Unknown Protocol: " + exc.Message);
            }
            catch (IOException exc)
            {
                Console.WriteLine("I/O Error: " + exc.Message);
            }
            return imagesList;
        }



        public List<Uri> Run()
        {

            List<Uri> imagesList = new List<Uri>();
            string link = null;
            string str;

            int curloc; // holds current location in response
            int currentWeight = 1;
            int maxWeight = 2;
            string mainURL = string.Empty;
            int matrixSize = 1000000;
            string[] webAdresses = new string[matrixSize];
            List<string>[] webURLS = new List<string>[matrixSize];
            List<Edge>[] edges = new List<Edge>[matrixSize];
            List<Results> stringFoundInURLs = new List<Results>();
            string tmp;
            Results oneResult = new Results();
            //bool turnOn = true;
            string currentURL = string.Empty;
            curloc = 0;

            for (int i = 0; i < 1000000; i++)
            {
                edges[i] = new List<Edge>();
                webURLS[i] = new List<string>();
                //stringFoundInURLs[i] = new List<Results>();
            }

            Edge edge = new Edge();
            List<string> globalFinalListURLS = new List<string>();


            string uristr = "https://plus.google.com/photos/110002866072327952621/albums/5540326895926152913";
            //"https://picasaweb.google.com/110002866072327952621/02COIL20";
            
            //"https://picasaweb.google.com/110002866072327952621/USCSIPITextures"; 
            //"http://www.google.pl";
            // holds current URI

            mainURL = uristr;
            List<string> results = new List<string>();// = getURLsFromURL("http://www.onet.pl");


            try
            {
                #region
                //do
                //{
                //    Console.WriteLine("Linking to " + uristr);
                //    // Create a WebRequest to the specified URI.
                //    HttpWebRequest req = (HttpWebRequest)
                //    WebRequest.Create(uristr);
                //    uristr = null; // disallow further use of this URI
                //    // Send that request and return the response.
                //    HttpWebResponse resp = (HttpWebResponse)
                //    req.GetResponse();
                //    // From the response, obtain an input stream.
                //    Stream istrm = resp.GetResponseStream();
                //    // Wrap the input stream in a StreamReader.
                //    StreamReader rdr = new StreamReader(istrm);
                //    // Read in the entire page.
                //    str = rdr.ReadToEnd();
                //    curloc = 0;
                //    int d = str.Length;
                //    string content = string.Empty;
                //    //link contains a web page link




                //    // Close the response.
                //    resp.Close();
                //} while (uristr != null);

                
                //foreach (var oneLink in results)
                //{
                //    //edges[1].Add(doEdge(mainURL, oneLink, 1));

                //}
                #endregion

                results = getURLsFromURL(mainURL);
                //for (int j = 1; j <= maxWeight; j++)
                //{
                foreach (var oneLink in results)
                {
                    if (oneLink != null)
                        webURLS[currentWeight].Add(oneLink);
                }
                //}

                currentWeight++;
                //
                for (int j = currentWeight; j <= maxWeight; j++)
                {
                    results = webURLS[j - 1];
                    foreach (var oneLink in results)
                    {
                        if (oneLink != null)
                            webURLS[j].AddRange(getURLsFromURL(oneLink));
                    }
                }
                
                //znalezc w contencie strony stringa z linkiem do obrazka
                currentWeight = 1;
                
                for (int j = currentWeight; j <= maxWeight+1; j++)
                {
                    results = webURLS[j - 1];
                    foreach (var oneLink in results)
                    {
                        if (oneLink != null)
                        {
                            imagesList.AddRange(getImageFromURLStringContent(oneLink));
                            //if (tmp.Length != 0)
                            //{
                            //    //imagesList.Add(tmp);
                            //}
                            if (imagesList.Capacity > 1000)
                                return imagesList;
                            
                        }
                    }
                    results = webURLS[j];
                }

                #region
                //for (int i = 1; i <= 1000; i++)
                //do
                //{
                //    // Find the next URI to link to.

                //    //link = FindLink(str, ref curloc);

                //    edges[currentWeight].Add(doEdge(mainURL, link, currentWeight));
                //    //str = string.Empty;
                //    //tu sie tworzy pierwsza gwiazda z krawedziami = 1


                //    while (link == null)
                //    {
                        
                //        str = string.Empty;

                //        foreach (var n in edges[currentWeight])
                //        {
                //            //przeczytaj tylko 1 strone i pobierz z niej linki
                //            if (turnOn)
                //            {
                //                str = getWebPageContent(n.to);
                //                currentURL = n.to;

                //            }
                //            link = FindLink(str, ref curloc);
                //            edges[currentWeight + 1].Add(doEdge(currentURL, link, currentWeight));
                //            turnOn = false;
                //            //curloc = 0;
                //        }
                //        currentWeight++;
                //        turnOn = true;

                //    }
                //} while (link != null);

                //content = getWebPageContent(link);
                //if (link != null)
                //{
                //    Console.WriteLine("Link found: " + link);
                //    //webAdresses.Add(link);
                //    if (currentWeight <= 2)
                //    {
                //        webAdresses[i] = link;
                //        webURLS.Add(link);

                //        if (currentWeight == 1)
                //            edge.from = mainURL;
                //        else
                //            edge.from = webAdresses[i];

                //        edge.to = link;
                //        edge.weight = currentWeight;
                //        edges.Add(edge);
                //        content = getWebPageContent(link);
                //        globalFinalListURLS.Add(FindString(content,ref curloc));
                //    }
                //    if (link == null)
                //        currentWeight++;


                //Console.Write("Link, More, Quit?");
                //answer = Console.ReadLine();

                //if (string.Compare(answer, "L", true) == 0)
                //{
                //    uristr = string.Copy(link);
                //    break;
                //}
                //else if (string.Compare(answer, "Q", true) == 0)
                //{
                //    break;
                //}
                //else if (string.Compare(answer, "M", true) == 0)
                //{
                //    Console.WriteLine("Searching for another link.");
                //}
                //}
                //else
                //{
                //    Console.WriteLine("No link found.");
                //    break;
                //}




                #endregion
                ;
                
            }
            catch (WebException exc)
            {
                Console.WriteLine("Network Error: " + exc.Message +
                "\nStatus code: " + exc.Status);
            }
            catch (ProtocolViolationException exc)
            {
                Console.WriteLine("Protocol Error: " + exc.Message);
            }
            catch (UriFormatException exc)
            {
                Console.WriteLine("URI Format Error: " + exc.Message);
            }
            catch (NotSupportedException exc)
            {
                Console.WriteLine("Unknown Protocol: " + exc.Message);
            }
            catch (IOException exc)
            {
                Console.WriteLine("I/O Error: " + exc.Message);
            }
            //Console.WriteLine("Terminating.");
            //Console.ReadKey();

            return imagesList;
        }
    }
}


