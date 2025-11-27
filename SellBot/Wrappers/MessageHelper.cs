using Discord;

namespace SellBot.Wrappers
{
    internal class MessageHelper
    {
        public static Embed SuccessEmbed(string message)
        {
            return CustomEmbed("Success ✔", message, Color.Green);
        }

        public static Embed FailedEmbed(string message)
        {
            return CustomEmbed("Error ✖", message, Color.Red);
        }

        public static Embed CustomEmbed(string title, string message, Color color)
        {
            return new EmbedBuilder
            {
                Title = title,
                Color = color,
                Description = message,
                Footer = new EmbedFooterBuilder { Text = "sellapi.io" },
            }.Build();
        }

        public static Embed CustomEmbed(string title, string message, Color color, List<EmbedFieldBuilder> fields)
        {
            return new EmbedBuilder
            {
                Title = title,
                Color = color,
                Description = message,
                Fields = fields,
                Footer = new EmbedFooterBuilder { Text = "sellapi.io" },
            }.Build();
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new();
            StreamWriter writer = new(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
