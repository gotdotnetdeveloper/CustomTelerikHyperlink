using System;

namespace WpfApp
{
    /// <summary>
    /// Контекст для открытия гиперссылки
    /// </summary>
    [Serializable]
    public class OpenLinkContext
    {
        public Guid Id;
        public int Type;
        public string Text;
    }
}