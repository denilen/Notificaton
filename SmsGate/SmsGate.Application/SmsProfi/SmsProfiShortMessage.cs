using System.Collections.Generic;

namespace SmsGate.Application.SmsProfi
{
    public class SmsProfiShortMessage
    {
        public List<Message> Messages { get; set; } = new();
        public bool Validate { get; set; } = true; //true - тестовая отправка, false - боевая
        public int Channel { get; set; } = 0; //канал доставки
        public string TimeZone { get; set; } = "recipient"; //тайм-зона клиента
    }

    public class Message
    {
        //номер телефона
        public string Recipient { get; set; } = null!;

        //текст сообщения
        public string Text { get; set; } = null!;

        //тип получателя
        public string RecipientType { get; set; } = "recipient"; //значение по умолчанию

        public string Id { get; set; } = null!; //[ 1 .. 32 ] characters

        public string Source { get; set; } = null!;

        public int Timeout { get; set; } // [ 60 .. 86400 ] int

        //сокращать ссылки
        public bool ShortenUrl = false;
    }
}
