using System.ComponentModel;

namespace Baker_Server.Database.Entities
{
    public enum BunNameEnum
    {
        [Description("Круассан")]
        Cruassan = 1,
        [Description("Крендель")]
        Crendel,
        [Description("Багет")]
        Baget,
        [Description("Сметанник")]
        Smetanik,
        [Description("Батон")]
        Baton
    }
}
