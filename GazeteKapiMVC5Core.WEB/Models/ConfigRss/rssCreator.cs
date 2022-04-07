using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GazeteKapiMVC5Core.WEB.Models.ConfigRss
{
    public class rssCreator
    {
        //public void createRss()
        //{
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.ContentType = "text/xml";
        //    XmlTextWriter rssWriter = new XmlTextWriter(HttpContext.Current.Response.OutputStream, Encoding.UTF8);
        //    rssWriter.WriteStartDocument();
        //    //RSS Node'larını burada açmaya başlıyoruz.
        //    rssWriter.WriteStartElement("rss");
        //    rssWriter.WriteAttributeString("version", "1.0");
        //    rssWriter.WriteStartElement("channel");
        //    rssWriter.WriteElementString("title", "gencayyildiz.com RSS ÖRNEK");
        //    rssWriter.WriteElementString("link", "siteadi.com/rss");
        //    rssWriter.WriteElementString("description", "Gençay Yıldız | Kod Yazmak Bir Sanattır");
        //    rssWriter.WriteElementString("copyright", "(c) 2014, Gençay Yıldız");
        //    rssWriter.WriteElementString("pubDate", "06/03/2014");
        //    rssWriter.WriteElementString("language", "tr-TR");
        //    rssWriter.WriteElementString("webMaster", "gyildizmail@gmail.com");
        //    /*RSS kaynağında yayınlanacak yeni içerikler nesne olarak bu listede tutuluyor.
        //    Siz isterseniz eğer burada veritabanı işlemleriyle dinamik olarak son yayınları
        //     çekebilir ve listeye atabilirsiniz.
        //     */
        //    List<rss> rssList = new List<rss>
        //{
        //    new rss
        //    {
        //        Id = 1,
        //        title = "Konu Başlığı 1",
        //        description = "Açıklama 1",
        //        date = new DateTime(2014,03,07).ToString(),
        //        link = "http://www.bilmemne.com"
        //    },
        //     new rss
        //    {
        //        Id = 2,
        //        title = "Konu Başlığı 2",
        //        description = "Açıklama 2",
        //        date = new DateTime(2014,03,08).ToString(),
        //        link = "http://www.bilmemne.com"
        //    }
        //};
        //    foreach (var rss in rssList)
        //    {
        //        rssWriter.WriteStartElement("item");
        //        rssWriter.WriteElementString("title", rss.title);
        //        rssWriter.WriteElementString("description", rss.description);
        //        rssWriter.WriteElementString("link", rss.link);
        //        rssWriter.WriteElementString("pubDate", rss.date);
        //        rssWriter.WriteEndElement();
        //    }
        //    rssWriter.WriteEndDocument();
        //    //Belgeyi tamamlayıp ekrana çıktısını yazıyoruz...
        //    rssWriter.Flush();
        //    rssWriter.Close();
        //    HttpContext.Response.End();
        //}
    }
}
