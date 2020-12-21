using System;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    public class DisplayStatus : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(value.ToString());
                    if (pingReply.Status.ToString().Equals("Success"))
                    {
                        // Пользователь в сети
                        return "В сети";
                    }
                    else
                    {
                        // Пользователь отключен
                        return string.Empty;
                    }
                }
                catch
                {
                    // Проблемы с подключением
                }
            }

            // Иначе просто вернуть пустое значение
            return string.Empty;
        }
        public object ConvertBack(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
