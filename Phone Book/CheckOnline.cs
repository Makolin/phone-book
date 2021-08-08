using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Phone_Book
{
    // Проверят список компьютеров на онлайн и если компьютер в сети, то обновляет базу данных
    internal static class CheckOnline
    {
        public static void SetStatus()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Computer> OnlineList = new List<Computer>(db.Computers.ToList());
                foreach (var oneNote in OnlineList)
                {
                    if (oneNote.LastOnline == null || oneNote.LastOnline < DateTime.Today)
                    {
                        try
                        {
                            Ping ping = new Ping();
                            PingReply pingReply = ping.Send(oneNote.NameComputer);
                            if (pingReply.Status.ToString().Equals("Success", StringComparison.Ordinal))
                            {
                                oneNote.Status = true;
                                oneNote.LastOnline = DateTime.Now;
                                db.Entry(oneNote).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                oneNote.Status = false;
                                db.Entry(oneNote).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
