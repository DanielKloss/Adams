using System;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class WindowsLiveTileSchedule
    {
        public static void CreateSchedule()
        {
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            var plannedUpdated = tileUpdater.GetScheduledTileNotifications();

            DateTime now = DateTime.Now;
            DateTime planTill = now.AddHours(4);

            DateTime updateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(1);

            if (plannedUpdated.Count > 0)
            {
                updateTime = plannedUpdated.Select(x => x.DeliveryTime.DateTime).Union(new[] { updateTime }).Max();
            }

            XmlDocument documentNow = WriteXml(now);

            tileUpdater.Update(new TileNotification(documentNow) { ExpirationTime = now.AddMinutes(1) });

            for (var startPlanning = updateTime; startPlanning < planTill; startPlanning = startPlanning.AddMinutes(1))
            {
                XmlDocument document = WriteXml(startPlanning);

                ScheduledTileNotification scheduledNotification = new ScheduledTileNotification(document, new DateTimeOffset(startPlanning)) { ExpirationTime = startPlanning.AddMinutes(1) };
                tileUpdater.AddToSchedule(scheduledNotification);
            }
        }

        private static XmlDocument WriteXml(DateTime now)
        {
            const string xml = @"<tile>
                                    <visual> 
                                        <binding template='TileSquareImage' branding='none'>
                                            <image id='1' src='Assets/{0}/{1}.png' alt='{0}:{1}'/>
                                        </binding>
                                        <binding  template='TileWideImage' branding='none'>
                                            <image id='1' src='Assets/Wide/{0}/{1}.png' alt='{0}:{1}'/>
                                        </binding>
                                    </visual>
                                </tile>";

            var tileXmlNow = string.Format(xml, now.ToString("%h"), now.Minute.ToString());
            XmlDocument documentNow = new XmlDocument();
            documentNow.LoadXml(tileXmlNow);
            return documentNow;
        }
    }
}