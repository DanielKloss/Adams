using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TheClockEnd.Data;
using System.Collections.ObjectModel;
using TheClockEnd.Models;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class DataReaderTests
    {
        [TestMethod]
        public void ReaderPropertyShouldBeTypeOfXmlDataReader()
        {
            ICustomDataReader dataReader = DataReaderFactory.GetDataReader("");
            Assert.IsInstanceOfType(dataReader, typeof(XmlDataReader));
        }

        [TestMethod]
        public async Task XmlDataReaderShouldGetAllTrophyYears()
        {
            string fileName = "testTrophy.xml";

            XmlDataReader xmlDataReader = new XmlDataReader(fileName);
            xmlDataReader.xmlFile = XDocument.Load("TestFiles/TestTrophies.xml");

            ObservableCollection<TrophyYear> expected = new ObservableCollection<TrophyYear>
            {
                new TrophyYear
                {
                    trophyUrls = new List<string>
                    {
                        "/Assets/Trophies/test1.png"
                    },
                    year = "1980"
                },
                new TrophyYear
                {
                    trophyUrls = new List<string>
                    {
                        "/Assets/Trophies/test2.png"
                    },
                    year = "1990"
                },
                new TrophyYear
                {
                    trophyUrls = new List<string>
                    {
                        "/Assets/Trophies/test3.png"
                    },
                    year = "2000"
                },
            };

            ObservableCollection<TrophyYear> actual = await xmlDataReader.GetAllTrophyYears();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
